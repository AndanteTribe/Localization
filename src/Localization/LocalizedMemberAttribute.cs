namespace Localization;

/// <summary>
/// Attribute specifying the members to be localized.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class LocalizedMemberAttribute : Attribute
{
}