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
        Assert.Equal(2, literals.Length);
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
        Assert.Single(literals);
        Assert.Single(embeds);
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

    [Fact]
    public void AnalyzeFormat_EmptyString_ReturnsEmptyArrays()
    {
        // Arrange
        var format = "";

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Empty(literals);
        Assert.Empty(embeds);
    }

    [Fact]
    public void AnalyzeFormat_OnlyLiteral_ReturnsOnlyLiterals()
    {
        // Arrange
        var format = "Just a plain string";

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Single(literals);
        Assert.Empty(embeds);
        Assert.Equal("Just a plain string", literals[0]);
    }

    [Theory]
    [InlineData("Missing close {0")]
    [InlineData("{0 missing close")]
    public void AnalyzeFormat_MissingCloseBrace_ThrowsFormatException(string format)
    {
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => FormatHelper.AnalyzeFormat(format));
        Assert.Contains("closing curly brace", exception.Message);
    }

    [Theory]
    [InlineData("Extra } close")]
    [InlineData("} at start")]
    public void AnalyzeFormat_MissingOpenBrace_ThrowsFormatException(string format)
    {
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => FormatHelper.AnalyzeFormat(format));
        Assert.Contains("opening curly brace", exception.Message);
    }

    [Theory]
    [InlineData("{a}")]
    [InlineData("{-1}")]
    [InlineData("{1.5}")]
    public void AnalyzeFormat_NonIntegerIndex_ThrowsFormatException(string format)
    {
        // Act & Assert
        var exception = Assert.Throws<FormatException>(() => FormatHelper.AnalyzeFormat(format));
        Assert.Contains("not an integer", exception.Message);
    }

    [Fact]
    public void AnalyzeFormat_MultipleEmbeds_PreservesOrder()
    {
        // Arrange
        var format = "{2} {0} {1}";

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Equal(3, literals.Length);  // Empty string, " ", " " (no trailing literal)
        Assert.Equal((2, ""), embeds[0]);
        Assert.Equal((0, ""), embeds[1]);
        Assert.Equal((1, ""), embeds[2]);
    }

    [Fact]
    public void AnalyzeFormat_ComplexFormatWithColons_ParsesCorrectly()
    {
        // Arrange
        var format = "{0:yyyy-MM-dd HH:mm:ss}";

        // Act
        var (literals, embeds) = FormatHelper.AnalyzeFormat(format);

        // Assert
        Assert.Single(literals);
        Assert.Equal((0, "yyyy-MM-dd HH:mm:ss"), embeds[0]);
    }
}



