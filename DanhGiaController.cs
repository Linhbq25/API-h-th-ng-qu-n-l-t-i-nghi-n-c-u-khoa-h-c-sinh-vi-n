using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DanhGiaController(IDanhGiaService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await service.GetByIdAsync(id);
        return data == null ? NotFound() : Ok(data);
    }

    [HttpGet("dang-ky/{maDangKy}")]
    public async Task<IActionResult> GetByDangKy(int maDangKy)
        => Ok(await service.GetByDangKyAsync(maDangKy));

    [HttpPost]
    public async Task<IActionResult> Add(ThemDanhGiaDto dto)
    {
        try
        {
            var id = await service.AddAsync(dto);

            return Ok(new
            {
                maDanhGia = id,
                message = "Thêm đánh giá thành công."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.DeleteAsync(id);
            return Ok(new { message = "Xóa đánh giá thành công." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}