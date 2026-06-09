using Microsoft.Data.SqlClient;
using System.Data;
using TongTinApp.Data;


namespace TongTinApp.Data
{
    public class Database
    {
       private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                                ?? throw new InvalidOperationException("Missing connection string");
        }

        public DataTable GetAll()
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = "SELECT ID, Name, Role, Status, Doing, RegBy, RegDate, UpdateBy, UpdateDate FROM MstMembers";
            using var cmd = new SqlCommand(sql, conn);
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

         public void Add(string name, string role, string Status, string Doing,
                string regBy, DateTime regDate,
                string updateBy, DateTime updateDate,
                string Receive)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = @"INSERT INTO MstMembers (Name, Role, Status, Doing, RegBy, RegDate, UpdateBy, UpdateDate, Receive)
                           VALUES (@Name, @Role, @Status, @Doing, @RegBy, @RegDate, @UpdateBy, @UpdateDate, @Receive)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@Status", Status);
            cmd.Parameters.AddWithValue("@Doing", Doing);
            cmd.Parameters.AddWithValue("@RegBy", regBy);
            cmd.Parameters.AddWithValue("@RegDate", regDate);
            cmd.Parameters.AddWithValue("@UpdateBy", updateBy);
            cmd.Parameters.AddWithValue("@UpdateDate", updateDate);
            cmd.Parameters.AddWithValue("@Receive", Receive);
            cmd.ExecuteNonQuery();
        }

        public void Update(int id, string name, string role, string status, string doing,
                           string updateBy, DateTime updateDate,
                           string regBy, DateTime regDate,
                           string Receive)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = @"UPDATE MstMembers 
                           SET Name=@Name, Role=@Role, Status=@Status, Doing=@Doing,
                               UpdateBy=@UpdateBy, UpdateDate=@UpdateDate,
                               RegBy=@RegBy, RegDate=@RegDate, Receive=@Receive
                           WHERE ID=@Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Role", role);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@Doing", doing);
            cmd.Parameters.AddWithValue("@UpdateBy", updateBy);
            cmd.Parameters.AddWithValue("@UpdateDate", updateDate);
            cmd.Parameters.AddWithValue("@RegBy", regBy);
            cmd.Parameters.AddWithValue("@RegDate", regDate);
            cmd.Parameters.AddWithValue("@Receive", Receive);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = "DELETE FROM MstMembers WHERE ID=@Id";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.ExecuteNonQuery();
        }

        public DataTable Search(string keyword)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            string sql = @"SELECT ID, Name, Role, Status, Doing, RegBy, RegDate, UpdateBy, UpdateDate 
                           FROM MstMembers 
                           WHERE Name LIKE @kw OR Role LIKE @kw";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
            using var adapter = new SqlDataAdapter(cmd);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
       public List<MonthlyPaymentRecord> GetMonthlyPayments()
        {
            var results = new List<MonthlyPaymentRecord>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

                string sql = @"​SELECT m.Id, m.Name, m.Role, m.Status, m.Doing,
                b.Book, (b.Money) AS Money,
                DATENAME(MONTH, b.Month) AS MonthName,
                YEAR(b.Month) AS Year
            FROM MstMembers m
            INNER JOIN tbBooks b ON m.Id = b.Id";

            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new MonthlyPaymentRecord
                {
                   Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2),
                    Status = reader.GetString(3),
                    Doing = reader.GetString(4),
                    Book = reader.GetInt32(5),
                    Money = Convert.ToDecimal(reader.GetDouble(6)),
                    Month = reader.GetString(7),
                    Year = reader.GetInt32(8)
                });
            }

            return results;
        }


        public List<MonthlyPaymentRecord> SearchMonthlyPayments(string text)
        {
            var results = new List<MonthlyPaymentRecord>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();

           string sql = @"
            SELECT m.Id, m.Name, m.Role, m.Status, m.Doing,
                b.Book, (b.Money) AS Money,
                DATENAME(MONTH, b.Month) AS MonthName,
                YEAR(b.Month) AS Year
            FROM MstMembers m
            INNER JOIN tbBooks b ON m.Id = b.Id
            WHERE m.Name LIKE @Text 
            OR DATENAME(MONTH, b.Month) LIKE @Text 
            OR CAST(YEAR(b.Month) AS NVARCHAR) LIKE @Text";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Text", "%" + text + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new MonthlyPaymentRecord
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2),
                    Status = reader.GetString(3),
                    Doing = reader.GetString(4),
                    Book = reader.GetInt32(5),
                    Money = Convert.ToDecimal(reader.GetDouble(6)),
                    Month = reader.GetString(7),
                    Year = reader.GetInt32(8)
                });
            }

            return results;
        }
        public List<Member> GetMembers()
        {
            var results = new List<Member>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT Id, Name, Role, Status, Doing FROM MstMembers WHERE Receive = 'Active'";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new Member
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2),
                    Status = reader.GetString(3),
                    Doing = reader.GetString(4)
                });
            }
            return results;
        }

        public List<Member> SearchMembers(string text)
        {
            var results = new List<Member>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();

            string sql = "SELECT ID, Name, Role, Status, Doing FROM MstMembers WHERE Name LIKE @Text";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Text", "%" + text + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new Member
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Role = reader.GetString(2),
                    Status = reader.GetString(3),
                    Doing = reader.GetString(4)
                });
            }
            return results;
        }
        public void UpdateWinner(int memberId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    "UPDATE MstMembers SET Receive = @Status, Doing = @Doing WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Status", "InActive");
                cmd.Parameters.AddWithValue("@Doing", "បានទទួល");
                cmd.Parameters.AddWithValue("@Id", memberId);
                cmd.ExecuteNonQuery();
            }
        }

    }
}
