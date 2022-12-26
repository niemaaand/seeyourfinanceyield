using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using SYFY_Core.DatabaseAccess;
using SYFY_Model.model;

namespace SYFY_DatabaseSimulation
{
    public class BankingAccountLoadSimulator : IBankingAccountLoader
    {

        private Dictionary<Guid, BankAccount> bankAccounts;
        public BankingAccountLoadSimulator()
        {
            bankAccounts = new Dictionary<Guid, BankAccount>();
        }

        Dictionary<Guid, BankAccount> IBankingAccountLoader.getAllBankAccounts()
        {            
            return bankAccounts;
        }

        BankAccount IBankingAccountLoader.getBankAccountByID(Guid guid)
        {
            if (bankAccounts.ContainsKey(guid)){
                return bankAccounts[guid];
            }
            else
            {
                throw new KeyNotFoundException(guid.ToString() + " does not exist.");
            }
        }

        void IBankingAccountLoader.saveBankAccount(BankAccount bankAccount)
        {
            bankAccounts.Add(bankAccount.Guid, bankAccount);    
        }

        BankAccount IBankingAccountLoader.createNewBankAccount(string name, string iban, string comment, CURRENCIES currency, ACCOUNTTYPE accounttype)
        {
            BankAccount bankAccount = new BankAccount(Guid.NewGuid());
            bankAccount.Name = name;
            bankAccount.Iban = iban;
            bankAccount.Comment = comment;
            bankAccount.Currency = currency;
            bankAccount.Accounttype = accounttype;

            return bankAccount;
        }
    }
}
