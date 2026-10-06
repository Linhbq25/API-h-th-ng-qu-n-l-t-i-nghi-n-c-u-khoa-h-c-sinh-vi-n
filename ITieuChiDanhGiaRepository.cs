using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface ITieuChiDanhGiaRepository
{
    Task<List<TieuChiDanhGia>> GetAllAsync();
    Task<TieuChiDanhGia?> GetByIdAsync(int id);
    Task<int> AddAsync(TieuChiDanhGia data);
    Task<bool> UpdateAsync(TieuChiDanhGia data);
    Task<bool> DeleteAsync(int id);
}