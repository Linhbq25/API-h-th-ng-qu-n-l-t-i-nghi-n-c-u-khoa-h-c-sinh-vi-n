using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class DanhGiaService(IDanhGiaRepository repository) : IDanhGiaService
{
    public async Task<List<DanhGiaDto>> GetAllAsync()
        => (await repository.GetAllAsync()).Select(Map).ToList();

    public async Task<DanhGiaDto?> GetByIdAsync(int id)
    {
        var data = await repository.GetByIdAsync(id);
        return data == null ? null : Map(data);
    }

    public async Task<List<DanhGiaDto>> GetByDangKyAsync(int maDangKy)
        => (await repository.GetByDangKyAsync(maDangKy)).Select(Map).ToList();

    public async Task<int> AddAsync(ThemDanhGiaDto dto)
    {
        if (!await repository.DangKyTonTaiAsync(dto.MaDangKy))
            throw new KeyNotFoundException(
                "Đăng ký đề tài không tồn tại hoặc chưa được duyệt.");

        if (!await repository.TaiKhoanTonTaiAsync(
                dto.MaTaiKhoanNguoiDanhGia))
            throw new KeyNotFoundException(
                "Tài khoản người đánh giá không tồn tại.");

        if (dto.ChiTiet.Count == 0)
            throw new ArgumentException(
                "Phải có ít nhất một chi tiết đánh giá.");

        decimal tongDiem = 0;
        var chiTiet = new List<ChiTietDanhGia>();

        foreach (var item in dto.ChiTiet)
        {
            if (!await repository.TieuChiTonTaiAsync(item.MaTieuChi))
                throw new KeyNotFoundException(
                    $"Tiêu chí {item.MaTieuChi} không tồn tại.");

            if (item.Diem < 0)
                throw new ArgumentException("Điểm không được âm.");

            tongDiem += item.Diem;

            chiTiet.Add(new ChiTietDanhGia
            {
                MaTieuChi = item.MaTieuChi,
                Diem = item.Diem,
                NhanXet = item.NhanXet
            });
        }

        return await repository.AddAsync(
            new DanhGia
            {
                MaDangKy = dto.MaDangKy,
                MaTaiKhoanNguoiDanhGia = dto.MaTaiKhoanNguoiDanhGia,
                NhanXet = dto.NhanXet,
                TongDiem = tongDiem
            },
            chiTiet);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException("Đánh giá không tồn tại.");

        await repository.DeleteAsync(id);
    }

    private static DanhGiaDto Map(DanhGia x) => new()
    {
        MaDanhGia = x.MaDanhGia,
        MaDangKy = x.MaDangKy,
        MaTaiKhoanNguoiDanhGia = x.MaTaiKhoanNguoiDanhGia,
        NhanXet = x.NhanXet,
        TongDiem = x.TongDiem,
        NgayDanhGia = x.NgayDanhGia
    };
}