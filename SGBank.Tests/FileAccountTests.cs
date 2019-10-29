using System;
using System.Text;
using System.Threading.Tasks;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using NUnit.Framework;
using SGBank.Models;
using System.IO;
using SGBank.Data;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace SGBank.Tests
{
	[TestFixture]
	public class FileAccountTests
	{
		string _pathForTest = ConfigurationManager.AppSettings["FilePath"].ToString();
		string _originalPath = ConfigurationManager.AppSettings["FileSeed"].ToString();
		//private const string _pathForTest = @"C:\Users\DaniB\Documents\SoftwareGuild\bitbucket\dani-benek-individual-work\SGBank\Accounts.txt";
		//private const string _originalPath = @"C:\Users\DaniB\Documents\SoftwareGuild\bitbucket\dani-benek-individual-work\SGBank\AccountsTestSeed.txt";

		[SetUp]
		public void Setup()
		{
			if (File.Exists(_pathForTest))
			{
				File.Delete(_pathForTest);
			}

			File.Copy(_originalPath, _pathForTest);
		}

		[Test]
		public void CanLoadFileAccounts()
		{
			FileAccountTestRepository repo = new FileAccountTestRepository();

			List<Account> accounts = repo.ReadAccounts();

			Assert.AreEqual(3, accounts.Count());

			Account check = accounts[0];

			Assert.AreEqual("11111", check.AccountNumber);
			Assert.AreEqual("Free Customer", check.Name);
			Assert.AreEqual(100, check.Balance);
			Assert.AreEqual(AccountType.Free, check.Type);
		}

		[Test]
		public void CanUpdateAccount()
		{
			FileAccountTestRepository repo = new FileAccountTestRepository();

			List<Account> accounts = repo.ReadAccounts();

			Account editedAccount = accounts[2];
			editedAccount.Balance = 1500M;

			repo.SaveAccount(editedAccount);

			Assert.AreEqual(3, accounts.Count());

			Account check = accounts[2];

			Assert.AreEqual("33333", check.AccountNumber);
			Assert.AreEqual("Premium Customer", check.Name);
			Assert.AreEqual(1500M, check.Balance);
			Assert.AreEqual(AccountType.Premium, check.Type);
		}
	}
}
