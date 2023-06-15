using System.Text.RegularExpressions;
using FluentValidation;
using HouseholdManager.Api.Domain.Product;

namespace HouseholdManager.Api.Endpoints.Products.CreateProduct;

public sealed partial class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    [GeneratedRegex(@"^\d{8}$", RegexOptions.Compiled)]
    private static partial Regex Ean8Regex();

    [GeneratedRegex(@"^\d{13}$", RegexOptions.Compiled)]
    private static partial Regex Ean13Regex();

    public CreateProductRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(ProductAggregate.Conventions.NAME_MIN_LENGTH)
            .MaximumLength(ProductAggregate.Conventions.NAME_MAX_LENGTH);

        When(x => !string.IsNullOrEmpty(x.Ean), () =>
        {
            RuleFor(x => x.Ean)
                .Must(BeValidEan!);
        });
    }

    private static bool BeValidEan(string ean)
        => Ean8Regex().IsMatch(ean) || Ean13Regex().IsMatch(ean);
}