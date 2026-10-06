using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class GiangVienService(IGiangVienRepository repository)
    : IGiangVienService
{
    public async Task<List<GiangVienDto>> GetAllAsync()
        => (await repository.GetAllAsync())
            .Select(x => new GiangVienDto
            {
                MaGiangVien = x.MaGiangVien,
                MaTaiKhoan = x.MaTaiKhoan,
                MaSoGiangVien = x.MaSoGiangVien,
                HoTen = x.HoTen,
                Email = x.Email,
                BoMon = x.BoMon,
                TrangThai = x.TrangThai
            }).ToList();

    public async Task<GiangVienDto?> GetByIdAsync(int id)
    {
        var x = await repository.GetByIdAsync(id);

        if (x == null) return null;

        return new GiangVienDto
        {
            MaGiangVien = x.MaGiangVien,
            MaTaiKhoan = x.MaTaiKhoan,
            MaSoGiangVien = x.MaSoGiangVien,
            HoTen = x.HoTen,
            Email = x.Email,
            BoMon = x.BoMon,
            TrangThai = x.TrangThai
        };
    }

    public async Task<int> AddAsync(ThemGiangVienDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.MaSoGiangVien) ||
            string.IsNullOrWhiteSpace(dto.HoTen) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.BoMon))
            throw new ArgumentException(
                "Thông tin giảng viên không được để trống.");

        if (!await repository.MaTaiKhoanTonTaiAsync(dto.MaTaiKhoan))
            throw new ArgumentException("Tài khoản không tồn tại.");

        if (await repository.DaCoGiangVienAsync(dto.MaTaiKhoan))
            throw new InvalidOperationException(
                "Tài khoản này đã được liên kết với giảng viên khác.");

        if (await repository.DaCoMaSoGiangVienAsync(dto.MaSoGiangVien.Trim()))
            throw new InvalidOperationException(
                "Mã số giảng viên đã tồn tại.");

        if (await repository.DaCoEmailAsync(dto.Email.Trim()))
            throw new InvalidOperationException("Email đã tồn tại.");

        var giangVien = new GiangVien
        {
            MaTaiKhoan = dto.MaTaiKhoan,
            MaSoGiangVien = dto.MaSoGiangVien.Trim(),
            HoTen = dto.HoTen.Trim(),
            Email = dto.Email.Trim(),
            BoMon = dto.BoMon.Trim(),
            TrangThai = dto.TrangThai
        };

        return await repository.AddAsync(giangVien);
    }

    public async Task UpdateAsync(int id, CapNhatGiangVienDto dto)
    {
        var giangVien = await repository.GetByIdAsync(id);

        if (giangVien == null)
            throw new KeyNotFoundException("Không tìm thấy giảng viên.");

        if (string.IsNullOrWhiteSpace(dto.MaSoGiangVien) ||
            string.IsNullOrWhiteSpace(dto.HoTen) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.BoMon))
            throw new ArgumentException(
                "Thông tin giảng viên không được để trống.");

        if (await repository.DaCoMaSoGiangVienAsync(
                dto.MaSoGiangVien.Trim(), id))
            throw new InvalidOperationException(
                "Mã số giảng viên đã tồn tại.");

        if (await repository.DaCoEmailAsync(dto.Email.Trim(), id))
            throw new InvalidOperationException("Email đã tồn tại.");

        giangVien.MaSoGiangVien = dto.MaSoGiangVien.Trim();
        giangVien.HoTen = dto.HoTen.Trim();
        giangVien.Email = dto.Email.Trim();
        giangVien.BoMon = dto.BoMon.Trim();
        giangVien.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(giangVien);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException("Không tìm thấy giảng viên.");

        if (await repository.DangDuocSuDungAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa giảng viên vì đang hướng dẫn đề tài.");

        await repository.DeleteAsync(id);
    }
}