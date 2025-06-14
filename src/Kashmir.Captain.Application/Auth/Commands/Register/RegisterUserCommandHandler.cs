using Kashmir.Captain.Domain;
using Kashmir.Captain.Domain.Entities;
using Kashmir.Captain.Domain.Interfaces;
using Kashmir.Captain.Shared.Entities;
using Kashmir.Captain.Shared.Validations;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Kashmir.Captain.Application.Auth
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterResponse>
    {
        private readonly UserManager<User> _userManager;

        public RegisterUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<RegisterResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User(request.Email, request.FirstName, request.LastName, request.PhoneNumber);

            var createdResult = await _userManager.CreateAsync(user, request.Password);
            if (createdResult.Succeeded)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, nameof(RoleType.Worker));
                if (addRoleResult.Succeeded)
                {
                    // var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // await SendEmail(user, token);

                    // return new ApiResponse<string>(true, ValidationMessages.User_Registered_Success);
                    return new RegisterResponse(true, ValidationMessages.User_Registered_Success, user.Id);
                }
                else // RollBack
                {
                    await _userManager.DeleteAsync(user);
                    return new RegisterResponse(false, addRoleResult.Errors.First().Description.ToString(), 0);
                }
            }

            return new RegisterResponse(false, createdResult.Errors.First().Description.ToString(), 0);
        }
    }
}