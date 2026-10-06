namespace quanlyNCKH.API.DTOs;

public class CapNhatDeTaiDto
{
    public int MaDotDangKy { get; set; }
    public string MaDeTaiCode { get; set; } = "";
    public string TenDeTai { get; set; } = "";
    public string? MoTa { get; set; }
    public int? MaGiangVien { get; set; }
    public int SoNhomToiDa { get; set; }
    public string TrangThai { get; set; } = "MoiTao";
}