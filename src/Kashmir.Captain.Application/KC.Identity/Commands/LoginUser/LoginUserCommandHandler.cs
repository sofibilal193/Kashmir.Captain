using KC.Infrastructure.Persistance.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Kashmir.Captain.Application.Extensions;
using Kashmir.Captain.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Kashmir.Captain.Application.Identity
{
	public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ApiResponse<LoginToken>>
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;
		private readonly SignInManager<User> _signInManager;

		public LoginUserCommandHandler(UserManager<User> userManager,
										SignInManager<User> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		public async Task<ApiResponse<LoginToken>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			// Find the user by email
			var user = await _userManager.FindByEmailAsync(request.Email)
				?? throw new NotFoundException(nameof(User), request.Email);

			if (string.IsNullOrEmpty(user.Email))
			{
				throw new ValidationException("User's email is not set.");
			}

			// Check the user's password
			var passwordCheck = await _userManager.CheckPasswordAsync(user, request.Password);
			if (!passwordCheck)
			{
				return new ApiResponse<LoginToken>(false, "invalid Credential");
			}

			// Sign in the user
			var signInResult = await _signInManager.PasswordSignInAsync(user.Email, request.Password, request.RememberMe, lockoutOnFailure: false);
			if (!signInResult.Succeeded)
			{
				if (signInResult.IsLockedOut)
				{
					return new ApiResponse<LoginToken>(false, "Locked Out");
				}
				throw new UnauthorizedAccessException();
			}
			var roles = await _userManager.GetRolesAsync(user);

			// Generate JWT token
			var authClaims = new List<Claim>
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Role,  (await _userManager.GetRolesAsync(user)).First())
					};

			await _userManager.AddClaimsAsync(user, authClaims);

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				expires: DateTime.Now.AddDays(3),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);

			// Return the token
			var loginToken = new LoginToken
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
			};

			return new ApiResponse<LoginToken>(true, "Logged In Successfully", loginToken);
		}
	}
}
