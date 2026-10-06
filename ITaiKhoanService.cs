using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface ITaiKhoanService
{
    Task<List<TaiKhoanDto>> GetAllAsync();
    Task<TaiKhoanDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemTaiKhoanDto dto);
    Task UpdateAsync(int id, CapNhatTaiKhoanDto dto);
    Task DeleteAsync(int id);
}