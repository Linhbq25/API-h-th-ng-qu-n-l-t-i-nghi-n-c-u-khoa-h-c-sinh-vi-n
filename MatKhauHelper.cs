namespace quanlyNCKH.API.Helpers;

public static class MatKhauHelper
{
    public static string Hash(string matKhau)
        => BCrypt.Net.BCrypt.HashPassword(matKhau);

    public static bool Verify(string matKhau, string hash)
        => BCrypt.Net.BCrypt.Verify(matKhau, hash);
}