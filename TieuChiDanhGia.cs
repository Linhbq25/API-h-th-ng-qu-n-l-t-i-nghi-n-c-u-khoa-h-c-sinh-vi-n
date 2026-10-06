namespace quanlyNCKH.API.Models;

public class TieuChiDanhGia
{
    public int MaTieuChi { get; set; }
    public string TenTieuChi { get; set; } = "";
    public decimal DiemToiDa { get; set; }
    public string? MoTa { get; set; }
    public bool TrangThai { get; set; }
}