using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IBaiNopRepository
{
    Task<List<BaiNop>> GetAllAsync();
    Task<BaiNop?> GetByIdAsync(int id);
    Task<List<BaiNop>> GetByDangKyAsync(int maDangKy);
    Task<int> AddAsync(BaiNop baiNop);
    Task<bool> UpdateAsync(BaiNop baiNop);
    Task<bool> DeleteAsync(int id);
    Task<bool> DangKyTonTaiAsync(int maDangKy);
}