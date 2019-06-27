using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
   //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityTestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            // return all the current user claims to prove the api authenticated correctly
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
