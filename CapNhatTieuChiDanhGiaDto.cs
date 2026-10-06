namespace quanlyNCKH.API.DTOs;

public class CapNhatTieuChiDanhGiaDto
{
    public string TenTieuChi { get; set; } = "";
    public decimal DiemToiDa { get; set; }
    public string? MoTa { get; set; }
    public bool TrangThai { get; set; }
}