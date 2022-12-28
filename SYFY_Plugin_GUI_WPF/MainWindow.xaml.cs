using SYFY_Application.DatabaseAccess;
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

            DataGrid dataGrid = new DataGrid();
            dataGrid.AutoGenerateColumns = false;
            dataGrid.ItemsSource = dataManager.GetAllBankAccounts().Values;

            DataGridTextColumn dgCol_Name = new DataGridTextColumn();
            dgCol_Name.Header = "Name";
            dgCol_Name.Binding = new Binding("Name");
            

            DataGridTextColumn dgCol_Iban = new DataGridTextColumn();
            dgCol_Iban.Header = "Number (IBAN)";
            dgCol_Iban.Binding = new Binding("Iban");

            DataGridTextColumn dgCol_Comment = new DataGridTextColumn();
            dgCol_Comment.Header = "Comment";
            dgCol_Comment.Binding = new Binding("Comment");

            DataGridTextColumn dgCol_Amount = new DataGridTextColumn();
            dgCol_Amount.Header = "Amount";
            dgCol_Amount.Binding = new Binding("Amount");

            dataGrid.Columns.Add(dgCol_Name);
            dataGrid.Columns.Add(dgCol_Iban);
            dataGrid.Columns.Add(dgCol_Comment);
            dataGrid.Columns.Add(dgCol_Amount);

            this.Content = dataGrid;
        
        }
    }
}
