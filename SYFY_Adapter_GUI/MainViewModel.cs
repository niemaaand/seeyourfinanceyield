using SYFY_Adapter_GUI.ViewDataHandlers;
using SYFY_Application.BusinessLogic;
using SYFY_Domain.model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SYFY_Adapter_GUI
{
    public class MainViewModel
    {

        public DataManagement dataManager;

        public ObservableCollection<BankAccount> bankAccounts { get; set; }
        public ObservableCollection<BankingTransaction> bankingTransactions { get; set; }
        public ObservableCollection<TransactionTag> transactionTags { get; set; }
        public ObservableCollection<TransactionTag> currentTransactionTags { get; set; }
        public ObservableCollection<TransactionTag> currentlyAvailableTransactionTags { get; set; }
                
        private List<IViewDataHandler> dataHandlers;

        public MainViewModel(DataManagement dataManager)
        {   
            this.dataManager = dataManager;
            
            bankAccounts = new ObservableCollection<BankAccount>();
            bankingTransactions = new ObservableCollection<BankingTransaction>();
            transactionTags = new ObservableCollection<TransactionTag>();
            currentTransactionTags = new ObservableCollection<TransactionTag>();
            currentlyAvailableTransactionTags = new ObservableCollection<TransactionTag>();

            dataHandlers = new List<IViewDataHandler>();            
        }
               
        public void AddDataHandler(List<IViewDataHandler> handler)
        {
            dataHandlers.AddRange(handler);

            foreach(IViewDataHandler h in handler)
            {
                h.LoadData();
            }
        }

        public void BTN_NewTransaction_Click(object? sender, EventArgs e)
        {
            //TODO
            BankingTransaction transaction = dataManager.CreateEmptyBankingTransaction();
            DataChanged(transaction, newlyCreated: true);
        }

        public void BTN_NewBankAccount_Click(object? sender, EventArgs e)
        {
            //TODO
            BankAccount b = dataManager.CreateEmptyBankAccount();
            DataChanged(b, newlyCreated:true);
        }

        public void BTN_NewTransactionTag_Click(object sender, EventArgs e)
        {
            //TODO
            TransactionTag tag = dataManager.CreateEmptyTransactionTag();
            DataChanged(tag, newlyCreated:true);
        }
        private void ExecActionForAllDataHandlers(Action<IViewDataHandler> value)
        {
            foreach (IViewDataHandler dataHandler in dataHandlers)
            {
                value.Invoke(dataHandler);
            }
        }

        public void DataChanged(DeleteableData d, bool deleted = false, bool newlyCreated = false)
        {
           ExecActionForAllDataHandlers((h) =>
            {
                h.DataChanged(d, deleted, newlyCreated);
            });
        }
                
        public void SaveChanges_Click()
        {
            try
            {
                ExecActionForAllDataHandlers((h) => h.SaveChanges());
            }
            catch (Exception ex)
            {

            }

            LoadData();
        }

        private void LoadData()
        {
            ExecActionForAllDataHandlers((h) => h.LoadData());
        }
            
        public void DiscardChanges_Click()
        {
            ExecActionForAllDataHandlers((h) => h.DiscardChanges());
        }

        public bool ExistUnsavedChanges()
        {
            //cannot use ExecActionForAllDataHandlers() here, because of return-value

            foreach (IViewDataHandler dataHandler in dataHandlers)
            {
                if (dataHandler.ExistUnsavedChanges())
                {
                    return true;
                }
            }
            
            return false;
        }

        public void ShowTags(BankingTransaction transaction)
        {
            currentTransactionTags.Clear();
            foreach (Guid tagId in transaction.TransactionTags)
            {
                currentTransactionTags.Add(dataManager.GetTransactionTagByID(tagId));
            }

            currentlyAvailableTransactionTags.Clear();
            foreach (TransactionTag tag in transactionTags)
            {
                if (!transaction.TransactionTags.Contains(tag.Guid))
                {
                    try
                    {
                        currentlyAvailableTransactionTags.Add(dataManager.GetTransactionTagByID(tag.Guid));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void AddTagToTransaction(BankingTransaction transaction, TransactionTag tag)
        {
            dataManager.AddTagToTransaction(transaction, tag);
            DataChanged(transaction);
            currentTransactionTags.Add(tag);
            currentlyAvailableTransactionTags.Remove(tag);
        }

        public void RemoveTagFromTransaction(BankingTransaction transaction, TransactionTag tag)
        {
            dataManager.RemoveTagFromTransaction(transaction, tag);
            DataChanged(transaction);
            currentTransactionTags.Remove(tag);
            currentlyAvailableTransactionTags.Add(tag);
        }
    }   
}
