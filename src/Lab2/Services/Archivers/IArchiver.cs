using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

public interface IArchiver
{
    void Archive(Message message);

    IEnumerable<Message> GetArchivedMessages();
}