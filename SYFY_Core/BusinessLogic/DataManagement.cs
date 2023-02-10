using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYFY_Application.BusinessLogic
{
    public class DataManagement
    {
        private TransactionExecution transactionExecutioner;
        private IDataBaseConnector dataBaseConnector;

        //private Dictionary<Guid, BankingTransaction> bankingTransactionsOriginal;
        //private Dictionary<Guid, BankAccount> bankAccountsOriginal;


        public DataManagement(IDataBaseConnector dataBaseConnector) {
            this.dataBaseConnector = dataBaseConnector;
            transactionExecutioner = new TransactionExecution(dataBaseConnector);
            
            /*bankingTransactionsOriginal = new Dictionary<Guid, BankingTransaction>();
            bankingTransactionsOriginal = dataBaseConnector.GetAllBankingTransactions();

            bankAccountsOriginal= new Dictionary<Guid, BankAccount>();
            bankAccountsOriginal = dataBaseConnector.GetAllBankAccounts();*/
        }

        public BankingTransaction SaveBankingTransaction(BankingTransaction transaction)
        {
            if (!dataBaseConnector.ExistsBankingTransaction(transaction.Guid)){
                transaction = transactionExecutioner.SaveBankingTransaction(transaction);
            }
            else
            {
                transaction = transactionExecutioner.AlterBankingTransaction(GetBankingTransactionByID(transaction.Guid), transaction);
            }

            return (BankingTransaction)transaction.Clone();
        }

        /// <summary>
        /// Saves or updates bank account. 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BankAccount SaveBankAccount(BankAccount bankAccount)
        {
            bankAccount = dataBaseConnector.SaveBankAccount(bankAccount);
            return (BankAccount)bankAccount.Clone();
        }


        public Dictionary<Guid, BankAccount> GetAllBankAccounts()
        {            
            // TODO
            // ?only return copy of bank accounts?
            Dictionary<Guid, BankAccount> bankAccountsCopy = new Dictionary<Guid, BankAccount>();

            foreach (BankAccount b in dataBaseConnector.GetAllBankAccounts().Values)
            {
                if(b.Guid != Guid.Empty)
                {
                    bankAccountsCopy.Add(b.Guid, (BankAccount)b.Clone());
                }
            }

            return bankAccountsCopy;

        }

        public Dictionary<Guid, BankingTransaction> GetAllBankingTransactions()
        {
            Dictionary<Guid, BankingTransaction> bankingTransactionsCopy = new Dictionary<Guid, BankingTransaction>();

            // return copy
            foreach (BankingTransaction b in dataBaseConnector.GetAllBankingTransactions().Values)
            {
                if(b.Guid != Guid.Empty)
                {
                    bankingTransactionsCopy.Add(b.Guid, (BankingTransaction)b.Clone());
                }
            }

            return bankingTransactionsCopy;
        }

        public BankAccount GetBankAccountByID(Guid id)
        {
            //TODO
            return (BankAccount)dataBaseConnector.GetBankAccountByID(id).Clone();
        }

        public Dictionary<Guid, TransactionTag> GetAllTransactionTags()
        {
            // TODO
            Dictionary<Guid, TransactionTag> tagsCopy = new Dictionary<Guid, TransactionTag>();
            foreach (TransactionTag t in dataBaseConnector.GetAllTransactionTags().Values)
            {
                if (t.Guid != Guid.Empty)
                {
                    tagsCopy.Add(t.Guid, (TransactionTag)t.Clone());
                }
            }

            return tagsCopy;
        }
             

        public BankingTransaction GetBankingTransactionByID(Guid guid)
        {
            return (BankingTransaction)dataBaseConnector.GetBankingTransactionById(guid).Clone();
        }

        public TransactionTag SaveTransactionTag(TransactionTag transactionTag)
        {
            // TODO
            transactionTag = dataBaseConnector.SaveTransactionTag(transactionTag);
            return (TransactionTag)transactionTag.Clone();
        }

        public void DeleteTransactionTag(TransactionTag tag)
        {
            dataBaseConnector.DeleteTransactionTag(tag);
        }

        public void DeleteBankAccount(BankAccount bankAccount)
        {
            dataBaseConnector.DeleteBankAccount(bankAccount);
        }

        public void DeleteBankingTransaction(BankingTransaction transaction)
        {
            transactionExecutioner.DeleteTransaction(transaction);
        }

        private Guid GetNoneBankAccountId()
        {
            return dataBaseConnector.GetDefaultBankAccount().Guid;

        }

        public BankingTransaction CreateEmptyBankingTransaction()
        {
            return new BankingTransaction(GetNoneBankAccountId(), GetNoneBankAccountId(), 0, DateTime.Today);
        }

        public BankAccount CreateEmptyBankAccount()
        {
            return new BankAccount("New Bank Account");
        }
    }
}
