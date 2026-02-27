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
}



