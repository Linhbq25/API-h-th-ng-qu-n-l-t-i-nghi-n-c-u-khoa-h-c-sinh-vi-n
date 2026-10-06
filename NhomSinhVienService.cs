using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class NhomSinhVienService(INhomSinhVienRepository repository)
    : INhomSinhVienService
{
    public async Task<List<NhomSinhVienDto>> GetAllAsync()
    {
        var data = await repository.GetAllAsync();

        return data.Select(Map).ToList();
    }

    public async Task<NhomSinhVienDto?> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);

        return data == null ? null : Map(data);
    }

    public async Task<int> AddAsync(ThemNhomSinhVienDto dto)
    {
        Validate(dto.MaNhomCode, dto.TenNhom);

        if (await repository.DaCoMaNhomCodeAsync(dto.MaNhomCode))
            throw new InvalidOperationException("Mã nhóm đã tồn tại.");

        if (!await repository.SinhVienTonTaiAsync(dto.MaNhomTruong))
            throw new ArgumentException("Sinh viên làm nhóm trưởng không tồn tại hoặc đã ngừng hoạt động.");

        if (await repository.SinhVienLaTruongNhomKhacAsync(dto.MaNhomTruong))
            throw new InvalidOperationException("Sinh viên này đã là nhóm trưởng của nhóm khác.");

        var nhom = new NhomSinhVien
        {
            MaNhomCode = dto.MaNhomCode,
            TenNhom = dto.TenNhom,
            MaNhomTruong = dto.MaNhomTruong,
            TrangThai = dto.TrangThai
        };

        return await repository.AddAsync(nhom);
    }

    public async Task UpdateAsync(int id, CapNhatNhomSinhVienDto dto)
    {
        var old = await repository.GetByIdAsync(id);

        if (old == null)
            throw new KeyNotFoundException("Không tìm thấy nhóm sinh viên.");

        Validate(dto.MaNhomCode, dto.TenNhom);

        if (await repository.DaCoMaNhomCodeAsync(dto.MaNhomCode, id))
            throw new InvalidOperationException("Mã nhóm đã tồn tại.");

        if (!await repository.SinhVienTonTaiAsync(dto.MaNhomTruong))
            throw new ArgumentException("Sinh viên làm nhóm trưởng không tồn tại hoặc đã ngừng hoạt động.");

        if (await repository.SinhVienLaTruongNhomKhacAsync(dto.MaNhomTruong, id))
            throw new InvalidOperationException("Sinh viên này đã là nhóm trưởng của nhóm khác.");

        old.MaNhomCode = dto.MaNhomCode;
        old.TenNhom = dto.TenNhom;
        old.MaNhomTruong = dto.MaNhomTruong;
        old.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(old);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException("Không tìm thấy nhóm sinh viên.");

        if (await repository.DangDuocSuDungAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa nhóm vì đang có thành viên hoặc đã đăng ký đề tài.");

        await repository.DeleteAsync(id);
    }

    private static void Validate(string maNhomCode, string tenNhom)
    {
        if (string.IsNullOrWhiteSpace(maNhomCode))
            throw new ArgumentException("Mã nhóm không được để trống.");

        if (string.IsNullOrWhiteSpace(tenNhom))
            throw new ArgumentException("Tên nhóm không được để trống.");
    }

    private static NhomSinhVienDto Map(NhomSinhVien x)
    {
        return new NhomSinhVienDto
        {
            MaNhom = x.MaNhom,
            MaNhomCode = x.MaNhomCode,
            TenNhom = x.TenNhom,
            MaNhomTruong = x.MaNhomTruong,
            TrangThai = x.TrangThai,
            NgayTao = x.NgayTao
        };
    }
}