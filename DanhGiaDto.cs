namespace quanlyNCKH.API.DTOs;

public class DanhGiaDto
{
    public int MaDanhGia { get; set; }
    public int MaDangKy { get; set; }
    public int MaTaiKhoanNguoiDanhGia { get; set; }
    public string? NhanXet { get; set; }
    public decimal? TongDiem { get; set; }
    public DateTime NgayDanhGia { get; set; }
}