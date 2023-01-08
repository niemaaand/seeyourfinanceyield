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
        //public DependencyProperty dp;

        public DataManagement dataManager;

        // public Dictionary<Guid, BankingTransaction>.ValueCollection Transactions { get; set; }

        public ObservableCollection<BankAccount> bankAccounts { get; set; }
        public ObservableCollection<BankingTransaction> bankingTransactions { get; set; }

        public ObservableCollection<TransactionTag> transactionTags { get; set; }
        // public ObservableCollection<BankAccountComBoxItem> ComBoxColItems_BankAccounts { get; set; }

        //  public Dictionary<Guid, BankAccount>.ValueCollection BankAccounts { get; set; }


        private List<BankingTransaction> changedTransactions;
        private List<BankAccount> changedBankAccounts;

        public MainViewModel(DataManagement dataManager)
        {
            //dp = DependencyProperty.Register("dataManager", typeof(IDataBaseConnector), typeof(MainViewModel));

            this.dataManager = dataManager;
            changedTransactions = new List<BankingTransaction>();
            changedBankAccounts = new List<BankAccount>();

            bankAccounts = new ObservableCollection<BankAccount>();
            foreach (BankAccount b in dataManager.GetAllBankAccounts().Values)
            {
                bankAccounts.Add(b);
            }

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

            //Dictionary<Guid, BankAccount> b = new Dictionary<Guid, BankAccount>();
            // Dictionary<Guid, BankingTransaction> t = new Dictionary<Guid, BankingTransaction>();
            // BankAccounts = dataManager.GetAllBankAccounts().Values;
            // Transactions = dataManager.GetAllBankingTransactions().Values;

            /*   ComBoxColItems_BankAccounts = new ObservableCollection<BankAccountComBoxItem>();
              DependencyProperty dependency = DependencyProperty.Register("MyProperty", typeof(string), typeof(DependencyProperty));

              ComboBox box = new ComboBox();
            */
            /* foreach (BankAccount b in BankAccounts)
             {
                 ComBoxColItems_BankAccounts.Add(new BankAccountComBoxItem(b.Guid, b.Name));

                 //ComboBoxItem comboBoxItem = new ComboBoxItem();
                 //comboBoxItem.SetValue(dependency, b.Guid);
                 //comboBoxItem.Content= b.Name;
                 //box.Items.Add(comboBoxItem);

             }*/






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
                dataManager.SaveBankingTransaction(transaction);
            }



            foreach (BankAccount bankAccount in changedBankAccounts)
            {
                dataManager.SaveBankAccount(bankAccount);
            }

            changedBankAccounts.Clear();
            changedTransactions.Clear();
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

    public class BankAccountComBoxItem
    {
        private Guid Id { get; }
        private string Name { get; }

        public BankAccountComBoxItem(Guid guid, string name)
        {
            Id = guid;
            Name = name;
        }

    }

}
