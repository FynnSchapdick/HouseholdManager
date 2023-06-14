using FluentValidation;
using FluentValidation.Results;

namespace HouseholdManager.Api.Endpoints;

public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.Arguments.SingleOrDefault(x => x is T) is not T validatable)
        {
            return Results.BadRequest();
        }

        ValidationResult validationResult = await _validator.ValidateAsync(validatable);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
        }

        return await next(context);
    }
}