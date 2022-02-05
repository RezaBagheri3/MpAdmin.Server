using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Entities;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class UpdateFactor : ControllerBase
    {
        private MpAdminContext _context;

        public UpdateFactor(MpAdminContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> AddWallPaperToFactor([FromBody] AddWallpaperToFactorModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                DAL.Entities.Factor factor = unitOfWork.FactorRepo.FirstOrDefault(r => r.Id == model.factorId);
                if (factor != null)
                {
                    FactorWallPaper factorWall = new FactorWallPaper()
                    {
                        WallPaperCode = model.wallPaperCode,
                        BatchNumber = model.batchNumber,
                        BuyPrice = model.buyPrice,
                        SalePrice = model.salePrice,
                        Profit = (model.salePrice - model.buyPrice) * model.quantity,
                        Quantity = model.quantity,
                        TotalPrice = model.quantity * model.salePrice,
                        FactorId = model.factorId
                    };
                    unitOfWork.FactorWallPaperRepo.Create(factorWall);
                    await unitOfWork.SaveAsync();


                    factor.TotalAmount += factorWall.TotalPrice;
                    factor.TotalQuantity += model.quantity;
                    factor.TotalProfit += factorWall.Profit;
                    factor.PayableAmount += factorWall.TotalPrice;
                    unitOfWork.FactorRepo.Update(factor);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 1,
                            message = "کاغذ با موفقيت به فاکتور اضافه شد ."
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "فاکتوري براي اضافه کردن کاغذ يافت نشد ."
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
