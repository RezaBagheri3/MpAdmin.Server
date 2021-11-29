using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.DAL.Enums;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;

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

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<int>> AddCustomer([FromBody] AddCustomerModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                DAL.Entities.Customer item = new DAL.Entities.Customer()
                {
                    FullName = model.fullName,
                    PhoneNumber = model.phoneNumber,
                    Address = model.address
                };

                if (model.customerType == 1)
                {
                    item.CustomerType = CustomerType.Customer;
                }
                else
                {
                    item.CustomerType = CustomerType.Store;
                }

                unitOfWork.CustomerRepo.Create(item);
                await unitOfWork.SaveAsync();

                return Ok(
                    new
                    {
                        result = 1
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
        public async Task<ActionResult<int>> DeleteCustomer([FromBody] DeleteCustomerModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var item = unitOfWork.CustomerRepo.FirstOrDefault(r => r.Id == model.id);

                if (item != null)
                {
                    await unitOfWork.CustomerRepo.DeleteAsync(item);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 1
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "چنين مشتري اي در بانک وجود ندارد ."
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
        public async Task<ActionResult<int>> UpdateCustomer([FromBody] CustomerUpdateModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                DAL.Entities.Customer item = unitOfWork.CustomerRepo.FirstOrDefault(r => r.Id == model.id);

                if (item != null)
                {
                    item.FullName = model.fullName;
                    item.PhoneNumber = model.phoneNumber;
                    item.Address = model.address;

                    if (model.customerType == 1)
                    {
                        item.CustomerType = CustomerType.Customer;
                    }
                    else
                    {
                        item.CustomerType = CustomerType.Store;
                    }

                    unitOfWork.CustomerRepo.Update(item);
                    await unitOfWork.SaveAsync();

                    return Ok(
                        new
                        {
                            result = 1
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "چنين مشتري اي در بانک يافت نشد ."
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
