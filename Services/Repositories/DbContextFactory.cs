using Services.EFCodeFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories
{
    public sealed class DbContextFactory : IDisposable
    {
        #region 单例
        private DbContextFactory() { }
        private volatile static RecordContext DbContext;
        public static RecordContext Instance
        {
            get
            {
                return DbContext ?? (DbContext = new RecordContext());
            }
        }

        #endregion
        public void Dispose()
        {
            DbContext?.Dispose();
        }
        ~DbContextFactory()
        {
            this.Dispose();
        }

    }
}
