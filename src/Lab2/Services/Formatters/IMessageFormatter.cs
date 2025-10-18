using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Formatters;

public interface IMessageFormatter
{
    void Format(Message message);
}