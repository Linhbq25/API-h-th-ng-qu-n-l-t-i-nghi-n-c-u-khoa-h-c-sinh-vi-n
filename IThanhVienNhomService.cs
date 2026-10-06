using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IThanhVienNhomService
{
    Task<List<ThanhVienNhomDto>> GetAllAsync();
    Task<List<ThanhVienNhomDto>> GetByNhomAsync(int maNhom);
    Task<ThanhVienNhomDto?> GetAsync(int maNhom, int maSinhVien);
    Task AddAsync(ThemThanhVienNhomDto dto);
    Task DeleteAsync(int maNhom, int maSinhVien);
}