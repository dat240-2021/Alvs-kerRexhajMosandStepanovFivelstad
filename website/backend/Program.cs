using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ImportDefaultImages();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
        private static void ImportDefaultImages()
        {
            var db = new SqliteConnection($"Data Source={Path.Combine("Infrastructure", "Data", "game.db")}");
            db.Open();
            var query = db.CreateCommand();
            query.CommandText = "SELECT * FROM Image";
            var result = query.ExecuteReader();
            bool runImport;
            if (result.HasRows == false)
            {
                runImport = true;
            }
            else
            {
                runImport = false; }
            db.Close();
            
                
            if (runImport)
            {
                Process proc = new Process();
                proc.StartInfo.FileName = "Python.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.Arguments = ".\\Infrastructure\\Data\\Insert_Images_to_Db.py";
                proc.Start();
                Console.WriteLine("Python Script is running for importing image data to database tables");
            }
        }
    }
}
