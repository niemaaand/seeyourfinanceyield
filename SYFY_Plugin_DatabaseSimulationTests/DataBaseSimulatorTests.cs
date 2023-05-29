using Microsoft.VisualStudio.TestTools.UnitTesting;
using SYFY_Domain.model;

namespace SYFY_Plugin_DatabaseSimulation.Tests
{
    [TestClass()]
    public class DataBaseSimulatorTests
    {
        [TestMethod()]
        public void CommitTest_Exception()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();

            // act/assert
            Assert.ThrowsException<NullReferenceException>(() => dataBaseSimulator.Commit());
        }

        [TestMethod()]
        public void CommitTest()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();
            dataBaseSimulator.StartDBTransaction();

            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                // act
                dataBaseSimulator.Commit();

                // assert
                string expected = string.Format("Commit Transaction!{0}", Environment.NewLine);
                Assert.AreEqual<string>(expected, stringWriter.ToString());
            }
        }

        [TestMethod()]
        public void ExistsBankingTransactionTest_Negative()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();


            // act
            bool result = dataBaseSimulator.ExistsBankingTransaction(new Guid("FDFD1B24-B1C5-49F1-98F4-C6BA753A5596"));

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void ExistsBankingTransactionTest_Positive()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();
            BankingTransaction bankingTransaction = new BankingTransaction(Guid.Empty, Guid.Empty, 0, new DateTime(0));
            bankingTransaction = dataBaseSimulator.SaveBankingTransaction(bankingTransaction);

            // act
            bool result = dataBaseSimulator.ExistsBankingTransaction(bankingTransaction.Guid);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void StartDBTransactionTest()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();

            // act
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);

                // act
                dataBaseSimulator.StartDBTransaction();

                // assert
                string expected = string.Format("Transaction started...{0}", Environment.NewLine);
                Assert.AreEqual<string>(expected, stringWriter.ToString());
            }
        }

        [TestMethod()]
        public void StartDBTransactionTest_Exception()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();
            dataBaseSimulator.StartDBTransaction();


            // act/assert
            Assert.ThrowsException<NullReferenceException>(() => dataBaseSimulator.StartDBTransaction());
        }

        [TestMethod()]
        public void DeleteBankingTransactionTest()
        {
            // arrange
            DataBaseSimulator dataBaseSimulator = new DataBaseSimulator();
            BankingTransaction bankingTransaction = new BankingTransaction(Guid.Empty, Guid.Empty, 0, new DateTime(0));
            bankingTransaction = dataBaseSimulator.SaveBankingTransaction(bankingTransaction);

            // act
            dataBaseSimulator.DeleteBankingTransaction(bankingTransaction);

            // assert
            Assert.IsTrue(dataBaseSimulator.GetBankingTransactionById(bankingTransaction.Guid).Deleted);
        }
    }
}