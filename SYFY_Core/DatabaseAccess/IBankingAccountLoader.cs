using System;
using System.Collections.Generic;
using System.Text;

using SYFY_Domain.model;

namespace SYFY_Application.DatabaseAccess
{
    public interface IBankingAccountLoader
    {

        public Dictionary<Guid, BankAccount> getAllBankAccounts();

        public BankAccount getBankAccountByID(Guid guid);

        public void saveBankAccount(BankAccount bankAccount);

       // public BankAccount createNewBankAccount(string name, string iban, string comment, 
        //    CURRENCIES currency, ACCOUNTTYPE accounttype);

    }
}
