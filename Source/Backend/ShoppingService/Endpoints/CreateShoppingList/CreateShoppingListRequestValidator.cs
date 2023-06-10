using FluentValidation;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.CreateShoppingList;

public sealed class CreateShoppingListRequestValidator : AbstractValidator<CreateShoppingListRequest>
{
    public CreateShoppingListRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ShoppingList.NameMinLength)
            .MaximumLength(ShoppingList.NameMaxLength);
    }
}