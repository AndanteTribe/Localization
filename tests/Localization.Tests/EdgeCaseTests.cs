namespace Localization.Tests;

/// <summary>
/// エッジケーステスト
/// </summary>
public class EdgeCaseTests
{
    [Theory]
    [InlineData("Text: {0}, Special: <>|?*", "value", "Text: value, Special: <>|?*")]
    [InlineData("Empty: {0}", "", "Empty: ")]
    public void Format_WithStringInputs_PreservesFormatting(string format, string value, string expected)
    {
        // Arrange
        var parsedFormat = LocalizeFormat.Parse(format);

        // Act
        var result = Localize.Format(parsedFormat, value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Number: {0}", 123, "Number: 123")]
    public void Format_WithNumberInput_PreservesFormatting(string format, int value, string expected)
    {
        // Arrange
        var parsedFormat = LocalizeFormat.Parse(format);

        // Act
        var result = Localize.Format(parsedFormat, value);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Format_WithBoolInput_PreservesFormatting()
    {
        // Arrange
        var format = LocalizeFormat.Parse("Bool: {0}");

        // Act
        var result = Localize.Format(format, true);

        // Assert
        Assert.Equal("Bool: True", result);
    }

    [Fact]
    public void Format_WithUnicodeCharacters_PreservesUnicode()
    {
        // Arrange
        var format = LocalizeFormat.Parse("こんにちは {0}、世界");

        // Act
        var result = Localize.Format(format, "🌍");

        // Assert
        Assert.Equal("こんにちは 🌍、世界", result);
    }

    [Fact]
    public void Format_WithEmptyLiterals_FormatsCorrectly()
    {
        // Arrange
        var format = LocalizeFormat.Parse("{0}{1}{2}");

        // Act
        var result = Localize.Format(format, "a", "b", "c");

        // Assert
        Assert.Equal("abc", result);
    }

    [Fact]
    public void Format_WithVeryLongFormat_WorksCorrectly()
    {
        // Arrange
        var longFormat = "Prefix " + string.Join(" ", Enumerable.Range(0, 100).Select(i => $"{{0}}")) + " Suffix";
        var format = LocalizeFormat.Parse(longFormat);

        // Act
        var result = Localize.Format(format, "value");

        // Assert
        Assert.NotEmpty(result);
        Assert.StartsWith("Prefix", result);
    }
}




