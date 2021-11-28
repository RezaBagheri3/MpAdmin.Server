using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class Customer : ControllerBase
    {
        private MpAdminContext _context;

        public Customer(MpAdminContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<DAL.Entities.Customer>>> GetCustomersList()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var customers = await unitOfWork.CustomerRepo.GetAsync().Result.ToListAsync();

                if (customers.Count > 0)
                {
                    return Ok(
                        new
                        {
                            result = 1,
                            customers
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "هيچ مشتري در بانک ثبت نشده است ."
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
