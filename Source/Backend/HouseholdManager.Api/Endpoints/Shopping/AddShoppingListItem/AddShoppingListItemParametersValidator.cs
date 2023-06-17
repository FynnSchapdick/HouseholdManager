using FluentValidation;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed class AddShoppingListItemParametersValidator : AbstractValidator<AddShoppingListItemParameters>
{
    public AddShoppingListItemParametersValidator()
    {
        RuleFor(x => x.Body.ProductId)
            .NotEmpty();

        RuleFor(x => x.Body.Amount)
            .GreaterThan(0);
    }
}