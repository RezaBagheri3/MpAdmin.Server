using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Enums;
using MpAdmin.Server.Domain;

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
    }
}
