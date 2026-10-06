using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class ThongKeService(
    IThongKeRepository repository) : IThongKeService
{
    public async Task<ThongKeDto> GetAsync()
    {
        return new ThongKeDto
        {
            TongDeTai = await repository.TongDeTaiAsync(),
            TongNhom = await repository.TongNhomAsync(),
            TongSinhVien = await repository.TongSinhVienAsync(),
            TongGiangVien = await repository.TongGiangVienAsync(),
            TongDangKy = await repository.TongDangKyAsync(),
            DangKyDaDuyet = await repository.DangKyDaDuyetAsync(),
            TongBaiNop = await repository.TongBaiNopAsync(),
            TongDanhGia = await repository.TongDanhGiaAsync()
        };
    }
}