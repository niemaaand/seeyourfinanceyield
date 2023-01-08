using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Application.BusinessLogic
{
    public class DataManagement
    {
        private TransactionExecution transactionExecutioner;
        private IDataBaseConnector dataBaseConnector;

        private Dictionary<Guid, BankingTransaction> bankingTransactionsOriginal;
        private Dictionary<Guid, BankAccount> bankAccountsOriginal;


        public DataManagement(IDataBaseConnector dataBaseConnector) {
            this.dataBaseConnector = dataBaseConnector;
            transactionExecutioner = new TransactionExecution(dataBaseConnector);
            bankingTransactionsOriginal = new Dictionary<Guid, BankingTransaction>();
            bankingTransactionsOriginal = dataBaseConnector.GetAllBankingTransactions();

            bankAccountsOriginal= new Dictionary<Guid, BankAccount>();
            bankAccountsOriginal = dataBaseConnector.GetAllBankAccounts();
        }

        public BankingTransaction SaveBankingTransaction(BankingTransaction transaction)
        {
            if (!bankingTransactionsOriginal.Keys.Contains(transaction.Guid)){
                transaction = transactionExecutioner.SaveBankingTransaction(transaction);
            }
            else
            {
                transaction = transactionExecutioner.AlterBankingTransaction(bankingTransactionsOriginal[transaction.Guid], transaction);
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

       /* public void AlterBankingTransaction(BankingTransaction oldBankingTransaction, BankingTransaction newBankingTransaction)
        {
            transactionExecutioner.AlterBankingTransaction(oldBankingTransaction, newBankingTransaction);
        }*/

        public Dictionary<Guid, BankAccount> GetAllBankAccounts()
        {            
            // TODO
            // ?only return copy of bank accounts?
            Dictionary<Guid, BankAccount> bankAccountsCopy = new Dictionary<Guid, BankAccount>();

            foreach (BankAccount b in dataBaseConnector.GetAllBankAccounts().Values)
            {
                bankAccountsCopy.Add(b.Guid, (BankAccount)b.Clone());
            }

            return bankAccountsCopy;

        }

        public Dictionary<Guid, BankingTransaction> GetAllBankingTransactions()
        {
            Dictionary<Guid, BankingTransaction> bankingTransactionsCopy = new Dictionary<Guid, BankingTransaction>();

            // return copy
            foreach (BankingTransaction b in dataBaseConnector.GetAllBankingTransactions().Values)
            {
                bankingTransactionsCopy.Add(b.Guid, (BankingTransaction)b.Clone());
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
            return dataBaseConnector.GetAllTransactionTags();
        }

        /*public void UpdateBankingTransaction(BankingTransaction transaction)
        {
            //TODO
            throw new NotImplementedException();
            //transactionExecutioner.AlterBankingTransaction(bankingTransactionsCopy[transaction.Guid], transaction);
        }*/

        public BankingTransaction GetBankingTransactionByID(Guid guid)
        {
            return (BankingTransaction)dataBaseConnector.GetBankingTransactionById(guid).Clone();
        }

        public TransactionTag SaveTransactionTag(TransactionTag transactionTag)
        {
            // TODO
            return dataBaseConnector.SaveTransactionTag(transactionTag);
        }
    }
}
