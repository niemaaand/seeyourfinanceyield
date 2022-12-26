using System;
using SYFY_Model.model;
using SYFY_Core.DatabaseAccess;
using SYFY_DatabaseSimulation;


namespace SYFY
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // get data
            IBankingAccountLoader bankingAccountLoader = new BankingAccountLoadSimulator();
            bankingAccountLoader.saveBankAccount(bankingAccountLoader.createNewBankAccount("test", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash));
            bankingAccountLoader.createNewBankAccount("test", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash);


            Console.WriteLine("Hello World!");

        }
    }
}
