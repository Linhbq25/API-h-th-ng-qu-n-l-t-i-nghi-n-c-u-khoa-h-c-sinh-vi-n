using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.Data;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController(DbConnectionFactory db) : ControllerBase
{
    [HttpGet("database")]
    public async Task<IActionResult> TestDatabase()
    {
        try
        {
            await using var connection = db.CreateConnection();
            await connection.OpenAsync();

            return Ok(new
            {
                message = "Kết nối SQL Server thành công!",
                database = connection.Database,
                server = connection.DataSource
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Kết nối SQL Server thất bại!",
                error = ex.Message
            });
        }
    }
}