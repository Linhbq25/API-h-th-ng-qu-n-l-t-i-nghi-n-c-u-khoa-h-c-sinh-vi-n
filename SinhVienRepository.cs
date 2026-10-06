using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class SinhVienRepository(DbConnectionFactory db) : ISinhVienRepository
{
    public async Task<List<SinhVien>> GetAllAsync()
    {
        var result = new List<SinhVien>();

        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaSinhVien, MaTaiKhoan, MaSoSinhVien,
                   HoTen, Email, Lop, Nganh, TrangThai
            FROM SinhVien
            ORDER BY MaSinhVien DESC
            """;

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            result.Add(Map(reader));

        return result;
    }

    public async Task<SinhVien?> GetByIdAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaSinhVien, MaTaiKhoan, MaSoSinhVien,
                   HoTen, Email, Lop, Nganh, TrangThai
            FROM SinhVien
            WHERE MaSinhVien = @MaSinhVien
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaSinhVien", id);

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> AddAsync(SinhVien sinhVien)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO SinhVien
            (MaTaiKhoan, MaSoSinhVien, HoTen, Email, Lop, Nganh, TrangThai)
            OUTPUT INSERTED.MaSinhVien
            VALUES
            (@MaTaiKhoan, @MaSoSinhVien, @HoTen, @Email,
             @Lop, @Nganh, @TrangThai)
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaTaiKhoan", sinhVien.MaTaiKhoan);
        command.Parameters.AddWithValue("@MaSoSinhVien", sinhVien.MaSoSinhVien);
        command.Parameters.AddWithValue("@HoTen", sinhVien.HoTen);
        command.Parameters.AddWithValue("@Email", sinhVien.Email);
        command.Parameters.AddWithValue("@Lop", sinhVien.Lop);
        command.Parameters.AddWithValue("@Nganh", sinhVien.Nganh);
        command.Parameters.AddWithValue("@TrangThai", sinhVien.TrangThai);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(SinhVien sinhVien)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            UPDATE SinhVien SET
                MaSoSinhVien = @MaSoSinhVien,
                HoTen = @HoTen,
                Email = @Email,
                Lop = @Lop,
                Nganh = @Nganh,
                TrangThai = @TrangThai
            WHERE MaSinhVien = @MaSinhVien
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaSinhVien", sinhVien.MaSinhVien);
        command.Parameters.AddWithValue("@MaSoSinhVien", sinhVien.MaSoSinhVien);
        command.Parameters.AddWithValue("@HoTen", sinhVien.HoTen);
        command.Parameters.AddWithValue("@Email", sinhVien.Email);
        command.Parameters.AddWithValue("@Lop", sinhVien.Lop);
        command.Parameters.AddWithValue("@Nganh", sinhVien.Nganh);
        command.Parameters.AddWithValue("@TrangThai", sinhVien.TrangThai);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            DELETE FROM SinhVien
            WHERE MaSinhVien = @MaSinhVien
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaSinhVien", id);

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

    public async Task<bool> DaCoSinhVienAsync(int maTaiKhoan)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM SinhVien
            WHERE MaTaiKhoan = @MaTaiKhoan
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    private static SinhVien Map(SqlDataReader reader) => new()
    {
        MaSinhVien = reader.GetInt32(0),
        MaTaiKhoan = reader.GetInt32(1),
        MaSoSinhVien = reader.GetString(2),
        HoTen = reader.GetString(3),
        Email = reader.GetString(4),
        Lop = reader.GetString(5),
        Nganh = reader.GetString(6),
        TrangThai = reader.GetBoolean(7)
    };
    public async Task<bool> DaCoMaSoSinhVienAsync(
    string maSoSinhVien, int? id = null)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
        SELECT COUNT(1)
        FROM SinhVien
        WHERE MaSoSinhVien = @MaSoSinhVien
          AND (@MaSinhVien IS NULL OR MaSinhVien <> @MaSinhVien)
        """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaSoSinhVien", maSoSinhVien);
        command.Parameters.AddWithValue("@MaSinhVien", (object?)id ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaCoEmailAsync(
        string email, int? id = null)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
        SELECT COUNT(1)
        FROM SinhVien
        WHERE Email = @Email
          AND (@MaSinhVien IS NULL OR MaSinhVien <> @MaSinhVien)
        """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@MaSinhVien", (object?)id ?? DBNull.Value);

        return (int)await command.ExecuteScalarAsync() > 0;
    }
    public async Task<bool> DangDuocSuDungAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
        SELECT CASE WHEN EXISTS (
            SELECT 1
            FROM ThanhVienNhom
            WHERE MaSinhVien = @MaSinhVien

            UNION ALL

            SELECT 1
            FROM NhomSinhVien
            WHERE MaNhomTruong = @MaSinhVien
        ) THEN 1 ELSE 0 END
        """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaSinhVien", id);

        return (int)await command.ExecuteScalarAsync() == 1;
    }
}