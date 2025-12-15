using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab5.Tests;

public class AccountServiceTests
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly AccountService _service;

    public AccountServiceTests()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _service = new AccountService(_accountRepository, _transactionRepository);
    }

    [Fact]
    public async Task Deposit_ShouldIncreaseBalance_WhenCalled()
    {
        // Arrange
        var accountNumber = new AccountNumber("1234567890");
        var initialBalance = new Money(1000m);
        var depositAmount = new Money(500m);

        var account = new Account(accountNumber, "hash", initialBalance);

        _accountRepository.FindByNumberAsync(accountNumber).Returns(account);

        // Act
        TransactionResult result = await _service.DepositAsync(accountNumber, depositAmount);

        // Assert
        Assert.IsType<TransactionResult.Success>(result);
        Assert.Equal(initialBalance + depositAmount, account.Balance);

        await _transactionRepository.Received(1).AddAsync(Arg.Any<Transaction>());
        await _accountRepository.Received(1).UpdateAsync(account);
    }

    [Fact]
    public async Task Withdraw_ShouldDecreaseBalance_WhenSufficientFunds()
    {
        // Arrange
        var accountNumber = new AccountNumber("1234567890");
        var initialBalance = new Money(1000m);
        var withdrawAmount = new Money(400m);

        var account = new Account(accountNumber, "hash", initialBalance);

        _accountRepository.FindByNumberAsync(accountNumber).Returns(account);

        // Act
        TransactionResult result = await _service.WithdrawAsync(accountNumber, withdrawAmount);

        // Assert
        Assert.IsType<TransactionResult.Success>(result);
        Assert.Equal(initialBalance - withdrawAmount, account.Balance);

        await _transactionRepository.Received(1).AddAsync(Arg.Any<Transaction>());
        await _accountRepository.Received(1).UpdateAsync(account);
    }

    [Fact]
    public async Task Withdraw_ShouldReturnFailure_WhenInsufficientFunds()
    {
        // Arrange
        var accountNumber = new AccountNumber("1234567890");
        var initialBalance = new Money(300m);
        var withdrawAmount = new Money(500m);

        var account = new Account(accountNumber, "hash", initialBalance);

        _accountRepository.FindByNumberAsync(accountNumber).Returns(account);

        // Act
        TransactionResult result = await _service.WithdrawAsync(accountNumber, withdrawAmount);

        // Assert
        Assert.IsType<TransactionResult.Failure>(result);

        Assert.Equal(initialBalance, account.Balance);

        await _transactionRepository.DidNotReceive().AddAsync(Arg.Any<Transaction>());
        await _accountRepository.DidNotReceive().UpdateAsync(Arg.Any<Account>());
    }
}