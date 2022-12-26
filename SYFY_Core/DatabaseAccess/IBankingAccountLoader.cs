using System;
using System.Collections.Generic;
using System.Text;

using SYFY_Model.model;

namespace SYFY_Core.DatabaseAccess
{
    public interface IBankingAccountLoader
    {

        public Dictionary<Guid, BankAccount> getAllBankAccounts();

        public BankAccount getBankAccountByID(Guid guid);

        public void saveBankAccount(BankAccount bankAccount);

        public BankAccount createNewBankAccount(string name, string iban, string comment, 
            CURRENCIES currency, ACCOUNTTYPE accounttype);

    }
}
