using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class DeTaiService(IDeTaiRepository repository) : IDeTaiService
{
    public async Task<int> AddAsync(ThemDeTaiDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.MaDeTaiCode))
            throw new ArgumentException("Mã đề tài không được để trống.");

        if (string.IsNullOrWhiteSpace(dto.TenDeTai))
            throw new ArgumentException("Tên đề tài không được để trống.");

        if (dto.SoNhomToiDa <= 0)
            throw new ArgumentException("Số nhóm tối đa phải lớn hơn 0.");

        var deTai = new DeTai
        {
            MaDotDangKy = dto.MaDotDangKy,
            MaDeTaiCode = dto.MaDeTaiCode.Trim(),
            TenDeTai = dto.TenDeTai.Trim(),
            MoTa = dto.MoTa?.Trim(),
            MaGiangVien = dto.MaGiangVien,
            SoNhomToiDa = dto.SoNhomToiDa,
            TrangThai = "MoiTao"
        };

        return await repository.AddAsync(deTai);
    }
    public async Task<bool> UpdateAsync(int id, CapNhatDeTaiDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.MaDeTaiCode))
            throw new ArgumentException("Mã đề tài không được để trống.");

        if (string.IsNullOrWhiteSpace(dto.TenDeTai))
            throw new ArgumentException("Tên đề tài không được để trống.");

        if (dto.SoNhomToiDa <= 0)
            throw new ArgumentException("Số nhóm tối đa phải lớn hơn 0.");

        var deTai = await repository.GetByIdAsync(id);

        if (deTai == null)
            return false;

        deTai.MaDotDangKy = dto.MaDotDangKy;
        deTai.MaDeTaiCode = dto.MaDeTaiCode.Trim();
        deTai.TenDeTai = dto.TenDeTai.Trim();
        deTai.MoTa = dto.MoTa?.Trim();
        deTai.MaGiangVien = dto.MaGiangVien;
        deTai.SoNhomToiDa = dto.SoNhomToiDa;
        deTai.TrangThai = dto.TrangThai;

        return await repository.UpdateAsync(deTai);
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var deTai = await repository.GetByIdAsync(id);

        if (deTai == null)
            throw new KeyNotFoundException("Không tìm thấy đề tài.");

        var result = await repository.DeleteAsync(id);

        if (!result)
            throw new InvalidOperationException(
                "Không thể xóa đề tài vì đã có nhóm đăng ký.");

        return true;
    }
}