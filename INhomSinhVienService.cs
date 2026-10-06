using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface INhomSinhVienService
{
    Task<List<NhomSinhVienDto>> GetAllAsync();
    Task<NhomSinhVienDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemNhomSinhVienDto dto);
    Task UpdateAsync(int id, CapNhatNhomSinhVienDto dto);
    Task DeleteAsync(int id);
}