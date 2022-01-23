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
                List<GetFactorModel> factorModels = new List<GetFactorModel>();

                List<DAL.Entities.Factor> Factors = await unitOfWork.FactorRepo.GetAsync(r => r.Final == Final.NotFinalized).Result.ToListAsync();

                if (Factors.Count == 0)
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
                    foreach (var item in Factors)
                    {
                        GetFactorModel model = new GetFactorModel()
                        {
                            id = item.Id,
                            customerId = item.CustomerId,
                            customerName = item.CustomerName,
                            customerType = (int)item.CustomerType,
                            dateTime = item.DateTime.ToPersianDate(),
                            discount = item.Discount,
                            totalQuantity = item.TotalQuantity,
                            totalAmount = item.TotalAmount,
                            totalProfit = item.TotalProfit,
                            final = (int)item.Final,
                            payableAmount = item.PayableAmount
                        };
                        factorModels.Add(model);
                    }

                    return Ok(
                        new
                        {
                            result = 1,
                            factorModels
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
