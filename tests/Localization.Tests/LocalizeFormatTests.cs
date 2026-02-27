namespace Localization.Tests;

/// <summary>
/// LocalizeFormatクラスのテスト
/// </summary>
public class LocalizeFormatTests
{
    [Theory]
    [InlineData("Hello {0}", 1, 1)]
    [InlineData("Value: {0:F2}", 1, 1)]
    [InlineData("No placeholders", 1, 0)]
    [InlineData("Test {0}", 1, 1)]
    public void Parse_VariousFormats_CreatesValidLocalizeFormat(string format, int expectedLiteralCount, int expectedEmbedCount)
    {
        // Act
        var localizeFormat = LocalizeFormat.Parse(format);

        // Assert
        Assert.Equal(expectedLiteralCount, localizeFormat.Literal.Length);
        Assert.Equal(expectedEmbedCount, localizeFormat.Embed.Length);
    }

    [Fact]
    public void Parse_SimpleFormat_ReturnsCorrectEmbedInfo()
    {
        // Arrange
        var format = "Hello {0}";

        // Act
        var localizeFormat = LocalizeFormat.Parse(format);

        // Assert
        Assert.Equal((0, ""), localizeFormat.Embed[0]);
    }

    [Fact]
    public void Parse_FormatSpecifier_ReturnsCorrectFormatString()
    {
        // Arrange
        var format = "Value: {0:F2}";

        // Act
        var localizeFormat = LocalizeFormat.Parse(format);

        // Assert
        Assert.Equal((0, "F2"), localizeFormat.Embed[0]);
    }

    [Theory]
    [InlineData("Hello {0}")]
    [InlineData("Value {0:C}, Count {1:N0}, Date {2:yyyy-MM-dd}")]
    [InlineData("No placeholders")]
    public void ToString_VariousFormats_RecreatesOriginalFormat(string originalFormat)
    {
        // Arrange
        var localizeFormat = LocalizeFormat.Parse(originalFormat);

        // Act
        var result = localizeFormat.ToString();

        // Assert
        Assert.Equal(originalFormat, result);
    }

    [Fact]
    public void ReadOnlySpan_Parse_WorksCorrectly()
    {
        // Arrange
        ReadOnlySpan<char> format = "Value: {0:F2}".AsSpan();

        // Act
        var localizeFormat = LocalizeFormat.Parse(format);

        // Assert
        Assert.Equal(1, localizeFormat.Literal.Length);
        Assert.Equal(1, localizeFormat.Embed.Length);
        Assert.Equal((0, "F2"), localizeFormat.Embed[0]);
    }

    [Fact]
    public void ToString_WithMultipleFormats_RecreatesCorrectly()
    {
        // Arrange
        var format = "{0:D} {1:X} {2:N2}";
        var localizeFormat = LocalizeFormat.Parse(format);

        // Act
        var result = localizeFormat.ToString();

        // Assert
        Assert.Equal(format, result);
    }

    [Fact]
    public void ToString_WithEmptyFormat_RecreatesCorrectly()
    {
        // Arrange
        var format = "{0} {1}";
        var localizeFormat = LocalizeFormat.Parse(format);

        // Act
        var result = localizeFormat.ToString();

        // Assert
        Assert.Equal(format, result);
    }

    [Fact]
    public void Parse_WithVeryLongFormatString_WorksCorrectly()
    {
        // Arrange
        var format = "Start: {0:F10}, Middle: {1:N5}, End: {2:E8}";

        // Act
        var localizeFormat = LocalizeFormat.Parse(format);

        // Assert
        Assert.Equal(3, localizeFormat.Literal.Length);
        Assert.Equal(3, localizeFormat.Embed.Length);
        Assert.Equal((0, "F10"), localizeFormat.Embed[0]);
        Assert.Equal((1, "N5"), localizeFormat.Embed[1]);
        Assert.Equal((2, "E8"), localizeFormat.Embed[2]);
    }
}



