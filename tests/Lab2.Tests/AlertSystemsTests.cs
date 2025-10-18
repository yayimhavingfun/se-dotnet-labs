using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class AlertSystemsTests
{
    [Fact]
    public void AlertRecipient_ShouldTriggerAlert_WhenSuspiciousWordInHeader()
    {
        // Arrange
        IAlertSystem mockAlertSystem = Substitute.For<IAlertSystem>();
        string[] suspiciousWords = new[] { "urgent", "password", "confidential" };
        var alertRecipient = new AlertRecipient(mockAlertSystem, suspiciousWords);

        var message = new Message("URGENT meeting", "Regular content", ImportanceLevel.Normal);

        // Act
        alertRecipient.Receive(message);

        // Assert
        mockAlertSystem.Received(1)
            .Alert(Arg.Is<string>(s =>
                s.Contains("URGENT meeting")));
    }

    [Fact]
    public void AlertRecipient_ShouldTriggerAlert_WhenSuspiciousWordInBody()
    {
        // Arrange
        IAlertSystem mockAlertSystem = Substitute.For<IAlertSystem>();
        string[] suspiciousWords = new[] { "urgent", "password", "confidential" };
        var alertRecipient = new AlertRecipient(mockAlertSystem, suspiciousWords);

        var message = new Message("Regular header", "Please reset your PASSWORD", ImportanceLevel.Normal);

        // Act
        alertRecipient.Receive(message);

        // Assert
        mockAlertSystem.Received(1)
            .Alert(Arg.Is<string>(s =>
                s.Contains("Regular header")));
    }

    [Fact]
    public void AlertRecipient_ShouldBeCaseInsensitive()
    {
        // Arrange
        IAlertSystem mockAlertSystem = Substitute.For<IAlertSystem>();
        string[] suspiciousWords = new[] { "urgent" };
        var alertRecipient = new AlertRecipient(mockAlertSystem, suspiciousWords);

        var message = new Message("UrGeNt update", "Content", ImportanceLevel.Normal);

        // Act
        alertRecipient.Receive(message);

        // Assert
        mockAlertSystem.Received(1).Alert(Arg.Any<string>());
    }

    [Fact]
    public void TextAlertSystem_ShouldOutputToConsole()
    {
        // Arrange
        var textAlertSystem = new TextAlertSystem();
        const string alertMessage = "Test alert message";

        // Act
        Exception exception = Record.Exception(() => textAlertSystem.Alert(alertMessage));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void SoundAlertSystem_ShouldNotThrowExceptions()
    {
        // Arrange
        var soundAlertSystem = new SoundAlertSystem();
        const string alertMessage = "Test sound alert";

        // Act
        Exception exception = Record.Exception(() => soundAlertSystem.Alert(alertMessage));

        // Assert
        Assert.Null(exception);
    }
}