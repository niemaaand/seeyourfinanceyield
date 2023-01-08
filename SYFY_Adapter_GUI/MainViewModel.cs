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
       
        private List<BankingTransaction> changedTransactions;
        private List<BankAccount> changedBankAccounts;

        public MainViewModel(DataManagement dataManager)
        {
            this.dataManager = dataManager;
            changedTransactions = new List<BankingTransaction>();
            changedBankAccounts = new List<BankAccount>();

            bankAccounts = new ObservableCollection<BankAccount>();
            LoadBankAccounts();

            bankingTransactions = new ObservableCollection<BankingTransaction>();
            foreach (BankingTransaction transaction in dataManager.GetAllBankingTransactions().Values)
            {
                bankingTransactions.Add(transaction);
            }

            transactionTags = new ObservableCollection<TransactionTag>();
            foreach (TransactionTag tag in dataManager.GetAllTransactionTags().Values)
            {
                transactionTags.Add(tag);
            }

            bankAccounts.CollectionChanged += On_BankAccountsChanged;

        }



        private void On_BankAccountsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void BTN_NewTransaction_Click(object? sender, EventArgs e)
        {

        }

        public void BTN_NewBankAccount_Click(object? sender, EventArgs e)
        {
            BankAccount b = new BankAccount("Neuer Bank Account");
            bankAccounts.Add(b);
            DataChanged(b);
        }
            
       

        public void DataChanged(DeleteableData d)
        {
            //TODO       
            //d.DataHandler.AddChanged();

            if(d is BankAccount)
            {
                changedBankAccounts.Add((BankAccount)d);
            }
            else if(d is BankingTransaction)
            {
                changedTransactions.Add((BankingTransaction)d);
            }
            else if(d is TransactionTag)
            {
            }

        }

        public void SaveChanges_Click(object sender, EventArgs e)
        {
            foreach(BankingTransaction transaction in changedTransactions)
            {
                BankingTransaction originalTransaction = dataManager.GetBankingTransactionByID(transaction.Guid);

                int index = bankingTransactions.IndexOf(transaction);
                bankingTransactions[index] = dataManager.SaveBankingTransaction(transaction);

                BankAccount fromOld = bankAccounts.FirstOrDefault(b => b.Guid.Equals(originalTransaction.FromBankAccount));
                BankAccount fromNew = bankAccounts.FirstOrDefault(b => b.Guid.Equals(transaction.FromBankAccount));
                BankAccount toOld = bankAccounts.FirstOrDefault(b => b.Guid.Equals(originalTransaction.ToBankAccount));
                BankAccount toNew = bankAccounts.FirstOrDefault(b => b.Guid.Equals(transaction.ToBankAccount));

                index = bankAccounts.IndexOf(fromOld);
                bankAccounts[index] = dataManager.GetBankAccountByID(fromOld.Guid);

                if (!fromOld.Guid.Equals(fromNew.Guid))
                {
                    index = bankAccounts.IndexOf(fromNew);
                    bankAccounts[index] = dataManager.GetBankAccountByID(fromNew.Guid);
                }

                index = bankAccounts.IndexOf(toOld);
                bankAccounts[index] = dataManager.GetBankAccountByID(toOld.Guid);

                if (!toOld.Guid.Equals(toNew.Guid))
                {
                    index = bankAccounts.IndexOf(toNew);
                    bankAccounts[index] = dataManager.GetBankAccountByID(toNew.Guid);
                }

            }

            foreach (BankAccount bankAccount in changedBankAccounts)
            {
                BankAccount originalBankAccount = dataManager.GetBankAccountByID(bankAccount.Guid);
                int index = bankAccounts.IndexOf(bankAccount);
                bankAccounts[index] = dataManager.SaveBankAccount(bankAccount);
            }

            LoadBankAccounts();


            changedBankAccounts.Clear();
            changedTransactions.Clear();
        }

        private void LoadBankAccounts()
        {
            bankAccounts.Clear();
            foreach (BankAccount b in dataManager.GetAllBankAccounts().Values)
            {
                bankAccounts.Add(b);
            }
        }
        
        public void DiscardChanges_Click(object sender, EventArgs e)
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

       
    }

}
