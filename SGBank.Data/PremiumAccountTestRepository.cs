using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using SGBank.Models.Interfaces;

namespace SGBank.Data
{
	public class PremiumAccountTestRepository : IAccountRepository
	{
		private static Account _account = new Account
		{
			Name = "Premium Account",
			Balance = 1000M,
			AccountNumber = "54321",
			Type = AccountType.Premium
		};

		public Account LoadAccount(string AccountNumber)
		{
			if (AccountNumber == "54321")
			{
				return _account;
			}
			else
			{
				return null;
			}
		}

		public void SaveAccount(Account account)
		{
			_account = account;
		}
	}
}
