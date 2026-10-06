using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IGiangVienRepository
{
    Task<List<GiangVien>> GetAllAsync();
    Task<GiangVien?> GetByIdAsync(int id);
    Task<int> AddAsync(GiangVien giangVien);
    Task<bool> UpdateAsync(GiangVien giangVien);
    Task<bool> DeleteAsync(int id);

    Task<bool> MaTaiKhoanTonTaiAsync(int maTaiKhoan);
    Task<bool> DaCoGiangVienAsync(int maTaiKhoan);
    Task<bool> DaCoMaSoGiangVienAsync(string maSoGiangVien, int? id = null);
    Task<bool> DaCoEmailAsync(string email, int? id = null);
    Task<bool> DangDuocSuDungAsync(int id);
}