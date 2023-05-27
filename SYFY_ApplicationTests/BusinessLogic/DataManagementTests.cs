using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SYFY_Application.BusinessLogic;
using SYFY_Application.DatabaseAccess;
using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SYFY_Application.BusinessLogic.Tests
{
    [TestClass()]
    public class DataManagementTests
    {
        [TestMethod()]
        public void SaveBankingTransactionTest_TransactionDoesNotExist()
        {
            // arrange
            BankAccount ba_from = new BankAccount("from");
            ba_from.Guid = new Guid("DAF5CD00-4DAF-4B05-A374-8D33D0C6D66C");

            BankAccount ba_to = new BankAccount("to");
            ba_to.Guid = new Guid("DD5F8960-27E1-4D2F-90E2-824E6EDC2109");

            BankingTransaction transaction = new BankingTransaction(ba_from.Guid, ba_to.Guid, 20, new DateTime(2020, 2, 25));
            BankingTransaction savedTransaction = (BankingTransaction)transaction.Clone();
            savedTransaction.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");

            Mock<IDataBaseConnectorAdapter> dbConnector = new Mock<IDataBaseConnectorAdapter>();
            dbConnector.Setup(x => x.ExistsBankingTransaction(transaction.Guid)).Returns(false);
            dbConnector.Setup(x => x.GetBankAccountByID(transaction.FromBankAccount)).Returns(ba_from);
            dbConnector.Setup(x => x.GetBankAccountByID(transaction.ToBankAccount)).Returns(ba_to);
            dbConnector.Setup(x => x.SaveBankAccount(ba_from)).Returns(ba_from);
            dbConnector.Setup(x => x.SaveBankAccount(ba_to)).Returns(ba_to);
            dbConnector.Setup(x => x.SaveBankingTransaction(transaction)).Returns(savedTransaction);

            DataManagement dataManagement = new DataManagement(dbConnector.Object);

            // act
            BankingTransaction result = dataManagement.SaveBankingTransaction(transaction);

            // assert
            Assert.IsTrue(result != savedTransaction); // copy of object
            Assert.IsTrue(result.Guid.Equals(savedTransaction.Guid));

            dbConnector.Verify(m => m.ExistsBankingTransaction(transaction.Guid), Times.Once);
            dbConnector.Verify(m => m.SaveBankAccount(ba_from), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankAccount(ba_to), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankingTransaction(transaction), Times.Exactly(1));

            Assert.IsTrue(ba_from.Amount == -20);
            Assert.IsTrue(ba_to.Amount == 20);
        }

        [TestMethod()]
        public void SaveBankingTransactionTest_TransactionExists()
        {
            // arrange
            BankAccount ba_from = new BankAccount("from");
            ba_from.Guid = new Guid("DAF5CD00-4DAF-4B05-A374-8D33D0C6D66C");

            BankAccount ba_to = new BankAccount("to");
            ba_to.Guid = new Guid("DD5F8960-27E1-4D2F-90E2-824E6EDC2109");

            BankingTransaction transaction = new BankingTransaction(ba_from.Guid, ba_to.Guid, 20, new DateTime(2020, 2, 25));
            BankingTransaction savedTransaction = (BankingTransaction)transaction.Clone();
            savedTransaction.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");

            Mock<IDataBaseConnectorAdapter> dbConnector = new Mock<IDataBaseConnectorAdapter>();
            dbConnector.Setup(x => x.ExistsBankingTransaction(transaction.Guid)).Returns(true);
            dbConnector.Setup(x => x.GetBankAccountByID(transaction.FromBankAccount)).Returns(ba_from);
            dbConnector.Setup(x => x.GetBankAccountByID(transaction.ToBankAccount)).Returns(ba_to);
            dbConnector.Setup(x => x.SaveBankAccount(ba_from)).Returns(ba_from);
            dbConnector.Setup(x => x.SaveBankAccount(ba_to)).Returns(ba_to);
            dbConnector.Setup(x => x.SaveBankingTransaction(transaction)).Returns(savedTransaction);
            dbConnector.Setup(x => x.GetBankingTransactionById(transaction.Guid)).Returns(transaction);

            DataManagement dataManagement = new DataManagement(dbConnector.Object);

            // act
            BankingTransaction result = dataManagement.SaveBankingTransaction(transaction);

            // assert
            Assert.IsTrue(result != savedTransaction); // copy of object
            Assert.IsTrue(result.Guid.Equals(savedTransaction.Guid));

            dbConnector.Verify(m => m.ExistsBankingTransaction(transaction.Guid), Times.Once);
            dbConnector.Verify(m => m.SaveBankAccount(ba_from), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankAccount(ba_to), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankingTransaction(transaction), Times.Exactly(1));

            Assert.IsTrue(ba_from.Amount == 0);
            Assert.IsTrue(ba_to.Amount == 0);
        }

        [TestMethod()]
        public void SaveBankingTransactionTest_TransactionExists_ChangeBAs()
        {
            // arrange
            BankAccount ba_from_old = new BankAccount("from");
            ba_from_old.Guid = new Guid("DAF5CD00-4DAF-4B05-A374-8D33D0C6D66C");

            BankAccount ba_from_new = new BankAccount("from_new");
            ba_from_new.Guid = new Guid("3E3BA0FD-CB41-49D7-87B0-6EAF2294CF7E");

            BankAccount ba_to = new BankAccount("to");
            ba_to.Guid = new Guid("DD5F8960-27E1-4D2F-90E2-824E6EDC2109");

            BankingTransaction transactionOld = new BankingTransaction(ba_from_old.Guid, ba_to.Guid, 20, new DateTime(2020, 2, 25));
            transactionOld.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");
            BankingTransaction transactionNew = new BankingTransaction(ba_from_new.Guid, ba_to.Guid, 50, new DateTime(2020, 2, 25));
            transactionNew.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");

            BankingTransaction savedTransaction = (BankingTransaction)transactionNew.Clone();

            Mock<IDataBaseConnectorAdapter> dbConnector = new Mock<IDataBaseConnectorAdapter>();
            dbConnector.Setup(x => x.ExistsBankingTransaction(transactionOld.Guid)).Returns(true);
            dbConnector.Setup(x => x.GetBankAccountByID(transactionOld.FromBankAccount)).Returns(ba_from_old);
            dbConnector.Setup(x => x.GetBankAccountByID(transactionOld.ToBankAccount)).Returns(ba_to);
            dbConnector.Setup(x => x.GetBankAccountByID(transactionNew.FromBankAccount)).Returns(ba_from_new);
            dbConnector.Setup(x => x.SaveBankAccount(ba_from_old)).Returns(ba_from_old);
            dbConnector.Setup(x => x.SaveBankAccount(ba_to)).Returns(ba_to);
            dbConnector.Setup(x => x.GetBankingTransactionById(transactionNew.Guid)).Returns(transactionOld);
            dbConnector.Setup(x => x.SaveBankingTransaction(transactionNew)).Returns(savedTransaction);

            DataManagement dataManagement = new DataManagement(dbConnector.Object);

            // act
            BankingTransaction result = dataManagement.SaveBankingTransaction(transactionNew);

            // assert
            Assert.IsTrue(result != savedTransaction); // copy of object
            Assert.IsTrue(result.Guid.Equals(savedTransaction.Guid));

            dbConnector.Verify(m => m.ExistsBankingTransaction(transactionOld.Guid), Times.Once);
            dbConnector.Verify(m => m.SaveBankAccount(ba_from_old), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankAccount(ba_from_new), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankAccount(ba_to), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankingTransaction(transactionNew), Times.Exactly(1));

            Assert.IsTrue(ba_from_old.Amount == 20);
            Assert.IsTrue(ba_from_new.Amount== -50);
            Assert.IsTrue(ba_to.Amount == 30);
        }

        [TestMethod()]
        public void SaveBankingTransactionTest_TransactionExists_Exception()
        {
            // arrange
            BankAccount ba_from_old = new BankAccount("from");
            ba_from_old.Guid = new Guid("DAF5CD00-4DAF-4B05-A374-8D33D0C6D66C");

            BankAccount ba_from_new = new BankAccount("from_new");
            ba_from_new.Guid = new Guid("3E3BA0FD-CB41-49D7-87B0-6EAF2294CF7E");

            BankAccount ba_to = new BankAccount("to");
            ba_to.Guid = new Guid("DD5F8960-27E1-4D2F-90E2-824E6EDC2109");

            BankingTransaction transactionOld = new BankingTransaction(ba_from_old.Guid, ba_to.Guid, 20, new DateTime(2020, 2, 25));
            transactionOld.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");
            BankingTransaction transactionNew = new BankingTransaction(ba_from_new.Guid, ba_to.Guid, 50, new DateTime(2020, 2, 25));
            transactionNew.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");

            BankingTransaction savedTransaction = (BankingTransaction)transactionNew.Clone();

            Mock<IDataBaseConnectorAdapter> dbConnector = new Mock<IDataBaseConnectorAdapter>();
            dbConnector.Setup(x => x.ExistsBankingTransaction(transactionOld.Guid)).Returns(true);
            dbConnector.Setup(x => x.GetBankAccountByID(transactionOld.FromBankAccount)).Returns(ba_from_old);
            dbConnector.Setup(x => x.GetBankAccountByID(transactionOld.ToBankAccount)).Returns(ba_to);
            dbConnector.Setup(x => x.GetBankAccountByID(transactionNew.FromBankAccount)).Returns(ba_from_new);

            dbConnector.Setup(x => x.SaveBankAccount(ba_from_old)).Throws(new Exception());

            dbConnector.Setup(x => x.GetBankingTransactionById(transactionNew.Guid)).Returns(transactionOld);
            dbConnector.Setup(x => x.SaveBankingTransaction(transactionNew)).Returns(savedTransaction);
            dbConnector.Setup(x => x.Rollback()).Verifiable();

            DataManagement dataManagement = new DataManagement(dbConnector.Object);

            // act/assert
            Assert.ThrowsException<Exception>(() => dataManagement.SaveBankingTransaction(transactionNew));

            // assert
            dbConnector.Verify(m => m.ExistsBankingTransaction(transactionOld.Guid), Times.Once);
            dbConnector.Verify(m => m.Rollback(), Times.Once);
        }


        [TestMethod()]
        public void DeleteBankingTransactionTest_TransactionExists()
        {
            // arrange
            BankAccount ba_from = new BankAccount("from");
            ba_from.Guid = new Guid("DAF5CD00-4DAF-4B05-A374-8D33D0C6D66C");

            BankAccount ba_to = new BankAccount("to");
            ba_to.Guid = new Guid("DD5F8960-27E1-4D2F-90E2-824E6EDC2109");

            BankingTransaction transaction = new BankingTransaction(ba_from.Guid, ba_to.Guid, 20, new DateTime(2020, 2, 25));
            BankingTransaction savedTransaction = (BankingTransaction)transaction.Clone();
            savedTransaction.Guid = new Guid("F4815BCF-7A41-4AD3-98FD-DB6997781A32");

            Mock<IDataBaseConnectorAdapter> dbConnector = new Mock<IDataBaseConnectorAdapter>();
            dbConnector.Setup(x => x.ExistsBankingTransaction(transaction.Guid)).Returns(true);
            dbConnector.Setup(x => x.GetBankAccountByID(transaction.FromBankAccount)).Returns(ba_from);
            dbConnector.Setup(x => x.GetBankAccountByID(transaction.ToBankAccount)).Returns(ba_to);
            dbConnector.Setup(x => x.SaveBankAccount(ba_from)).Returns(ba_from);
            dbConnector.Setup(x => x.SaveBankAccount(ba_to)).Returns(ba_to);
            dbConnector.Setup(x => x.GetBankingTransactionById(transaction.Guid)).Returns(transaction);
            dbConnector.Setup(x => x.DeleteBankingTransaction(transaction));

            DataManagement dataManagement = new DataManagement(dbConnector.Object);

            // act
            dataManagement.DeleteBankingTransaction(transaction);

            // assert            
            dbConnector.Verify(m => m.ExistsBankingTransaction(transaction.Guid), Times.Once);
            dbConnector.Verify(m => m.SaveBankAccount(ba_from), Times.Exactly(1));
            dbConnector.Verify(m => m.SaveBankAccount(ba_to), Times.Exactly(1));
            dbConnector.Verify(m => m.DeleteBankingTransaction(transaction), Times.Exactly(1));

            Assert.IsTrue(ba_from.Amount == 20);
            Assert.IsTrue(ba_to.Amount == -20);
        }

        

        [TestMethod()]
        public void DeleteBankingTransactionTest_TransactionDoesNotExists()
        {
            // arrange
            BankingTransaction transaction = new BankingTransaction(Guid.Empty, Guid.Empty, 20, new DateTime(2020, 2, 25));

            Mock<IDataBaseConnectorAdapter> dbConnector = new Mock<IDataBaseConnectorAdapter>();
            dbConnector.Setup(x => x.ExistsBankingTransaction(transaction.Guid)).Returns(false);

            DataManagement dataManagement = new DataManagement(dbConnector.Object);

            // act
            dataManagement.DeleteBankingTransaction(transaction);

            // assert            
            dbConnector.Verify(m => m.ExistsBankingTransaction(transaction.Guid), Times.Once);
        }

    }
}