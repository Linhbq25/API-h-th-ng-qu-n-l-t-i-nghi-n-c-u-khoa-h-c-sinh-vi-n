using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IThanhVienNhomRepository
{
    Task<List<ThanhVienNhom>> GetAllAsync();
    Task<List<ThanhVienNhom>> GetByNhomAsync(int maNhom);
    Task<ThanhVienNhom?> GetAsync(int maNhom, int maSinhVien);
    Task AddAsync(ThanhVienNhom thanhVien);
    Task<bool> DeleteAsync(int maNhom, int maSinhVien);

    Task<bool> NhomTonTaiAsync(int maNhom);
    Task<bool> SinhVienTonTaiAsync(int maSinhVien);
    Task<bool> DaLaThanhVienAsync(int maNhom, int maSinhVien);
    Task<bool> SinhVienThuocNhomKhacAsync(int maSinhVien, int? maNhom = null);
    Task<bool> DaCoNhomTruongAsync(int maNhom);
}