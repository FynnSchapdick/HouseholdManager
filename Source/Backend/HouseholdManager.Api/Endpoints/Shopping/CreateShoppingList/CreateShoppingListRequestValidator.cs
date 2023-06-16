using FluentValidation;
using HouseholdManager.Domain.Shopping;

namespace HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;

public sealed class CreateShoppingListRequestValidator : AbstractValidator<CreateShoppingListRequest>
{
    public CreateShoppingListRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ShoppingListAggregate.Conventions.NAME_MIN_LENGTH)
            .MaximumLength(ShoppingListAggregate.Conventions.NAME_MAX_LENGTH);
    }
}