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

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> GeneralFactorStatistics([FromBody] GeneralFactorStatisticModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                List<DAL.Entities.Factor> CustomerFactors = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Customer).ToList();
                List<DAL.Entities.Factor> StoreFactors = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized && r.CustomerType == CustomerType.Store).ToList();

                var Year = 0;
                if (model.Status == 1)
                {
                    Year = DateTime.Now.GetPersianYear();
                }
                else
                {
                    Year = model.Year;
                }

                var CustomerFactorsCount = CustomerFactors.Where(c => c.DateTime.GetPersianYear() == Year).Count();
                var StoreFactorsCount = StoreFactors.Where(c => c.DateTime.GetPersianYear() == Year).Count();

                var CustomerTotalQuantity = CustomerFactors.Where(c => c.DateTime.GetPersianYear() == Year).Select(p => p.TotalQuantity).Sum();
                var StoreTotalQuantity = StoreFactors.Where(c => c.DateTime.GetPersianYear() == Year).Select(p => p.TotalQuantity).Sum();

                var BigCustomerPayableAmountList = new List<long>();
                List<int> SmallCustomerPayableAmountList = CustomerFactors.Where(c => c.DateTime.GetPersianYear() == Year).Select(p => p.PayableAmount).ToList();

                foreach (var item in SmallCustomerPayableAmountList)
                {
                    BigCustomerPayableAmountList.Add(item);
                }

                var BigStorePayableAmountList = new List<long>();
                List<int> SmallStorePayableAmountList = StoreFactors.Where(c => c.DateTime.GetPersianYear() == Year).Select(p => p.PayableAmount).ToList();

                foreach (var item in SmallStorePayableAmountList)
                {
                    BigStorePayableAmountList.Add(item);
                }

                long CustomerPayableAmount = BigCustomerPayableAmountList.Sum();
                long StorePayableAmount = BigStorePayableAmountList.Sum();

                var BigCustomerTotalProfitList = new List<long>();
                List<int> SmallCustomerTotalProfitList = CustomerFactors.Where(c => c.DateTime.GetPersianYear() == Year).Select(p => p.TotalProfit).ToList();

                foreach (var item in SmallCustomerTotalProfitList)
                {
                    BigCustomerTotalProfitList.Add(item);
                }

                var BigStoreTotalProfitList = new List<long>();
                List<int> SmallStoreTotalProfitList = StoreFactors.Where(c => c.DateTime.GetPersianYear() == Year).Select(p => p.TotalProfit).ToList();

                foreach (var item in SmallStoreTotalProfitList)
                {
                    BigStoreTotalProfitList.Add(item);
                }

                long CustomerTotalProfit = BigCustomerTotalProfitList.Sum();
                long StoreTotalProfit = BigStoreTotalProfitList.Sum();

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
                List<DAL.Entities.Factor> AllFactors = unitOfWork.FactorRepo.Get(r => r.Final == Final.Finalized).ToList();



                CustomerFactors = CustomerFactors.Where(r => r.DateTime.GetPersianYear() == Year).ToList();
                StoreFactors = StoreFactors.Where(r => r.DateTime.GetPersianYear() == Year).ToList();
                AllFactors = AllFactors.Where(r => r.DateTime.GetPersianYear() == Year).ToList();

                List<FactorStatisticByMonthModel> StatisticByMonth = new List<FactorStatisticByMonthModel>();
                List<int> ChartStatisticByFactorCount = new List<int>();
                List<int> ChartStatisticByTotalQuantity = new List<int>();
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

                    int MonthStatisticByFactorCount = AllFactors.Where(r => r.DateTime.GetPersianMonth() == i).Count();
                    ChartStatisticByFactorCount.Add(MonthStatisticByFactorCount);
                    int MonthStatisticByTotalQuantity = AllFactors.Where(r => r.DateTime.GetPersianMonth() == i).Select(p => p.TotalQuantity).Sum();
                    ChartStatisticByTotalQuantity.Add(MonthStatisticByTotalQuantity);
                }

                return Ok(
                    new
                    {
                        StatisticByMonth,
                        ChartStatisticByFactorCount,
                        ChartStatisticByTotalQuantity
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
