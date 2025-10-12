using Itmo.ObjectOrientedProgramming.Lab2.Models;
using Itmo.ObjectOrientedProgramming.Lab2.Services.Archivers;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class ArchiverRecipient : IRecipient
{
    private readonly IArchiver _archiver;

    public ArchiverRecipient(IArchiver archiver)
    {
        _archiver = archiver;
    }

    public void Receive(Message message)
    {
        _archiver.Archive(message);
    }
}