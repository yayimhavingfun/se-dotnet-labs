using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class MessageDistributorTests
{
    [Fact]
    public void SendToTopic_ShouldDeliverMessageToAllRecipientsInTopic()
    {
        // Arrange
        var distributor = new MessageDistributor();

        IRecipient mockUserRecipient = Substitute.For<IRecipient>();
        IRecipient mockArchiverRecipient = Substitute.For<IRecipient>();
        IRecipient mockAlertRecipient = Substitute.For<IRecipient>();

        distributor.CreateTopic("test-topic", mockUserRecipient, mockArchiverRecipient, mockAlertRecipient);

        var message = new Message("Test Header", "Test Body", ImportanceLevel.High);

        // Act
        distributor.SendToTopic("test-topic", message);

        // Assert
        mockUserRecipient.Received(1).Receive(message);
        mockArchiverRecipient.Received(1).Receive(message);
        mockAlertRecipient.Received(1).Receive(message);
    }

    [Fact]
    public void SendToAllTopics_ShouldDeliverMessageToAllExistingTopics()
    {
        // Arrange
        var distributor = new MessageDistributor();

        IRecipient mockDevRecipient = Substitute.For<IRecipient>();
        IRecipient mockAdminRecipient = Substitute.For<IRecipient>();
        IRecipient mockLogRecipient = Substitute.For<IRecipient>();

        distributor.CreateTopic("developers", mockDevRecipient);
        distributor.CreateTopic("admins", mockAdminRecipient);
        distributor.CreateTopic("logging", mockLogRecipient);

        var message = new Message("Broadcast", "System maintenance", ImportanceLevel.Normal);

        // Act
        distributor.SendToAllTopics(message);

        // Assert
        mockDevRecipient.Received(1).Receive(message);
        mockAdminRecipient.Received(1).Receive(message);
        mockLogRecipient.Received(1).Receive(message);
    }
}