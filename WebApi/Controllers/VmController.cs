using System.Configuration;
using System.Net;
using Corsinvest.ProxmoxVE.Api;
using Corsinvest.ProxmoxVE.Api.Shared.Models.Vm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/vm")]
    [ApiController]
    public class VmController : ControllerBase
    {
        private readonly ILogger<VmController> _logger;
        private readonly IConfiguration _configuration;
        private readonly VmService _vmService;

        public VmController(ILogger<VmController> logger, IConfiguration configuration, VmService vmService)
        {
            _configuration = configuration;
            _logger = logger;
            _vmService = vmService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("create")]
        public async Task<object> CreateVm(CreateVmRequest request)
        {
            var result = await _vmService.CreateVmAsync(request.Vm, request.Type);
            
            if (!result.IsSuccessStatusCode)
            {
                return BadRequest("Не смогли создать машину");
            }

            return result.Response;
        }

        [Authorize(Roles = "User")]
        [HttpGet("start")]
        public async Task<object> StartVm(int vmid)
        {
            var result = await _vmService.StartVm(vmid);
            if (result == null)
            {
                return BadRequest();
            }

            return result.Response;
        }

        [Authorize(Roles = "User")]
        [HttpGet("stop")]
        public async Task<object> StopVm(int vmid)
        {
            var result = await _vmService.StopVm(vmid);
            if (result == null)
            {
                return BadRequest();
            }

            return result.Response;
        }

        [Authorize(Roles = "User")]
        [HttpGet("status")]
        [Produces(typeof(VmBaseStatusCurrent))]
        public async Task<ActionResult<object>> GetStatus(int vmid)
        {
            var result = await _vmService.GetStatus(vmid);
            if (result != null)
            {
                return result["data"];
            }

            return BadRequest();
        }

        [Authorize(Roles = "User")]
        [HttpPost("setPassword")]
        public async Task<ActionResult> SetPassword(ChangeCredentialsRequest creds)
        {
            var result = await _vmService.SetPassword(creds.Vmid, creds.Username, creds.Password, creds.SshKey);
            if (result != null)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
