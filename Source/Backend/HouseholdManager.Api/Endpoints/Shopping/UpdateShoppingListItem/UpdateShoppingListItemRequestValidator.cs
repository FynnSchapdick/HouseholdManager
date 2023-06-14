using FluentValidation;

namespace HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;

public sealed class UpdateShoppingListItemRequestValidator : AbstractValidator<UpdateShoppingListItemRequest>
{
    public UpdateShoppingListItemRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}