using AnimeNotification.EntityFrameworkCore.Sqlite;
using System.Threading.Tasks;

namespace AnimeNotification.Sqlite
{
    public class TransactionService
    {
        private readonly AnimeNotificationDbContext _context;

        public TransactionService(AnimeNotificationDbContext context)
        {
            _context = context;
        }

        public async Task Start()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
