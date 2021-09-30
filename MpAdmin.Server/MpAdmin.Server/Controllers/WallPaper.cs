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
    public class WallPaper : ControllerBase
    {
        private MpAdminContext _context;

        public WallPaper(MpAdminContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<DAL.Entities.WallPaper>>> GetWallPapers()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var items = await unitOfWork.WallPaperRepo.GetAsync().Result.OrderBy(r => r.Stock).ToListAsync();

                if (items.Count != 0)
                {
                    return Ok(
                        new
                        {
                            result = 1,
                            wallpapers = items
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            wallpapers = items,
                            message = "هيچ کاغذي در بانک ثبت نشده است ."
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

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<DAL.Entities.WallPaper>> AddWallPaper([FromBody] AddWallPaperModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                DAL.Entities.WallPaper item = new DAL.Entities.WallPaper()
                {
                    Code = model.code,
                    BatchNumber = model.batchNumber,
                    Album = model.album,
                    Stock = model.stock,
                    BuyPrice = model.buyPrice,
                    TotalPrice = model.stock * model.buyPrice
                };

                unitOfWork.WallPaperRepo.Create(item);
                await unitOfWork.SaveAsync();

                var NewWallPaper = unitOfWork.WallPaperRepo.FirstOrDefault(r => r.Code == model.code);

                return Ok(
                    new
                    {
                        result = 1,
                        wallpaper = NewWallPaper
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
        public async Task<ActionResult<int>> DeleteWallPaper([FromBody] WallPaperDeleteModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var CheckItem = unitOfWork.WallPaperRepo.FirstOrDefault(r => r.Id == model.wallPaperId);

                if (CheckItem != null)
                {
                    await unitOfWork.WallPaperRepo.DeleteAsync(CheckItem);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 1
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "چنين کاغذي در بانک براي حذف يافت نشد ."
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
