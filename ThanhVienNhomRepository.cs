using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class ThanhVienNhomRepository(DbConnectionFactory db) : IThanhVienNhomRepository
{
    public async Task<List<ThanhVienNhom>> GetAllAsync()
    {
        var list = new List<ThanhVienNhom>();

        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaNhom, MaSinhVien, LaNhomTruong, NgayThamGia
            FROM ThanhVienNhom
            ORDER BY MaNhom, MaSinhVien
            """;

        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<List<ThanhVienNhom>> GetByNhomAsync(int maNhom)
    {
        var list = new List<ThanhVienNhom>();

        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaNhom, MaSinhVien, LaNhomTruong, NgayThamGia
            FROM ThanhVienNhom
            WHERE MaNhom = @MaNhom
            ORDER BY MaSinhVien
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", maNhom);

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<ThanhVienNhom?> GetAsync(
        int maNhom,
        int maSinhVien)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaNhom, MaSinhVien, LaNhomTruong, NgayThamGia
            FROM ThanhVienNhom
            WHERE MaNhom = @MaNhom
              AND MaSinhVien = @MaSinhVien
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhom", maNhom);
        cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);

        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task AddAsync(ThanhVienNhom thanhVien)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            INSERT INTO ThanhVienNhom
                (MaNhom, MaSinhVien, LaNhomTruong)
            VALUES
                (@MaNhom, @MaSinhVien, @LaNhomTruong)
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhom", thanhVien.MaNhom);
        cmd.Parameters.AddWithValue("@MaSinhVien", thanhVien.MaSinhVien);
        cmd.Parameters.AddWithValue("@LaNhomTruong", thanhVien.LaNhomTruong);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<bool> DeleteAsync(
        int maNhom,
        int maSinhVien)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            DELETE FROM ThanhVienNhom
            WHERE MaNhom = @MaNhom
              AND MaSinhVien = @MaSinhVien
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhom", maNhom);
        cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> NhomTonTaiAsync(int maNhom)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM NhomSinhVien
            WHERE MaNhom = @MaNhom
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", maNhom);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> SinhVienTonTaiAsync(int maSinhVien)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM SinhVien
            WHERE MaSinhVien = @MaSinhVien
              AND TrangThai = 1
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaLaThanhVienAsync(
        int maNhom,
        int maSinhVien)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM ThanhVienNhom
            WHERE MaNhom = @MaNhom
              AND MaSinhVien = @MaSinhVien
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhom", maNhom);
        cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> SinhVienThuocNhomKhacAsync(
        int maSinhVien,
        int? maNhom = null)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM ThanhVienNhom
            WHERE MaSinhVien = @MaSinhVien
              AND (@MaNhom IS NULL OR MaNhom <> @MaNhom)
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
        cmd.Parameters.AddWithValue("@MaNhom", (object?)maNhom ?? DBNull.Value);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DaCoNhomTruongAsync(int maNhom)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM ThanhVienNhom
            WHERE MaNhom = @MaNhom
              AND LaNhomTruong = 1
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", maNhom);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    private static ThanhVienNhom Map(SqlDataReader reader)
    {
        return new ThanhVienNhom
        {
            MaNhom = reader.GetInt32(0),
            MaSinhVien = reader.GetInt32(1),
            LaNhomTruong = reader.GetBoolean(2),
            NgayThamGia = reader.GetDateTime(3)
        };
    }
}