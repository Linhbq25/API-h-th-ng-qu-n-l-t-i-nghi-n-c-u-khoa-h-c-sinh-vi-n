using Microsoft.AspNetCore.Mvc;
using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Repositories;
using quanlyNCKH.API.Services;

namespace quanlyNCKH.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeTaiController(
    IDeTaiRepository repository,
    IDeTaiService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        string? tuKhoa,
        string? trangThai,
        int? maDotDangKy,
        int trang = 1,
        int soLuong = 10)
    {
        if (trang < 1) trang = 1;
        if (soLuong < 1 || soLuong > 100) soLuong = 10;

        var (data, total) = await repository.SearchAsync(
            tuKhoa, trangThai, maDotDangKy, trang, soLuong);

        return Ok(new
        {
            duLieu = data,
            tongSo = total,
            trang,
            soLuong,
            tongTrang = (int)Math.Ceiling(
                (double)total / soLuong)
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await repository.GetByIdAsync(id);

        return data == null
            ? NotFound(new { message = "Không tìm thấy đề tài!" })
            : Ok(data);
    }

    [HttpPost]
    public async Task<IActionResult> Add(ThemDeTaiDto dto)
    {
        try
        {
            var id = await service.AddAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id },
                new
                {
                    maDeTai = id,
                    message = "Thêm đề tài thành công."
                });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, CapNhatDeTaiDto dto)
    {
        try
        {
            var result = await service.UpdateAsync(id, dto);

            return result
                ? Ok(new { message = "Cập nhật đề tài thành công." })
                : NotFound(new { message = "Không tìm thấy đề tài!" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
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
                message = "Xóa đề tài thành công."
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