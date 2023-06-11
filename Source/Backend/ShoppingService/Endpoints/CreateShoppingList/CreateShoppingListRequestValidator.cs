using FluentValidation;
using ShoppingService.Domain;

namespace ShoppingService.Endpoints.CreateShoppingList;

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