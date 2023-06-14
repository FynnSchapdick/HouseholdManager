using FluentValidation;
using HouseholdManager.Api.Domain;

namespace HouseholdManager.Api.Endpoints.Shopping.CreateShoppingList;

public sealed class CreateShoppingListRequestValidator : AbstractValidator<CreateShoppingListRequest>
{
    public CreateShoppingListRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ShoppingList.Conventions.NAME_MIN_LENGTH)
            .MaximumLength(ShoppingList.Conventions.NAME_MAX_LENGTH);
    }
}