using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class InMemoryArchivatorTests
{
    [Fact]
    public void InMemoryArchiver_ShouldStoreAndRetrieveMessages()
    {
        // Arrange
        var archiver = new InMemoryArchiver();
        var message1 = new Message("First", "First message content", ImportanceLevel.Normal);
        var message2 = new Message("Second", "Second message content", ImportanceLevel.High);
        var message3 = new Message("Third", "Third message content", ImportanceLevel.Low);

        // Act
        archiver.Archive(message1);
        archiver.Archive(message2);
        archiver.Archive(message3);

        var archivedMessages = archiver.GetArchivedMessages().ToList();

        // Assert
        Assert.Equal(3, archivedMessages.Count);
        Assert.Contains(message1, archivedMessages);
        Assert.Contains(message2, archivedMessages);
        Assert.Contains(message3, archivedMessages);
        Assert.Equal(3, archiver.Count);
    }

    [Fact]
    public void InMemoryArchiver_ShouldStoreMessagesInOrder()
    {
        // Arrange
        var archiver = new InMemoryArchiver();
        var message1 = new Message("First", "Content 1", ImportanceLevel.Normal);
        var message2 = new Message("Second", "Content 2", ImportanceLevel.High);
        var message3 = new Message("Third", "Content 3", ImportanceLevel.Low);

        // Act
        archiver.Archive(message1);
        archiver.Archive(message2);
        archiver.Archive(message3);

        var archivedMessages = archiver.GetArchivedMessages().ToList();

        // Assert
        Assert.Equal(message1, archivedMessages[0]);
        Assert.Equal(message2, archivedMessages[1]);
        Assert.Equal(message3, archivedMessages[2]);
    }
}