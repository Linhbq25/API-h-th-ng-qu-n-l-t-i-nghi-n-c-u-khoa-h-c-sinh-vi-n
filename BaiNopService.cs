using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class BaiNopService(IBaiNopRepository repository) : IBaiNopService
{
    public async Task<List<BaiNopDto>> GetAllAsync()
    {
        var data = await repository.GetAllAsync();
        return data.Select(Map).ToList();
    }

    public async Task<BaiNopDto?> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        return data == null ? null : Map(data);
    }

    public async Task<List<BaiNopDto>> GetByDangKyAsync(int maDangKy)
    {
        var data = await repository.GetByDangKyAsync(maDangKy);
        return data.Select(Map).ToList();
    }

    public async Task<int> AddAsync(ThemBaiNopDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TieuDe))
            throw new ArgumentException(
                "Tiêu đề không được để trống.");

        if (dto.PhanTramTienDo < 0 ||
            dto.PhanTramTienDo > 100)
            throw new ArgumentException(
                "Phần trăm tiến độ phải từ 0 đến 100.");

        if (!await repository.DangKyTonTaiAsync(dto.MaDangKy))
            throw new KeyNotFoundException(
                "Đăng ký đề tài không tồn tại hoặc chưa được duyệt.");

        var baiNop = new BaiNop
        {
            MaDangKy = dto.MaDangKy,
            TieuDe = dto.TieuDe,
            NoiDung = dto.NoiDung,
            PhanTramTienDo = dto.PhanTramTienDo,
            DuongDanFile = dto.DuongDanFile,
            HanNop = dto.HanNop
        };

        return await repository.AddAsync(baiNop);
    }

    public async Task UpdateAsync(
        int id,
        CapNhatBaiNopDto dto)
    {
        var data = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException(
                "Bài nộp không tồn tại.");

        if (string.IsNullOrWhiteSpace(dto.TieuDe))
            throw new ArgumentException(
                "Tiêu đề không được để trống.");

        if (dto.PhanTramTienDo < 0 ||
            dto.PhanTramTienDo > 100)
            throw new ArgumentException(
                "Phần trăm tiến độ phải từ 0 đến 100.");

        data.TieuDe = dto.TieuDe;
        data.NoiDung = dto.NoiDung;
        data.PhanTramTienDo = dto.PhanTramTienDo;
        data.DuongDanFile = dto.DuongDanFile;
        data.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(data);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException(
                "Bài nộp không tồn tại.");

        await repository.DeleteAsync(id);
    }

    private static BaiNopDto Map(BaiNop x)
    {
        return new BaiNopDto
        {
            MaBaiNop = x.MaBaiNop,
            MaDangKy = x.MaDangKy,
            TieuDe = x.TieuDe,
            NoiDung = x.NoiDung,
            PhanTramTienDo = x.PhanTramTienDo,
            DuongDanFile = x.DuongDanFile,
            HanNop = x.HanNop,
            NgayNop = x.NgayNop,
            TrangThai = x.TrangThai
        };
    }
}