using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility.Db
{
    public class UnitOfWorkDbContextRunner : IDbContextRunner
    {
        public void RunInDbContext<TDbContext>(Action<TDbContext> runAction)
            where TDbContext : DbContext, new()
        {
            using (var unitOfWork = new UnitOfWork<TDbContext>())
            {
                runAction(unitOfWork.CurrentObjectContext);
            }
        }

        public TResult RunInDbContext<TDbContext, TResult>(Func<TDbContext, TResult> runFunction)
            where TDbContext : DbContext, new()
        {
            using (var unitOfWork = new UnitOfWork<TDbContext>())
            {
                return runFunction(unitOfWork.CurrentObjectContext);
            }
        }

        public void RunAndSaveInDbContext<TDbContext>(Action<TDbContext> runAction)
            where TDbContext : DbContext, new()
        {
            using (var unitOfWork = new UnitOfWork<TDbContext>())
            {
                runAction(unitOfWork.CurrentObjectContext);
                unitOfWork.CurrentObjectContext.SaveChanges();
            }
        }

        public TResult RunAndSaveInDbContext<TDbContext, TResult>(Func<TDbContext, TResult> runFunction)
            where TDbContext : DbContext, new()
        {
            using (var unitOfWork = new UnitOfWork<TDbContext>())
            {
                var result = runFunction(unitOfWork.CurrentObjectContext);
                unitOfWork.CurrentObjectContext.SaveChanges();
                return result;
            }
        }
    }
}
