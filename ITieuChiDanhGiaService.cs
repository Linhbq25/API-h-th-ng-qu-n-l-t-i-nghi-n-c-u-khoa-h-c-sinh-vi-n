using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface ITieuChiDanhGiaService
{
    Task<List<TieuChiDanhGiaDto>> GetAllAsync();
    Task<TieuChiDanhGiaDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemTieuChiDanhGiaDto dto);
    Task UpdateAsync(int id, CapNhatTieuChiDanhGiaDto dto);
    Task DeleteAsync(int id);
}