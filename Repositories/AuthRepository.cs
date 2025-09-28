using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using SubscriptionDemo.Api.Models;
using BCrypt.Net;

namespace SubscriptionDemo.Api.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly string _conn;
    public AuthRepository(IConfiguration config) => _conn = config.GetConnectionString("DefaultConnection");

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var conn = new SqlConnection(_conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT TOP 1 * FROM Users WHERE Username = @Username";
        cmd.Parameters.AddWithValue("@Username", username);
        await conn.OpenAsync();
        using var rdr = await cmd.ExecuteReaderAsync();
        if (!await rdr.ReadAsync()) return null;
        return new User
        {
            Id = (int)rdr["Id"],
            Username = rdr["Username"].ToString()!,
            PasswordHash = rdr["PasswordHash"].ToString()!,
            Role = rdr["Role"].ToString()!,
            IsActive = (bool)rdr["IsActive"],
            ResetToken = rdr["ResetToken"] == DBNull.Value ? null : rdr["ResetToken"].ToString(),
            ResetTokenExpiry = rdr["ResetTokenExpiry"] == DBNull.Value ? null : (DateTime?)rdr["ResetTokenExpiry"],
            CreatedAt = (DateTime)rdr["CreatedAt"]
        };
    }

    public async Task<int> CreateAsync(User user)
    {
        using var conn = new SqlConnection(_conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Users (Username, PasswordHash, Role, IsActive, CreatedAt)
            VALUES (@Username, @PasswordHash, @Role, @IsActive, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);
        ";
        cmd.Parameters.AddWithValue("@Username", user.Username);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@Role", user.Role);
        cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
        try
        {
            await conn.OpenAsync();
            var idObj = await cmd.ExecuteScalarAsync();
            if (idObj == null || idObj == DBNull.Value)
                throw new Exception("Insert failed or no ID returned.");
            var id = (int)idObj;
            return id;
        }
        catch (Exception ex)
        {
            // Log or inspect ex.Message for details
            throw;
        }
    }
}
