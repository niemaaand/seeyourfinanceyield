using System;
using SYFY_Model.model;
using SYFY_Core.DatabaseAccess;
using SYFY_Plugin_DatabaseSimulation;
using SYFY_Application.DatabaseAccess;
using SYFY_Application.BusinessLogic;
using System.Linq;
using SYFY_Plugin_GUI_WPF;
using SYFY_Adapter_GUI;
using System.Threading;

namespace SYFY
{
    class SYFYMain
    {
        [STAThread]
               
        static void Main(string[] args)
        {
            IDataBaseConnector dataManager = new DataBaseSimulator();
            DataManagement dataManagement = new DataManagement(dataManager);


            BankAccount b1 = dataManagement.GetAllBankAccounts().ElementAt(0).Value;
            BankAccount b2 = dataManagement.GetAllBankAccounts().ElementAt(1).Value;

            BankingTransaction t = new BankingTransaction(b1.Guid, b2.Guid, 3456, DateTime.Today);

            dataManagement.SaveBankingTransaction(t);

            BankingTransaction tnew = (BankingTransaction) t.Clone();
            tnew.Comment = "Neuer Kommentar";
            tnew.Amount = 108;
            dataManagement.SaveBankingTransaction(tnew);

            //MainWindow bankAccountView = new MainWindow(dataManager);
            //bankAccountView.ShowDialog();

            MainViewModel mainViewModel = new MainViewModel(dataManagement);
            MainWindow mainWindow = new MainWindow(dataManagement, mainViewModel);
            mainWindow.ShowDialog();


            /*Thread bankAccountViewThread = new Thread(BuildBankAccountView);
            bankAccountViewThread.SetApartmentState(ApartmentState.STA);
            bankAccountViewThread.Start();
            */




            // get data
            /*IBankingAccountLoader bankingAccountLoader = new BankingAccountLoadSimulator();
            bankingAccountLoader.saveBankAccount(bankingAccountLoader.createNewBankAccount("testfrom", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash));
            bankingAccountLoader.createNewBankAccount("testto", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash);
            
            IBankingTransactionLoader bankingTransactionLoader = new BankingTransactionLoadSimulator();
            //bankingTransactionLoader.SaveBankingTransaction(bankingTransactionLoader.CreateNewBankingTransaction());
            */
            Console.WriteLine("Hello World!");

        }

      /*  private static void BuildBankAccountView()
        {
            bankAccountView = new MainWindow(dataManager);

            Thread t = new Thread(x);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            
            waitUntilWindowClosed = new EventWaitHandle(false, EventResetMode.ManualReset);
            waitUntilWindowClosed.WaitOne();
        }

        private static void x(object obj)
        {
            bankAccountView.Show();
            bankAccountView.Closed += BankAccountView_Closed;
        }

        private static void BankAccountView_Closed(object sender, EventArgs e)
        {
            waitUntilWindowClosed.Set();
        }*/
    }
}
