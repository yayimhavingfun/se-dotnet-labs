using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;

public interface ICommand
{
    FileSystemResult Execute();
}