using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DotDangKyController(IDotDangKyService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await service.GetByIdAsync(id);
        return data == null ? NotFound() : Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ThemDotDangKyDto dto)
    {
        var id = await service.AddAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { MaDotDangKy = id });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CapNhatDotDangKyDto dto)
    {
        await service.UpdateAsync(id, dto);
        return Ok(new { message = "Cập nhật đợt đăng ký thành công." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return Ok(new { message = "Xóa đợt đăng ký thành công." });
    }
}