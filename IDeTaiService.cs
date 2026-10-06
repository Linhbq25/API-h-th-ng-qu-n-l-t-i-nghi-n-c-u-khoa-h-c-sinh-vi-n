using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IDeTaiService
{
    Task<int> AddAsync(ThemDeTaiDto dto);
    Task<bool> UpdateAsync(int id, CapNhatDeTaiDto dto);
    Task<bool> DeleteAsync(int id);
}