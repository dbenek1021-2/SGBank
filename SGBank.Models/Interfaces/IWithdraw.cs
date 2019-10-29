using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models.Responses;

namespace SGBank.Models.Interfaces
{
	public interface IWithdraw
	{
		AccountWithdrawResponse Withdraw(Account account, decimal amount);
	}
}
