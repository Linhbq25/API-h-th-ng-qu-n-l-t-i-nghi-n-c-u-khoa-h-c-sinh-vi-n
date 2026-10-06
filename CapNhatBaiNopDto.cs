namespace quanlyNCKH.API.DTOs;

public class CapNhatBaiNopDto
{
    public string TieuDe { get; set; } = "";
    public string? NoiDung { get; set; }
    public decimal PhanTramTienDo { get; set; }
    public string? DuongDanFile { get; set; }
    public DateTime HanNop { get; set; }
    public string TrangThai { get; set; } = "Nhap";
}