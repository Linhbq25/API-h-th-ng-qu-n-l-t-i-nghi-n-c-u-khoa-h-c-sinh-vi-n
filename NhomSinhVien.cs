namespace quanlyNCKH.API.Models;

public class NhomSinhVien
{
    public int MaNhom { get; set; }
    public string MaNhomCode { get; set; } = "";
    public string TenNhom { get; set; } = "";
    public int MaNhomTruong { get; set; }
    public string TrangThai { get; set; } = "";
    public DateTime NgayTao { get; set; }
}