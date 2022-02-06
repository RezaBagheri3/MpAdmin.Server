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

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> DeleteWallPaperFromFactor([FromBody] DeleteWallPaperFromFactorModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                FactorWallPaper WallPaper = unitOfWork.FactorWallPaperRepo.FirstOrDefault(p => p.Id == model.wallId);
                if (WallPaper != null)
                {
                    DAL.Entities.Factor factor = unitOfWork.FactorRepo.FirstOrDefault(r => r.Id == WallPaper.FactorId);

                    factor.TotalAmount -= WallPaper.TotalPrice;
                    factor.TotalQuantity -= WallPaper.Quantity;
                    factor.TotalProfit -= WallPaper.Profit;
                    factor.PayableAmount -= WallPaper.TotalPrice;
                    unitOfWork.FactorRepo.Update(factor);
                    await unitOfWork.SaveAsync();

                    await unitOfWork.FactorWallPaperRepo.DeleteAsync(WallPaper);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 1,
                            message = "حذف کاغذ از فاکتور با موفقيت انجام شد ."
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "چنين کاغذي براي حذف از فاکتور يافت نشد ."
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
        public async Task<ActionResult<int>> UpdateWallPaperOfFactor([FromBody] UpdateWallPaperOfFactorModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                FactorWallPaper FactorWall = unitOfWork.FactorWallPaperRepo.FirstOrDefault(r => r.Id == model.id);
                if (FactorWall != null)
                {
                    DAL.Entities.Factor Factor = unitOfWork.FactorRepo.FirstOrDefault(p => p.Id == FactorWall.FactorId);
                    int NewTotalPrice = model.quantity * model.salePrice;
                    int NewProfit = (model.salePrice - FactorWall.BuyPrice) * model.quantity;

                    if (FactorWall.Quantity < model.quantity)
                    {
                        int RemainQuantity = model.quantity - FactorWall.Quantity;
                        Factor.TotalQuantity += RemainQuantity;
                    }
                    else if (FactorWall.Quantity > model.quantity)
                    {
                        int RemainQuantity = FactorWall.Quantity - model.quantity;
                        Factor.TotalQuantity -= RemainQuantity;
                    }

                    if (FactorWall.Profit < NewProfit)
                    {
                        int RemainProfit = NewProfit - FactorWall.Profit;
                        Factor.TotalProfit += RemainProfit;
                    }
                    else if (FactorWall.Profit > NewProfit)
                    {
                        int RemainProfit = FactorWall.Profit - NewProfit;
                        Factor.TotalProfit -= RemainProfit;
                    }

                    if (FactorWall.TotalPrice < NewTotalPrice)
                    {
                        int RemainPrice = NewTotalPrice - FactorWall.TotalPrice;
                        Factor.TotalAmount += RemainPrice;
                        Factor.PayableAmount += RemainPrice;
                    }
                    else if (FactorWall.TotalPrice > NewTotalPrice)
                    {
                        int RemainPrice = FactorWall.TotalPrice - NewTotalPrice;
                        Factor.TotalAmount -= RemainPrice;
                        Factor.PayableAmount -= RemainPrice;
                    }

                    unitOfWork.FactorRepo.Update(Factor);
                    await unitOfWork.SaveAsync();

                    FactorWall.Quantity = model.quantity;
                    FactorWall.TotalPrice = NewTotalPrice;
                    FactorWall.Profit = NewProfit;
                    FactorWall.SalePrice = model.salePrice;

                    unitOfWork.FactorWallPaperRepo.Update(FactorWall);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 1,
                            message = "اطلاعات کاغذ با موفقيت ويرايش شد ."
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "چنين کاغذي براي ويرايش يافت نشد ."
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
