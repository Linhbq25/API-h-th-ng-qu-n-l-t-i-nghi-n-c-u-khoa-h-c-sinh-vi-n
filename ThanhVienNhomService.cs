using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class ThanhVienNhomService(IThanhVienNhomRepository repository)
    : IThanhVienNhomService
{
    public async Task<List<ThanhVienNhomDto>> GetAllAsync()
    {
        var data = await repository.GetAllAsync();
        return data.Select(Map).ToList();
    }

    public async Task<List<ThanhVienNhomDto>> GetByNhomAsync(int maNhom)
    {
        var data = await repository.GetByNhomAsync(maNhom);
        return data.Select(Map).ToList();
    }

    public async Task<ThanhVienNhomDto?> GetAsync(
        int maNhom,
        int maSinhVien)
    {
        var data = await repository.GetAsync(maNhom, maSinhVien);
        return data == null ? null : Map(data);
    }

    public async Task AddAsync(ThemThanhVienNhomDto dto)
    {
        if (!await repository.NhomTonTaiAsync(dto.MaNhom))
            throw new ArgumentException("Nhóm sinh viên không tồn tại.");

        if (!await repository.SinhVienTonTaiAsync(dto.MaSinhVien))
            throw new ArgumentException(
                "Sinh viên không tồn tại hoặc đã ngừng hoạt động.");

        if (await repository.DaLaThanhVienAsync(
                dto.MaNhom,
                dto.MaSinhVien))
            throw new InvalidOperationException(
                "Sinh viên đã là thành viên của nhóm.");

        if (await repository.SinhVienThuocNhomKhacAsync(
                dto.MaSinhVien,
                dto.MaNhom))
            throw new InvalidOperationException(
                "Sinh viên đã thuộc nhóm khác.");

        if (dto.LaNhomTruong &&
            await repository.DaCoNhomTruongAsync(dto.MaNhom))
            throw new InvalidOperationException(
                "Nhóm đã có nhóm trưởng.");

        var thanhVien = new ThanhVienNhom
        {
            MaNhom = dto.MaNhom,
            MaSinhVien = dto.MaSinhVien,
            LaNhomTruong = dto.LaNhomTruong
        };

        await repository.AddAsync(thanhVien);
    }

    public async Task DeleteAsync(int maNhom, int maSinhVien)
    {
        var data = await repository.GetAsync(maNhom, maSinhVien);

        if (data == null)
            throw new KeyNotFoundException(
                "Không tìm thấy thành viên trong nhóm.");

        if (data.LaNhomTruong)
            throw new InvalidOperationException(
                "Không thể xóa nhóm trưởng khỏi nhóm.");

        await repository.DeleteAsync(maNhom, maSinhVien);
    }

    private static ThanhVienNhomDto Map(ThanhVienNhom x)
    {
        return new ThanhVienNhomDto
        {
            MaNhom = x.MaNhom,
            MaSinhVien = x.MaSinhVien,
            LaNhomTruong = x.LaNhomTruong,
            NgayThamGia = x.NgayThamGia
        };
    }
}