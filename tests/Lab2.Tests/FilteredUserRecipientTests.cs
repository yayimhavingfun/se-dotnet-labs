using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients.Proxies;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class FilteredUserRecipientTests
{
    [Fact]
    public void UserWithFilteredAndNormalRecipients_ShouldReceiveMessageOnce_WhenImportanceBelowFilterThreshold()
    {
        // Arrange
        var realUser = new User("TestUser");

        var normalRecipient = new UserRecipient(realUser);
        var filteredRecipient = new FilteringProxy(
            new UserRecipient(realUser),
            ImportanceLevel.High);

        var group = new GroupRecipient();
        group.Add(normalRecipient);
        group.Add(filteredRecipient);

        var lowPriorityMessage = new Message("Low Priority", "Not important", ImportanceLevel.Low);

        // Act
        group.Receive(lowPriorityMessage);

        // Assert
        Assert.Single(realUser.ReceivedMessages);
    }
}