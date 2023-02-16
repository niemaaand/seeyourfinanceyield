using SYFY_Adapter_GUI.ViewDataHandlers;
using SYFY_Application.BusinessLogic;
using SYFY_Domain.model;
using System.Collections.ObjectModel;


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

        /*private HashSet<BankingTransaction> changedTransactions;
        //private HashSet<BankAccount> changedBankAccounts;
        private HashSet<TransactionTag> changedTransactionTags;


        private HashSet<BankingTransaction> deletedTransactions;
        //private HashSet<BankAccount> deletedBankAccounts;
        private HashSet<TransactionTag> deletedTransactionTags;
        */

        private IViewDataHandler bankAccountHandler;
        private IViewDataHandler bankingTransactionsHandler;
        private IViewDataHandler transactionTagsHandler;
        private List<IViewDataHandler> dataHandlers;


        public MainViewModel(DataManagement dataManager)
        {
            this.dataManager = dataManager;
            /*changedTransactions = new HashSet<BankingTransaction>();
            //changedBankAccounts = new HashSet<BankAccount>();
            changedTransactionTags = new HashSet<TransactionTag>();

            deletedTransactions = new HashSet<BankingTransaction>();
            //deletedBankAccounts = new HashSet<BankAccount>();
            deletedTransactionTags = new HashSet<TransactionTag>();
            */
            bankAccounts = new ObservableCollection<BankAccount>();
            //LoadBankAccounts();

            bankingTransactions = new ObservableCollection<BankingTransaction>();
            //LoadBankingTransactions();            

            transactionTags = new ObservableCollection<TransactionTag>();
            //LoadTransactionTags();

            currentTransactionTags = new ObservableCollection<TransactionTag>();
            currentlyAvailableTransactionTags = new ObservableCollection<TransactionTag>();


            bankAccountHandler = new ViewDataBankAccountHandler(bankAccounts, dataManager);
            bankingTransactionsHandler = new ViewDataBankingTransactionHandler(bankingTransactions, dataManager);
            transactionTagsHandler = new ViewDataTransactionTagHandler(transactionTags, dataManager);

            dataHandlers = new List<IViewDataHandler>
            {
                bankAccountHandler, bankingTransactionsHandler, transactionTagsHandler
            };

            LoadData();

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
            if(d is BankAccount)
            {
                bankAccountHandler.DataChanged(d, deleted);
            }
            else if(d is BankingTransaction)
            {
                bankingTransactionsHandler.DataChanged(d, deleted);
                /*
                if (!deleted)
                {
                    changedTransactions.Add((BankingTransaction)d);                        
                }
                else
                {
                    deletedTransactions.Add((BankingTransaction)d);
                    bankingTransactions.Remove((BankingTransaction)d);
                }*/
            }
            else if(d is TransactionTag)
            {
                transactionTagsHandler.DataChanged(d, deleted);
                /*if (!deleted)
                {
                    changedTransactionTags.Add((TransactionTag)d);
                }
                else
                {
                    deletedTransactionTags.Add((TransactionTag)d);
                    transactionTags.Remove((TransactionTag)d);
                }*/
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        public void SaveChanges_Click()
        {
            try
            {
                foreach (IViewDataHandler dataHandler in dataHandlers)
                {
                    dataHandler.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }

            LoadData();
            //LoadBankAccounts();
            //LoadBankingTransactions();

            /*
            foreach (BankingTransaction transaction in changedTransactions)
            {
                // save/update banking-transaction
                index = bankingTransactions.IndexOf(transaction);

                if (index != -1)
                {
                    bankingTransactions[index] = dataManager.SaveBankingTransaction(transaction);
                }
                else
                {
                    bankingTransactions.Add(dataManager.SaveBankingTransaction(transaction));
                }

            }

            /*foreach (BankAccount bankAccount in changedBankAccounts)
            {
                index = bankAccounts.IndexOf(bankAccount);

                if (index != -1)
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
                index = transactionTags.IndexOf(tag);

                if (index != -1)
                {
                    transactionTags[index] = dataManager.SaveTransactionTag(tag);
                }
                else
                {
                    transactionTags.Add(dataManager.SaveTransactionTag(tag));
                }
            }

            foreach (BankingTransaction transaction in deletedTransactions)
            {
                dataManager.DeleteBankingTransaction(transaction);
            }

            /*foreach (BankAccount bankAccount in deletedBankAccounts)
            {
                dataManager.DeleteBankAccount(bankAccount);
            }

            foreach (TransactionTag tag in deletedTransactionTags)
            {
                dataManager.DeleteTransactionTag(tag);
            }

            LoadBankAccounts();
            //LoadBankingTransactions();


            //changedBankAccounts.Clear();
            changedTransactions.Clear();
            changedTransactionTags.Clear();

            //deletedBankAccounts.Clear();
            deletedTransactions.Clear();
            deletedTransactionTags.Clear();
            */
        }

        private void LoadData()
        {
            foreach(IViewDataHandler dataHandler in dataHandlers)
            {
                dataHandler.LoadData();
            }
        }
              
        public void ShowTags(BankingTransaction transaction)
        {
            currentTransactionTags.Clear();
            foreach(Guid tagId in transaction.TransactionTags)
            {
                currentTransactionTags.Add(dataManager.GetTransactionTagByID(tagId));
            }

            currentlyAvailableTransactionTags.Clear();
            foreach(TransactionTag tag in transactionTags)
            {
                if (!transaction.TransactionTags.Contains(tag.Guid))
                {
                    currentlyAvailableTransactionTags.Add(dataManager.GetTransactionTagByID(tag.Guid));
                }
            }
        }


        public void DiscardChanges_Click()
        {
            // do reload on gui
            /*int index = 0;

            foreach(BankAccount b in changedBankAccounts)
            {
                index = bankAccounts.IndexOf(b);
                bankAccounts[index] = dataManager.GetBankAccountByID(b.Guid);
            }

            foreach(BankingTransaction b in changedTransactions)
            {
                index = bankingTransactions.IndexOf(b);
                bankingTransactions[index] = dataManager.GetBankingTransactionByID(b.Guid);
            }

            foreach(TransactionTag tag in changedTransactionTags)
            {
                index = transactionTags.IndexOf(tag);
                transactionTags[index] = dataManager.GetTransactionTagByID(tag.Guid);
            }


            foreach(BankAccount b in deletedBankAccounts)
            {
                bankAccounts.Add(b);
            }

            foreach(BankingTransaction b in deletedTransactions)
            {
                bankingTransactions.Add(b);
            }

            foreach(TransactionTag t in deletedTransactionTags)
            {
                transactionTags.Add(t);
            }

            // remove unchanged elements
            //changedBankAccounts.Clear();
            changedTransactions.Clear();
            changedTransactionTags.Clear();

            deletedTransactions.Clear();
            //deletedBankAccounts.Clear();
            deletedTransactionTags.Clear();
            */
            foreach(IViewDataHandler dataHandler in dataHandlers)
            {
                dataHandler.DiscardChanges();
            }
        }

        public bool ExistUnsavedChanges()
        {
            /*if(//changedBankAccounts.Count != 0 
                //|| deletedBankAccounts.Count != 0
                 changedTransactions.Count != 0 
                || deletedTransactions.Count != 0
                || changedTransactionTags.Count != 0
                || deletedTransactionTags.Count != 0)
            {
                return true;
            }*/

            foreach(IViewDataHandler dataHandler in dataHandlers)
            {
                if (dataHandler.ExistUnsavedChanges())
                {
                    return true;
                }
            }

            return false;
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
}
