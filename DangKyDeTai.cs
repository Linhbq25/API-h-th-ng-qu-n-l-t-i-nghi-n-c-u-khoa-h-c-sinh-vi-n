namespace quanlyNCKH.API.Models;

public class DangKyDeTai
{
    public int MaDangKy { get; set; }
    public int MaDeTai { get; set; }
    public int MaNhom { get; set; }
    public DateTime NgayDangKy { get; set; }
    public string TrangThai { get; set; } = "";
    public string? LyDoTuChoi { get; set; }
    public DateTime? NgayDuyet { get; set; }
}