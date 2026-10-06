using System.Data;
using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class NhomSinhVienRepository(DbConnectionFactory db) : INhomSinhVienRepository
{
    public async Task<List<NhomSinhVien>> GetAllAsync()
    {
        var list = new List<NhomSinhVien>();

        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaNhom, MaNhomCode, TenNhom, MaNhomTruong,
                   TrangThai, NgayTao
            FROM NhomSinhVien
            ORDER BY MaNhom DESC
            """;

        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<NhomSinhVien?> GetByIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT MaNhom, MaNhomCode, TenNhom, MaNhomTruong,
                   TrangThai, NgayTao
            FROM NhomSinhVien
            WHERE MaNhom = @MaNhom
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", id);

        using var reader = await cmd.ExecuteReaderAsync();

        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> AddAsync(NhomSinhVien nhom)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            INSERT INTO NhomSinhVien
                (MaNhomCode, TenNhom, MaNhomTruong, TrangThai)
            OUTPUT INSERTED.MaNhom
            VALUES
                (@MaNhomCode, @TenNhom, @MaNhomTruong, @TrangThai)
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhomCode", nhom.MaNhomCode);
        cmd.Parameters.AddWithValue("@TenNhom", nhom.TenNhom);
        cmd.Parameters.AddWithValue("@MaNhomTruong", nhom.MaNhomTruong);
        cmd.Parameters.AddWithValue("@TrangThai", nhom.TrangThai);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(NhomSinhVien nhom)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            UPDATE NhomSinhVien
            SET MaNhomCode = @MaNhomCode,
                TenNhom = @TenNhom,
                MaNhomTruong = @MaNhomTruong,
                TrangThai = @TrangThai
            WHERE MaNhom = @MaNhom
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaNhom", nhom.MaNhom);
        cmd.Parameters.AddWithValue("@MaNhomCode", nhom.MaNhomCode);
        cmd.Parameters.AddWithValue("@TenNhom", nhom.TenNhom);
        cmd.Parameters.AddWithValue("@MaNhomTruong", nhom.MaNhomTruong);
        cmd.Parameters.AddWithValue("@TrangThai", nhom.TrangThai);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            DELETE FROM NhomSinhVien
            WHERE MaNhom = @MaNhom
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DaCoMaNhomCodeAsync(string maNhomCode, int? id = null)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM NhomSinhVien
            WHERE MaNhomCode = @MaNhomCode
              AND (@MaNhom IS NULL OR MaNhom <> @MaNhom)
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhomCode", maNhomCode);
        cmd.Parameters.AddWithValue("@MaNhom", (object?)id ?? DBNull.Value);

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

    public async Task<bool> SinhVienLaTruongNhomKhacAsync(
        int maSinhVien,
        int? id = null)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT COUNT(1)
            FROM NhomSinhVien
            WHERE MaNhomTruong = @MaSinhVien
              AND (@MaNhom IS NULL OR MaNhom <> @MaNhom)
            """;

        using var cmd = new SqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@MaSinhVien", maSinhVien);
        cmd.Parameters.AddWithValue("@MaNhom", (object?)id ?? DBNull.Value);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> DangDuocSuDungAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        const string sql = """
            SELECT CASE WHEN EXISTS
            (
                SELECT 1
                FROM ThanhVienNhom
                WHERE MaNhom = @MaNhom

                UNION ALL

                SELECT 1
                FROM DangKyDeTai
                WHERE MaNhom = @MaNhom
            )
            THEN 1 ELSE 0 END
            """;

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaNhom", id);

        return (int)await cmd.ExecuteScalarAsync() == 1;
    }

    private static NhomSinhVien Map(SqlDataReader reader)
    {
        return new NhomSinhVien
        {
            MaNhom = reader.GetInt32(0),
            MaNhomCode = reader.GetString(1),
            TenNhom = reader.GetString(2),
            MaNhomTruong = reader.GetInt32(3),
            TrangThai = reader.GetString(4),
            NgayTao = reader.GetDateTime(5)
        };
    }
}