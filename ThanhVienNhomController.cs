using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThanhVienNhomController(IThanhVienNhomService service)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("nhom/{maNhom:int}")]
    public async Task<IActionResult> GetByNhom(int maNhom)
        => Ok(await service.GetByNhomAsync(maNhom));

    [HttpGet("{maNhom:int}/{maSinhVien:int}")]
    public async Task<IActionResult> Get(
        int maNhom,
        int maSinhVien)
    {
        var data = await service.GetAsync(maNhom, maSinhVien);

        return data == null ? NotFound() : Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ThemThanhVienNhomDto dto)
    {
        try
        {
            await service.AddAsync(dto);

            return Ok(new
            {
                message = "Thêm thành viên vào nhóm thành công."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{maNhom:int}/{maSinhVien:int}")]
    public async Task<IActionResult> Delete(
        int maNhom,
        int maSinhVien)
    {
        try
        {
            await service.DeleteAsync(maNhom, maSinhVien);

            return Ok(new
            {
                message = "Xóa thành viên khỏi nhóm thành công."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}