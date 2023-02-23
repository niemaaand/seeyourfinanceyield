using SYFY_Domain.model;
using System;
using System.Collections.Generic;

namespace SYFY_Application.DatabaseAccess
{
    public interface IBankingTransactionLoader
    {
        public Dictionary<Guid, BankingTransaction> getAllBankingTransactions();

        public BankingTransaction GetBankingTransactionById(Guid guid);

        public void SaveBankingTransaction(BankingTransaction bankingTransaction);


    
        //public BankingTransaction CreateNewBankingTransaction();

    }
}
