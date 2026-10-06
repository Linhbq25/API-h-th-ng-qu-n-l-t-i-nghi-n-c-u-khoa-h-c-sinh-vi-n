namespace quanlyNCKH.API.Models;

public class SinhVien
{
    public int MaSinhVien { get; set; }
    public int MaTaiKhoan { get; set; }
    public string MaSoSinhVien { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string Email { get; set; } = "";
    public string Lop { get; set; } = "";
    public string Nganh { get; set; } = "";
    public bool TrangThai { get; set; }
}