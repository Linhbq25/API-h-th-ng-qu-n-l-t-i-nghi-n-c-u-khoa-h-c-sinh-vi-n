namespace quanlyNCKH.API.Repositories;

public interface IThongKeRepository
{
    Task<int> TongDeTaiAsync();
    Task<int> TongNhomAsync();
    Task<int> TongSinhVienAsync();
    Task<int> TongGiangVienAsync();
    Task<int> TongDangKyAsync();
    Task<int> DangKyDaDuyetAsync();
    Task<int> TongBaiNopAsync();
    Task<int> TongDanhGiaAsync();
}