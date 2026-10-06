using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IDotDangKyRepository
{
    Task<List<DotDangKy>> GetAllAsync();
    Task<DotDangKy?> GetByIdAsync(int id);
    Task<int> AddAsync(DotDangKy dot);
    Task<bool> UpdateAsync(DotDangKy dot);
    Task<bool> DeleteAsync(int id);
    Task<bool> HasDeTaiAsync(int id);
}