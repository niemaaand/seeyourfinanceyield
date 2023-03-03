using SYFY_Application.DatabaseAccess;
using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Plugin_DatabaseSimulation
{
    public class DataBaseSimulator : IDataBaseConnector
    {

        private Dictionary<Guid, BankingTransaction> _Transactions;
        private Dictionary<Guid, BankAccount> _BankAccounts;
        private Dictionary<Guid, TransactionTag> _TransactionTags;
        private bool _CurrentlyPerformingTransaction;

        public DataBaseSimulator()
        {
            _Transactions = new Dictionary<Guid, BankingTransaction>();
            _BankAccounts = new Dictionary<Guid, BankAccount>();
            _TransactionTags = new Dictionary<Guid, TransactionTag>();

            _CurrentlyPerformingTransaction = false;
        }

        bool IDataBaseConnector.ExistsBankingTransaction(Guid guid)
        {
            return _Transactions.ContainsKey(guid) ? true : false;
        }

        void IDataBaseConnector.Commit()
        {
            //TODO
            if (_CurrentlyPerformingTransaction == false)
            {
                throw new NullReferenceException("No data base transaction running currently!");
            }

            _CurrentlyPerformingTransaction = false;
            Console.WriteLine("Commit Transaction!");
        }


        Dictionary<Guid, BankAccount> IDataBaseConnector.GetAllBankAccounts()
        {
            // load bank accounts from data base


            //TODO
            return _BankAccounts;
        }

        Dictionary<Guid, BankingTransaction> IDataBaseConnector.GetAllBankingTransactions()
        {
            //TODO
            return _Transactions;
        }

        Dictionary<Guid, TransactionTag> IDataBaseConnector.GetAllTransactionTags()
        {
            //TODO
            return _TransactionTags;
        }

        public BankAccount GetBankAccountByID(Guid guid)
        {
            //TODO
            return _BankAccounts.ContainsKey(guid)?_BankAccounts[guid]:null;
        }

        public BankingTransaction GetBankingTransactionById(Guid guid)
        {
            //TODO
            return _Transactions.ContainsKey(guid)?_Transactions[guid]:null;
        }

        public TransactionTag GetTransactionTagById(Guid guid)
        {
            //TODO
            return _TransactionTags.ContainsKey(guid) ? _TransactionTags[guid] : null;
        }


        private Guid NewGuid()
        {
            //TODO
            Guid g = Guid.NewGuid();
            return g;
        }

        void IDataBaseConnector.Rollback()
        {
            //TODO
            _CurrentlyPerformingTransaction = false;
            Console.WriteLine("Rollback!");
        }

        BankAccount IDataBaseConnector.SaveBankAccount(BankAccount bankAccount)
        {
            //TODO
            if (SaveData(bankAccount, GetBankAccountByID(bankAccount.Guid), _TransactionTags.Keys))
            {
                // save newly
                Guid id;

                do
                {
                    id = NewGuid();
                } while (_TransactionTags.ContainsKey(id));

                bankAccount.Guid = id;
                _BankAccounts.Add(bankAccount.Guid, bankAccount);
            }
            else
            {
                // update
                _BankAccounts[bankAccount.Guid] = bankAccount;
            }
        

            return _BankAccounts[bankAccount.Guid];

        }

        BankingTransaction IDataBaseConnector.SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            //TODO

            if (SaveData(bankingTransaction, GetBankingTransactionById(bankingTransaction.Guid), _Transactions.Keys))
            {
                // save newly
                Guid id;

                do
                {
                    id = NewGuid();
                } while (_TransactionTags.ContainsKey(id));

                bankingTransaction.Guid = id;
                _Transactions.Add(bankingTransaction.Guid, bankingTransaction);
            }
            else
            {
                // update
                _Transactions[bankingTransaction.Guid] = bankingTransaction;
            }

            return _Transactions[bankingTransaction.Guid];
        }

       

        TransactionTag IDataBaseConnector.SaveTransactionTag(TransactionTag transactionTag)
        {
            if(SaveData(transactionTag, GetTransactionTagById(transactionTag.Guid), _TransactionTags.Keys))
            {
                // save newly
                Guid id; 

                do
                {
                    id = NewGuid();
                } while (_TransactionTags.ContainsKey(id));

                transactionTag.Guid = id;
                _TransactionTags.Add(transactionTag.Guid, transactionTag);
            }
            else
            {
                // update
                _TransactionTags[transactionTag.Guid] = transactionTag;
            }

            return _TransactionTags[transactionTag.Guid];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="savedData"></param>
        /// <param name="keys"></param>
        /// <returns>true: save newly, false: update</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private bool SaveData(DeleteableData data, DeleteableData savedData, ICollection<Guid> keys)
        {
            if(savedData == null)
            {
                return true;
            }
            else if(savedData.Deleted || data.Deleted)
            {
                throw new InvalidOperationException("Data is deleted and can therefore not be changed.");
            }
            else
            {
                return false;
            }

        }

        public BankAccount GetDefaultBankAccount()
        {
            Guid defaultId = new Guid("F4635B58-8D25-40A1-95B3-C9CDB424205A");

            if (!_BankAccounts.ContainsKey(defaultId))
            {
                _BankAccounts.Add(defaultId, new BankAccount("", comment: "DEFAULT_EMPTY_BANKACCOUNT") { Guid = defaultId});
            }

            return _BankAccounts[defaultId];
        }


        void IDataBaseConnector.StartDBTransaction()
        {
            _CurrentlyPerformingTransaction = true;
            Console.WriteLine("Transaction started...");
            //TODO
        }

        public void DeleteBankingTransaction(BankingTransaction transaction)
        {
            if (_Transactions.ContainsKey(transaction.Guid))
            {
                _Transactions[transaction.Guid].Delete();
            }
        }

        public void DeleteTransactionTag(TransactionTag tag)
        {
            if (_TransactionTags.ContainsKey(tag.Guid))
            {
                _TransactionTags[tag.Guid].Delete();
            }
        }

        public void DeleteBankAccount(BankAccount bankAccount)
        {
            if (_BankAccounts.ContainsKey(bankAccount.Guid))
            {
                _BankAccounts[bankAccount.Guid].Delete();
            }
        }

        bool IDataBaseConnector.ExistsTransactionTag(Guid guid)
        {
            if (_TransactionTags.ContainsKey(guid))
            {
                return true;
            }

            return false;
        }

        bool IDataBaseConnector.ExistsBankAccount(Guid guid)
        {
            if (_BankAccounts.ContainsKey(guid))
            {
                return true;
            }

            return false;
        }
    }
}
