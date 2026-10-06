using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface ITaiKhoanRepository
{
    Task<List<TaiKhoan>> GetAllAsync();
    Task<TaiKhoan?> GetByIdAsync(int id);

    Task<TaiKhoan?> GetByTenDangNhapAsync(
        string tenDangNhap);

    Task<TaiKhoan?> GetByUsernameAsync(
        string tenDangNhap);

    Task<int> AddAsync(TaiKhoan taiKhoan);
    Task<bool> UpdateAsync(TaiKhoan taiKhoan);
    Task<bool> DeleteAsync(int id);

    Task<bool> DaCoTenDangNhapAsync(
        string tenDangNhap,
        int? id = null);

    Task<bool> IsUsedAsync(int id);
}