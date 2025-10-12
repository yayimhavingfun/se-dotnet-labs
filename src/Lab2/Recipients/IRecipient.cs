using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public interface IRecipient
{
    void Receive(Message message);
}