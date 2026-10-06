using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface ISinhVienRepository
{
    Task<List<SinhVien>> GetAllAsync();
    Task<SinhVien?> GetByIdAsync(int id);
    Task<int> AddAsync(SinhVien sinhVien);
    Task<bool> UpdateAsync(SinhVien sinhVien);
    Task<bool> DeleteAsync(int id);
    Task<bool> MaTaiKhoanTonTaiAsync(int maTaiKhoan);
    Task<bool> DaCoSinhVienAsync(int maTaiKhoan);
    Task<bool> DaCoMaSoSinhVienAsync(string maSoSinhVien, int? id = null);
    Task<bool> DaCoEmailAsync(string email, int? id = null);
    Task<bool> DangDuocSuDungAsync(int id);
}