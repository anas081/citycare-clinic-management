using Microsoft.AspNetCore.Mvc;
using ClinicManagment.Services;
using System.Collections.Generic;

namespace ClinicManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly ClinicService _clinicService;

        public HistoryController(ClinicService clinicService)
        {
            _clinicService = clinicService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetHistory()
        {
            return Ok(_clinicService.GetHistory());
        }
    }
}
