using FluentValidation;

namespace ShoppingService.Endpoints.UpdateShoppingListItem;

public sealed class UpdateShoppingListItemRequestValidator : AbstractValidator<UpdateShoppingListItemRequest>
{
    public UpdateShoppingListItemRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}