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
    }
}
