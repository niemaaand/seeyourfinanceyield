using SYFY_Application.BusinessLogic;
using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace SYFY_Adapter_GUI
{
    public class MainViewModel
    {

        public DataManagement dataManager;

        public ObservableCollection<BankAccount> bankAccounts { get; set; }
        public ObservableCollection<BankingTransaction> bankingTransactions { get; set; }
        public ObservableCollection<TransactionTag> transactionTags { get; set; }
        public ObservableCollection<TransactionTag> currentTransactionTags { get; set; }

        private List<BankingTransaction> changedTransactions;
        private List<BankAccount> changedBankAccounts;
        private List<TransactionTag> changedTransactionTags;


        private List<BankingTransaction> deletedTransactions;
        private List<BankAccount> deletedBankAccounts;
        private List<TransactionTag> deletedTransactionTags;

        public MainViewModel(DataManagement dataManager)
        {
            this.dataManager = dataManager;
            changedTransactions = new List<BankingTransaction>();
            changedBankAccounts = new List<BankAccount>();
            changedTransactionTags = new List<TransactionTag>();

            deletedTransactions = new List<BankingTransaction>();
            deletedBankAccounts = new List<BankAccount>();
            deletedTransactionTags = new List<TransactionTag>();

            bankAccounts = new ObservableCollection<BankAccount>();
            LoadBankAccounts();

            bankingTransactions = new ObservableCollection<BankingTransaction>();
            LoadBankingTransactions();            

            transactionTags = new ObservableCollection<TransactionTag>();
            LoadTransactionTags();

            currentTransactionTags = new ObservableCollection<TransactionTag>();


            bankAccounts.CollectionChanged += On_BankAccountsChanged;

        }



        private void On_BankAccountsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void BTN_NewTransaction_Click(object? sender, EventArgs e)
        {

            BankingTransaction transaction = dataManager.CreateEmptyBankingTransaction();
            bankingTransactions.Insert(0, transaction);
            DataChanged(transaction);

        }

        public void BTN_NewBankAccount_Click(object? sender, EventArgs e)
        {
            //TODO
            BankAccount b = dataManager.CreateEmptyBankAccount();
            bankAccounts.Insert(0, b);
            DataChanged(b);
        }

        public void BTN_NewTransactionTag_Click(object sender, EventArgs e)
        {
            //TODO
            TransactionTag tag = dataManager.CreateEmptyTransactionTag();
            transactionTags.Insert(0, tag);
            DataChanged(tag);
        }

        public void DataChanged(DeleteableData d, bool deleted = false)
        {
            //TODO       
            //d.DataHandler.AddChanged();

            if(d is BankAccount)
            {
                if (!deleted)
                {
                    changedBankAccounts.Add((BankAccount)d);
                }
                else
                {
                    //deletedBankAccounts.Add((BankAccount)d);
                    bankAccounts.Remove((BankAccount)d);
                    dataManager.DeleteBankAccount((BankAccount)d);
                }
            }
            else if(d is BankingTransaction)
            {
                if (!deleted)
                {
                    changedTransactions.Add((BankingTransaction)d);
                }
                else
                {
                    //deletedTransactions.Add((BankingTransaction)d);
                    bankingTransactions.Remove((BankingTransaction)d);
                    dataManager.DeleteBankingTransaction((BankingTransaction)d);
                }
            }
            else if(d is TransactionTag)
            {
                if (!deleted)
                {
                    changedTransactionTags.Add((TransactionTag)d);
                }
                else
                {
                    //deletedTransactionTags.Add((TransactionTag)d);
                    transactionTags.Remove((TransactionTag)d);
                    dataManager.DeleteTransactionTag((TransactionTag)d);
                }
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public void SaveChanges_Click()
        {
            foreach(BankingTransaction transaction in changedTransactions)
            {
                // BankingTransaction originalTransaction = dataManager.GetBankingTransactionByID(transaction.Guid);

                // save/update banking-transaction
                int index = bankingTransactions.IndexOf(transaction);

                if(index != -1)
                {
                    bankingTransactions[index] = dataManager.SaveBankingTransaction(transaction);
                }
                else
                {
                    bankingTransactions.Add(dataManager.SaveBankingTransaction(transaction));
                }

            }

            foreach (BankAccount bankAccount in changedBankAccounts)
            {
                //BankAccount originalBankAccount = dataManager.GetBankAccountByID(bankAccount.Guid);
                int index = bankAccounts.IndexOf(bankAccount);

                if(index != -1)
                {
                    bankAccounts[index] = dataManager.SaveBankAccount(bankAccount);
                }
                else
                {
                    bankAccounts.Add(dataManager.SaveBankAccount(bankAccount));
                }

            }
            
            foreach (TransactionTag tag in changedTransactionTags)
            {
                int index = transactionTags.IndexOf(tag);

                if(index != -1)
                {
                    transactionTags[index] = dataManager.SaveTransactionTag(tag);
                }
                else
                {
                    transactionTags.Add(dataManager.SaveTransactionTag(tag));
                }
            }



            LoadBankAccounts();
            //LoadBankingTransactions();


            changedBankAccounts.Clear();
            changedTransactions.Clear();
            changedTransactionTags.Clear();
        }

        private void LoadBankAccounts()
        {
            bankAccounts.Clear();
            foreach (BankAccount b in dataManager.GetAllBankAccounts().Values)
            {
                if (!b.Deleted)
                {
                    bankAccounts.Add(b);
                }
            }
        }

        private void LoadBankingTransactions()
        {
            bankingTransactions.Clear();
            foreach (BankingTransaction transaction in dataManager.GetAllBankingTransactions().Values)
            {
                if (!transaction.Deleted)
                {
                    bankingTransactions.Add(transaction);
                }
            }
        }

        private void LoadTransactionTags()
        {
            foreach (TransactionTag tag in dataManager.GetAllTransactionTags().Values)
            {
                if (!tag.Deleted)
                {
                    transactionTags.Add(tag);
                }
            }
        }

        public void ShowTags(BankingTransaction transaction)
        {
            currentTransactionTags.Clear();
            foreach(TransactionTag tag in transaction.TransactionTags.Values)
            {
                currentTransactionTags.Add(tag);
            }
        }


        public void DiscardChanges_Click()
        {
            // do reload on gui
            // use for instead of foreach to be able to change the UI's bank accounts variables. 
            for(int i = 0; i < changedBankAccounts.Count; i++)
            {
                int index = bankAccounts.IndexOf(changedBankAccounts[i]);
                bankAccounts[index] = dataManager.GetBankAccountByID(changedBankAccounts[i].Guid);

            }

            for (int i = 0; i < changedTransactions.Count; i++)
            {
                changedTransactions[i] = dataManager.GetBankingTransactionByID(changedTransactions[i].Guid);
            }
            

            // remove unchanged elements
            changedBankAccounts.Clear();
            changedTransactions.Clear();
        }

        public bool ExistUnsavedChanges()
        {
            if(changedBankAccounts.Count != 0 || changedTransactions.Count != 0 || changedTransactionTags.Count != 0)
            {
                return true;
            }

            return false;
        }

       
    }

}
