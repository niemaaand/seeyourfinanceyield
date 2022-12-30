using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using SYFY_Plugin_GUI_WPF.Converters;
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

namespace SYFY_Plugin_GUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {




        public MainWindow(IDataBaseConnector dataManager)
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(dataManager);

            BankAccountGuidToNameConverter baGuidNameConv = new BankAccountGuidToNameConverter(dataManager);
            
            Binding bind_FromBankAccount = new Binding("FromBankAccount");
            bind_FromBankAccount.Converter = baGuidNameConv;
            dgCol_Trans_FromBankAccount.Binding = bind_FromBankAccount;

            Binding bind_ToBankAccount = new Binding("ToBankAccount");
            bind_ToBankAccount.Converter = baGuidNameConv;
            dgCol_Trans_ToBankAccount.Binding = bind_ToBankAccount;


            //DependencyProperty dependency = DependencyProperty.Register("BankAccount", typeof(string), typeof(DependencyProperty));
            ComboBox box = new ComboBox();

            foreach (BankAccount b in dataManager.GetAllBankAccounts().Values)
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
        
        }

        private void On_BeginEditDataGrid(object? sender, DataGridBeginningEditEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}
