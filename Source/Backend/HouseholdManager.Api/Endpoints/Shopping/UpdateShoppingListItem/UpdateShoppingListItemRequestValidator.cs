using FluentValidation;

namespace HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;

public sealed class UpdateShoppingListItemRequestValidator : AbstractValidator<UpdateShoppingListItemParameters>
{
    public UpdateShoppingListItemRequestValidator()
    {
        RuleFor(x => x.Body.Amount)
            .GreaterThan(0);
    }
}