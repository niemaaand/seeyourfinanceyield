using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SYFY_Core.DatabaseAccess;
using SYFY_Model.model;

namespace SYFY_Plugin_DatabaseSimulation
{
    public class BankingTransactionLoadSimulator : IBankingTransactionLoader
    {
       /* BankingTransaction IBankingTransactionLoader.CreateNewBankingTransaction()
        {
            throw new NotImplementedException();
        }*/

        Dictionary<Guid, BankingTransaction> IBankingTransactionLoader.getAllBankingTransactions()
        {
            throw new NotImplementedException();
        }

        BankingTransaction IBankingTransactionLoader.GetBankingTransactionById(Guid guid)
        {
            throw new NotImplementedException();
        }

        void IBankingTransactionLoader.SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            throw new NotImplementedException();
        }
    }
}
