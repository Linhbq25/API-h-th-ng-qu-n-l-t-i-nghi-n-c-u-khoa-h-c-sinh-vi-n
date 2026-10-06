namespace quanlyNCKH.API.Models;

public class BaiNop
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