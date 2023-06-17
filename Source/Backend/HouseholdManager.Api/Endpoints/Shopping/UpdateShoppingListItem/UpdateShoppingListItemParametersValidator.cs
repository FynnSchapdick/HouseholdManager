using FluentValidation;

namespace HouseholdManager.Api.Endpoints.Shopping.UpdateShoppingListItem;

public sealed class UpdateShoppingListItemParametersValidator : AbstractValidator<UpdateShoppingListItemParameters>
{
    public UpdateShoppingListItemParametersValidator()
    {
        RuleFor(x => x.Body.Amount)
            .GreaterThan(0);
    }
}