using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplombTech.WMS.Utility.Db
{
    public interface IDbContextRunner
    {
        void RunInDbContext<TDbContext>(Action<TDbContext> runAction)
            where TDbContext : DbContext, new();

        TResult RunInDbContext<TDbContext, TResult>(Func<TDbContext, TResult> runFunction)
            where TDbContext : DbContext, new();

        void RunAndSaveInDbContext<TDbContext>(Action<TDbContext> runAction)
            where TDbContext : DbContext, new();

        TResult RunAndSaveInDbContext<TDbContext, TResult>(Func<TDbContext, TResult> runFunction)
            where TDbContext : DbContext, new();
    }
}
