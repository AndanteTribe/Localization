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

    [Fact]
    public void Format_WithLargeLiteralLength_ExceedsStackAllocationThreshold()
    {
        // Arrange - Create a format that exceeds 256 character threshold for stack allocation
        var largeLiteral = new string('x', 300);
        var format = LocalizeFormat.Parse($"{largeLiteral}{{0}}");

        // Act
        var result = Localize.Format(format, "test");

        // Assert
        Assert.Contains("test", result);
        Assert.Contains(largeLiteral, result);
    }

    [Fact]
    public void Format_TwoArgs_WithLargeLiteralLength_WorksCorrectly()
    {
        // Arrange
        var largeLiteral = new string('a', 150);
        var format = LocalizeFormat.Parse($"{largeLiteral}{{0}}{largeLiteral}{{1}}");

        // Act
        var result = Localize.Format(format, "X", "Y");

        // Assert
        Assert.Contains("X", result);
        Assert.Contains("Y", result);
    }

    [Fact]
    public void Format_ThreeArgs_WithLargeLiteralLength_WorksCorrectly()
    {
        // Arrange
        var largeLiteral = new string('b', 100);
        var format = LocalizeFormat.Parse($"{largeLiteral}{{0}}{{1}}{{2}}");

        // Act
        var result = Localize.Format(format, 1, 2, 3);

        // Assert
        Assert.Contains("1", result);
        Assert.Contains("2", result);
        Assert.Contains("3", result);
    }

    [Fact]
    public void Format_FourArgs_WithLargeLiteralLength_WorksCorrectly()
    {
        // Arrange
        var largeLiteral = new string('c', 80);
        var format = LocalizeFormat.Parse($"{largeLiteral}{{0}}{{1}}{{2}}{{3}}");

        // Act
        var result = Localize.Format(format, "A", "B", "C", "D");

        // Assert
        Assert.Contains("A", result);
        Assert.Contains("D", result);
    }

    [Fact]
    public void Format_FiveArgs_WithLargeLiteralLength_WorksCorrectly()
    {
        // Arrange
        var largeLiteral = new string('d', 70);
        var format = LocalizeFormat.Parse($"{largeLiteral}{{0}}{{1}}{{2}}{{3}}{{4}}");

        // Act
        var result = Localize.Format(format, 1, 2, 3, 4, 5);

        // Assert
        Assert.Contains("1", result);
        Assert.Contains("5", result);
    }

    [Fact]
    public void ToString_WithLargeLiteralLength_WorksCorrectly()
    {
        // Arrange
        var largeLiteral = new string('e', 300);
        var originalFormat = $"{largeLiteral}{{0}}";
        var format = LocalizeFormat.Parse(originalFormat);

        // Act
        var result = format.ToString();

        // Assert
        Assert.Equal(originalFormat, result);
    }
}




