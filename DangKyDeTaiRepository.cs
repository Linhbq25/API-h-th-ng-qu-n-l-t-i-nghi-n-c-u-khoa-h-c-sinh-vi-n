using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class DangKyDeTaiRepository(DbConnectionFactory db)
    : IDangKyDeTaiRepository
{
    public async Task<List<DangKyDeTai>> GetAllAsync()
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaDangKy, MaDeTai, MaNhom, NgayDangKy,
                   TrangThai, LyDoTuChoi, NgayDuyet
            FROM DangKyDeTai
            ORDER BY MaDangKy DESC
            """;

        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        var result = new List<DangKyDeTai>();

        while (await reader.ReadAsync())
            result.Add(Map(reader));

        return result;
    }

    public async Task<DangKyDeTai?> GetByIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaDangKy, MaDeTai, MaNhom, NgayDangKy,
                   TrangThai, LyDoTuChoi, NgayDuyet
            FROM DangKyDeTai
            WHERE MaDangKy = @MaDangKy
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaDangKy", id);

        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> AddAsync(DangKyDeTai dangKy)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            INSERT INTO DangKyDeTai
                (MaDeTai, MaNhom, NgayDangKy, TrangThai)
            OUTPUT INSERTED.MaDangKy
            VALUES
                (@MaDeTai, @MaNhom, GETDATE(), 'ChoDuyet')
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaDeTai", dangKy.MaDeTai);
        cmd.Parameters.AddWithValue("@MaNhom", dangKy.MaNhom);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(DangKyDeTai dangKy)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            UPDATE DangKyDeTai
            SET TrangThai = @TrangThai,
                LyDoTuChoi = @LyDoTuChoi,
                NgayDuyet = @NgayDuyet
            WHERE MaDangKy = @MaDangKy
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaDangKy", dangKy.MaDangKy);
        cmd.Parameters.AddWithValue("@TrangThai", dangKy.TrangThai);
        cmd.Parameters.AddWithValue(
            "@LyDoTuChoi",
            (object?)dangKy.LyDoTuChoi ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@NgayDuyet",
            (object?)dangKy.NgayDuyet ?? DBNull.Value);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            DELETE FROM DangKyDeTai
            WHERE MaDangKy = @MaDangKy
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaDangKy", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeTaiTonTaiAsync(int maDeTai)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(*)
            FROM DeTai
            WHERE MaDeTai = @MaDeTai
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaDeTai", maDeTai);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> NhomTonTaiAsync(int maNhom)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(*)
            FROM NhomSinhVien
            WHERE MaNhom = @MaNhom
              AND TrangThai = 'DangHoatDong'
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", maNhom);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaDangKyAsync(int maDeTai, int maNhom)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(*)
            FROM DangKyDeTai
            WHERE MaDeTai = @MaDeTai
              AND MaNhom = @MaNhom
              AND TrangThai <> 'Huy'
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaDeTai", maDeTai);
        cmd.Parameters.AddWithValue("@MaNhom", maNhom);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> NhomDaDangKyDeTaiKhacAsync(
        int maNhom,
        int? maDangKy = null)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(*)
            FROM DangKyDeTai
            WHERE MaNhom = @MaNhom
              AND TrangThai IN ('ChoDuyet', 'DaDuyet')
              AND (@MaDangKy IS NULL OR MaDangKy <> @MaDangKy)
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhom", maNhom);
        cmd.Parameters.AddWithValue(
            "@MaDangKy",
            (object?)maDangKy ?? DBNull.Value);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<int> DemSoNhomDaDuyetAsync(int maDeTai)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(*)
            FROM DangKyDeTai
            WHERE MaDeTai = @MaDeTai
              AND TrangThai = 'DaDuyet'
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaDeTai", maDeTai);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<int> LaySoNhomToiDaAsync(int maDeTai)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT SoNhomToiDa
            FROM DeTai
            WHERE MaDeTai = @MaDeTai
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaDeTai", maDeTai);

        return (int)await cmd.ExecuteScalarAsync();
    }

    private static DangKyDeTai Map(SqlDataReader reader)
    {
        return new DangKyDeTai
        {
            MaDangKy = reader.GetInt32(0),
            MaDeTai = reader.GetInt32(1),
            MaNhom = reader.GetInt32(2),
            NgayDangKy = reader.GetDateTime(3),
            TrangThai = reader.GetString(4),
            LyDoTuChoi = reader.IsDBNull(5)
                ? null
                : reader.GetString(5),
            NgayDuyet = reader.IsDBNull(6)
                ? null
                : reader.GetDateTime(6)
        };
    }
    public async Task<bool> DuyetAsync(int maDangKy)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        using var transaction = await conn.BeginTransactionAsync();

        try
        {
            const string sql = """
            UPDATE DangKyDeTai
            SET TrangThai = 'DaDuyet',
                NgayDuyet = GETDATE(),
                LyDoTuChoi = NULL
            WHERE MaDangKy = @MaDangKy
              AND TrangThai = 'ChoDuyet'
            """;

            using var cmd = new SqlCommand(sql, conn, (SqlTransaction)transaction);
            cmd.Parameters.AddWithValue("@MaDangKy", maDangKy);

            var result = await cmd.ExecuteNonQueryAsync();

            if (result == 0)
            {
                await transaction.RollbackAsync();
                return false;
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}