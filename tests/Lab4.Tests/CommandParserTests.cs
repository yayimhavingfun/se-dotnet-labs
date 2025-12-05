using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab4.Tests;

public class CommandParserTests
{
    private readonly CommandParser _parser;

    public CommandParserTests()
    {
        _parser = new CommandParser(builder =>
        {
            builder
                .AddHandler(new ConnectCommandHandler())
                .AddHandler(new DisconnectCommandHandler())
                .AddHandler(new TreeCommandHandler())
                .AddHandler(new FileCommandHandler());
        });
    }

    [Theory]
    [InlineData("connect C:\\test", "connect", "C:\\test", "local")]
    [InlineData("connect D:\\projects -m local", "connect", "D:\\projects", "local")]
    [InlineData("connect /home/user -m local", "connect", "/home/user", "local")]
    public void Parse_ConnectCommand_ReturnsCorrectTypeAndParameters(
        string input,
        string expectedCommandName,
        string expectedAddress,
        string expectedMode)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedAddress, parsedCommand.Parameters["address"]);
        Assert.Equal(expectedMode, parsedCommand.Parameters["mode"]);
        Assert.Equal(expectedAddress, parsedCommand.Arguments[0]);
    }

    [Fact]
    public void Parse_DisconnectCommand_ReturnsCorrectType()
    {
        // Arrange
        const string input = "disconnect";

        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal("disconnect", parsedCommand.CommandName);
        Assert.Empty(parsedCommand.Parameters);
        Assert.Empty(parsedCommand.Arguments);
        Assert.Empty(parsedCommand.Flags);
    }

    [Theory]
    [InlineData("tree goto folder", "tree_goto", "folder")]
    [InlineData("tree goto C:\\test\\folder", "tree_goto", "C:\\test\\folder")]
    [InlineData("tree goto ..\\parent", "tree_goto", "..\\parent")]
    [InlineData("tree goto .", "tree_goto", ".")]
    public void Parse_TreeGotoCommand_ReturnsCorrectTypeAndPath(
        string input,
        string expectedCommandName,
        string expectedPath)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedPath, parsedCommand.Parameters["path"]);
        Assert.Equal(expectedPath, parsedCommand.Arguments[0]);
    }

    [Theory]
    [InlineData("tree list -d 1", "tree_list", "1")]
    [InlineData("tree list -d 5", "tree_list", "5")]
    [InlineData("tree list -d 10", "tree_list", "10")]
    public void Parse_TreeListCommand_ReturnsCorrectTypeAndDepth(
        string input,
        string expectedCommandName,
        string expectedDepth)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedDepth, parsedCommand.Parameters["depth"]);
        Assert.Equal(expectedDepth, parsedCommand.Flags["d"]);
    }

    [Theory]
    [InlineData("file show document.txt -m console", "file_show", "document.txt", "console")]
    [InlineData("file show C:\\files\\readme.md -m console", "file_show", "C:\\files\\readme.md", "console")]
    [InlineData("file show ../file.txt -m console", "file_show", "../file.txt", "console")]
    public void Parse_FileShowCommand_ReturnsCorrectTypeAndParameters(
        string input,
        string expectedCommandName,
        string expectedPath,
        string expectedMode)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedPath, parsedCommand.Parameters["path"]);
        Assert.Equal(expectedMode, parsedCommand.Parameters["mode"]);
        Assert.Equal(expectedPath, parsedCommand.Arguments[0]);
        Assert.Equal(expectedMode, parsedCommand.Flags["m"]);
    }

    [Theory]
    [InlineData("file move source.txt destination.txt", "file_move", "source.txt", "destination.txt")]
    [InlineData("file move C:\\from\\file.txt C:\\to\\file.txt", "file_move", "C:\\from\\file.txt", "C:\\to\\file.txt")]
    [InlineData("file move ../old.txt ./new.txt", "file_move", "../old.txt", "./new.txt")]
    public void Parse_FileMoveCommand_ReturnsCorrectTypeAndPaths(
        string input,
        string expectedCommandName,
        string expectedSource,
        string expectedDestination)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedSource, parsedCommand.Parameters["source"]);
        Assert.Equal(expectedDestination, parsedCommand.Parameters["destination"]);
        Assert.Equal(expectedSource, parsedCommand.Arguments[0]);
        Assert.Equal(expectedDestination, parsedCommand.Arguments[1]);
    }

    [Theory]
    [InlineData("file copy original.txt backup.txt", "file_copy", "original.txt", "backup.txt")]
    [InlineData(
        "file copy C:\\data\\file.txt D:\\backup\\file.txt",
        "file_copy",
        "C:\\data\\file.txt",
        "D:\\backup\\file.txt")]
    public void Parse_FileCopyCommand_ReturnsCorrectTypeAndPaths(
        string input,
        string expectedCommandName,
        string expectedSource,
        string expectedDestination)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedSource, parsedCommand.Parameters["source"]);
        Assert.Equal(expectedDestination, parsedCommand.Parameters["destination"]);
        Assert.Equal(expectedSource, parsedCommand.Arguments[0]);
        Assert.Equal(expectedDestination, parsedCommand.Arguments[1]);
    }

    [Theory]
    [InlineData("file delete trash.txt", "file_delete", "trash.txt")]
    [InlineData("file delete C:\\temp\\old.txt", "file_delete", "C:\\temp\\old.txt")]
    [InlineData("file delete ../unused.txt", "file_delete", "../unused.txt")]
    public void Parse_FileDeleteCommand_ReturnsCorrectTypeAndPath(
        string input,
        string expectedCommandName,
        string expectedPath)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedPath, parsedCommand.Parameters["path"]);
        Assert.Equal(expectedPath, parsedCommand.Arguments[0]);
    }

    [Theory]
    [InlineData("file rename old.txt new.txt", "file_rename", "old.txt", "new.txt")]
    [InlineData("file rename document.txt document_v2.txt", "file_rename", "document.txt", "document_v2.txt")]
    [InlineData(
        "file rename C:\\files\\oldname.txt newname.txt",
        "file_rename",
        "C:\\files\\oldname.txt",
        "newname.txt")]
    public void Parse_FileRenameCommand_ReturnsCorrectTypeAndNames(
        string input,
        string expectedCommandName,
        string expectedPath,
        string expectedNewName)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal(expectedCommandName, parsedCommand.CommandName);
        Assert.Equal(expectedPath, parsedCommand.Parameters["path"]);
        Assert.Equal(expectedNewName, parsedCommand.Parameters["newName"]);
        Assert.Equal(expectedPath, parsedCommand.Arguments[0]);
        Assert.Equal(expectedNewName, parsedCommand.Arguments[1]);
    }

    [Theory]
    [InlineData("connect \"C:\\Program Files\\test\"", "C:\\Program Files\\test")]
    [InlineData("connect 'C:\\My Documents\\test'", "C:\\My Documents\\test")]
    public void Parse_ConnectCommandWithSpacesInPath_HandlesQuotesCorrectly(
        string input,
        string expectedAddress)
    {
        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal("connect", parsedCommand.CommandName);
        Assert.Equal(expectedAddress, parsedCommand.Parameters["address"]);
        Assert.Equal(expectedAddress, parsedCommand.Arguments[0]);
    }

    [Fact]
    public void Parse_ConnectCommandWithoutAddress_ThrowsArgumentException()
    {
        // Arrange
        const string input = "connect";

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => _parser.Parse(input));

        Assert.IsType<ArgumentException>(exception);
        Assert.Contains("connect", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Parse_TreeGotoCommandWithoutPath_ThrowsArgumentException()
    {
        // Arrange
        const string input = "tree goto";

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => _parser.Parse(input));
        Assert.Contains("requires path", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Parse_TreeListCommandWithoutDepthFlag_ThrowsArgumentException()
    {
        // Arrange
        const string input = "tree list";

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => _parser.Parse(input));
        Assert.Contains("requires -d flag", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Parse_FileShowCommandWithoutModeFlag_ThrowsArgumentException()
    {
        // Arrange
        const string input = "file show file.txt";

        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => _parser.Parse(input));
        Assert.Contains("requires -m flag", exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("file move source.txt", "requires source and destination")]
    [InlineData("file copy source.txt", "requires source and destination")]
    [InlineData("file rename old.txt", "requires path and new name")]
    public void Parse_IncompleteCommands_ThrowsArgumentException(string input, string expectedError)
    {
        // Act & Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(() => _parser.Parse(input));
        Assert.Contains(expectedError, exception.Message, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("unknown command")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("connect -m local")] // без адреса
    [InlineData("tree list -d not_a_number")] // не число
    public void Parse_InvalidCommands_ThrowsArgumentException(string input)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _parser.Parse(input));
    }

    [Theory]
    [InlineData("connect C:\\test", true)]
    [InlineData("disconnect", true)]
    [InlineData("tree goto folder", true)]
    [InlineData("tree list -d 2", true)]
    [InlineData("file show file.txt -m console", true)]
    [InlineData("unknown command", false)]
    [InlineData("", false)]
    [InlineData("connect", false)] // без адреса
    public void CanParse_VariousCommands_ReturnsExpectedResult(string input, bool expectedResult)
    {
        // Act
        bool result = _parser.CanParse(input);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void Parse_CommandWithMultipleFlags_HandlesCorrectly()
    {
        // Arrange
        const string input = "connect C:\\test -m local";

        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal("connect", parsedCommand.CommandName);
        Assert.Equal("C:\\test", parsedCommand.Parameters["address"]);
        Assert.Equal("local", parsedCommand.Parameters["mode"]);
    }

    [Fact]
    public void Parse_CommandWithFlagValue_HandlesCorrectly()
    {
        // Arrange
        const string input = "tree list -d 3 -o json";

        // Act
        ParsedCommand parsedCommand = _parser.Parse(input);

        // Assert
        Assert.Equal("tree_list", parsedCommand.CommandName);
        Assert.Equal("3", parsedCommand.Parameters["depth"]);
        Assert.Equal("json", parsedCommand.Flags["o"]);
    }
}