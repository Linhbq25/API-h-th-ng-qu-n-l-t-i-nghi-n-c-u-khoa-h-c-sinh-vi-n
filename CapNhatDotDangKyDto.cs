namespace quanlyNCKH.API.DTOs;

public class CapNhatDotDangKyDto
{
    public string TenDotDangKy { get; set; } = "";
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public int SoNhomToiDaMoiDeTai { get; set; }
    public string TrangThai { get; set; } = "";
}