using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IBaiNopService
{
    Task<List<BaiNopDto>> GetAllAsync();
    Task<BaiNopDto?> GetByIdAsync(int id);
    Task<List<BaiNopDto>> GetByDangKyAsync(int maDangKy);
    Task<int> AddAsync(ThemBaiNopDto dto);
    Task UpdateAsync(int id, CapNhatBaiNopDto dto);
    Task DeleteAsync(int id);
}