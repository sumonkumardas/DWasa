using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility.Db
{
    public class UnitOfWork<TDbContext> : IDisposable where TDbContext : DbContext, new()
    {
        private static UnitOfWork<TDbContext> _currentScope;
        private TDbContext _objectContext;
        private bool _isDisposed;


        public TDbContext CurrentObjectContext => _currentScope?._objectContext;

        public UnitOfWork()
        {
            if (_currentScope != null && !_currentScope._isDisposed)
                throw new InvalidOperationException("ObjectContextScope instances cannot be nested.");

            _objectContext = new TDbContext();
            _isDisposed = false;
            _currentScope = this;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _currentScope = null;
                _objectContext.Dispose();
                _objectContext = null;
                _isDisposed = true;
            }
        }
    }
}
