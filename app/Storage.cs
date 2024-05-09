using Microsoft.Extensions.Configuration;
using Npgsql;

public class Storage
{
      private readonly string _connString;
      private readonly string _tableName;
      public Storage(string tableName)
      {
            var configBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configBuilder.Build();
            _connString = configuration.GetConnectionString("MetricsDatabase");
            _tableName = tableName;
      }

      // Подключение/создание
      public async Task CreateTable()
      {
            try
            {
                  var con = new NpgsqlConnection(
                  connectionString: _connString);
                  con.Open();
                  using var cmd = new NpgsqlCommand();
                  cmd.Connection = con;

                  cmd.CommandText= $"CREATE TABLE IF NOT EXISTS {_tableName} (id SERIAL PRIMARY KEY," +
                  "date_time VARCHAR(20)," +
                  "MMTB BIGINT," +
                  "PRMB BIGINT," +
                  "PCST NUMERIC," +
                  "NRBT BIGINT," +
                  "NTBT BIGINT," +
                  "FSB BIGINT," +
                  "FFB BIGINT)";
                  await cmd.ExecuteNonQueryAsync();
                  Console.WriteLine("[INFO] Table created/connected.");
            }
            catch (Exception  ex)
            {
                  Console.WriteLine($"[Error] {ex.Message}");
                  throw;
            }
      }

      // Добавление данных в БД
      public async Task Insert(string currentDateTime, long PRMB_value, long MMTB_value, double PCST_value, long NRBT_value, long NTBT_value, long FSB_value, long FFB_value)
      {
            try
            {
                  var con = new NpgsqlConnection(
                         connectionString: _connString);

                  con.Open();
                  using var cmd = new NpgsqlCommand();
                  cmd.Connection = con;

                  cmd.CommandText = $"INSERT INTO {_tableName} (date_time, PRMB, MMTB, PCST, NRBT, NTBT, FSB, FFB) VALUES (@date_time, @PRMB, @MMTB, @PCST, @NRBT, @NTBT, @FSB, @FFB);";
                  cmd.Parameters.AddWithValue("date_time", currentDateTime);
                  cmd.Parameters.AddWithValue("PRMB", PRMB_value);
                  cmd.Parameters.AddWithValue("MMTB", MMTB_value);
                  cmd.Parameters.AddWithValue("PCST", PCST_value);
                  cmd.Parameters.AddWithValue("NRBT", NRBT_value);
                  cmd.Parameters.AddWithValue("NTBT", NTBT_value);
                  cmd.Parameters.AddWithValue("FSB", FSB_value);
                  cmd.Parameters.AddWithValue("FFB", FFB_value);
                  await cmd.ExecuteNonQueryAsync();
                  
                  Console.WriteLine($"[INFO] Data for {currentDateTime} inserted successfully.");
            }
            catch (Exception ex)
            {
                  Console.WriteLine($"[Error] {ex.Message}");
                  throw;
            }
      }

      // Удаление данных по id
      public async Task Delete(int id)
      {
            try
            {
                  var con = new NpgsqlConnection(
                         connectionString: _connString);

                  con.Open();
                  using var cmd = new NpgsqlCommand();
                  cmd.Connection = con;

                  cmd.CommandText = $"DELETE FROM {_tableName} WHERE id = {id};";
                  await cmd.ExecuteNonQueryAsync();
                  Console.WriteLine($"[INFO] Record with id = {id} succesfully deleted.");
            }
            catch (Exception ex)
            {
                  Console.WriteLine($"[Error] {ex.Message}");
                  throw;
            }
      }

      // Получение записей из БД
      public async Task<IEnumerable<MetricsRecord>> GetMetricsRecords()
      {
            try
            {
                  var con = new NpgsqlConnection(
                         connectionString: _connString);

                  con.Open();
                  using var cmd = new NpgsqlCommand();
                  cmd.Connection = con;

                  cmd.CommandText = $"SELECT * from {_tableName};";
                  NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
                  
                  var result = new List<MetricsRecord>();

                  while (await reader.ReadAsync())
                  {
                        result.Add(new MetricsRecord(
                              id: (int)reader["id"],
                              date_time: (string)reader["date_time"],
                              MMTB: (long)reader["MMTB"],
                              PRMB: (long)reader["PRMB"],
                              PCST: (double)reader.GetDecimal(reader.GetOrdinal("PCST")),
                              NRBT: (long)reader["NRBT"],
                              NTBT: (long)reader["NTBT"],
                              FSB: (long)reader["FSB"],
                              FFB: (long)reader["FFB"]
                        ));
                  }
                  
                  return result;
            }
            catch (Exception ex)
            {
                  Console.WriteLine($"[Error] {ex.Message}");
                  throw;
            }     
      }
}