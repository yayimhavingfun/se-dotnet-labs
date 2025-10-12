using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients;
using Itmo.ObjectOrientedProgramming.Lab2.Recipients.Decorators;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Logging;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab2.Tests;

public class LoggingTests
{
    [Fact]
    public void LoggingDecorator_ShouldWriteLog_WhenMessageReceived()
    {
        // Arrange
        IRecipient mockRecipient = Substitute.For<IRecipient>();
        ILogger mockLogger = Substitute.For<ILogger>();
        var loggingDecorator = new LoggingDecorator(mockRecipient, mockLogger);

        var message = new Message("Test Header", "Test Body", ImportanceLevel.Normal);

        // Act
        loggingDecorator.Receive(message);

        // Assert
        mockLogger.Received(1).Log(Arg.Is<string>(s => s.Contains("Test Header")));
    }

    [Fact]
    public void LoggingDecorator_ShouldCallWrappedRecipient_AfterLogging()
    {
        // Arrange
        IRecipient mockRecipient = Substitute.For<IRecipient>();
        ILogger mockLogger = Substitute.For<ILogger>();
        var loggingDecorator = new LoggingDecorator(mockRecipient, mockLogger);

        var message = new Message("Test Header", "Test Body", ImportanceLevel.Normal);

        // Act
        loggingDecorator.Receive(message);

        // Assert
        mockRecipient.Received(1).Receive(message);
    }

    [Fact]
    public void LoggingDecorator_ShouldLogError_WhenRecipientThrowsException()
    {
        // Arrange
        IRecipient mockRecipient = Substitute.For<IRecipient>();
        ILogger mockLogger = Substitute.For<ILogger>();
        var loggingDecorator = new LoggingDecorator(mockRecipient, mockLogger);

        var message = new Message("Test Header", "Test Body", ImportanceLevel.Normal);

        mockRecipient.When(x => x.Receive(Arg.Any<Message>()))
            .Do(x => throw new InvalidOperationException("Test exception"));

        // Act
        Assert.Throws<InvalidOperationException>(() => loggingDecorator.Receive(message));

        // Assert
        mockLogger.Received(1).LogError(Arg.Is<string>(s => s.Contains("Test Header") && s.Contains("Test exception")));
    }
}