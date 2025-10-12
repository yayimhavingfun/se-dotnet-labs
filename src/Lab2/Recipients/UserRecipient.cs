using Itmo.ObjectOrientedProgramming.Lab2.Models;

namespace Itmo.ObjectOrientedProgramming.Lab2.Recipients;

public class UserRecipient : IRecipient
{
    private readonly User _user;

    public UserRecipient(User user)
    {
        _user = user;
    }

    public void Receive(Message message) => _user.ReceiveMessage(message);
}