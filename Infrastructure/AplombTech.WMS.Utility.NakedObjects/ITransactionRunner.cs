using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility.NakedObjects
{
    public interface ITransactionRunner
    {
        void RunTransaction(Action runAction);
        T RunTransaction<T>(Func<T> runFunction);
    }
}
