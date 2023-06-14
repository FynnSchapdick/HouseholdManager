using Throw;

namespace HouseholdManager.Api.Extensions;

public static class ValidatableExtensions
{
    public static ref readonly Validatable<string> IfNotEan8Or13(this in Validatable<string> validatable)
    {
        if (!IsEan8Or13(validatable.Value))
        {
            throw new ArgumentException("EAN is not a valid EAN13 or EAN8", validatable.ParamName);
        }

        return ref validatable;
    }

    private static bool IsEan8Or13(string value)
    {
        return value.Length is 8 or 13;
    }
}