using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class DeTaiRepository(DbConnectionFactory db) : IDeTaiRepository
{
    public async Task<List<DeTai>> GetAllAsync()
    {
        var result = new List<DeTai>();

        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaDeTai, MaDotDangKy, MaDeTaiCode, TenDeTai,
                   MoTa, MaGiangVien, SoNhomToiDa, TrangThai, NgayTao
            FROM DeTai
            ORDER BY MaDeTai DESC
            """;

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            result.Add(Map(reader));

        return result;
    }

    public async Task<DeTai?> GetByIdAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaDeTai, MaDotDangKy, MaDeTaiCode, TenDeTai,
                   MoTa, MaGiangVien, SoNhomToiDa, TrangThai, NgayTao
            FROM DeTai
            WHERE MaDeTai = @MaDeTai
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaDeTai", id);

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<(List<DeTai> Data, int Total)> SearchAsync(
        string? tuKhoa,
        string? trangThai,
        int? maDotDangKy,
        int trang,
        int soLuong)
    {
        var data = new List<DeTai>();

        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaDeTai, MaDotDangKy, MaDeTaiCode, TenDeTai,
                   MoTa, MaGiangVien, SoNhomToiDa, TrangThai, NgayTao
            FROM DeTai
            WHERE (@TuKhoa IS NULL
                   OR MaDeTaiCode LIKE '%' + @TuKhoa + '%'
                   OR TenDeTai LIKE '%' + @TuKhoa + '%'
                   OR MoTa LIKE '%' + @TuKhoa + '%')
              AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
              AND (@MaDotDangKy IS NULL OR MaDotDangKy = @MaDotDangKy)
            ORDER BY MaDeTai DESC
            OFFSET @BoQua ROWS FETCH NEXT @SoLuong ROWS ONLY;

            SELECT COUNT(*)
            FROM DeTai
            WHERE (@TuKhoa IS NULL
                   OR MaDeTaiCode LIKE '%' + @TuKhoa + '%'
                   OR TenDeTai LIKE '%' + @TuKhoa + '%'
                   OR MoTa LIKE '%' + @TuKhoa + '%')
              AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
              AND (@MaDotDangKy IS NULL OR MaDotDangKy = @MaDotDangKy);
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@TuKhoa",
            string.IsNullOrWhiteSpace(tuKhoa)
                ? DBNull.Value
                : tuKhoa.Trim());

        command.Parameters.AddWithValue(
            "@TrangThai",
            string.IsNullOrWhiteSpace(trangThai)
                ? DBNull.Value
                : trangThai.Trim());

        command.Parameters.AddWithValue(
            "@MaDotDangKy",
            maDotDangKy ?? (object)DBNull.Value);

        command.Parameters.AddWithValue(
            "@BoQua",
            (trang - 1) * soLuong);

        command.Parameters.AddWithValue(
            "@SoLuong",
            soLuong);

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            data.Add(Map(reader));

        await reader.NextResultAsync();

        var total = await reader.ReadAsync()
            ? reader.GetInt32(0)
            : 0;

        return (data, total);
    }

    public async Task<int> AddAsync(DeTai deTai)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO DeTai
            (MaDotDangKy, MaDeTaiCode, TenDeTai, MoTa,
             MaGiangVien, SoNhomToiDa, TrangThai)
            OUTPUT INSERTED.MaDeTai
            VALUES
            (@MaDotDangKy, @MaDeTaiCode, @TenDeTai, @MoTa,
             @MaGiangVien, @SoNhomToiDa, @TrangThai)
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaDotDangKy", deTai.MaDotDangKy);
        command.Parameters.AddWithValue("@MaDeTaiCode", deTai.MaDeTaiCode);
        command.Parameters.AddWithValue("@TenDeTai", deTai.TenDeTai);
        command.Parameters.AddWithValue(
            "@MoTa",
            deTai.MoTa ?? (object)DBNull.Value);
        command.Parameters.AddWithValue(
            "@MaGiangVien",
            deTai.MaGiangVien ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@SoNhomToiDa", deTai.SoNhomToiDa);
        command.Parameters.AddWithValue("@TrangThai", deTai.TrangThai);

        return (int)await command.ExecuteScalarAsync();
    }

    private static DeTai Map(SqlDataReader reader) => new()
    {
        MaDeTai = reader.GetInt32(0),
        MaDotDangKy = reader.GetInt32(1),
        MaDeTaiCode = reader.GetString(2),
        TenDeTai = reader.GetString(3),
        MoTa = reader.IsDBNull(4) ? null : reader.GetString(4),
        MaGiangVien = reader.IsDBNull(5) ? null : reader.GetInt32(5),
        SoNhomToiDa = reader.GetInt32(6),
        TrangThai = reader.GetString(7),
        NgayTao = reader.GetDateTime(8)
    };
    public async Task<bool> UpdateAsync(DeTai deTai)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
        UPDATE DeTai SET
            MaDotDangKy = @MaDotDangKy,
            MaDeTaiCode = @MaDeTaiCode,
            TenDeTai = @TenDeTai,
            MoTa = @MoTa,
            MaGiangVien = @MaGiangVien,
            SoNhomToiDa = @SoNhomToiDa,
            TrangThai = @TrangThai
        WHERE MaDeTai = @MaDeTai
        """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaDeTai", deTai.MaDeTai);
        command.Parameters.AddWithValue("@MaDotDangKy", deTai.MaDotDangKy);
        command.Parameters.AddWithValue("@MaDeTaiCode", deTai.MaDeTaiCode);
        command.Parameters.AddWithValue("@TenDeTai", deTai.TenDeTai);
        command.Parameters.AddWithValue("@MoTa", deTai.MoTa ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@MaGiangVien", deTai.MaGiangVien ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@SoNhomToiDa", deTai.SoNhomToiDa);
        command.Parameters.AddWithValue("@TrangThai", deTai.TrangThai);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
        DELETE FROM DeTai
        WHERE MaDeTai = @MaDeTai
        AND NOT EXISTS (
            SELECT 1 FROM DangKyDeTai
            WHERE MaDeTai = @MaDeTai
        )
        """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaDeTai", id);

        return await command.ExecuteNonQueryAsync() > 0;
    }
}