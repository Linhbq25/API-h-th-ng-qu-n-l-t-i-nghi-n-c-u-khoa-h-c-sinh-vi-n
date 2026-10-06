using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class DangKyDeTaiService(IDangKyDeTaiRepository repository)
    : IDangKyDeTaiService
{
    public async Task<List<DangKyDeTaiDto>> GetAllAsync()
    {
        var data = await repository.GetAllAsync();
        return data.Select(Map).ToList();
    }

    public async Task<DangKyDeTaiDto?> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        return data == null ? null : Map(data);
    }

    public async Task<int> AddAsync(ThemDangKyDeTaiDto dto)
    {
        if (!await repository.DeTaiTonTaiAsync(dto.MaDeTai))
            throw new ArgumentException("Đề tài không tồn tại.");

        if (!await repository.NhomTonTaiAsync(dto.MaNhom))
            throw new ArgumentException(
                "Nhóm sinh viên không tồn tại hoặc đã ngừng hoạt động.");

        if (await repository.DaDangKyAsync(dto.MaDeTai, dto.MaNhom))
            throw new InvalidOperationException(
                "Nhóm đã đăng ký đề tài này.");

        if (await repository.NhomDaDangKyDeTaiKhacAsync(dto.MaNhom))
            throw new InvalidOperationException(
                "Nhóm đã đăng ký một đề tài khác.");

        var soNhom = await repository.DemSoNhomDaDuyetAsync(dto.MaDeTai);
        var toiDa = await repository.LaySoNhomToiDaAsync(dto.MaDeTai);

        if (soNhom >= toiDa)
            throw new InvalidOperationException(
                "Đề tài đã đủ số nhóm đăng ký.");

        return await repository.AddAsync(new DangKyDeTai
        {
            MaDeTai = dto.MaDeTai,
            MaNhom = dto.MaNhom
        });
    }

    public async Task DuyetAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);

        if (data == null)
            throw new KeyNotFoundException(
                "Không tìm thấy đăng ký đề tài.");

        if (data.TrangThai != "ChoDuyet")
            throw new InvalidOperationException(
                "Chỉ có thể duyệt đăng ký đang chờ duyệt.");

        var soNhom = await repository.DemSoNhomDaDuyetAsync(data.MaDeTai);
        var toiDa = await repository.LaySoNhomToiDaAsync(data.MaDeTai);

        if (soNhom >= toiDa)
            throw new InvalidOperationException(
                "Đề tài đã đủ số nhóm đăng ký.");

        if (!await repository.DuyetAsync(id))
            throw new InvalidOperationException(
                "Không thể duyệt đăng ký.");
    }

    public async Task TuChoiAsync(
        int id,
        TuChoiDangKyDeTaiDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.LyDoTuChoi))
            throw new ArgumentException(
                "Lý do từ chối không được để trống.");

        var data = await repository.GetByIdAsync(id);

        if (data == null)
            throw new KeyNotFoundException(
                "Không tìm thấy đăng ký đề tài.");

        if (data.TrangThai != "ChoDuyet")
            throw new InvalidOperationException(
                "Chỉ có thể từ chối đăng ký đang chờ duyệt.");

        data.TrangThai = "TuChoi";
        data.LyDoTuChoi = dto.LyDoTuChoi;
        data.NgayDuyet = DateTime.Now;

        await repository.UpdateAsync(data);
    }

    public async Task HuyAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);

        if (data == null)
            throw new KeyNotFoundException(
                "Không tìm thấy đăng ký đề tài.");

        if (data.TrangThai != "ChoDuyet" &&
            data.TrangThai != "DaDuyet")
            throw new InvalidOperationException(
                "Không thể hủy đăng ký ở trạng thái hiện tại.");

        data.TrangThai = "Huy";

        await repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);

        if (data == null)
            throw new KeyNotFoundException(
                "Không tìm thấy đăng ký đề tài.");

        if (data.TrangThai != "ChoDuyet")
            throw new InvalidOperationException(
                "Chỉ có thể xóa đăng ký đang chờ duyệt.");

        await repository.DeleteAsync(id);
    }

    private static DangKyDeTaiDto Map(DangKyDeTai x)
    {
        return new DangKyDeTaiDto
        {
            MaDangKy = x.MaDangKy,
            MaDeTai = x.MaDeTai,
            MaNhom = x.MaNhom,
            NgayDangKy = x.NgayDangKy,
            TrangThai = x.TrangThai,
            LyDoTuChoi = x.LyDoTuChoi,
            NgayDuyet = x.NgayDuyet
        };
    }
}