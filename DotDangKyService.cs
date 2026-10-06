using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class DotDangKyService(IDotDangKyRepository repository) : IDotDangKyService
{
    private static readonly string[] TrangThaiHopLe =
        ["ChuaMo", "DangMo", "DaDong"];

    public async Task<List<DotDangKyDto>> GetAllAsync()
    {
        var data = await repository.GetAllAsync();

        return data.Select(Map).ToList();
    }

    public async Task<DotDangKyDto?> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        return data == null ? null : Map(data);
    }

    public async Task<int> AddAsync(ThemDotDangKyDto dto)
    {
        Validate(dto.TenDotDangKy, dto.NgayBatDau, dto.NgayKetThuc,
            dto.SoNhomToiDaMoiDeTai, dto.TrangThai);

        return await repository.AddAsync(new DotDangKy
        {
            TenDotDangKy = dto.TenDotDangKy.Trim(),
            NgayBatDau = dto.NgayBatDau,
            NgayKetThuc = dto.NgayKetThuc,
            SoNhomToiDaMoiDeTai = dto.SoNhomToiDaMoiDeTai,
            TrangThai = dto.TrangThai
        });
    }

    public async Task UpdateAsync(int id, CapNhatDotDangKyDto dto)
    {
        Validate(dto.TenDotDangKy, dto.NgayBatDau, dto.NgayKetThuc,
            dto.SoNhomToiDaMoiDeTai, dto.TrangThai);

        var dot = await repository.GetByIdAsync(id);

        if (dot == null)
            throw new KeyNotFoundException("Không tìm thấy đợt đăng ký.");

        dot.TenDotDangKy = dto.TenDotDangKy.Trim();
        dot.NgayBatDau = dto.NgayBatDau;
        dot.NgayKetThuc = dto.NgayKetThuc;
        dot.SoNhomToiDaMoiDeTai = dto.SoNhomToiDaMoiDeTai;
        dot.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(dot);
    }

    public async Task DeleteAsync(int id)
    {
        var dot = await repository.GetByIdAsync(id);

        if (dot == null)
            throw new KeyNotFoundException("Không tìm thấy đợt đăng ký.");

        if (dot.TrangThai != "ChuaMo")
            throw new InvalidOperationException(
                "Chỉ được xóa đợt đăng ký chưa mở.");

        if (await repository.HasDeTaiAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa đợt đăng ký vì đã có đề tài.");

        if (!await repository.DeleteAsync(id))
            throw new InvalidOperationException("Không thể xóa đợt đăng ký.");
    }

    private static void Validate(
        string ten,
        DateTime batDau,
        DateTime ketThuc,
        int soNhom,
        string trangThai)
    {
        if (string.IsNullOrWhiteSpace(ten))
            throw new ArgumentException("Tên đợt đăng ký không được để trống.");

        if (ketThuc <= batDau)
            throw new ArgumentException(
                "Ngày kết thúc phải lớn hơn ngày bắt đầu.");

        if (soNhom <= 0)
            throw new ArgumentException(
                "Số nhóm tối đa phải lớn hơn 0.");

        if (!TrangThaiHopLe.Contains(trangThai))
            throw new ArgumentException("Trạng thái không hợp lệ.");
    }

    private static DotDangKyDto Map(DotDangKy x) => new()
    {
        MaDotDangKy = x.MaDotDangKy,
        TenDotDangKy = x.TenDotDangKy,
        NgayBatDau = x.NgayBatDau,
        NgayKetThuc = x.NgayKetThuc,
        SoNhomToiDaMoiDeTai = x.SoNhomToiDaMoiDeTai,
        TrangThai = x.TrangThai,
        NgayTao = x.NgayTao
    };
}