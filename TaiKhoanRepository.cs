using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class TaiKhoanRepository(
    DbConnectionFactory db) : ITaiKhoanRepository
{
    public async Task<List<TaiKhoan>> GetAllAsync()
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            SELECT MaTaiKhoan, TenDangNhap, MatKhauHash,
                   VaiTro, TrangThai, NgayTao
            FROM TaiKhoan
            ORDER BY MaTaiKhoan DESC
            """, connection);

        using var reader = await command.ExecuteReaderAsync();

        var result = new List<TaiKhoan>();

        while (await reader.ReadAsync())
            result.Add(Map(reader));

        return result;
    }

    public async Task<TaiKhoan?> GetByIdAsync(int id)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            SELECT MaTaiKhoan, TenDangNhap, MatKhauHash,
                   VaiTro, TrangThai, NgayTao
            FROM TaiKhoan
            WHERE MaTaiKhoan = @MaTaiKhoan
            """, connection);

        command.Parameters.AddWithValue("@MaTaiKhoan", id);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return Map(reader);
    }

    public async Task<TaiKhoan?> GetByTenDangNhapAsync(
        string tenDangNhap)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            SELECT MaTaiKhoan, TenDangNhap, MatKhauHash,
                   VaiTro, TrangThai, NgayTao
            FROM TaiKhoan
            WHERE TenDangNhap = @TenDangNhap
            """, connection);

        command.Parameters.AddWithValue(
            "@TenDangNhap",
            tenDangNhap);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return Map(reader);
    }

    public async Task<TaiKhoan?> GetByUsernameAsync(
        string tenDangNhap)
    {
        return await GetByTenDangNhapAsync(tenDangNhap);
    }

    public async Task<int> AddAsync(TaiKhoan taiKhoan)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            INSERT INTO TaiKhoan
            (
                TenDangNhap,
                MatKhauHash,
                VaiTro,
                TrangThai,
                NgayTao
            )
            OUTPUT INSERTED.MaTaiKhoan
            VALUES
            (
                @TenDangNhap,
                @MatKhauHash,
                @VaiTro,
                @TrangThai,
                GETDATE()
            )
            """, connection);

        command.Parameters.AddWithValue(
            "@TenDangNhap",
            taiKhoan.TenDangNhap);

        command.Parameters.AddWithValue(
            "@MatKhauHash",
            taiKhoan.MatKhauHash);

        command.Parameters.AddWithValue(
            "@VaiTro",
            taiKhoan.VaiTro);

        command.Parameters.AddWithValue(
            "@TrangThai",
            taiKhoan.TrangThai);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(TaiKhoan taiKhoan)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            UPDATE TaiKhoan
            SET TenDangNhap = @TenDangNhap,
                MatKhauHash = @MatKhauHash,
                VaiTro = @VaiTro,
                TrangThai = @TrangThai
            WHERE MaTaiKhoan = @MaTaiKhoan
            """, connection);

        command.Parameters.AddWithValue(
            "@MaTaiKhoan",
            taiKhoan.MaTaiKhoan);

        command.Parameters.AddWithValue(
            "@TenDangNhap",
            taiKhoan.TenDangNhap);

        command.Parameters.AddWithValue(
            "@MatKhauHash",
            taiKhoan.MatKhauHash);

        command.Parameters.AddWithValue(
            "@VaiTro",
            taiKhoan.VaiTro);

        command.Parameters.AddWithValue(
            "@TrangThai",
            taiKhoan.TrangThai);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            DELETE FROM TaiKhoan
            WHERE MaTaiKhoan = @MaTaiKhoan
            """, connection);

        command.Parameters.AddWithValue(
            "@MaTaiKhoan",
            id);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DaCoTenDangNhapAsync(
        string tenDangNhap,
        int? id = null)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            SELECT COUNT(*)
            FROM TaiKhoan
            WHERE TenDangNhap = @TenDangNhap
              AND (
                    @MaTaiKhoan IS NULL
                    OR MaTaiKhoan <> @MaTaiKhoan
                  )
            """, connection);

        command.Parameters.AddWithValue(
            "@TenDangNhap",
            tenDangNhap);

        command.Parameters.AddWithValue(
            "@MaTaiKhoan",
            (object?)id ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> IsUsedAsync(int id)
    {
        using var connection = db.CreateConnection();
        await connection.OpenAsync();

        var command = new SqlCommand("""
            SELECT CASE
                WHEN EXISTS
                (
                    SELECT 1
                    FROM SinhVien
                    WHERE MaTaiKhoan = @MaTaiKhoan
                )
                OR EXISTS
                (
                    SELECT 1
                    FROM GiangVien
                    WHERE MaTaiKhoan = @MaTaiKhoan
                )
                THEN 1
                ELSE 0
            END
            """, connection);

        command.Parameters.AddWithValue(
            "@MaTaiKhoan",
            id);

        return (int)await command.ExecuteScalarAsync() == 1;
    }

    private static TaiKhoan Map(SqlDataReader reader)
    {
        return new TaiKhoan
        {
            MaTaiKhoan = reader.GetInt32(0),
            TenDangNhap = reader.GetString(1),
            MatKhauHash = reader.GetString(2),
            VaiTro = reader.GetString(3),
            TrangThai = reader.GetBoolean(4),
            NgayTao = reader.GetDateTime(5)
        };
    }
}