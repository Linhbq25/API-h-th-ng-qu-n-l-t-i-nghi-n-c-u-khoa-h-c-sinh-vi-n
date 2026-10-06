using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NhomSinhVienController(INhomSinhVienService service) : ControllerBase
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
    public async Task<IActionResult> Add(ThemNhomSinhVienDto dto)
    {
        try
        {
            var id = await service.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { MaNhom = id });
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        CapNhatNhomSinhVienDto dto)
    {
        try
        {
            await service.UpdateAsync(id, dto);

            return Ok(new
            {
                message = "Cập nhật nhóm sinh viên thành công."
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
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.DeleteAsync(id);

            return Ok(new
            {
                message = "Xóa nhóm sinh viên thành công."
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