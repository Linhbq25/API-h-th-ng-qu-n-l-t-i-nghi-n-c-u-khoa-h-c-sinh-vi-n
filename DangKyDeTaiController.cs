using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DangKyDeTaiController(IDangKyDeTaiService service)
    : ControllerBase
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
    public async Task<IActionResult> Add(ThemDangKyDeTaiDto dto)
    {
        try
        {
            var id = await service.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { MaDangKy = id });
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

    [HttpPut("{id:int}/duyet")]
    public async Task<IActionResult> Duyet(int id)
    {
        try
        {
            await service.DuyetAsync(id);

            return Ok(new
            {
                message = "Duyệt đăng ký đề tài thành công."
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

    [HttpPut("{id:int}/tu-choi")]
    public async Task<IActionResult> TuChoi(
        int id,
        TuChoiDangKyDeTaiDto dto)
    {
        try
        {
            await service.TuChoiAsync(id, dto);

            return Ok(new
            {
                message = "Từ chối đăng ký đề tài thành công."
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

    [HttpPut("{id:int}/huy")]
    public async Task<IActionResult> Huy(int id)
    {
        try
        {
            await service.HuyAsync(id);

            return Ok(new
            {
                message = "Hủy đăng ký thành công."
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.DeleteAsync(id);

            return Ok(new
            {
                message = "Xóa đăng ký thành công."
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