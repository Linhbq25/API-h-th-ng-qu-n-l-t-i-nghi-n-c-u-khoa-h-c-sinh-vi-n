using quanlyNCKH.API.DTOs;
using quanlyNCKH.API.Helpers;
using quanlyNCKH.API.Models;
using quanlyNCKH.API.Repositories;

namespace quanlyNCKH.API.Services;

public class TaiKhoanService(ITaiKhoanRepository repository) : ITaiKhoanService
{
    private static readonly string[] VaiTroHopLe =
        ["Khoa", "GiangVien", "SinhVien", "HoiDong"];

    public async Task<List<TaiKhoanDto>> GetAllAsync()
    {
        var data = await repository.GetAllAsync();
        return data.Select(Map).ToList();
    }

    public async Task<TaiKhoanDto?> GetByIdAsync(int id)    
    {
        var data = await repository.GetByIdAsync(id);
        return data == null ? null : Map(data);
    }

    public async Task<int> AddAsync(ThemTaiKhoanDto dto)
    {
        Validate(dto.TenDangNhap, dto.MatKhau, dto.VaiTro);

        if (await repository.GetByUsernameAsync(dto.TenDangNhap.Trim()) != null)
            throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");

        return await repository.AddAsync(new TaiKhoan
        {
            TenDangNhap = dto.TenDangNhap.Trim(),
            MatKhauHash = MatKhauHelper.Hash(dto.MatKhau),
            VaiTro = dto.VaiTro,
            TrangThai = dto.TrangThai
        });
    }

    public async Task UpdateAsync(int id, CapNhatTaiKhoanDto dto)
    {
        if (!VaiTroHopLe.Contains(dto.VaiTro))
            throw new ArgumentException("Vai trò không hợp lệ.");

        var taiKhoan = await repository.GetByIdAsync(id);

        if (taiKhoan == null)
            throw new KeyNotFoundException("Không tìm thấy tài khoản.");

        if (!string.IsNullOrWhiteSpace(dto.MatKhau))
            taiKhoan.MatKhauHash = MatKhauHelper.Hash(dto.MatKhau);

        taiKhoan.VaiTro = dto.VaiTro;
        taiKhoan.TrangThai = dto.TrangThai;

        await repository.UpdateAsync(taiKhoan);
    }

    public async Task DeleteAsync(int id)
    {
        if (await repository.GetByIdAsync(id) == null)
            throw new KeyNotFoundException("Không tìm thấy tài khoản.");

        if (await repository.IsUsedAsync(id))
            throw new InvalidOperationException(
                "Không thể xóa tài khoản vì đang được sử dụng.");

        if (!await repository.DeleteAsync(id))
            throw new InvalidOperationException("Không thể xóa tài khoản.");
    }

    
    private static void Validate(string tenDangNhap, string matKhau, string vaiTro)
    {
        if (string.IsNullOrWhiteSpace(tenDangNhap))
            throw new ArgumentException("Tên đăng nhập không được để trống.");

        if (string.IsNullOrWhiteSpace(matKhau))
            throw new ArgumentException("Mật khẩu không được để trống.");

        if (!VaiTroHopLe.Contains(vaiTro))
            throw new ArgumentException("Vai trò không hợp lệ.");
    }

    private static TaiKhoanDto Map(TaiKhoan x) => new()
    {
        MaTaiKhoan = x.MaTaiKhoan,
        TenDangNhap = x.TenDangNhap,
        VaiTro = x.VaiTro,
        TrangThai = x.TrangThai,
        NgayTao = x.NgayTao
    };
}