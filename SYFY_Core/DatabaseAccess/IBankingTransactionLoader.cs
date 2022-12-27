using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Core.DatabaseAccess
{
    public interface IBankingTransactionLoader
    {
        public Dictionary<Guid, BankingTransaction> getAllBankingTransactions();

        public BankingTransaction GetBankingTransactionById(Guid guid);

        public void SaveBankingTransaction(BankingTransaction bankingTransaction);


    
        //public BankingTransaction CreateNewBankingTransaction();

    }
}
