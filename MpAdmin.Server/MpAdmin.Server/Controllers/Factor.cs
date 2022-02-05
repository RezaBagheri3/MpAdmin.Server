using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Entities;
using MpAdmin.Server.DAL.Enums;
using MpAdmin.Server.DateTimeExtensions;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class Factor : ControllerBase
    {
        private MpAdminContext _context;

        public Factor(MpAdminContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> AddFactor([FromBody] AddFactorModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                DAL.Entities.Factor FactorItem = new DAL.Entities.Factor()
                {
                    CustomerId = model.customerId,
                    CustomerName = model.customerName,
                    CustomerType = model.customerType == 1 ? CustomerType.Customer : CustomerType.Store,
                    DateTime = DateTime.Now,
                    TotalQuantity = model.totalQuantity,
                    TotalAmount = model.totalAmount,
                    Final = Final.NotFinalized,
                    TotalProfit = model.factorWallPapers.Select(r => r.profit).Sum(),
                    Discount = model.discount,
                    PayableAmount = model.payableAmount
                };

                unitOfWork.FactorRepo.Create(FactorItem);
                await unitOfWork.SaveAsync();

                int FactorId = unitOfWork.FactorRepo.Get().OrderBy(r => r.Id).LastOrDefault().Id;

                foreach (var factorWallPaper in model.factorWallPapers)
                {
                    FactorWallPaper WallPaperItem = new FactorWallPaper()
                    {
                        WallPaperCode = factorWallPaper.wallPaperCode,
                        BatchNumber = factorWallPaper.batchNumber,
                        Quantity = factorWallPaper.quantity,
                        BuyPrice = factorWallPaper.buyPrice,
                        SalePrice = factorWallPaper.salePrice,
                        Profit = factorWallPaper.profit,
                        TotalPrice = factorWallPaper.totalPrice,
                        FactorId = FactorId
                    };

                    unitOfWork.FactorWallPaperRepo.Create(WallPaperItem);
                    await unitOfWork.SaveAsync();
                }

                return Ok(
                    new
                    {
                        result = 1
                    }
                );
            }
            catch (Exception e)
            {
                return Ok(
                    new
                    {
                        e
                    }
                );
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<DAL.Entities.Factor>>> GetNotFinalizedFactor()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var Factors = unitOfWork.FactorRepo.Get(r => r.Final == Final.NotFinalized).Select(p => new
                {
                    p.Id,
                    p.CustomerName,
                    p.CustomerType,
                    DateTime = p.DateTime.ToPersianDate(),
                    p.Final,
                    p.TotalAmount,
                    p.Discount,
                    p.PayableAmount,
                    p.TotalQuantity,
                    p.TotalProfit,
                    p.CustomerId,
                    factorWallPapers = p.FactorWallPapers.Select(t => new
                    {
                        t.Id,
                        t.WallPaperCode,
                        t.Quantity,
                        t.BuyPrice,
                        t.SalePrice,
                        t.Profit,
                        t.TotalPrice,
                        t.FactorId
                    })
                });

                if (Factors.Count() == 0)
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "هیچ فاکتور نهایی نشده ای موجود نمی باشد ."
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 1,
                            Factors
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
        public async Task<ActionResult<int>> FinalizedFactor([FromBody] FinalFactorModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                DAL.Entities.Factor Factor = unitOfWork.FactorRepo.SingleOrDefault(r => r.Id == model.id);

                if (model.final == 1)
                {
                    Factor.Final = Final.Finalized;

                    unitOfWork.FactorRepo.Update(Factor);
                    await unitOfWork.SaveAsync();

                    List<FactorWallPaper> FactorWallPapers = await unitOfWork.FactorWallPaperRepo.GetAsync(p => p.FactorId == model.id).Result.ToListAsync();

                    foreach (var item in FactorWallPapers)
                    {
                        if (item.BatchNumber == "")
                        {
                            DAL.Entities.WallPaper WallPaper = unitOfWork.WallPaperRepo.SingleOrDefault(c => c.Code == item.WallPaperCode);
                            WallPaper.Stock -= item.Quantity;

                            unitOfWork.WallPaperRepo.Update(WallPaper);
                            await unitOfWork.SaveAsync();
                        }
                        else
                        {
                            DAL.Entities.WallPaper WallPaper = unitOfWork.WallPaperRepo.SingleOrDefault(c => c.Code == item.WallPaperCode && c.BatchNumber == item.BatchNumber);
                            WallPaper.Stock -= item.Quantity;

                            unitOfWork.WallPaperRepo.Update(WallPaper);
                            await unitOfWork.SaveAsync();
                        }
                    }

                    return Ok(
                        new
                        {
                            result = 1,
                            message = "فاکتور مدنظر با موفقیت ثبت نهایی گردید ."
                        }
                    );
                }
                else
                {
                    await unitOfWork.FactorRepo.DeleteAsync(Factor);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 2,
                            message = "فاکتور مدنظر با موفقیت حذف شد ."
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

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<DAL.Entities.Factor>>> GetFinalizedFactor()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var Factors = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized).OrderByDescending(d => d).Take(100).Select(p => new
                {
                    p.Id,
                    p.CustomerName,
                    p.CustomerType,
                    DateTime = p.DateTime.ToPersianDate(),
                    p.Final,
                    p.TotalAmount,
                    p.Discount,
                    p.PayableAmount,
                    p.TotalQuantity,
                    p.TotalProfit,
                    p.CustomerId,
                    factorWallPapers = p.FactorWallPapers.Select(t => new
                    {
                        t.Id,
                        t.WallPaperCode,
                        t.Quantity,
                        t.BuyPrice,
                        t.SalePrice,
                        t.Profit,
                        t.TotalPrice,
                        t.FactorId
                    })
                });

                if (Factors.Count() > 0)
                {
                    return Ok(
                        new
                        {
                            Result = 1,
                            Factors
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "هیچ فاکتور نهایی شده ای یافت نشد ."
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
        public async Task<ActionResult<DAL.Entities.Factor>> GetFactorById([FromBody] GetFactorByIdModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var Factors = unitOfWork.FactorRepo.Get(r => r.Id == model.id).Select(p => new
                {
                    p.Id,
                    p.CustomerName,
                    p.CustomerType,
                    DateTime = p.DateTime.ToPersianDate(),
                    p.Final,
                    p.TotalAmount,
                    p.PayableAmount,
                    p.TotalQuantity,
                    p.TotalProfit,
                    p.Discount,
                    p.FactorWallPapers
                });

                if (Factors.Count() != 0)
                {
                    return Ok(
                        new
                        {
                            result = 1,
                            Factor = Factors.First()
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "فاکتور با چنين شناسه اي يافت نشد ."
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
