using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IGiangVienService
{
    Task<List<GiangVienDto>> GetAllAsync();
    Task<GiangVienDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemGiangVienDto dto);
    Task UpdateAsync(int id, CapNhatGiangVienDto dto);
    Task DeleteAsync(int id);
}