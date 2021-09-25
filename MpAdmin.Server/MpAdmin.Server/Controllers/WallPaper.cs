using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MpAdmin.Server.DAL.Context;
using MpAdmin.Server.Domain;

namespace MpAdmin.Server.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class WallPaper : ControllerBase
    {
        private MpAdminContext _context;

        public WallPaper(MpAdminContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<DAL.Entities.WallPaper>>> GetWallPapers()
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                var items = await unitOfWork.WallPaperRepo.GetAsync();

                if (items != null)
                {
                    return Ok(
                        new
                        {
                            result = 1,
                            wallpapers = items
                        }
                    );
                }
                else
                {
                    return Ok(
                        new
                        {
                            result = 2,
                            message = "هيچ کاغذي در بانک ثبت نشده است ."
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
