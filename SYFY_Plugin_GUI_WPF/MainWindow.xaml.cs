using SYFY_Domain.model;
using SYFY_Plugin_GUI_WPF.Converters;
using SYFY_Adapter_GUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            //TODO:
            //converters auslagern in adapter
            //-> datentypen im view-model so wählen, dass direkt angezeigt werden kann 


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

            // set data-binding for selected transaction-tags
            /*TagGuidSelectedConv tagGuidSelectedConv = new TagGuidSelectedConv(this.dataManager);

            Binding bind_selectedTag = new Binding("Guid");
            bind_selectedTag.Converter = tagGuidSelectedConv;
            */
            /*var cb_selectedTag = new FrameworkElementFactory(typeof(CheckBox));

            var tb_selectedTag_Name = new FrameworkElementFactory(typeof(TextBlock));
            tb_selectedTag_Name.SetBinding(TextBlock.TextProperty, new Binding("Name"));

            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(tb_selectedTag_Name);

            CB_SelectedTags


            DataTemplate dt_cb_selectedTags = new DataTemplate();
            dt_cb_selectedTags.VisualTree

            CB_test.ItemTemplate = new DataTemplate();
            */


            // events to notify about changed data
            dg_BankAccounts.RowEditEnding += On_RowEditEnding;
            dg_Transactions.RowEditEnding += On_RowEditEnding;
            dg_TransactionTags.RowEditEnding += On_RowEditEnding;

            // event to update transaction-tags shown to transaction
            dg_Transactions.SelectedCellsChanged += On_SelectedCellsChanged_Transactions;


            // event for tab changed
            tabControl.SelectionChanged += On_TabSelectionChanged;

            


           
        }

        private void On_SelectedCellsChanged_Transactions(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                var current = dg_Transactions.SelectedItem;

                if (current is BankingTransaction)
                {
                    // newly selected banking transaction
                    GetDataContextFromSender(sender).ShowTags((BankingTransaction)current);
                    Grid_Transactions_TransactionTagsSelection.Visibility = Visibility.Visible;
                }
                else 
                {
                    Grid_Transactions_TransactionTagsSelection.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {

            }
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
                finally
                {
                    Grid_Transactions_TransactionTagsSelection.Visibility = Visibility.Hidden;
                }
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

        private void BTN_RemoveTagFromTransaction_Click(object sender, RoutedEventArgs e)
        {
            BankingTransaction transaction = dg_Transactions.SelectedItem as BankingTransaction;

            if (transaction is BankingTransaction)
            {
                TransactionTag tag = dg_Transactions_AlignedTransactionTags.SelectedItem as TransactionTag;

                if (tag is TransactionTag)
                {
                    GetDataContextFromSender(sender).RemoveTagFromTransaction(transaction, tag);
                }
                else
                {
                    MessageBox.Show("Please select Tag to remove from selected Tags.", 
                        "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BTN_AddTagToTransaction_Click(object sender, RoutedEventArgs e)
        {
            BankingTransaction transaction = dg_Transactions.SelectedItem as BankingTransaction;
            
            if(transaction is BankingTransaction)
            {
                TransactionTag tag = dg_AvailableTags.SelectedItem as TransactionTag;

                if(tag is TransactionTag)
                {
                    GetDataContextFromSender(sender).AddTagToTransaction(transaction, tag);
                }
                else
                {
                    MessageBox.Show("Please select Tag to add from available Tags.", 
                        "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void BTN_ExpandFilterTransactions_Click(object sender, RoutedEventArgs e)
        {
            if(BTN_ExpandFilterTransactions.IsChecked == true)
            {
                RW_TransactionsFilter.Height = new GridLength(200);


                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(100);

                Button btn_reset = new Button();
                btn_reset.Content = "Reset Filter";
                btn_reset.HorizontalAlignment= HorizontalAlignment.Right;
                btn_reset.VerticalAlignment = VerticalAlignment.Center;
                btn_reset.Width = 80;
                Grid.SetRow(btn_reset, 0);

                Button btn_apply = new Button();
                btn_apply.Content = "Apply Filter";
                btn_apply.HorizontalAlignment = HorizontalAlignment.Center;
                btn_apply.VerticalAlignment = VerticalAlignment.Center;
                btn_apply.Width = 80;
                Grid.SetRow(btn_apply, 0);

                TransactionsFilterView t = new TransactionsFilterView();
                Grid.SetRow(t.GetGrid(), 0);

                //GRD_FilterTransactions.RowDefinitions.Add(row);
                GRD_FilterTransactions.Children.Add(btn_apply);
                GRD_FilterTransactions.Children.Add(btn_reset);
                int i = GRD_FilterTransactions.Children.Add(t.GetGrid());
            }
            else
            {
                RW_TransactionsFilter.Height = new GridLength(40, GridUnitType.Auto);
                GRD_FilterTransactions.Children.RemoveAt(3);
                GRD_FilterTransactions.Children.RemoveAt(2);
                GRD_FilterTransactions.Children.RemoveAt(1);
                //GRD_FilterTransactions.RowDefinitions.RemoveAt(1);
            }

            
            
        }
    }

    internal class TransactionsFilterView
    {
        private Grid grid;
        private TextBox TB_MinAmount;
        private TextBox TB_MaxAmount;
        



        public TransactionsFilterView()
        {
            grid = new Grid();

            for(int i = 0; i < 2; i++)
            {
                grid.RowDefinitions.Add(NewRow(40));
            }


            for (int i = 0; i < 4; i++)
            {
                grid.ColumnDefinitions.Add(NewColumn(100));
            }
            
            Label LBL_MinAmount = new Label();
            LBL_MinAmount.Content = "Min Amount";
            Grid.SetRow(LBL_MinAmount, 0);
            Grid.SetColumn(LBL_MinAmount, 0);

            TB_MinAmount = new TextBox();
            Grid.SetRow(TB_MinAmount, 0);
            Grid.SetColumn(TB_MinAmount, 1);

            Label LBL_MaxAmount = new Label() { Content = "Max Amount" };
            Grid.SetRow(LBL_MaxAmount, 0);
            Grid.SetColumn(LBL_MaxAmount, 2);
            
            TB_MaxAmount= new TextBox();
            Grid.SetRow(TB_MaxAmount, 0);
            Grid.SetColumn(TB_MaxAmount, 3);

            grid.Children.Add(LBL_MaxAmount);
            grid.Children.Add(TB_MaxAmount);
            grid.Children.Add(LBL_MinAmount);
            grid.Children.Add(TB_MinAmount);

        }

        public Grid GetGrid()
        {
            return grid;
        }

        private ColumnDefinition NewColumn(int width)
        {
            return new ColumnDefinition() { Width = new GridLength(width) };
        }

        private RowDefinition NewRow(int height)
        {
            return new RowDefinition() { Height = new GridLength(height) };
        }

    }
}
