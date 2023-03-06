using SYFY_Application.DatabaseAccess;
using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYFY_Application.BusinessLogic
{
    public class DataManagement
    {
        private TransactionExecution transactionExecutioner;
        private IDataBaseConnectorAdapter dataBaseConnector;

        public DataManagement(IDataBaseConnectorAdapter dataBaseConnector) {
            this.dataBaseConnector = dataBaseConnector;
            transactionExecutioner = new TransactionExecution(dataBaseConnector);            
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

            Guid defaultId = GetNoneBankAccountId();

            foreach (BankAccount b in dataBaseConnector.GetAllBankAccounts().Values)
            {
                if(b.Guid != Guid.Empty && !b.Deleted && !b.Guid.Equals(defaultId))
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
                if(b.Guid != Guid.Empty && !b.Deleted)
                {
                    bankingTransactionsCopy.Add(b.Guid, (BankingTransaction)b.Clone());
                }
            }

            return bankingTransactionsCopy;
        }

        public BankAccount GetBankAccountByID(Guid id)
        {
            //TODO
            BankAccount b = (BankAccount)dataBaseConnector.GetBankAccountByID(id);
            if (b.Deleted)
            {
                throw new NotImplementedException();
            }

            return (BankAccount)b.Clone();
        }

        public Dictionary<Guid, TransactionTag> GetAllTransactionTags()
        {
            // TODO
            Dictionary<Guid, TransactionTag> tagsCopy = new Dictionary<Guid, TransactionTag>();
            foreach (TransactionTag t in dataBaseConnector.GetAllTransactionTags().Values)
            {
                if (t.Guid != Guid.Empty && !t.Deleted)
                {
                    tagsCopy.Add(t.Guid, (TransactionTag)t.Clone());
                }
            }

            return tagsCopy;
        }
             

        public BankingTransaction GetBankingTransactionByID(Guid guid)
        {
            BankingTransaction b = (BankingTransaction)dataBaseConnector.GetBankingTransactionById(guid);
            return b.Deleted ? throw new NotImplementedException() : (BankingTransaction)b.Clone();
        }

        public TransactionTag GetTransactionTagByID(Guid guid)
        {
            TransactionTag t = (TransactionTag)dataBaseConnector.GetTransactionTagById(guid);
            return t.Deleted ? throw new NotImplementedException() : (TransactionTag)t.Clone();
        }

        public TransactionTag SaveTransactionTag(TransactionTag transactionTag)
        {
            // TODO
            transactionTag = dataBaseConnector.SaveTransactionTag(transactionTag);
            return (TransactionTag)transactionTag.Clone();
        }

        public void DeleteTransactionTag(TransactionTag tag)
        {
            if (!dataBaseConnector.ExistsTransactionTag(tag.Guid))
            {
                dataBaseConnector.DeleteTransactionTag(tag);
            }
        }

        public void DeleteBankAccount(BankAccount bankAccount)
        {
            if (!dataBaseConnector.ExistsBankAccount(bankAccount.Guid))
            {
                dataBaseConnector.DeleteBankAccount(bankAccount);
            }
        }

        public void DeleteBankingTransaction(BankingTransaction transaction)
        {
            if (dataBaseConnector.ExistsBankingTransaction(transaction.Guid))
            {
                transactionExecutioner.DeleteTransaction(transaction);
            }
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

        public TransactionTag CreateEmptyTransactionTag()
        {
            return new TransactionTag("New Transaction Tag");
        }

        public bool ExistsBankingTransaction(Guid guid)
        {
            return dataBaseConnector.ExistsBankingTransaction(guid);
        }

        public bool ExistsBankAccount(Guid guid)
        {
           return dataBaseConnector.ExistsBankAccount(guid);
        }

        public bool ExistsTransactionTag(Guid guid)
        {
           return dataBaseConnector.ExistsTransactionTag(guid);
        }

        public void RemoveTagFromTransaction(BankingTransaction transaction, TransactionTag tag)
        {
            if(transaction.TransactionTags.Contains(tag.Guid))
            {
                transaction.TransactionTags.Remove(tag.Guid);
            }
        }

        public void AddTagToTransaction(BankingTransaction transaction, TransactionTag tag)
        {
            if (ExistsTransactionTag(tag.Guid))
            {
                transaction.TransactionTags.Add(tag.Guid);
            }
        }
    }
}
