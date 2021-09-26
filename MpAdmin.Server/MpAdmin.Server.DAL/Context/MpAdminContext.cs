using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Entities;

namespace MpAdmin.Server.DAL.Context
{
    public class MpAdminContext : DbContext
    {
        public MpAdminContext(DbContextOptions<MpAdminContext> options) : base(options)
        {

        }

        #region DbSets

        public DbSet<User> Users { get; set; }

        public DbSet<WallPaper> WallPapers { get; set; }

        public DbSet<BotChat> BotChats { get; set; }

        public DbSet<TelegramUser> TelegramUsers { get; set; }

        #endregion
    }
}
