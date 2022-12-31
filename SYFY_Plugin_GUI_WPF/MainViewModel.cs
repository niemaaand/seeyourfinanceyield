using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SYFY_Plugin_GUI_WPF
{
    internal class MainViewModel
    {
        //public DependencyProperty dp;

        public IDataBaseConnector dataManager;

        public Dictionary<Guid, BankingTransaction>.ValueCollection Transactions { get; set; }

        public ObservableCollection<BankAccount> bankAccounts { get; set; }
        public ObservableCollection<BankingTransaction> bankingTransactions { get; set; }

        public ObservableCollection<TransactionTag> transactionTags { get; set; }
        public ObservableCollection<BankAccountComBoxItem> ComBoxColItems_BankAccounts { get; set; }

        public Dictionary<Guid, BankAccount>.ValueCollection BankAccounts { get; set; }
               
        public MainViewModel(IDataBaseConnector dataManager) 
        {
            //dp = DependencyProperty.Register("dataManager", typeof(IDataBaseConnector), typeof(MainViewModel));

            this.dataManager = dataManager;

            bankAccounts = new ObservableCollection<BankAccount>();
            foreach(BankAccount b in dataManager.GetAllBankAccounts().Values)
            {
                bankAccounts.Add((BankAccount)b.Clone());
            }

            bankingTransactions = new ObservableCollection<BankingTransaction>();
            foreach (BankingTransaction transaction in dataManager.GetAllBankingTransactions().Values)
            {
                bankingTransactions.Add((BankingTransaction)transaction.Clone());
            }

            transactionTags = new ObservableCollection<TransactionTag>();
            foreach(TransactionTag tag in dataManager.GetAllTransactionTags().Values)
            {
                transactionTags.Add(tag);
            }

            bankAccounts.CollectionChanged += On_BankAccountsChanged;

            //Dictionary<Guid, BankAccount> b = new Dictionary<Guid, BankAccount>();
            Dictionary<Guid, BankingTransaction> t = new Dictionary<Guid, BankingTransaction>();
            BankAccounts = dataManager.GetAllBankAccounts().Values;
            Transactions = dataManager.GetAllBankingTransactions().Values;

             ComBoxColItems_BankAccounts = new ObservableCollection<BankAccountComBoxItem>();
            DependencyProperty dependency = DependencyProperty.Register("MyProperty", typeof(string), typeof(DependencyProperty));

            ComboBox box = new ComboBox();

            foreach (BankAccount b in BankAccounts)
            {
                ComBoxColItems_BankAccounts.Add(new BankAccountComBoxItem(b.Guid, b.Name));

                //ComboBoxItem comboBoxItem = new ComboBoxItem();
                //comboBoxItem.SetValue(dependency, b.Guid);
                //comboBoxItem.Content= b.Name;
                //box.Items.Add(comboBoxItem);

            }

            
            
            


        }

       

        private void On_BankAccountsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
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
