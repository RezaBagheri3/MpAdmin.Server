using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Entities;
using WorkReportApp.Domain;

namespace MpAdmin.Server.Domain.Repositories
{
    public class WallPaperRepo : Repository<WallPaper, int>
    {
        public WallPaperRepo(MpAdminContext DbContext) : base(DbContext)
        {

        }
    }
}
