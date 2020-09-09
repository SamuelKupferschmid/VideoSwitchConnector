using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BmdSwitcher;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace VideoSwitchConnector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SwitchersController : ControllerBase
    {
        private readonly Switcher switcher;

        public SwitchersController(Switcher switcher)
        {
            this.switcher = switcher;
        }

        [HttpGet]
        public IEnumerable<Switcher> Get()
        {
            yield return this.switcher;
        }
    }
}
