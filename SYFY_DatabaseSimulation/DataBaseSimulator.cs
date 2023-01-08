using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
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

            _CurrentlyPerformingTransaction= false;
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

        BankAccount IDataBaseConnector.GetBankAccountByID(Guid guid)
        {
            //TODO
            return _BankAccounts[guid];
        }

        BankingTransaction IDataBaseConnector.GetBankingTransactionById(Guid guid)
        {
            //TODO
            return _Transactions[guid];
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
            if (_BankAccounts.ContainsKey(bankAccount.Guid))
            {
                // update
                if (_BankAccounts.ContainsKey(bankAccount.Guid))
                {
                    _BankAccounts[bankAccount.Guid] = bankAccount;
                }
            }
            else
            {
                // save newly
                bankAccount.Guid = NewGuid();
                
                if (!_BankAccounts.ContainsKey(bankAccount.Guid))
                {
                    _BankAccounts.Add(bankAccount.Guid, bankAccount);
                }
            }

            return _BankAccounts[bankAccount.Guid];

        }

        BankingTransaction IDataBaseConnector.SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            //TODO

            if (_Transactions.ContainsKey(bankingTransaction.Guid))
            {
                // update
                    _Transactions[bankingTransaction.Guid] = bankingTransaction;
            }
            else
            {
                // save newly
                bankingTransaction.Guid = NewGuid();

                if (!_Transactions.ContainsKey(bankingTransaction.Guid))
                {
                    _Transactions.Add(bankingTransaction.Guid, bankingTransaction);
                }
            }
            
            return _Transactions[bankingTransaction.Guid];
        }

        TransactionTag IDataBaseConnector.SaveTransactionTag(TransactionTag transactionTag)
        {
            transactionTag.Guid = NewGuid();

            if (!_TransactionTags.ContainsKey(transactionTag.Guid))
            {
                _TransactionTags.Add(transactionTag.Guid, transactionTag);
                return transactionTag;
            }

            throw new Exception("Already in DB!");
        }

       
        void IDataBaseConnector.StartDBTransaction()
        {
            _CurrentlyPerformingTransaction = true;
            Console.WriteLine("Transaction started...");
            //TODO
        }
       
    }
}
