using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using SGBank.Models.Interfaces;

namespace SGBank.Data
{
    public class FileAccountTestRepository : IAccountRepository
    {

		string _path = ConfigurationManager.AppSettings["FilePath"].ToString();
		//private const string _path = @"C:\Users\DaniB\Documents\SoftwareGuild\bitbucket\dani-benek-individual-work\SGBank\Accounts.txt";

		//public FileAccountTestRepository(string path)
		//{
		//	path = _path;
		//}

		//public FileAccountTestRepository()
		//{
		//}

		public List<Account> ReadAccounts()
        {
            List<Account> accounts = new List<Account>();
            string[] allLines = File.ReadAllLines(_path);

            for (int i = 1; i < allLines.Length; i++)
            {
                Account account = new Account();
				string[] filelines = allLines[i].Split(',');

				account.AccountNumber = filelines[0];
				account.Name = filelines[1];
                account.Balance = decimal.Parse(filelines[2]);
				if (filelines[3] == "F")
				{
					account.Type = AccountType.Free;
				}
				else if (filelines[3] == "B")
				{
					account.Type = AccountType.Basic;
				}
				else if (filelines[3] == "P")
				{
					account.Type = AccountType.Premium;
				}
				else
				{
					throw new Exception("Account type is not valid. Contact IT.");
				}
				accounts.Add(account);
            }
            return accounts;
        }

		public Account LoadAccount(string AccountNumber)
        {
            var accounts = ReadAccounts();
            foreach(Account account in accounts)
            {
                if (AccountNumber == account.AccountNumber)
                {
                    return account;
                }
            }
            return null;
        }

        public void SaveAccount(Account account)
        {
			var accounts = ReadAccounts();
			var index = accounts.FindIndex(i => i.AccountNumber == account.AccountNumber);

			accounts[index] = account;

			ExportToFile(accounts);
		}

		private void ExportToFile(List<Account> accounts)
		{
			if (File.Exists(_path))
				File.Delete(_path);

			using (StreamWriter sr = new StreamWriter(_path))
			{
				sr.WriteLine("AccountNumber,Name,Balance,Type");
				foreach (var account in accounts)
				{
					sr.WriteLine(LineFormatForAccounts(account));
				}
			}
		}

		private string LineFormatForAccounts(Account account)
		{
			string accountType;
			if (account.Type == AccountType.Free)
			{
				accountType = "F";
			}
			else if (account.Type == AccountType.Basic)
			{
				accountType = "B";
			}
			else if (account.Type == AccountType.Premium)
			{
				accountType = "P";
			}
			else
			{
				throw new Exception("Account type is not valid. Contact IT.");
			}
			return string.Format("{0},{1},{2},{3}", account.AccountNumber,
					account.Name, account.Balance.ToString(), accountType);
		}
	}
}