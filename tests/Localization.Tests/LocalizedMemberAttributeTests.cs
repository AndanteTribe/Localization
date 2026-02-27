namespace Localization.Tests;

/// <summary>
/// LocalizedMemberAttributeのテスト
/// </summary>
public class LocalizedMemberAttributeTests
{
    [Theory]
    [InlineData(typeof(TestClassWithLocalizedProperty), "LocalizedText", true)]
    [InlineData(typeof(TestClassWithLocalizedField), "LocalizedText", false)]
    public void Attribute_CanBeApplied_ToMemberTypes(Type type, string memberName, bool isProperty)
    {
        // Arrange & Act
        var member = isProperty
            ? type.GetProperty(memberName) as System.Reflection.MemberInfo
            : type.GetField(memberName) as System.Reflection.MemberInfo;

        var attributes = member?.GetCustomAttributes(typeof(LocalizedMemberAttribute), false);

        // Assert
        Assert.NotNull(attributes);
        Assert.NotEmpty(attributes);
    }

    private class TestClassWithLocalizedProperty
    {
        [LocalizedMember]
        public string LocalizedText { get; set; } = "Test";
    }

    private class TestClassWithLocalizedField
    {
        [LocalizedMember]
        public string LocalizedText = "Test";
    }
}



