using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;
using MpAdmin.Server.Models;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private MpAdminContext _context;

        public LoginController(MpAdminContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CheckLoginInfo([FromBody] LoginInfoModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var item = await unitOfWork.UserRepo.FirstOrDefaultAsync(r => r.UserName == model.UserName && r.Password == model.Password);

                if (item != null)
                {
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
                            result = 2
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
