using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface ISinhVienService
{
    Task<List<SinhVienDto>> GetAllAsync();
    Task<SinhVienDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemSinhVienDto dto);
    Task UpdateAsync(int id, CapNhatSinhVienDto dto);
    Task DeleteAsync(int id);
}