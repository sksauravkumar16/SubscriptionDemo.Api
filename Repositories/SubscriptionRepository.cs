using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SubscriptionDemo.Api.Models;

namespace SubscriptionDemo.Api.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly string _conn;
    public SubscriptionRepository(IConfiguration config) => _conn = config.GetConnectionString("DefaultConnection");

    public async Task<IEnumerable<CustomerSubscription>> GetAsync(string customerId, string? subscriptionName, DateTime? start, DateTime? end)
    {
        var list = new List<CustomerSubscription>();
        using var conn = new SqlConnection(_conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT * FROM CustomerSubscriptions
            WHERE CustomerId = @CustomerId
            AND (@SubscriptionName IS NULL OR SubscriptionName = @SubscriptionName)
            AND (@StartDate IS NULL OR StartDate >= @StartDate)
            AND (@EndDate IS NULL OR EndDate <= @EndDate)
            ORDER BY Id;
        ";
        cmd.Parameters.AddWithValue("@CustomerId", customerId);
        cmd.Parameters.AddWithValue("@SubscriptionName", (object?)subscriptionName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@StartDate", (object?)start ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@EndDate", (object?)end ?? DBNull.Value);

        await conn.OpenAsync();
        using var rdr = await cmd.ExecuteReaderAsync();
        while (await rdr.ReadAsync())
        {
            list.Add(new CustomerSubscription
            {
                Id = (int)rdr["Id"],
                CustomerId = rdr["CustomerId"].ToString()!,
                CustomerName = rdr["CustomerName"].ToString()!,
                SubscriptionName = rdr["SubscriptionName"].ToString()!,
                SubscriptionCount = rdr["SubscriptionCount"] == DBNull.Value ? 0 : (int)rdr["SubscriptionCount"],
                StartDate = rdr["StartDate"] == DBNull.Value ? null : (DateTime?)rdr["StartDate"],
                EndDate = rdr["EndDate"] == DBNull.Value ? null : (DateTime?)rdr["EndDate"],
                IsActive = (bool)rdr["IsActive"],
                CreatedAt = (DateTime)rdr["CreatedAt"],
                UpdatedAt = rdr["UpdatedAt"] == DBNull.Value ? null : (DateTime?)rdr["UpdatedAt"]
            });
        }
        return list;
    }

    public async Task<int> CreateAsync(CustomerSubscription item)
    {
        using var conn = new SqlConnection(_conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO CustomerSubscriptions (CustomerId, CustomerName, SubscriptionName, SubscriptionCount, StartDate, EndDate, IsActive, CreatedAt)
            VALUES (@CustomerId, @CustomerName, @SubscriptionName, @SubscriptionCount, @StartDate, @EndDate, @IsActive, GETDATE());
            SELECT CAST(SCOPE_IDENTITY() as int);
        ";
        cmd.Parameters.AddWithValue("@CustomerId", item.CustomerId);
        cmd.Parameters.AddWithValue("@CustomerName", item.CustomerName);
        cmd.Parameters.AddWithValue("@SubscriptionName", item.SubscriptionName);
        cmd.Parameters.AddWithValue("@SubscriptionCount", item.SubscriptionCount);
        cmd.Parameters.AddWithValue("@StartDate", (object?)item.StartDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@EndDate", (object?)item.EndDate ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@IsActive", item.IsActive);
        await conn.OpenAsync();
        var id = (int)await cmd.ExecuteScalarAsync();
        return id;
    }

    public async Task<bool> UpdateCountAsync(string customerId, string subscriptionName, int delta)
    {
        using var conn = new SqlConnection(_conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            UPDATE CustomerSubscriptions
            SET SubscriptionCount = CASE WHEN SubscriptionCount + @Delta >= 0 THEN SubscriptionCount + @Delta ELSE 0 END,
                UpdatedAt = GETDATE()
            WHERE CustomerId = @CustomerId AND SubscriptionName = @SubscriptionName;
        ";
        cmd.Parameters.AddWithValue("@Delta", delta);
        cmd.Parameters.AddWithValue("@CustomerId", customerId);
        cmd.Parameters.AddWithValue("@SubscriptionName", subscriptionName);
        await conn.OpenAsync();
        var affected = await cmd.ExecuteNonQueryAsync();
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var conn = new SqlConnection(_conn);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM CustomerSubscriptions WHERE Id = @Id";
        cmd.Parameters.AddWithValue("@Id", id);

        await conn.OpenAsync();
        var affected = await cmd.ExecuteNonQueryAsync();
        return affected > 0; // true if at least 1 row deleted
    }
}
