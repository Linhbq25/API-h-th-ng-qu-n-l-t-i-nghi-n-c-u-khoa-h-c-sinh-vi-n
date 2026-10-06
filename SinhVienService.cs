using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class SinhVienService(ISinhVienRepository repository) : ISinhVienService
{
    public async Task<List<SinhVienDto>> GetAllAsync()
        => (await repository.GetAllAsync())
            .Select(x => new SinhVienDto
            {
                MaSinhVien = x.MaSinhVien,
                MaTaiKhoan = x.MaTaiKhoan,
                MaSoSinhVien = x.MaSoSinhVien,
                HoTen = x.HoTen,
                Email = x.Email,
                Lop = x.Lop,
                Nganh = x.Nganh,
                TrangThai = x.TrangThai
            }).ToList();

    public async Task<SinhVienDto?> GetByIdAsync(int id)
    {
        var x = await repository.GetByIdAsync(id);

        if (x == null) return null;

        return new SinhVienDto
        {
            MaSinhVien = x.MaSinhVien,
            MaTaiKhoan = x.MaTaiKhoan,
            MaSoSinhVien = x.MaSoSinhVien,
            HoTen = x.HoTen,
            Email = x.Email,
            Lop = x.Lop,
            Nganh = x.Nganh,
            TrangThai = x.TrangThai
        };
    }

    public async Task<int> AddAsync(ThemSinhVienDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.MaSoSinhVien) ||
            string.IsNullOrWhiteSpace(dto.HoTen) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Lop) ||
            string.IsNullOrWhiteSpace(dto.Nganh))
            throw new ArgumentException("Thông tin sinh viên không được để trống.");

        if (!await repository.MaTaiKhoanTonTaiAsync(dto.MaTaiKhoan))
            throw new ArgumentException("Tài khoản không tồn tại.");

        if (await repository.DaCoSinhVienAsync(dto.MaTaiKhoan))
            throw new InvalidOperationException(
                "Tài khoản này đã được liên kết với sinh viên khác.");

        if (await repository.DaCoMaSoSinhVienAsync(dto.MaSoSinhVien.Trim()))
            throw new InvalidOperationException("Mã số sinh viên đã tồn tại.");

        if (await repository.DaCoEmailAsync(dto.Email.Trim()))
            throw new InvalidOperationException("Email đã tồn tại.");

        var sinhVien = new SinhVien
        {
            MaTaiKhoan = dto.MaTaiKhoan,
            MaSoSinhVien = dto.MaSoSinhVien.Trim(),
            HoTen = dto.HoTen.Trim(),
            Email = dto.Email.Trim(),
            Lop = dto.Lop.Trim(),
            Nganh = dto.Nganh.Trim(),
            TrangThai = dto.TrangThai
        };

        return await repository.AddAsync(sinhVien);
    }

    public async Task UpdateAsync(int id, CapNhatSinhVienDto dto)
    {
        var sinhVien = await repository.GetByIdAsync(id);

        if (sinhVien == null)
            throw new KeyNotFoundException("Không tìm thấy sinh viên.");

        if (string.IsNullOrWhiteSpace(dto.MaSoSinhVien) ||
            string.IsNullOrWhiteSpace(dto.HoTen) ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Lop) ||
            string.IsNullOrWhiteSpace(dto.Nganh))
            throw new ArgumentException("Thông tin sinh viên không được để trống.");

        sinhVien.MaSoSinhVien = dto.MaSoSinhVien.Trim();
        sinhVien.HoTen = dto.HoTen.Trim();
        sinhVien.Email = dto.Email.Trim();
        sinhVien.Lop = dto.Lop.Trim();
        sinhVien.Nganh = dto.Nganh.Trim();
        sinhVien.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(sinhVien);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException("Không tìm thấy sinh viên.");

        if (await repository.DangDuocSuDungAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa sinh viên vì đang tham gia nhóm.");

        await repository.DeleteAsync(id);
    }
}