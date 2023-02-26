using SYFY_Adapter_GUI.ViewDataHandlers;
using SYFY_Application.BusinessLogic;
using SYFY_Domain.model;
using System.Collections.ObjectModel;
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

            dataHandlers = new List<IViewDataHandler>
            {
                new ViewDataBankAccountHandler(bankAccounts, dataManager),
                new ViewDataBankingTransactionHandler(bankingTransactions, dataManager),
                new ViewDataTransactionTagHandler(transactionTags, dataManager)
            };

            LoadData();

        }

        public void BTN_NewTransaction_Click(object? sender, EventArgs e)
        {
            //TODO
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
        private void ExecActionForAllDataHandlers(Action<IViewDataHandler> value)
        {
            foreach (IViewDataHandler dataHandler in dataHandlers)
            {
                value.Invoke(dataHandler);
            }
        }

        public void DataChanged(DeleteableData d, bool deleted = false)
        {
           ExecActionForAllDataHandlers((h) =>
            {
                if (h.Handles(d))
                {
                    h.DataChanged(d,deleted);
                }
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
                    currentlyAvailableTransactionTags.Add(dataManager.GetTransactionTagByID(tag.Guid));
                }
            }
        }

        public void AddTagToTransaction(BankingTransaction transaction, TransactionTag tag)
        {
            transaction.TransactionTags.Add(tag.Guid);
            DataChanged(transaction);
            currentTransactionTags.Add(tag);
            currentlyAvailableTransactionTags.Remove(tag);
        }

        public void RemoveTagFromTransaction(BankingTransaction transaction, TransactionTag tag)
        {
            transaction.TransactionTags.Remove(tag.Guid);
            DataChanged(transaction);
            currentTransactionTags.Remove(tag);
            currentlyAvailableTransactionTags.Add(tag);
        }
    }

    public interface a
    {
        string Name { get; }

    }

    public class Test : INotifyPropertyChanged, a
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _Name;

        public Test(string name)
        {
            Name = name;
        }

        public string Name {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged();
            } }

        protected void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            PropertyChanged?.Invoke(propertyName, new PropertyChangedEventArgs(propertyName));
        }

    }
}
