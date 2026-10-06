namespace quanlyNCKH.API.DTOs;

public class GiangVienDto
{
    public int MaGiangVien { get; set; }
    public int MaTaiKhoan { get; set; }
    public string MaSoGiangVien { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string Email { get; set; } = "";
    public string BoMon { get; set; } = "";
    public bool TrangThai { get; set; }
}