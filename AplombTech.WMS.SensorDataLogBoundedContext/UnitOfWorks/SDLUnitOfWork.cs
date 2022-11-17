using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AplombTech.WMS.SensorDataLogBoundedContext.Facade;

namespace AplombTech.WMS.SensorDataLogBoundedContext.UnitOfWorks
{
    public class SDLUnitOfWork : IDisposable
    {
        //[ThreadStatic]
        private static SDLUnitOfWork _currentScope;
        private SensorDataLogContext _objectContext;
        private bool _isDisposed;  //, _saveAllChangesAtEndOfScope;

        /// <summary>
        /// Gets or sets a boolean value that indicates whether to automatically save
        /// all object changes at end of the scope.
        /// </summary>
        //public bool SaveAllChangesAtEndOfScope
        //{
        //    get { return _saveAllChangesAtEndOfScope; }
        //    set { _saveAllChangesAtEndOfScope = value; }
        //}

        /// <summary>
        /// Returns a reference to the CMSObjectContext that is created
        /// for the current scope. If no scope currently exists, null is returned.
        /// </summary>

        public SensorDataLogContext CurrentObjectContext => _currentScope?._objectContext;

        /// <summary>
        /// Default constructor. Object changes are not automatically saved
        /// at the end of the scope.
        /// </summary>
        //public WMSUnitOfWork()
        //    : this(false)
        //{ }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="saveAllChangesAtEndOfScope">
        /// A boolean value that indicates whether to automatically save
        /// all object changes at end of the scope.
        /// </param>
        public SDLUnitOfWork()
        {
            if (_currentScope != null && !_currentScope._isDisposed)
                throw new InvalidOperationException("ObjectContextScope instances " +
                "cannot be nested.");
            //_saveAllChangesAtEndOfScope = saveAllChangesAtEndOfScope;
            /* Create a new ObjectContext instance: */
            _objectContext = new SensorDataLogContext();
            _isDisposed = false;
            //Thread.BeginThreadAffinity();
            /* Set the current scope to this UnitOfWorkScope object: */
            _currentScope = this;
        }

        /// <summary>
        /// Called on the end of the scope. Disposes the NorthwindObjectContext.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                /* End of scope, so clear the thread static
                * _currentScope member: */
                _currentScope = null;
                //Thread.EndThreadAffinity();
                //if (_saveAllChangesAtEndOfScope)
                //    _objectContext.SaveChanges();
                /* Dispose the scoped ObjectContext instance: */
                _objectContext.Dispose();
                _objectContext = null;
                _isDisposed = true;
            }
        }
    }
}
