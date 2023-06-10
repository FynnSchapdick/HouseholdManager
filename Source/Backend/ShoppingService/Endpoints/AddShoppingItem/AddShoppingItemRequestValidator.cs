using System.Text.RegularExpressions;
using FluentValidation;

namespace ShoppingService.Endpoints.AddShoppingItem;

public sealed class AddShoppingItemRequestValidator : AbstractValidator<AddShoppingItemRequest>
{
    private static readonly Regex Ean8Regex = new(@"^\d{8}$", RegexOptions.Compiled);
    private static readonly Regex Ean13Regex = new(@"^\d{13}$", RegexOptions.Compiled);
    
    public AddShoppingItemRequestValidator()
    {
        RuleFor(x => x.Ean)
            .NotEmpty()
            .Must(BeValidEan);

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
    
    private bool BeValidEan(string ean)
        => Ean8Regex.IsMatch(ean) || Ean13Regex.IsMatch(ean);
}