namespace Localization.Tests;

/// <summary>
/// FormatHelperクラスのテスト
/// </summary>
public class FormatHelperTests
{
    [Theory]
    [InlineData("Hello {0}, you are {1} years old", 3, 2)]
    [InlineData("Price: {0:C}, Count: {1:N0}", 2, 2)]
    [InlineData("No placeholders here", 1, 0)]
    [InlineData("{0}{1}{2}", 3, 3)]
    public void AnalyzeFormat_VariousFormats_ReturnsCorrectCounts(string format, int expectedLiteralCount, int expectedEmbedCount)
    {
        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Equal(expectedLiteralCount, literals.Length);
        Assert.Equal(expectedEmbedCount, embeds.Length);
    }

    [Fact]
    public void AnalyzeFormat_SimpleFormat_ReturnsCorrectParts()
    {
        // Arrange
        var format = "Hello {0}, you are {1} years old";

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Equal("Hello ", literals[0]);
        Assert.Equal(", you are ", literals[1]);
        Assert.Equal(" years old", literals[2]);
        Assert.Equal((0, ""), embeds[0]);
        Assert.Equal((1, ""), embeds[1]);
    }

    [Fact]
    public void AnalyzeFormat_FormatWithFormatSpecifier_ReturnsFormatSpecifier()
    {
        // Arrange
        var format = "Price: {0:C}, Count: {1:N0}";

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Equal((0, "C"), embeds[0]);
        Assert.Equal((1, "N0"), embeds[1]);
    }

    [Fact]
    public void AnalyzeFormat_ReadOnlySpan_WorksCorrectly()
    {
        // Arrange
        ReadOnlySpan<char> format = "Test {0}".AsSpan();

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Equal(1, literals.Length);
        Assert.Equal(1, embeds.Length);
        Assert.Equal((0, ""), embeds[0]);
    }

    [Theory]
    [InlineData("Invalid {0")]
    [InlineData("Invalid } brace")]
    [InlineData("Invalid {abc}")]
    public void AnalyzeFormat_InvalidFormat_ThrowsFormatException(string format)
    {
        // Act & Assert
        Assert.Throws<FormatException>(() => FormatHelper.AnalyzeFormat(format));
    }
}



