using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;

namespace VehicleKhatabook.SchemaBuilder
{
    public class SqlScriptRunner
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private SqlConnection _connection = null!;

        public SqlScriptRunner(ILogger logger, string connectionString)
        {
            _logger = logger;
            _connectionString = connectionString;
        }

        public async Task ApplyScriptsAsync(List<string> folders)
        {
            using (_connection = new SqlConnection(_connectionString))
            {
                await _connection.OpenAsync();
                foreach (string folder in folders)
                {
                    await ApplySqlScriptsInFolderAsync(folder);
                }
            }
        }

        private async Task ApplySqlScriptsInFolderAsync(string folder)
        {
            try
            {
                string location = Assembly.GetExecutingAssembly().Location;
                string path = Path.Combine(Path.GetDirectoryName(location) ?? "", folder);
                _logger.LogInformation("Checking for SQL scripts in: " + path + ".");

                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path, "*.sql");
                    if (files.Length == 0)
                    {
                        _logger.LogInformation("No SQL files exist in folder " + path + ".  Skipping...");
                        return;
                    }

                    IOrderedEnumerable<string> orderedFiles = files.OrderBy((string e) => e);
                    foreach (string fileName in orderedFiles)
                    {
                        await ApplyFileAsync(fileName);
                    }
                }
                else
                {
                    _logger.LogInformation("Directory " + path + " does not exist.  Skipping...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred when applying scripts in folder: " + folder + ", Error is: " + ex.Message);
            }
        }

        private async Task ApplyFileAsync(string fileName)
        {
            try
            {
                _logger.LogInformation("Applying file '" + fileName + "'...");

                string sqlContent = await File.ReadAllTextAsync(fileName);
                using SqlCommand command = _connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = sqlContent;
                await command.ExecuteNonQueryAsync();

                _logger.LogInformation("File '" + fileName + "', was applied successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to apply file '" + fileName + "', Error is: " + ex.Message);
            }
        }
    }
}
