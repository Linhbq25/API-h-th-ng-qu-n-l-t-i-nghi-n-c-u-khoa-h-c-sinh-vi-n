using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IDanhGiaService
{
    Task<List<DanhGiaDto>> GetAllAsync();
    Task<DanhGiaDto?> GetByIdAsync(int id);
    Task<List<DanhGiaDto>> GetByDangKyAsync(int maDangKy);
    Task<int> AddAsync(ThemDanhGiaDto dto);
    Task DeleteAsync(int id);
}