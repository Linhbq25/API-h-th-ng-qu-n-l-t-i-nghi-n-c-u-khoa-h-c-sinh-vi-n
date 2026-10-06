using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaiNopController(
    IBaiNopService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await service.GetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var data = await service.GetByIdAsync(id);

            return data == null
                ? NotFound(new { message = "Không tìm thấy bài nộp." })
                : Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("dang-ky/{maDangKy}")]
    public async Task<IActionResult> GetByDangKy(int maDangKy)
    {
        try
        {
            return Ok(
                await service.GetByDangKyAsync(maDangKy));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        ThemBaiNopDto dto)
    {
        try
        {
            var id = await service.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new { maBaiNop = id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        CapNhatBaiNopDto dto)
    {
        try
        {
            await service.UpdateAsync(id, dto);

            return Ok(new
            {
                message = "Cập nhật bài nộp thành công."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await service.DeleteAsync(id);

            return Ok(new
            {
                message = "Xóa bài nộp thành công."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}