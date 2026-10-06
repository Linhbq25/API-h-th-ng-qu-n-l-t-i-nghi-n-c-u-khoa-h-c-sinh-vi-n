using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IDangKyDeTaiRepository
{
    Task<List<DangKyDeTai>> GetAllAsync();
    Task<DangKyDeTai?> GetByIdAsync(int id);
    Task<int> AddAsync(DangKyDeTai dangKy);
    Task<bool> UpdateAsync(DangKyDeTai dangKy);
    Task<bool> DeleteAsync(int id);

    Task<bool> DeTaiTonTaiAsync(int maDeTai);
    Task<bool> NhomTonTaiAsync(int maNhom);
    Task<bool> DaDangKyAsync(int maDeTai, int maNhom);
    Task<bool> NhomDaDangKyDeTaiKhacAsync(int maNhom, int? maDangKy = null);
    Task<int> DemSoNhomDaDuyetAsync(int maDeTai);
    Task<int> LaySoNhomToiDaAsync(int maDeTai);

    Task<bool> DuyetAsync(int maDangKy);
}