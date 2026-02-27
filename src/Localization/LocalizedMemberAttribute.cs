namespace Localization;

/// <summary>
/// ローカライズ対象メンバーを指定する属性.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class LocalizedMemberAttribute : Attribute
{
}