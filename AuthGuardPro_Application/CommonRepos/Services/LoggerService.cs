using StayEasePro_Application.CommonRepos.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayEasePro_Application.CommonRepos.Services
{
    public class LoggerService : ILoggerService
    {
        public async Task LocalLogs(Exception ex)
        {
            try
            {
                string path = @"C:\Users\AjayKumarYechuru\source\repos\AuthGuardPro\AuthGuardPro\Properties\Logs\";

                // Ensure the directory exists
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Define the log file path with the current date (so a new file is created daily)
                string filePath = Path.Combine(path, $"Log_{DateTime.Now:ddMMyyyy}.txt");

                // Prepare the log message
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Exception: {ex.Message}\n{ex.StackTrace}\n";


                // Check if the file already exists, and if so, add two line breaks at the start of the log message
                if (File.Exists(filePath))
                {
                    logMessage = "\n\n" + logMessage;
                }

                // Append the log message to the file
                await File.AppendAllTextAsync(filePath, logMessage);
            }
            catch (Exception ex2)
            {
                throw new Exception("An error occurred while logging the exception.", ex2);
            }

        }

    }
}
