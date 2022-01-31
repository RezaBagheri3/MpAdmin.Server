using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Enums;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;
using MpAdmin.Server.DateTimeExtensions;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CustomerStatistic : ControllerBase
    {
        private MpAdminContext _context;

        public CustomerStatistic(MpAdminContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> FactorStatistic([FromBody] GetCustomerStatisticModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                PersianCalendar pc = new PersianCalendar();
                var PersianMonth = pc.GetMonth(DateTime.Now);

                List<DAL.Entities.Factor> Factors = await unitOfWork.FactorRepo.GetAsync(r => r.CustomerId == model.id && r.Final == Final.Finalized).Result.ToListAsync();
                List<DAL.Entities.Factor> LastMonthFactors = Factors.Where(p => p.DateTime.GetPersianMonth() == PersianMonth).ToList();

                return Ok(
                    new
                    {
                        LastMonthTotalFactor = LastMonthFactors.Count,
                        LastMonthTotalQuantity = LastMonthFactors.Sum(f => f.TotalQuantity),
                        TotalFactor = Factors.Count,
                        TotalQuantity = Factors.Sum(d => d.TotalQuantity)
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
        public async Task<ActionResult<List<CustomersActivityModel>>> CustomersActivity()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var CustomersByFactor = unitOfWork.CustomerRepo.Get(c => c.CustomerType == CustomerType.Customer).OrderByDescending(r => r.Factors.Where(g => g.Final == Final.Finalized).Count()).Select(p => new
                {
                    customerName = p.FullName,
                    count = p.Factors.Where(g => g.Final == Final.Finalized).Count()
                }).ToList();
                var CustomersByQuantity = unitOfWork.CustomerRepo.Get(c => c.CustomerType == CustomerType.Customer).OrderByDescending(r => r.Factors.Where(g => g.Final == Final.Finalized).Select(p => p.TotalQuantity).Sum()).Select(p => new
                {
                    customerName = p.FullName,
                    quantity = p.Factors.Where(g => g.Final == Final.Finalized).Select(p => p.TotalQuantity).Sum()
                }).ToList();

                return Ok(
                    new
                    {
                        CustomersByFactor,
                        CustomersByQuantity
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
