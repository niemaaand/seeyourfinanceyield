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

            InitializeComponent();
            this.DataContext = mainViewModel;

            BankAccountGuidToNameConverter baGuidNameConv = new BankAccountGuidToNameConverter(this.dataManager);

            Binding bind_FromBankAccount = new Binding("FromBankAccount");
            bind_FromBankAccount.Converter = baGuidNameConv;
            dgCol_Trans_FromBankAccount.Binding = bind_FromBankAccount;

            Binding bind_ToBankAccount = new Binding("ToBankAccount");
            bind_ToBankAccount.Converter = baGuidNameConv;
            dgCol_Trans_ToBankAccount.Binding = bind_ToBankAccount;

            // events to notify about changed data
            dg_BankAccounts.CellEditEnding += On_CellEditEnding;
            dg_BankAccounts.RowEditEnding += On_RowEditEnding;
            dg_Transactions.RowEditEnding += On_RowEditEnding;

            // event to update transaction-tags shown to transaction
            dg_Transactions.CurrentCellChanged += On_CurrentCellChanged;

            // event for tab changed
            tabControl.SelectionChanged += On_TabSelectionChanged;






            /*
            //DependencyProperty dependency = DependencyProperty.Register("BankAccount", typeof(string), typeof(DependencyProperty));
            ComboBox box = new ComboBox();

            foreach (BankAccount b in this.dataManager.GetAllBankAccounts().Values)
            {
                //ComBoxColItems_BankAccounts.Add(new BankAccountComBoxItem(b.Guid, b.Name));



                ComboBoxItem comboBoxItem = new ComboBoxItem()
                {
                    Content = "Content",
                };
                //comboBoxItem.SetValue(dependency, b.Guid);
                //comboBoxItem.Content = b.Name;
                box.Items.Add(comboBoxItem);

            }

            //dgCol_Trans_BANew.CellTemplate = new DataTemplate(typeof(ComboBox));


            DataGridTextColumn dgCol_Name = new DataGridTextColumn();
            dgCol_Name.Header = "Name";
            dgCol_Name.Binding = new Binding("Name");
            dgCol_Name.IsReadOnly = false;


            DataGridTextColumn dgCol_Iban = new DataGridTextColumn();
            dgCol_Iban.Header = "Number (IBAN)";
            dgCol_Iban.Binding = new Binding("Iban");

            DataGridTextColumn dgCol_Comment = new DataGridTextColumn();
            dgCol_Comment.Header = "Comment";
            dgCol_Comment.Binding = new Binding("Comment");

            DataGridTextColumn dgCol_Amount = new DataGridTextColumn();
            dgCol_Amount.Header = "Amount";
            dgCol_Amount.Binding = new Binding("Amount");


            DataGrid dataGridBankAccounts = new DataGrid();
            dataGridBankAccounts.AutoGenerateColumns = false;
            dataGridBankAccounts.ItemsSource = dataManager.GetAllBankAccounts().Values;
            dataGridBankAccounts.Columns.Add(dgCol_Name);
            dataGridBankAccounts.Columns.Add(dgCol_Iban);
            dataGridBankAccounts.Columns.Add(dgCol_Comment);
            dataGridBankAccounts.Columns.Add(dgCol_Amount);
            dataGridBankAccounts.IsReadOnly = false;
            dataGridBankAccounts.CanUserAddRows = true;
            dataGridBankAccounts.BeginningEdit += On_BeginEditDataGrid;

            DataGridTextColumn dgCol_TransDate = new DataGridTextColumn();
            dgCol_TransDate.Header = "Date";
            dgCol_TransDate.Binding = new Binding("TransactionDate");


            DataGridComboBoxColumn dgCol_BankAccountFrom = new DataGridComboBoxColumn();
            dgCol_BankAccountFrom.Header = "Bank account from";
            dgCol_BankAccountFrom.SelectedValueBinding = new Binding("FromBankAccount");
            dgCol_BankAccountFrom.ItemsSource = dataManager.GetAllBankAccounts();
            dgCol_BankAccountFrom.DisplayMemberPath = "Name";
            dgCol_BankAccountFrom.SelectedValuePath = "Guid";

            DataGrid dg_Transactions = new DataGrid();
            dg_Transactions.AutoGenerateColumns = false;
            dg_Transactions.ItemsSource = dataManager.GetAllBankingTransactions().Values;
            dg_Transactions.Columns.Add(dgCol_TransDate);
            dg_Transactions.Columns.Add(dgCol_BankAccountFrom);




            TabItem bankAccountsTab = new TabItem();
            bankAccountsTab.Header = "BankAccounts";
            bankAccountsTab.Content = dataGridBankAccounts;

            TabItem transactionsTab = new TabItem();
            transactionsTab.Header = "Transactions";
            transactionsTab.Content = dg_Transactions;

            TabControl tabControl = new TabControl();
            tabControl.Items.Add(bankAccountsTab);
            tabControl.Items.Add(transactionsTab);


            //dgCol_Trans_FromBankAccount.Item


            //this.Content = tabControl;
            */
        }

        private void On_TabSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
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

                    if(result == MessageBoxResult.Yes)
                    {
                        GetDataContextFromSender(sender).SaveChanges_Click();
                    }else if(result == MessageBoxResult.No)
                    {
                        GetDataContextFromSender(sender).DiscardChanges_Click();
                    }else if(result== MessageBoxResult.Cancel)
                    {
                        e.Handled= true;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                }
            }
        }

        private void On_CurrentCellChanged(object? sender, EventArgs e)
        {
            try
            {
                GetDataContextFromSender(sender).ShowTags(((BankingTransaction)((DataGrid)sender).CurrentCell.Item));
            }catch(Exception ex)
            {

            }
        }

        private void On_BeginEditDataGrid(object? sender, DataGridBeginningEditEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void On_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
        {
            string oldComment = ((BankAccount)((DataGrid)sender).SelectedItem).Comment;
            string newComment = ((TextBox)e.EditingElement).Text;
            //((TextBox)e.EditingElement).Text = "HALLO";
            DataManagement dataManager = ((MainViewModel)((DataGrid)sender).DataContext).dataManager;
        }

        private void On_RowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
        {
            //BankAccount b = ((BankAccount)((DataGrid)sender).SelectedItem);
            GetDataContextFromSender(sender).DataChanged((DeleteableData)((DataGrid)sender).SelectedItem);
        }


        private void BTN_NewTransaction_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).BTN_NewTransaction_Click(sender, e);
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

        private void BTN_SaveChangesTransactions_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).SaveChanges_Click();
        }

        private void BTN_DiscardChangesTransactions_Click(object sender, RoutedEventArgs e)
        {
            //GetDataContextFromSender(sender).DiscardChangesTransactions(sender, e);
        }

        private void BTN_DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            BankingTransaction selected = (BankingTransaction)dg_Transactions.SelectedItem;
            GetDataContextFromSender(sender).DataChanged(selected, true);
        }

        private void BTN_DeleteBankAccount_Click(object sender, RoutedEventArgs e)
        {
            BankAccount selected = (BankAccount)dg_BankAccounts.SelectedItem;
            GetDataContextFromSender(sender).DataChanged(selected, true);
        }

        public void BTN_NewBankAccount_Click(object? sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).BTN_NewBankAccount_Click(sender, e);
        }

        private void BTN_SaveChangesBankAccounts_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).SaveChanges_Click();
        }

        private void BTN_DiscardChangesBankAccounts_Click(object sender, RoutedEventArgs e)
        {
            GetDataContextFromSender(sender).DiscardChanges_Click();
        }


        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshDataGrids();
        }

        private void RefreshDataGrids()
        {
            try
            {
                dg_BankAccounts.Items.Refresh();
                dg_Transactions.Items.Refresh();
                dg_TransactionTags.Items.Refresh();
                dg_Transactions_Tags.Items.Refresh();
            }catch(Exception ex)
            {

            }
        }
    }
}
