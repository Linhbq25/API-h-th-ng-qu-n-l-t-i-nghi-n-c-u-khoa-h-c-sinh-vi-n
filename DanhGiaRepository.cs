using Microsoft.Data.SqlClient;
using quanlyNCKH.API.Data;
using quanlyNCKH.API.Models;

namespace quanlyNCKH.API.Repositories;

public class DanhGiaRepository(DbConnectionFactory db) : IDanhGiaRepository
{
    public async Task<List<DanhGia>> GetAllAsync()
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaDanhGia, MaDangKy, MaTaiKhoanNguoiDanhGia,
                   NhanXet, TongDiem, NgayDanhGia
            FROM DanhGia
            ORDER BY MaDanhGia DESC
            """, conn);

        using var reader = await cmd.ExecuteReaderAsync();
        var list = new List<DanhGia>();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<DanhGia?> GetByIdAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaDanhGia, MaDangKy, MaTaiKhoanNguoiDanhGia,
                   NhanXet, TongDiem, NgayDanhGia
            FROM DanhGia
            WHERE MaDanhGia = @id
            """, conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync() ? Map(reader) : null;
    }

    public async Task<List<DanhGia>> GetByDangKyAsync(int maDangKy)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT MaDanhGia, MaDangKy, MaTaiKhoanNguoiDanhGia,
                   NhanXet, TongDiem, NgayDanhGia
            FROM DanhGia
            WHERE MaDangKy = @MaDangKy
            ORDER BY MaDanhGia DESC
            """, conn);

        cmd.Parameters.AddWithValue("@MaDangKy", maDangKy);

        using var reader = await cmd.ExecuteReaderAsync();
        var list = new List<DanhGia>();

        while (await reader.ReadAsync())
            list.Add(Map(reader));

        return list;
    }

    public async Task<int> AddAsync(
        DanhGia danhGia,
        List<ChiTietDanhGia> chiTiet)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        using var transaction = conn.BeginTransaction();

        try
        {
            var cmd = new SqlCommand("""
                INSERT INTO DanhGia
                (MaDangKy, MaTaiKhoanNguoiDanhGia, NhanXet, TongDiem, NgayDanhGia)
                OUTPUT INSERTED.MaDanhGia
                VALUES
                (@MaDangKy, @MaTaiKhoanNguoiDanhGia,
                 @NhanXet, @TongDiem, GETDATE())
                """, conn, transaction);

            cmd.Parameters.AddWithValue("@MaDangKy", danhGia.MaDangKy);
            cmd.Parameters.AddWithValue(
                "@MaTaiKhoanNguoiDanhGia",
                danhGia.MaTaiKhoanNguoiDanhGia);
            cmd.Parameters.AddWithValue(
                "@NhanXet",
                (object?)danhGia.NhanXet ?? DBNull.Value);
            cmd.Parameters.AddWithValue(
                "@TongDiem",
                (object?)danhGia.TongDiem ?? DBNull.Value);

            var maDanhGia = (int)await cmd.ExecuteScalarAsync();

            foreach (var ct in chiTiet)
            {
                var cmdCt = new SqlCommand("""
                    INSERT INTO ChiTietDanhGia
                    (MaDanhGia, MaTieuChi, Diem, NhanXet)
                    VALUES
                    (@MaDanhGia, @MaTieuChi, @Diem, @NhanXet)
                    """, conn, transaction);

                cmdCt.Parameters.AddWithValue("@MaDanhGia", maDanhGia);
                cmdCt.Parameters.AddWithValue("@MaTieuChi", ct.MaTieuChi);
                cmdCt.Parameters.AddWithValue("@Diem", ct.Diem);
                cmdCt.Parameters.AddWithValue(
                    "@NhanXet",
                    (object?)ct.NhanXet ?? DBNull.Value);

                await cmdCt.ExecuteNonQueryAsync();
            }

            transaction.Commit();
            return maDanhGia;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        using var transaction = conn.BeginTransaction();

        try
        {
            var cmdCt = new SqlCommand(
                "DELETE FROM ChiTietDanhGia WHERE MaDanhGia = @id",
                conn,
                transaction);

            cmdCt.Parameters.AddWithValue("@id", id);
            await cmdCt.ExecuteNonQueryAsync();

            var cmd = new SqlCommand(
                "DELETE FROM DanhGia WHERE MaDanhGia = @id",
                conn,
                transaction);

            cmd.Parameters.AddWithValue("@id", id);

            var result = await cmd.ExecuteNonQueryAsync();

            transaction.Commit();
            return result > 0;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
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

    public async Task<bool> TaiKhoanTonTaiAsync(int maTaiKhoan)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT COUNT(*)
            FROM TaiKhoan
            WHERE MaTaiKhoan = @MaTaiKhoan
              AND TrangThai = 1
            """, conn);

        cmd.Parameters.AddWithValue("@MaTaiKhoan", maTaiKhoan);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    public async Task<bool> TieuChiTonTaiAsync(int maTieuChi)
    {
        using var conn = db.CreateConnection();
        await conn.OpenAsync();

        var cmd = new SqlCommand("""
            SELECT COUNT(*)
            FROM TieuChiDanhGia
            WHERE MaTieuChi = @MaTieuChi
              AND TrangThai = 1
            """, conn);

        cmd.Parameters.AddWithValue("@MaTieuChi", maTieuChi);

        return (int)await cmd.ExecuteScalarAsync() > 0;
    }

    private static DanhGia Map(SqlDataReader r) => new()
    {
        MaDanhGia = r.GetInt32(0),
        MaDangKy = r.GetInt32(1),
        MaTaiKhoanNguoiDanhGia = r.GetInt32(2),
        NhanXet = r.IsDBNull(3) ? null : r.GetString(3),
        TongDiem = r.IsDBNull(4) ? null : r.GetDecimal(4),
        NgayDanhGia = r.GetDateTime(5)
    };
}