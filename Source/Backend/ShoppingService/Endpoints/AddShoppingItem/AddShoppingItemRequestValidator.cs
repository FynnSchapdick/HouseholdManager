using FluentValidation;

namespace ShoppingService.Endpoints.AddShoppingItem;

public sealed class AddShoppingItemRequestValidator : AbstractValidator<AddShoppingItemRequest>
{
    public AddShoppingItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}