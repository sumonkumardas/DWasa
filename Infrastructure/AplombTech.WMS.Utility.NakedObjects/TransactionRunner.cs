using NakedObjects.Architecture.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility.NakedObjects
{
    public class TransactionRunner : ITransactionRunner
    {
        private readonly ITransactionManager _transactionManager;

        public TransactionRunner(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public void RunTransaction(Action runAction)
        {
            try
            {
                _transactionManager.StartTransaction();
                runAction();
                _transactionManager.EndTransaction();
            }
            catch (Exception ex)
            {
                _transactionManager.AbortTransaction();
                throw ex;
            }
        }

        public T RunTransaction<T>(Func<T> runFunction)
        {
            try
            {
                _transactionManager.StartTransaction();
                var result = runFunction();
                _transactionManager.EndTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _transactionManager.AbortTransaction();
                throw ex;
            }
        }
    }
}
