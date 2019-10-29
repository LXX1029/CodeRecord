namespace Services.Repositories
{
    using System;
    using Services.EFCodeFirst;

    public sealed class DbContextFactory : IDisposable
    {
        private DbContextFactory() { }

        private static volatile RecordContext dbContext;
        public static RecordContext Instance
        {
            get
            {
                return dbContext ?? (dbContext = new RecordContext());
            }
        }

        public void Dispose()
        {
            dbContext?.Dispose();
        }

        ~DbContextFactory()
        {
            Dispose();
        }
    }
}
