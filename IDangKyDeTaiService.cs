using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IDangKyDeTaiService
{
    Task<List<DangKyDeTaiDto>> GetAllAsync();
    Task<DangKyDeTaiDto?> GetByIdAsync(int id);
    Task<int> AddAsync(ThemDangKyDeTaiDto dto);
    Task DuyetAsync(int id);
    Task TuChoiAsync(int id, TuChoiDangKyDeTaiDto dto);
    Task HuyAsync(int id);
    Task DeleteAsync(int id);
}