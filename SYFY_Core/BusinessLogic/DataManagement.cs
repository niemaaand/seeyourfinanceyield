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
        //private Dictionary<Guid, BankingTransaction> bankingTransactionsCopy;
        //private Dictionary<Guid, BankAccount> bankAccountsCopy;

        private Dictionary<Guid, BankingTransaction> bankingTransactionsOriginal;
        private Dictionary<Guid, BankAccount> bankAccountsOriginal;



        public DataManagement(IDataBaseConnector dataBaseConnector) {
            this.dataBaseConnector = dataBaseConnector;
            transactionExecutioner = new TransactionExecution(dataBaseConnector);

            /*bankingTransactionsCopy = new Dictionary<Guid, BankingTransaction>();
            bankingTransactionsCopy = GetAllBankingTransactions();

            bankAccountsCopy = new Dictionary<Guid, BankAccount>();
            bankAccountsCopy = GetAllBankAccounts();
            */
            bankingTransactionsOriginal = new Dictionary<Guid, BankingTransaction>();
            bankingTransactionsOriginal = dataBaseConnector.GetAllBankingTransactions();

            bankAccountsOriginal= new Dictionary<Guid, BankAccount>();
            bankAccountsOriginal = dataBaseConnector.GetAllBankAccounts();
        }

        public BankingTransaction SaveBankingTransaction(BankingTransaction transaction)
        {
            if(!bankingTransactionsOriginal.Keys.Contains(transaction.Guid)){
                transaction = transactionExecutioner.SaveBankingTransaction(transaction);
                //bankingTransactionsCopy.Add(transaction.Guid, (BankingTransaction)transaction.Clone());
            }
            else
            {
                transactionExecutioner.AlterBankingTransaction(bankingTransactionsOriginal[transaction.Guid], transaction);
                //bankingTransactionsCopy[transaction.Guid] = (BankingTransaction)transaction.Clone();
            }

            //bankAccountsCopy[transaction.ToBankAccount] = GetBankAccountByID(transaction.ToBankAccount);
            //bankAccountsCopy[transaction.FromBankAccount] = GetBankAccountByID(transaction.FromBankAccount);

            return transaction;
        }

        /// <summary>
        /// Saves or updates bank account. 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BankAccount SaveBankAccount(BankAccount bankAccount)
        {
            /*if (bankAccountsCopy.Keys.Contains(bankAccount.Guid)) 
            {
                dataBaseConnector.UpdateBankAccount((BankAccount)bankAccount.Clone());
                //bankAccountsCopy[bankAccount.Guid] = (BankAccount)bankAccount.Clone();
            }else
            {
                bankAccount = dataBaseConnector.SaveBankAccount(bankAccount);
                //bankAccountsCopy.Add(bankAccount.Guid, (BankAccount)bankAccount.Clone());
            }*/

            bankAccount = dataBaseConnector.SaveBankAccount(bankAccount);
            return (BankAccount)bankAccount.Clone();
        }

        public void AlterBankingTransaction(BankingTransaction oldBankingTransaction, BankingTransaction newBankingTransaction)
        {
            transactionExecutioner.AlterBankingTransaction(oldBankingTransaction, newBankingTransaction);
        }

        public Dictionary<Guid, BankAccount> GetAllBankAccounts()
        {
            /*if(bankAccountsCopy.Count == 0)
            {
                foreach (BankAccount b in dataBaseConnector.GetAllBankAccounts().Values)
                {
                    bankAccountsCopy.Add(b.Guid, (BankAccount)b.Clone());
                }
            }            */

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
            /*if(bankingTransactionsCopy.Count == 0)
            {
                // return copy
                foreach (BankingTransaction b in dataBaseConnector.GetAllBankingTransactions().Values)
                {
                    bankingTransactionsCopy.Add(b.Guid, (BankingTransaction)b.Clone());
                }
            }            */

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

        public void UpdateBankingTransaction(BankingTransaction transaction)
        {
            //TODO
            throw new NotImplementedException();
            //transactionExecutioner.AlterBankingTransaction(bankingTransactionsCopy[transaction.Guid], transaction);
        }

        public BankingTransaction GetBankingTransactionByID(Guid guid)
        {
            return (BankingTransaction)dataBaseConnector.GetBankingTransactionById(guid).Clone();
        }
    }
}
