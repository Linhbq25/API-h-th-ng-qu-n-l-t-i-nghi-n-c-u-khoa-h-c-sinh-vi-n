namespace quanlyNCKH.API.Models;

public class DotDangKy
{
    public int MaDotDangKy { get; set; }
    public string TenDotDangKy { get; set; } = "";
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public int SoNhomToiDaMoiDeTai { get; set; }
    public string TrangThai { get; set; } = "";
    public DateTime NgayTao { get; set; }
}