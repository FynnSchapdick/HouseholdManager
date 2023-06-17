using FluentValidation;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed class AddShoppingItemParametersValidator : AbstractValidator<AddShoppingItemParameters>
{
    public AddShoppingItemParametersValidator()
    {
        RuleFor(x => x.Body.ProductId)
            .NotEmpty();

        RuleFor(x => x.Body.Amount)
            .GreaterThan(0);
    }
}