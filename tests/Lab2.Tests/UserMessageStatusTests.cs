using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class UserMessageStatusTests
{
    [Fact]
    public void ReceiveMessage_StatusShouldBeUnread()
    {
        // Arrange
        var user = new User("TestUser");
        var message = new Message("header", "body", ImportanceLevel.Normal);

        // Act
        user.ReceiveMessage(message);

        // Assert
        Assert.False(user.IsRead(message));
        Assert.Contains(message, user.ReceivedMessages);
        Assert.Equal(1, user.UnreadCount);
    }

    [Fact]
    public void MarkAsRead_UnreadMessage_ShouldChangeToReadStatus()
    {
        // Arrange
        var user = new User("TestUser");
        var message = new Message("header", "body", ImportanceLevel.Normal);
        user.ReceiveMessage(message);

        Message receivedMessage = user.ReceivedMessages.First();

        Assert.False(user.IsRead(receivedMessage));

        // Act
        user.MarkAsRead(receivedMessage);

        // Assert
        Assert.True(user.IsRead(receivedMessage));
        Assert.Equal(0, user.UnreadCount);
    }

    [Fact]
    public void MarkAsRead_AlreadyReadMessage_ShouldThrowException()
    {
        // Arrange
        var user = new User("TestUser");
        var message = new Message("header", "body", ImportanceLevel.Normal);
        user.ReceiveMessage(message);

        Message receivedMessage = user.ReceivedMessages.First();

        // Act
        user.MarkAsRead(receivedMessage);

        // Assert
        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(() => user.MarkAsRead(receivedMessage));
        Assert.Equal("Message is already marked as read", exception.Message);
    }
}