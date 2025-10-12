using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Formatters;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class FormattingArchivatorTests
{
    [Fact]
    public void FormattingArchiver_ShouldCallFormatter_WhenMessageArchived()
    {
        // Arrange
        IMessageFormatter mockFormatter = Substitute.For<IMessageFormatter>();
        var formattingArchiver = new FormattingArchiver(mockFormatter);
        var message = new Message("Test Header", "Test Body", ImportanceLevel.Normal);

        // Act
        formattingArchiver.Archive(message);

        // Assert
        mockFormatter.Received(1).Format(message);
    }

    [Fact]
    public void FormattingArchiver_ShouldStoreMessage_AfterFormatting()
    {
        // Arrange
        IMessageFormatter mockFormatter = Substitute.For<IMessageFormatter>();
        var formattingArchiver = new FormattingArchiver(mockFormatter);
        var message = new Message("Test Header", "Test Body", ImportanceLevel.Normal);

        // Act
        formattingArchiver.Archive(message);

        // Assert
        Assert.Single(formattingArchiver.GetArchivedMessages());
        Assert.Contains(message, formattingArchiver.GetArchivedMessages());
    }

    [Fact]
    public void FormattingArchiver_ShouldWorkWithDifferentFormatters()
    {
        // Arrange
        IMessageFormatter mockConsoleFormatter = Substitute.For<IMessageFormatter>();
        IMessageFormatter mockFileFormatter = Substitute.For<IMessageFormatter>();

        var consoleArchiver = new FormattingArchiver(mockConsoleFormatter);
        var fileArchiver = new FormattingArchiver(mockFileFormatter);

        var message = new Message("Test", "Body", ImportanceLevel.High);

        // Act
        consoleArchiver.Archive(message);
        fileArchiver.Archive(message);

        // Assert
        mockConsoleFormatter.Received(1).Format(message);
        mockFileFormatter.Received(1).Format(message);
    }
}