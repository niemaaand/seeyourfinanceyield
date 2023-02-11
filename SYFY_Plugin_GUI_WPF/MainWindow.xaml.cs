using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using SYFY_Plugin_GUI_WPF.Converters;
using SYFY_Adapter_GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SYFY_Application.BusinessLogic;
using System.Linq.Expressions;

namespace SYFY_Plugin_GUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataManagement dataManager;

        public MainWindow(DataManagement dataManager, MainViewModel mainViewModel)
        {
            this.dataManager = dataManager;

            BankAccountGuidToNameConverter baGuidNameConv = new BankAccountGuidToNameConverter(this.dataManager);

            InitializeComponent();
            this.DataContext = mainViewModel;

            // set data-bindint for from bank account
            Binding bind_FromBankAccount = new Binding("FromBankAccount");
            bind_FromBankAccount.Converter = baGuidNameConv;

            var tb_fromBankAccount = new FrameworkElementFactory(typeof(TextBlock));
            tb_fromBankAccount.SetBinding(TextBlock.TextProperty,
                bind_FromBankAccount);

            DataTemplate dt_fromBankAccount = new DataTemplate() { VisualTree = tb_fromBankAccount };

            dgCol_Trans_FromBankAccountCB.CellTemplate = dt_fromBankAccount;

            // set data-binding for to bank account
            Binding bind_ToBankAccount = new Binding("ToBankAccount");
            bind_ToBankAccount.Converter = baGuidNameConv;

            var tb_toBankAccount = new FrameworkElementFactory(typeof(TextBlock));
            tb_toBankAccount.SetBinding(TextBlock.TextProperty,
                bind_ToBankAccount);

            DataTemplate dt_toBankAccount = new DataTemplate() { VisualTree = tb_toBankAccount };

            dgCol_Trans_ToBankAccountCB.CellTemplate = dt_toBankAccount;


            // events to notify about changed data
            dg_BankAccounts.RowEditEnding += On_RowEditEnding;
            dg_Transactions.RowEditEnding += On_RowEditEnding;
            dg_TransactionTags.RowEditEnding += On_RowEditEnding;

            // event to update transaction-tags shown to transaction
            dg_Transactions.CurrentCellChanged += On_CurrentCellChanged_Transactions;
            

            // event for tab changed
            tabControl.SelectionChanged += On_TabSelectionChanged;

           
        }

        private void On_TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                //TODO: if editing is not finished yet, then the changes will not be recognized. 
                // These changes will be recognized after switching again, since after first change of tab, 
                // the editing ends. 

                try
                {
                    // are there unsaved changes?
                    if (GetDataContextFromSender(sender).ExistUnsavedChanges())
                    {
                        MessageBoxResult result = MessageBox.Show("You have unsaved changes! " +
                            "\n Do you want to save your changes?"
                            + "\n Yes: Save Changes \n No: Discard Changes " +
                            "\n Cancel: Changes will not be applied, until they are saved.",
                            "Save Changes?",
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            GetDataContextFromSender(sender).SaveChanges_Click();
                        }
                        else if (result == MessageBoxResult.No)
                        {
                            GetDataContextFromSender(sender).DiscardChanges_Click();
                        }
                        else if (result == MessageBoxResult.Cancel)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                    }

                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void On_CurrentCellChanged_Transactions(object? sender, EventArgs e)
        {
            try
            {
                var current = ((DataGrid)sender).CurrentCell.Item;

                if (current is BankingTransaction)
                {
                    GetDataContextFromSender(sender).ShowTags((BankingTransaction)current);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void On_RowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
        {
            GetDataContextFromSender(sender).DataChanged((DeleteableData)((DataGrid)sender).SelectedItem);
        }

        private MainViewModel GetDataContextFromSender(Object? sender)
        {
            try
            {
                return (((MainViewModel)((Control)sender).DataContext));
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }

        private void BTN_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetDataContextFromSender(sender).SaveChanges_Click();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BTN_DiscardChanges_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).DiscardChanges_Click();
        }

        private void BTN_NewTransaction_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).BTN_NewTransaction_Click(sender, e);
        }

        private void BTN_DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            BankingTransaction selected = (BankingTransaction)dg_Transactions.SelectedItem;
            GetDataContextFromSender(sender).DataChanged(selected, true);
        }
        public void BTN_NewBankAccount_Click(object? sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).BTN_NewBankAccount_Click(sender, e);
        }

        private void BTN_DeleteBankAccount_Click(object sender, RoutedEventArgs e)
        {
            BankAccount selected = (BankAccount)dg_BankAccounts.SelectedItem;
            GetDataContextFromSender(sender).DataChanged(selected, true);
        }

        private void BTN_NewTag_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).BTN_NewTransactionTag_Click(sender, e);
        }

        private void BTN_DeleteTag_Click(object sender, RoutedEventArgs e)
        {
            TransactionTag selected = (TransactionTag)dg_TransactionTags.SelectedItem;
            GetDataContextFromSender(sender).DataChanged(selected, true);
        }


        /*        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
                {
                    RefreshDataGrids();
                }
        */
        private void RefreshDataGrids()
        {
            try
            {
                dg_BankAccounts.Items.Refresh();
                dg_Transactions.Items.Refresh();
                dg_TransactionTags.Items.Refresh();
                dg_Transactions_AlignedTransactionTags.Items.Refresh();
            }catch(Exception ex)
            {

            }
        }       
    }
}
