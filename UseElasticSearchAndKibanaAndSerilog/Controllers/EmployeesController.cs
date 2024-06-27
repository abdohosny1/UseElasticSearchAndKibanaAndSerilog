using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyUseElasticSearchAndKibanaAndSerilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeesController> _logger;



        public EmployeesController(IUnitOfWork unitOfWork, ILogger<EmployeesController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                throw new Exception("make erro in the app");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Something went wrong",5);
            }
            _logger.LogInformation("Hello from action ");
            return Ok("done ");
        }

    }
}
