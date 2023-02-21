using SYFY_Application.DatabaseAccess;
using SYFY_Domain.data;
using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SYFY_Application.BusinessLogic
{
    public class DataManagement : IBasicEntityOperations
    {
        private TransactionExecution transactionExecutioner;
        private IDataBaseConnector dataBaseConnector;

        private static DataManagement _DataManagement_Instance = null;

        public static DataManagement GetInstance(IDataBaseConnector dataBaseConnector)
        {
            if(_DataManagement_Instance is null)
            {
                _DataManagement_Instance = new DataManagement(dataBaseConnector);
            }

            return _DataManagement_Instance;
        }

        private DataManagement(IDataBaseConnector dataBaseConnector) {
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

            foreach (BankAccount b in dataBaseConnector.GetAllBankAccounts().Values)
            {
                if(b.Guid != Guid.Empty && !b.Deleted)
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
            if (!dataBaseConnector.ExistsBankAccount(Guid.Empty))
            {
                BankAccount b = NewBankAccount("", comment: "DEFAULT_EMPTY_BANKACCOUNT");
                dataBaseConnector.SaveBankAccount(b);
            }

            return Guid.Empty;
        }

        public BankingTransaction NewBankingTransaction(
             Guid from, Guid to, int amount, DateTime transactionDate, DateTime postingDate = default,
             string comment = "", HashSet<Guid> tags = null)
        {
            return new BankingTransaction(this, from, to, amount, transactionDate, postingDate,
                comment, tags);
        }

        public BankAccount NewBankAccount(string name, string iban = "", string comment = "",
            CURRENCIES currency = CURRENCIES.EUR, ACCOUNTTYPE type = ACCOUNTTYPE.Giro)
        {
            return new BankAccount(this, name, iban, comment, currency, type);
        }

        public TransactionTag NewTransactionTag(string name, string comment = "")
        {
            return new TransactionTag(this, name, comment);
        }


        public BankingTransaction CreateEmptyBankingTransaction()
        {
            return NewBankingTransaction(GetNoneBankAccountId(), GetNoneBankAccountId(), 0, DateTime.Today);
        }

        public BankAccount CreateEmptyBankAccount()
        {
            return NewBankAccount("New Bank Account");
        }

        public TransactionTag CreateEmptyTransactionTag()
        {
            return NewTransactionTag("New Transaction Tag");
        }

        public void BankingTransactionChanged(BankingTransaction b, bool deleted = false)
        {
            throw new NotImplementedException();
        }
    }
}
