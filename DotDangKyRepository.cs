using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class DotDangKyRepository(DbConnectionFactory db) : IDotDangKyRepository
{
    public async Task<List<DotDangKy>> GetAllAsync()
    {
        var result = new List<DotDangKy>();

        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaDotDangKy, TenDotDangKy, NgayBatDau,
                   NgayKetThuc, SoNhomToiDaMoiDeTai,
                   TrangThai, NgayTao
            FROM DotDangKy
            ORDER BY MaDotDangKy DESC
            """;

        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            result.Add(Map(reader));

        return result;
    }

    public async Task<DotDangKy?> GetByIdAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            SELECT MaDotDangKy, TenDotDangKy, NgayBatDau,
                   NgayKetThuc, SoNhomToiDaMoiDeTai,
                   TrangThai, NgayTao
            FROM DotDangKy
            WHERE MaDotDangKy = @MaDotDangKy
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaDotDangKy", id);

        await using var reader = await command.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> AddAsync(DotDangKy dot)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            INSERT INTO DotDangKy
            (TenDotDangKy, NgayBatDau, NgayKetThuc,
             SoNhomToiDaMoiDeTai, TrangThai)
            OUTPUT INSERTED.MaDotDangKy
            VALUES
            (@TenDotDangKy, @NgayBatDau, @NgayKetThuc,
             @SoNhomToiDaMoiDeTai, @TrangThai)
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@TenDotDangKy", dot.TenDotDangKy);
        command.Parameters.AddWithValue("@NgayBatDau", dot.NgayBatDau);
        command.Parameters.AddWithValue("@NgayKetThuc", dot.NgayKetThuc);
        command.Parameters.AddWithValue(
            "@SoNhomToiDaMoiDeTai",
            dot.SoNhomToiDaMoiDeTai);
        command.Parameters.AddWithValue("@TrangThai", dot.TrangThai);

        return (int)await command.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(DotDangKy dot)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            UPDATE DotDangKy SET
                TenDotDangKy = @TenDotDangKy,
                NgayBatDau = @NgayBatDau,
                NgayKetThuc = @NgayKetThuc,
                SoNhomToiDaMoiDeTai = @SoNhomToiDaMoiDeTai,
                TrangThai = @TrangThai
            WHERE MaDotDangKy = @MaDotDangKy
            """;

        await using var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@MaDotDangKy", dot.MaDotDangKy);
        command.Parameters.AddWithValue("@TenDotDangKy", dot.TenDotDangKy);
        command.Parameters.AddWithValue("@NgayBatDau", dot.NgayBatDau);
        command.Parameters.AddWithValue("@NgayKetThuc", dot.NgayKetThuc);
        command.Parameters.AddWithValue(
            "@SoNhomToiDaMoiDeTai",
            dot.SoNhomToiDaMoiDeTai);
        command.Parameters.AddWithValue("@TrangThai", dot.TrangThai);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
            DELETE FROM DotDangKy
            WHERE MaDotDangKy = @MaDotDangKy
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaDotDangKy", id);

        return await command.ExecuteNonQueryAsync() > 0;
    }

    private static DotDangKy Map(SqlDataReader reader) => new()
    {
        MaDotDangKy = reader.GetInt32(0),
        TenDotDangKy = reader.GetString(1),
        NgayBatDau = reader.GetDateTime(2),
        NgayKetThuc = reader.GetDateTime(3),
        SoNhomToiDaMoiDeTai = reader.GetInt32(4),
        TrangThai = reader.GetString(5),
        NgayTao = reader.GetDateTime(6)
    };
    public async Task<bool> HasDeTaiAsync(int id)
    {
        await using var connection = db.CreateConnection();
        await connection.OpenAsync();

        const string sql = """
        SELECT COUNT(1)
        FROM DeTai
        WHERE MaDotDangKy = @MaDotDangKy
        """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@MaDotDangKy", id);

        return (int)await command.ExecuteScalarAsync() > 0;
    }
}