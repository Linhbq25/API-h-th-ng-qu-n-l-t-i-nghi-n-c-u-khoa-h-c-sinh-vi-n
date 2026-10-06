using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class BaiNopRepository(DbConnectionFactory db) : IBaiNopRepository
{
    public async Task<List<BaiNop>> GetAllAsync()
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaBaiNop, MaDangKy, TieuDe, NoiDung,
                   PhanTramTienDo, DuongDanFile, HanNop,
                   NgayNop, TrangThai
            FROM BaiNop
            ORDER BY MaBaiNop DESC
            """, conn);

        using var reader = await cmd.ExecuteReaderAsync();
        var list = new List<BaiNop>();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<BaiNop?> GetByIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaBaiNop, MaDangKy, TieuDe, NoiDung,
                   PhanTramTienDo, DuongDanFile, HanNop,
                   NgayNop, TrangThai
            FROM BaiNop
            WHERE MaBaiNop = @id
            """, conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync()
            ? Map(reader)
            : null;
    }

    public async Task<List<BaiNop>> GetByDangKyAsync(int maDangKy)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaBaiNop, MaDangKy, TieuDe, NoiDung,
                   PhanTramTienDo, DuongDanFile, HanNop,
                   NgayNop, TrangThai
            FROM BaiNop
            WHERE MaDangKy = @MaDangKy
            ORDER BY MaBaiNop DESC
            """, conn);

        cmd.Parameters.AddWithValue("@MaDangKy", maDangKy);

        using var reader = await cmd.ExecuteReaderAsync();
        var list = new List<BaiNop>();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<int> AddAsync(BaiNop baiNop)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            INSERT INTO BaiNop
            (
                MaDangKy,
                TieuDe,
                NoiDung,
                PhanTramTienDo,
                DuongDanFile,
                HanNop,
                NgayNop,
                TrangThai
            )
            OUTPUT INSERTED.MaBaiNop
            VALUES
            (
                @MaDangKy,
                @TieuDe,
                @NoiDung,
                @PhanTramTienDo,
                @DuongDanFile,
                @HanNop,
                GETDATE(),
                'DaNop'
            )
            """, conn);

        cmd.Parameters.AddWithValue("@MaDangKy", baiNop.MaDangKy);
        cmd.Parameters.AddWithValue("@TieuDe", baiNop.TieuDe);
        cmd.Parameters.AddWithValue(
            "@NoiDung",
            (object?)baiNop.NoiDung ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@PhanTramTienDo",
            baiNop.PhanTramTienDo);
        cmd.Parameters.AddWithValue(
            "@DuongDanFile",
            (object?)baiNop.DuongDanFile ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@HanNop",
            (object?)baiNop.HanNop ?? DBNull.Value);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(BaiNop baiNop)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            UPDATE BaiNop
            SET TieuDe = @TieuDe,
                NoiDung = @NoiDung,
                PhanTramTienDo = @PhanTramTienDo,
                DuongDanFile = @DuongDanFile,
                TrangThai = @TrangThai
            WHERE MaBaiNop = @MaBaiNop
            """, conn);

        cmd.Parameters.AddWithValue("@MaBaiNop", baiNop.MaBaiNop);
        cmd.Parameters.AddWithValue("@TieuDe", baiNop.TieuDe);
        cmd.Parameters.AddWithValue(
            "@NoiDung",
            (object?)baiNop.NoiDung ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@PhanTramTienDo",
            baiNop.PhanTramTienDo);
        cmd.Parameters.AddWithValue(
            "@DuongDanFile",
            (object?)baiNop.DuongDanFile ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TrangThai", baiNop.TrangThai);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "DELETE FROM BaiNop WHERE MaBaiNop = @id",
            conn);

        cmd.Parameters.AddWithValue("@id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DangKyTonTaiAsync(int maDangKy)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT COUNT(*)
            FROM DangKyDeTai
            WHERE MaDangKy = @MaDangKy
              AND TrangThai = 'DaDuyet'
            """, conn);

        cmd.Parameters.AddWithValue("@MaDangKy", maDangKy);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    private static BaiNop Map(SqlDataReader r)
    {
        return new BaiNop
        {
            MaBaiNop = r.GetInt32(0),
            MaDangKy = r.GetInt32(1),
            TieuDe = r.GetString(2),
            NoiDung = r.IsDBNull(3) ? null : r.GetString(3),
            PhanTramTienDo = r.GetDecimal(4),
            DuongDanFile = r.IsDBNull(5) ? null : r.GetString(5),
            HanNop = r.GetDateTime(6),
            NgayNop = r.IsDBNull(7) ? null : r.GetDateTime(7),
            TrangThai = r.GetString(8)
        };
    }
}