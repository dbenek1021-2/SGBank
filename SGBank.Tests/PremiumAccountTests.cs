using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;

namespace SGBank.Tests
{
	[TestFixture]
	public class PremiumAccountTests
	{
		[TestCase("54321", true)]
		public void CanLoadBasicAccountTestData(string name, bool expectedResult)
		{
			AccountManager manager = AccountManagerFactory.Create();

			AccountLookupResponse response = manager.LookupAccount(name);

			Assert.IsNotNull(response.Account);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(name, response.Account.AccountNumber);
		}

		[TestCase("54321", "Premium Account", 100, AccountType.Free, 100, false)]
		[TestCase("54321", "Premium Account", 100, AccountType.Premium, -600, false)]
		[TestCase("54321", "Premium Account", 100, AccountType.Premium, 250, true)]
		public void PremiumAccountDepositRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, bool expectedResult)
		{
			IDeposit deposit = new NoLimitDepositRule();

			Account account = new Account();

			account.AccountNumber = accountNumber;
			account.Name = name;
			account.Balance = balance;
			account.Type = accountType;

			AccountDepositResponse response = deposit.Deposit(account, amount);
			Assert.AreEqual(expectedResult, response.Success);

		}

		[TestCase("54321", "Premium Account", 1500, AccountType.Free, -1000, 1500, false)]
		[TestCase("54321", "Premium Account", 1000, AccountType.Premium, -1600, -610, true)]
		public void PremiumAccountWithdrawRuleTest(string accountNumber, string name, decimal balance, AccountType accountType, decimal amount, decimal newBalance, bool expectedResult)
		{
			IWithdraw withdraw = new PremiumAccountWithdrawRule();

			Account account = new Account();

			account.AccountNumber = accountNumber;
			account.Name = name;
			account.Balance = balance;
			account.Type = accountType;

			AccountWithdrawResponse response = withdraw.Withdraw(account, amount);
			Assert.AreEqual(expectedResult, response.Success);

			if (response.Success == true)
			{
				Assert.AreEqual(newBalance, response.Account.Balance);
			}
		}
	}
}
