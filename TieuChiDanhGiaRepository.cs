using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class TieuChiDanhGiaRepository(DbConnectionFactory db) : ITieuChiDanhGiaRepository
{
    public async Task<List<TieuChiDanhGia>> GetAllAsync()
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaTieuChi, TenTieuChi, DiemToiDa, MoTa, TrangThai
            FROM TieuChiDanhGia
            ORDER BY MaTieuChi
            """, conn);

        using var reader = await cmd.ExecuteReaderAsync();
        var list = new List<TieuChiDanhGia>();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<TieuChiDanhGia?> GetByIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaTieuChi, TenTieuChi, DiemToiDa, MoTa, TrangThai
            FROM TieuChiDanhGia
            WHERE MaTieuChi = @id
            """, conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<int> AddAsync(TieuChiDanhGia data)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            INSERT INTO TieuChiDanhGia
            (TenTieuChi, DiemToiDa, MoTa, TrangThai)
            OUTPUT INSERTED.MaTieuChi
            VALUES (@TenTieuChi, @DiemToiDa, @MoTa, @TrangThai)
            """, conn);

        cmd.Parameters.AddWithValue("@TenTieuChi", data.TenTieuChi);
        cmd.Parameters.AddWithValue("@DiemToiDa", data.DiemToiDa);
        cmd.Parameters.AddWithValue("@MoTa", (object?)data.MoTa ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TrangThai", data.TrangThai);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task<bool> UpdateAsync(TieuChiDanhGia data)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            UPDATE TieuChiDanhGia
            SET TenTieuChi = @TenTieuChi,
                DiemToiDa = @DiemToiDa,
                MoTa = @MoTa,
                TrangThai = @TrangThai
            WHERE MaTieuChi = @MaTieuChi
            """, conn);

        cmd.Parameters.AddWithValue("@MaTieuChi", data.MaTieuChi);
        cmd.Parameters.AddWithValue("@TenTieuChi", data.TenTieuChi);
        cmd.Parameters.AddWithValue("@DiemToiDa", data.DiemToiDa);
        cmd.Parameters.AddWithValue("@MoTa", (object?)data.MoTa ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@TrangThai", data.TrangThai);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand(
            "DELETE FROM TieuChiDanhGia WHERE MaTieuChi = @id", conn);

        cmd.Parameters.AddWithValue("@id", id);

        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    private static TieuChiDanhGia Map(SqlDataReader r) => new()
    {
        MaTieuChi = r.GetInt32(0),
        TenTieuChi = r.GetString(1),
        DiemToiDa = r.GetDecimal(2),
        MoTa = r.IsDBNull(3) ? null : r.GetString(3),
        TrangThai = r.GetBoolean(4)
    };
}