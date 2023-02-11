using SYFY_Application.BusinessLogic;
using SYFY_Model.model;
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

        private HashSet<BankingTransaction> changedTransactions;
        private HashSet<BankAccount> changedBankAccounts;
        private HashSet<TransactionTag> changedTransactionTags;


        private HashSet<BankingTransaction> deletedTransactions;
        private HashSet<BankAccount> deletedBankAccounts;
        private HashSet<TransactionTag> deletedTransactionTags;

        public MainViewModel(DataManagement dataManager)
        {
            this.dataManager = dataManager;
            changedTransactions = new HashSet<BankingTransaction>();
            changedBankAccounts = new HashSet<BankAccount>();
            changedTransactionTags = new HashSet<TransactionTag>();

            deletedTransactions = new HashSet<BankingTransaction>();
            deletedBankAccounts = new HashSet<BankAccount>();
            deletedTransactionTags = new HashSet<TransactionTag>();

            bankAccounts = new ObservableCollection<BankAccount>();
            LoadBankAccounts();

            bankingTransactions = new ObservableCollection<BankingTransaction>();
            LoadBankingTransactions();            

            transactionTags = new ObservableCollection<TransactionTag>();
            LoadTransactionTags();

            currentTransactionTags = new ObservableCollection<TransactionTag>();

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
                if (!deleted)
                {
                    changedBankAccounts.Add((BankAccount)d);
                }
                else
                {
                    deletedBankAccounts.Add((BankAccount)d);
                    bankAccounts.Remove((BankAccount)d);
                    //dataManager.DeleteBankAccount((BankAccount)d);
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
                    deletedTransactions.Add((BankingTransaction)d);
                    bankingTransactions.Remove((BankingTransaction)d);
                    //dataManager.DeleteBankingTransaction((BankingTransaction)d);
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
                    deletedTransactionTags.Add((TransactionTag)d);
                    transactionTags.Remove((TransactionTag)d);
                    //dataManager.DeleteTransactionTag((TransactionTag)d);
                }
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

                foreach (BankingTransaction transaction in changedTransactions)
                {
                    // BankingTransaction originalTransaction = dataManager.GetBankingTransactionByID(transaction.Guid);

                    // save/update banking-transaction
                    int index = bankingTransactions.IndexOf(transaction);

                    if (index != -1)
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
                    int index = transactionTags.IndexOf(tag);

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

                foreach (BankAccount bankAccount in deletedBankAccounts)
                {
                    dataManager.DeleteBankAccount(bankAccount);
                }

                foreach (TransactionTag tag in deletedTransactionTags)
                {
                    dataManager.DeleteTransactionTag(tag);
                }
            }
            finally
            {
                LoadBankAccounts();
                //LoadBankingTransactions();


                changedBankAccounts.Clear();
                changedTransactions.Clear();
                changedTransactionTags.Clear();

                deletedBankAccounts.Clear();
                deletedTransactions.Clear();
                deletedTransactionTags.Clear();
            }
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
            foreach(Guid tagId in transaction.TransactionTags)
            {
                currentTransactionTags.Add(dataManager.GetTransactionTagByID(tagId));
            }
        }


        public void DiscardChanges_Click()
        {
            // do reload on gui
            int index = 0;

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
            changedBankAccounts.Clear();
            changedTransactions.Clear();
            changedTransactionTags.Clear();

            deletedTransactions.Clear();
            deletedBankAccounts.Clear();
            deletedTransactionTags.Clear();
        }

        public bool ExistUnsavedChanges()
        {
            if(changedBankAccounts.Count != 0 
                || deletedBankAccounts.Count != 0
                || changedTransactions.Count != 0 
                || deletedTransactions.Count != 0
                || changedTransactionTags.Count != 0
                || deletedTransactionTags.Count != 0)
            {
                return true;
            }

            return false;
        }

       
    }

}
