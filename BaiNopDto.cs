namespace quanlyNCKH.API.DTOs;

public class BaiNopDto
{
    public int MaBaiNop { get; set; }
    public int MaDangKy { get; set; }
    public string TieuDe { get; set; } = "";
    public string? NoiDung { get; set; }
    public decimal PhanTramTienDo { get; set; }
    public string? DuongDanFile { get; set; }
    public DateTime HanNop { get; set; }
    public DateTime? NgayNop { get; set; }
    public string TrangThai { get; set; } = "";
}