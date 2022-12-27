using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Application.DatabaseAccess
{
    public interface IDataManager
    {
        public Guid NewGuid();

        public void StartDBTransaction();

        public void Commit();

        public void Rollback();

        public void UpdateBankAccount(BankAccount bankAccount);

        public Dictionary<Guid, BankingTransaction> getAllBankingTransactions();

        public BankingTransaction GetBankingTransactionById(Guid guid);

        public void SaveBankingTransaction(BankingTransaction bankingTransaction);

        public void UpdateBankingTransaction(BankingTransaction bankingTransaction);

        public BankingTransaction CreateNewBankingTransaction(DateTime transactionDate, DateTime postingDate,
            Guid fromBankAccount, Guid toBankAccount, decimal amount, string comment, Dictionary<Guid, TransactionTag> tags);


        public Dictionary<Guid, BankAccount> getAllBankAccounts();

        public BankAccount getBankAccountByID(Guid guid);

        public void saveBankAccount(BankAccount bankAccount);

        public BankAccount createNewBankAccount(string name, string iban, string comment,
            CURRENCIES currency, ACCOUNTTYPE accounttype);


    }
}
