using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IDanhGiaRepository
{
    Task<List<DanhGia>> GetAllAsync();
    Task<DanhGia?> GetByIdAsync(int id);
    Task<List<DanhGia>> GetByDangKyAsync(int maDangKy);
    Task<int> AddAsync(DanhGia danhGia, List<ChiTietDanhGia> chiTiet);
    Task<bool> DeleteAsync(int id);
    Task<bool> DangKyTonTaiAsync(int maDangKy);
    Task<bool> TaiKhoanTonTaiAsync(int maTaiKhoan);
    Task<bool> TieuChiTonTaiAsync(int maTieuChi);
}