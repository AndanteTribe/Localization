namespace Localization.Tests;

/// <summary>
/// Localizeクラスのテスト
/// </summary>
public class LocalizeTests
{
    [Theory]
    [InlineData("Hello {0}", "World", "Hello World")]
    [InlineData("Name: {0}", "Test", "Name: Test")]
    public void Format_WithSingleStringArgument_FormatsCorrectly(string format, string arg, string expected)
    {
        // Arrange
        var parsedFormat = LocalizeFormat.Parse(format);

        // Act
        var result = Localize.Format(parsedFormat, arg);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Count: {0}", 42, "Count: 42")]
    public void Format_WithSingleNumberArgument_FormatsCorrectly(string format, int arg, string expected)
    {
        // Arrange
        var parsedFormat = LocalizeFormat.Parse(format);

        // Act
        var result = Localize.Format(parsedFormat, arg);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Format_WithSingleDoubleArgument_FormatsCorrectly()
    {
        // Arrange
        var parsedFormat = LocalizeFormat.Parse("Value: {0}");

        // Act
        var result = Localize.Format(parsedFormat, 3.14);

        // Assert
        Assert.Equal("Value: 3.14", result);
    }

    [Fact]
    public void Format_WithTwoArguments_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Hello {0}, you are {1} years old");

        // Act
        var result = Localize.Format(format, "John", 30);

        // Assert
        Assert.Equal("Hello John, you are 30 years old", result);
    }

    [Fact]
    public void Format_WithThreeArguments_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Name: {0}, Age: {1}, City: {2}");

        // Act
        var result = Localize.Format(format, "Alice", 25, "Tokyo");

        // Assert
        Assert.Equal("Name: Alice, Age: 25, City: Tokyo", result);
    }

    [Fact]
    public void Format_WithFourArguments_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Name: {0}, Age: {1}, City: {2}, Country: {3}");

        // Act
        var result = Localize.Format(format, "Bob", 28, "Osaka", "Japan");

        // Assert
        Assert.Equal("Name: Bob, Age: 28, City: Osaka, Country: Japan", result);
    }

    [Fact]
    public void Format_WithFiveArguments_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("A: {0}, B: {1}, C: {2}, D: {3}, E: {4}");

        // Act
        var result = Localize.Format(format, "a", "b", "c", "d", "e");

        // Assert
        Assert.Equal("A: a, B: b, C: c, D: d, E: e", result);
    }

    [Theory]
    [InlineData("Price: {0:C}", 123.45)]
    [InlineData("Count: {0:N0}", 1000)]
    [InlineData("Binary: {0:B}", 5)]
    [InlineData("Value: {0:F2}", 3.14159)]
    [InlineData("Value: {0:E3}", 123456.789)]
    public void Format_WithFormatSpecifier_FormatsCorrectly(string format, object value)
    {
        // Arrange
        var parsedFormat = LocalizeFormat.Parse(format);

        // Act
        var result = Localize.Format(parsedFormat, value);

        // Assert
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Format_WithDateTimeType_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Date: {0:yyyy-MM-dd}");
        var date = new DateTime(2024, 12, 25);

        // Act
        var result = Localize.Format(format, date);

        // Assert
        Assert.Equal("Date: 2024-12-25", result);
    }

    [Fact]
    public void Format_WithNullValue_HandlesNullCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Value: {0}");
        string? nullValue = null;

        // Act
        var result = Localize.Format(format, nullValue);

        // Assert
        Assert.Equal("Value: ", result);
    }

    [Fact]
    public void Format_WithReorderedIndices_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("{1} is {0}");

        // Act
        var result = Localize.Format(format, "awesome", "C#");

        // Assert
        Assert.Equal("C# is awesome", result);
    }

    [Fact]
    public void Format_TwoArgs_WithMixedFormatSpecifiers_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Name: {0}, Count: {1:N0}");

        // Act
        var result = Localize.Format(format, "Test", 5000);

        // Assert
        Assert.Equal("Name: Test, Count: 5,000", result);
    }

    [Fact]
    public void Format_ThreeArgs_WithAllDifferentTypes_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Text: {0}, Number: {1}, Date: {2:MM/dd}");

        // Act
        var result = Localize.Format(format, "Hello", 42, new DateTime(2024, 12, 25));

        // Assert
        Assert.Equal("Text: Hello, Number: 42, Date: 12/25", result);
    }
}




