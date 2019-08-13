using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestTasks.Data.Domains;

namespace TestN1.Controllers
{
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly EntityDataProvider _dataProvider;

        public DataController(EntityDataProvider dataProvider)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync([FromQuery] string get)
        {
            if (!Guid.TryParse(get, out Guid guid)) return BadRequest();

            var res = await _dataProvider.FindByIdAsync(guid);

            return Ok(res);
        }
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task PostAsync([FromQuery] string insert)
        {
           await _dataProvider.CreateAsync(insert);
        }
    }
}