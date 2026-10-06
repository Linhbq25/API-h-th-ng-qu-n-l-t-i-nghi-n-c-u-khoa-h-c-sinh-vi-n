using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public interface IDeTaiRepository
{
    Task<List<DeTai>> GetAllAsync();
    Task<DeTai?> GetByIdAsync(int id);

    Task<(List<DeTai> Data, int Total)> SearchAsync(
        string? tuKhoa,
        string? trangThai,
        int? maDotDangKy,
        int trang,
        int soLuong);

    Task<int> AddAsync(DeTai deTai);
    Task<bool> UpdateAsync(DeTai deTai);
    Task<bool> DeleteAsync(int id);
}