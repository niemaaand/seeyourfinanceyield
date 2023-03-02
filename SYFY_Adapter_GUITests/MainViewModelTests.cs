using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SYFY_Adapter_GUI;
using SYFY_Application.BusinessLogic;
using SYFY_Application.DatabaseAccess;
using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Adapter_GUI.Tests
{
    [TestClass()]
    public class MainViewModelTests
    {
        [TestMethod()]
        public void AddTagToTransactionTest()
        {
            // arrange
            BankingTransaction transaction = new BankingTransaction(Guid.Empty, Guid.Empty, 0, new DateTime(2020, 2, 25));
            TransactionTag tag = new TransactionTag("New Tag");

            Dictionary<Guid, TransactionTag> tags = new Dictionary<Guid, TransactionTag>();
            tags.Add(tag.Guid, tag);

            Mock<IDataBaseConnector> dbConnector = new Mock<IDataBaseConnector>();
            dbConnector.Setup(x => x.GetAllBankAccounts()).Returns(new Dictionary<Guid, BankAccount>());
            dbConnector.Setup(x => x.GetAllBankingTransactions()).Returns(new Dictionary<Guid, BankingTransaction>());
            dbConnector.Setup(x => x.GetAllTransactionTags()).Returns(tags);

            Mock<DataManagement> d = new Mock<DataManagement>(dbConnector.Object);

            MainViewModel mainViewModel = new MainViewModel(d.Object);

            // act
            mainViewModel.AddTagToTransaction(transaction, tag);

            // assert
            Assert.IsTrue(transaction.TransactionTags.Contains(tag.Guid)); // removed from data
            Assert.IsFalse(mainViewModel.currentlyAvailableTransactionTags.Contains(tag)); // correctly shown in gui
            Assert.IsTrue(mainViewModel.currentTransactionTags.Contains(tag));
        }
    }
}