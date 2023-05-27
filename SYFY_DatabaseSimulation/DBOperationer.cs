using System;

namespace SYFY_Plugin_DatabaseSimulation
{
    internal class DBOperationer
    {
        private bool _CurrentlyPerformingTransaction;

        public DBOperationer()
        {
            _CurrentlyPerformingTransaction = false;
        }

        public void Commit()
        {
            //TODO
            if (_CurrentlyPerformingTransaction == false)
            {
                throw new NullReferenceException("No data base transaction running currently!");
            }

            _CurrentlyPerformingTransaction = false;
            Console.WriteLine("Commit Transaction!");
        }

        public void Rollback()
        {
            //TODO
            if(_CurrentlyPerformingTransaction == false)
            {
                throw new NullReferenceException("No db-transaction going on currently, no rollback possible.");
            }
            _CurrentlyPerformingTransaction = false;
            Console.WriteLine("Rollback!");
        }

        public void OpenTransaction()
        {
            //TODO
            if(_CurrentlyPerformingTransaction == true)
            {
                throw new NullReferenceException("Previous db-transaction not committed yet.");
            }

            _CurrentlyPerformingTransaction = true;
            Console.WriteLine("Transaction started...");
        }
    }
}
