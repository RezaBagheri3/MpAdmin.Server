using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Enums;
using MpAdmin.Server.DateTimeExtensions;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class FactorStatistics : ControllerBase
    {
        private MpAdminContext _context;

        public FactorStatistics(MpAdminContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> FactorStatisticByCustomerType()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var CustomerFactorCount = unitOfWork.FactorRepo.Get(r => r.CustomerType == CustomerType.Customer).Count();
                var StoreFactorCount = unitOfWork.FactorRepo.Get(p => p.CustomerType == CustomerType.Store).Count();
                var CustomerTotalQuantity = unitOfWork.FactorRepo.Get(d => d.CustomerType == CustomerType.Customer).Select(c => c.TotalQuantity).Sum();
                var StoreTotalQuantity = unitOfWork.FactorRepo.Get(b => b.CustomerType == CustomerType.Store).Select(v => v.TotalQuantity).Sum();

                return Ok(
                    new
                    {
                        CustomerFactorCount,
                        StoreFactorCount,
                        CustomerTotalQuantity,
                        StoreTotalQuantity
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
        public async Task<ActionResult<int>> GeneralFactorStatistics()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var CustomerFactorsCount = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Customer).Count();
                var StoreFactorsCount = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Store).Count();

                var CustomerTotalQuantity = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Customer).Select(p => p.TotalQuantity).Sum();
                var StoreTotalQuantity = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Store).Select(p => p.TotalQuantity).Sum();

                var CustomerPayableAmount = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Customer).Select(p => p.PayableAmount).Sum();
                var StorePayableAmount = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Store).Select(p => p.PayableAmount).Sum();

                var CustomerTotalProfit = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Customer).Select(p => p.TotalProfit).Sum();
                var StoreTotalProfit = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Store).Select(p => p.TotalProfit).Sum();

                return Ok(
                    new
                    {
                        CustomerFactorsCount,
                        StoreFactorsCount,
                        CustomerTotalQuantity,
                        StoreTotalQuantity,
                        CustomerPayableAmount,
                        StorePayableAmount,
                        CustomerTotalProfit,
                        StoreTotalProfit
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
        public async Task<ActionResult<List<FactorStatisticByMonthModel>>> FactorsStatisticsByMonth()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                int Year = DateTime.Now.GetPersianYear();
                List<DAL.Entities.Factor> CustomerFactors = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Customer).ToList();
                List<DAL.Entities.Factor> StoreFactors = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Store).ToList();

                CustomerFactors = CustomerFactors.Where(r => r.DateTime.GetPersianYear() == Year).ToList();
                StoreFactors = StoreFactors.Where(r => r.DateTime.GetPersianYear() == Year).ToList();

                List<FactorStatisticByMonthModel> StatisticByMonth = new List<FactorStatisticByMonthModel>();
                for (int i = 1; i <= 12; i++)
                {
                    FactorStatisticByMonthModel model = new FactorStatisticByMonthModel();
                    model.CustomerFactorsCount = CustomerFactors.Where(r => r.DateTime.GetPersianMonth() == i).Count();
                    model.StoreFactorsCount = StoreFactors.Where(r => r.DateTime.GetPersianMonth() == i).Count();
                    model.CustomerTotalQuantity = CustomerFactors.Where(r => r.DateTime.GetPersianMonth() == i).Select(p => p.TotalQuantity).Sum();
                    model.StoreTotalQuantity = StoreFactors.Where(r => r.DateTime.GetPersianMonth() == i).Select(p => p.TotalQuantity).Sum();
                    model.CustomerTotalProfit = CustomerFactors.Where(r => r.DateTime.GetPersianMonth() == i).Select(p => p.TotalProfit).Sum();
                    model.StoreTotalProfit = StoreFactors.Where(r => r.DateTime.GetPersianMonth() == i).Select(p => p.TotalProfit).Sum();

                    StatisticByMonth.Add(model);
                }

                return Ok(
                    new
                    {
                        StatisticByMonth
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
