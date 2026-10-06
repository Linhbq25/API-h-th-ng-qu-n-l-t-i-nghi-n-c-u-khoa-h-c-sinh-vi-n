using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IDotDangKyService
{
    Task<List<DotDangKyDto>> GetAllAsync();
    Task<DotDangKyDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemDotDangKyDto dto);
    Task UpdateAsync(int id, CapNhatDotDangKyDto dto);
    Task DeleteAsync(int id);
}