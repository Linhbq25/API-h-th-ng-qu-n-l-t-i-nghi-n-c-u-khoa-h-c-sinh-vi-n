using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class GiangVienRepository(DbConnectionFactory db) : IGiangVienRepository
{
    public async Task<List<GiangVien>> GetAllAsync()
    {
        var result = new List<GiangVien>();

        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaGiangVien, MaTaiKhoan, MaSoGiangVien,
                   HoTen, Email, BoMon, TrangThai
            FROM GiangVien
            ORDER BY MaGiangVien DESC
            """;

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            result.Add(Map(reader));

        return result;
    }

    public async Task<GiangVien?> GetByIdAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaGiangVien, MaTaiKhoan, MaSoGiangVien,
                   HoTen, Email, BoMon, TrangThai
            FROM GiangVien
            WHERE MaGiangVien = @MaGiangVien
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaGiangVien", id);

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> AddAsync(GiangVien giangVien)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO GiangVien
            (MaTaiKhoan, MaSoGiangVien, HoTen, Email, BoMon, TrangThai)
            OUTPUT INSERTED.MaGiangVien
            VALUES
            (@MaTaiKhoan, @MaSoGiangVien, @HoTen,
             @Email, @BoMon, @TrangThai)
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaTaiKhoan", giangVien.MaTaiKhoan);
        command.Parameters.AddWithValue("@MaSoGiangVien", giangVien.MaSoGiangVien);
        command.Parameters.AddWithValue("@HoTen", giangVien.HoTen);
        command.Parameters.AddWithValue("@Email", giangVien.Email);
        command.Parameters.AddWithValue("@BoMon", giangVien.BoMon);
        command.Parameters.AddWithValue("@TrangThai", giangVien.TrangThai);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(GiangVien giangVien)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            UPDATE GiangVien SET
                MaSoGiangVien = @MaSoGiangVien,
                HoTen = @HoTen,
                Email = @Email,
                BoMon = @BoMon,
                TrangThai = @TrangThai
            WHERE MaGiangVien = @MaGiangVien
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaGiangVien", giangVien.MaGiangVien);
        command.Parameters.AddWithValue("@MaSoGiangVien", giangVien.MaSoGiangVien);
        command.Parameters.AddWithValue("@HoTen", giangVien.HoTen);
        command.Parameters.AddWithValue("@Email", giangVien.Email);
        command.Parameters.AddWithValue("@BoMon", giangVien.BoMon);
        command.Parameters.AddWithValue("@TrangThai", giangVien.TrangThai);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            DELETE FROM GiangVien
            WHERE MaGiangVien = @MaGiangVien
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaGiangVien", id);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> MaTaiKhoanTonTaiAsync(int maTaiKhoan)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM TaiKhoan
            WHERE MaTaiKhoan = @MaTaiKhoan
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaCoGiangVienAsync(int maTaiKhoan)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM GiangVien
            WHERE MaTaiKhoan = @MaTaiKhoan
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaCoMaSoGiangVienAsync(
        string maSoGiangVien, int? id = null)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM GiangVien
            WHERE MaSoGiangVien = @MaSoGiangVien
              AND (@MaGiangVien IS NULL OR MaGiangVien <> @MaGiangVien)
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaSoGiangVien", maSoGiangVien);
        command.Parameters.AddWithValue(
            "@MaGiangVien", (object?)id ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaCoEmailAsync(
        string email, int? id = null)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM GiangVien
            WHERE Email = @Email
              AND (@MaGiangVien IS NULL OR MaGiangVien <> @MaGiangVien)
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue(
            "@MaGiangVien", (object?)id ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DangDuocSuDungAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM DeTai
            WHERE MaGiangVien = @MaGiangVien
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaGiangVien", id);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private static GiangVien Map(SqlDataReader reader) => new()
    {
        MaGiangVien = reader.GetInt32(0),
        MaTaiKhoan = reader.GetInt32(1),
        MaSoGiangVien = reader.GetString(2),
        HoTen = reader.GetString(3),
        Email = reader.GetString(4),
        BoMon = reader.GetString(5),
        TrangThai = reader.GetBoolean(6)
    };
}