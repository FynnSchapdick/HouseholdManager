using FluentValidation;

namespace HouseholdManager.Api.Endpoints.Shopping.AddShoppingListItem;

public sealed class AddShoppingItemRequestValidator : AbstractValidator<AddShoppingListItemRequest>
{
    public AddShoppingItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}