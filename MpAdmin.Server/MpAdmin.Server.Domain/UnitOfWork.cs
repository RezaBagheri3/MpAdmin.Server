using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Entities;
using MpAdmin.Server.Domain.Repositories;

namespace MpAdmin.Server.Domain
{
    public class UnitOfWork
    {
        public MpAdminContext DbContext { get; private set; }

        public UnitOfWork(MpAdminContext dbContext)
        {
            DbContext = dbContext;
        }

        private UserRepo _userRepo;
        public UserRepo UserRepo => _userRepo ??= new UserRepo(DbContext);

        private WallPaperRepo _wallPaperRepo;
        public WallPaperRepo WallPaperRepo => _wallPaperRepo ??= new WallPaperRepo(DbContext);

        private BotChatRepo _botChatRepo;
        public BotChatRepo BotChatRepo => _botChatRepo ??= new BotChatRepo(DbContext);

        private TelegramUserRepo _telegramUserRepo;
        public TelegramUserRepo TelegramUserRepo => _telegramUserRepo ??= new TelegramUserRepo(DbContext);

        #region Methods

        public void Commit()
        {
            DbContext.Database.CommitTransaction();
        }

        public void Rollback()
        {
            DbContext.Database.RollbackTransaction();
        }

        public void Save()
        {
            try
            {
                DbContext.SaveChanges();
                if (DbContext.Database.CurrentTransaction != null)
                    Commit();
            }
            catch (Exception ex)
            {
                if (DbContext.Database.CurrentTransaction != null)
                    Rollback();

                throw ex;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await DbContext.SaveChangesAsync();
                if (DbContext.Database.CurrentTransaction != null)
                    Commit();
            }
            catch (Exception ex)
            {
                if (DbContext.Database.CurrentTransaction != null)
                    Rollback();
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        #endregion

    }
}
