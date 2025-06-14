using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Kashmir.Captain.Shared.Extensions;

namespace Kashmir.Captain.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var optionalValidation = request as IOptionalValidation;
            if (optionalValidation?.IsValidationDisabled == true)
            {
                return await next();
            }
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("----- Validating command {CommandType}", typeName);

            var failures = new List<ValidationFailure>();
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(request, cancellationToken);
                failures.AddRange(result.Errors);
            }

            if (failures.Count > 0)
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);
                throw new ValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", failures);
            }

            return await next();
        }
    }
}
