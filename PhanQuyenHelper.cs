namespace quanlyNCKH.API.Helpers;

public static class PhanQuyenHelper
{
    public const string Khoa = "Khoa";
    public const string GiangVien = "GiangVien";
    public const string SinhVien = "SinhVien";
    public const string HoiDong = "HoiDong";

    public static bool HopLe(string vaiTro)
        => vaiTro is Khoa or GiangVien or SinhVien or HoiDong;
}