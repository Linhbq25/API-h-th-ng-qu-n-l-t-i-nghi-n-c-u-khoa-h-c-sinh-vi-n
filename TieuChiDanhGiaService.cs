using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class TieuChiDanhGiaService(ITieuChiDanhGiaRepository repository)
    : ITieuChiDanhGiaService
{
    public async Task<List<TieuChiDanhGiaDto>> GetAllAsync()
        => (await repository.GetAllAsync()).Select(Map).ToList();

    public async Task<TieuChiDanhGiaDto?> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        return data == null ? null : Map(data);
    }

    public async Task<int> AddAsync(ThemTieuChiDanhGiaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TenTieuChi))
            throw new ArgumentException("Tên tiêu chí không được để trống.");

        if (dto.DiemToiDa <= 0)
            throw new ArgumentException("Điểm tối đa phải lớn hơn 0.");

        return await repository.AddAsync(new TieuChiDanhGia
        {
            TenTieuChi = dto.TenTieuChi,
            DiemToiDa = dto.DiemToiDa,
            MoTa = dto.MoTa,
            TrangThai = dto.TrangThai
        });
    }

    public async Task UpdateAsync(int id, CapNhatTieuChiDanhGiaDto dto)
    {
        var data = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException("Tiêu chí không tồn tại.");

        if (string.IsNullOrWhiteSpace(dto.TenTieuChi))
            throw new ArgumentException("Tên tiêu chí không được để trống.");

        if (dto.DiemToiDa <= 0)
            throw new ArgumentException("Điểm tối đa phải lớn hơn 0.");

        data.TenTieuChi = dto.TenTieuChi;
        data.DiemToiDa = dto.DiemToiDa;
        data.MoTa = dto.MoTa;
        data.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException("Tiêu chí không tồn tại.");

        await repository.DeleteAsync(id);
    }

    private static TieuChiDanhGiaDto Map(TieuChiDanhGia x) => new()
    {
        MaTieuChi = x.MaTieuChi,
        TenTieuChi = x.TenTieuChi,
        DiemToiDa = x.DiemToiDa,
        MoTa = x.MoTa,
        TrangThai = x.TrangThai
    };
}