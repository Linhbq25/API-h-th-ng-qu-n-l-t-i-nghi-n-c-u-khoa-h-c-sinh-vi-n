using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface INhomSinhVienRepository
{
    Task<List<NhomSinhVien>> GetAllAsync();
    Task<NhomSinhVien?> GetByIdAsync(int id);
    Task<int> AddAsync(NhomSinhVien nhom);
    Task<bool> UpdateAsync(NhomSinhVien nhom);
    Task<bool> DeleteAsync(int id);

    Task<bool> DaCoMaNhomCodeAsync(string maNhomCode, int? id = null);
    Task<bool> SinhVienTonTaiAsync(int maSinhVien);
    Task<bool> SinhVienLaTruongNhomKhacAsync(int maSinhVien, int? id = null);
    Task<bool> DangDuocSuDungAsync(int id);
}