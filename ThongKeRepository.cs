using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;

namespace quanlyNCKH.API.Repositories;

public class ThongKeRepository(
    DbConnectionFactory db) : IThongKeRepository
{
    private async Task<int> CountAsync(string sql)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand(sql, connection);

        return Convert.ToInt32(
            await command.ExecuteScalarAsync());
    }

    public Task<int> TongDeTaiAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM DeTai
            """);

    public Task<int> TongNhomAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM NhomSinhVien
            """);

    public Task<int> TongSinhVienAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM SinhVien
            """);

    public Task<int> TongGiangVienAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM GiangVien
            """);

    public Task<int> TongDangKyAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM DangKyDeTai
            """);

    public Task<int> DangKyDaDuyetAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM DangKyDeTai
            WHERE TrangThai = 'DaDuyet'
            """);

    public Task<int> TongBaiNopAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM BaiNop
            """);

    public Task<int> TongDanhGiaAsync()
        => CountAsync("""
            SELECT COUNT(*)
            FROM DanhGia
            """);
}