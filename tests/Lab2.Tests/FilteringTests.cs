using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients.Proxies;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class FilteringTests
{
    [Fact]
    public void FilteringProxy_ShouldNotDeliverMessage_WhenImportanceBelowThreshold()
    {
        // Arrange
        IRecipient mockRecipient = Substitute.For<IRecipient>();
        var filteringProxy = new FilteringProxy(mockRecipient, ImportanceLevel.High);

        var lowPriorityMessage = new Message("Low Priority", "This is not important", ImportanceLevel.Low);

        // Act
        filteringProxy.Receive(lowPriorityMessage);

        // Assert
        mockRecipient.DidNotReceive().Receive(Arg.Any<Message>());
    }

    [Fact]
    public void FilteringProxy_ShouldDeliverMessage_WhenImportanceMeetsThreshold()
    {
        // Arrange
        IRecipient mockRecipient = Substitute.For<IRecipient>();
        var filteringProxy = new FilteringProxy(mockRecipient, ImportanceLevel.Normal);

        var highPriorityMessage = new Message("High Priority", "This is important", ImportanceLevel.High);

        // Act
        filteringProxy.Receive(highPriorityMessage);

        // Assert
        mockRecipient.Received(1).Receive(highPriorityMessage);
    }
}