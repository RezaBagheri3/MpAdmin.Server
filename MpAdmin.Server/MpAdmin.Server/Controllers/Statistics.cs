using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;

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

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> WallPaperAlbumStatistics([FromBody] WallPaperAlbumStatisticsModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var CheckAlbumExist = unitOfWork.WallPaperRepo.FirstOrDefault(s => s.Album == model.album);

                if (CheckAlbumExist != null)
                {
                    var wallPaperAlbumCodeCount = unitOfWork.WallPaperRepo.Get(f => f.Album == model.album).Count();

                    var totalWallPaperAlbumStock = await unitOfWork.WallPaperRepo.GetAsync(r => r.Album == model.album).Result.Select(p => p.Stock).SumAsync();

                    var totalWallPaperAlbumPrice = await unitOfWork.WallPaperRepo.GetAsync(c => c.Album == model.album).Result.Select(g => g.TotalPrice).SumAsync();

                    return Ok(
                        new
                        {
                            result = 1,
                            totalWallPaperAlbumStock,
                            wallPaperAlbumCodeCount,
                            totalWallPaperAlbumPrice
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "کاغذی با نام آلبوم وارد شده یافت نشد ."
                        }
                    );
                }
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
