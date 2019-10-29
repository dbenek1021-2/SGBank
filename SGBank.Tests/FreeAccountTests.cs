﻿using NUnit.Framework;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Tests
{
	[TestFixture]
	public class FreeAccountTests
	{
		[TestCase("12345", true)]
		public void CanLoadFreeAccountTestData(string name, bool expectedResult)
		{
			AccountManager manager = AccountManagerFactory.Create();

			AccountLookupResponse response = manager.LookupAccount(name);

			Assert.IsNotNull(response.Account);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(name, response.Account.AccountNumber);
		}

		[TestCase("12345", "Free Account", 100, AccountType.Free, 250, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Free, -100, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Basic, 50, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Free, 50, true)]
		public void FreeAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
		{
			IDeposit deposit = new FreeAccountDepositRule();

			Account account = new Account();

			account.AccountNumber = accountNumber;
			account.Name = name;
			account.Balance = balance;
			account.Type = accountType;

			AccountDepositResponse response = deposit.Deposit(account, amount);
			Assert.AreEqual(expectedResult, response.Success);
		}

		[TestCase("12345", "Free Account", 100, AccountType.Free, 80, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Free, -100, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Basic, 50, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Free, -100, false)]
		[TestCase("12345", "Free Account", 100, AccountType.Free, -50, true)]
		public void FreeAccountWithdrawlRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
		{
			IWithdraw withdraw = new FreeAccountWithdrawRule();

			Account account = new Account();

			account.AccountNumber = accountNumber;
			account.Name = name;
			account.Balance = balance;
			account.Type = accountType;

			AccountWithdrawResponse response = withdraw.Withdraw(account, amount);
			Assert.AreEqual(expectedResult, response.Success);
		}
	}
}
