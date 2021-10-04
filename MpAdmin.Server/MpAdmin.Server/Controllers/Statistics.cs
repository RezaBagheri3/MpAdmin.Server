using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class Statistics : ControllerBase
    {
        private MpAdminContext _context;

        public Statistics(MpAdminContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> WallPaperStatistics()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var wallPaperCodeCount = unitOfWork.WallPaperRepo.Get().Count();

                var totalWallPaperStock = await unitOfWork.WallPaperRepo.GetAsync().Result.Select(r => r.Stock).SumAsync();

                var totalWallPapersPrice = await unitOfWork.WallPaperRepo.GetAsync().Result.Select(p => p.TotalPrice).SumAsync();

                return Ok(
                    new
                    {
                        wallPaperCodeCount,
                        totalWallPaperStock,
                        totalWallPapersPrice
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(
                    new
                    {
                        e
                    }
                );
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> TelegramBotStatistics()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var botUserCount = unitOfWork.TelegramUserRepo.Get().Count();

                return Ok(
                    new
                    {
                        botUserCount
                    }
                );
            }
            catch (Exception e)
            {
                return BadRequest(
                    new
                    {
                        e
                    }
                );
            }
        }
    }
}
