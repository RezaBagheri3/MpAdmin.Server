using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Entities;
using MpAdmin.Server.DAL.Enums;
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

                int FactorId = unitOfWork.FactorRepo.Get().OrderBy(r=>r.Id).LastOrDefault().Id;

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
    }
}
