using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using Serilog;

namespace HomeAsessment.Business
{
    public class VersionManagement
    {
        #region Private Properties
        private const string _directory = "Version";
        private const string _filename = "ProductInfo.cs";
        private string _filePath = Path.Combine(_directory, _filename);
        #endregion

        /// <summary>
        /// Provides access to the ProductInfo file for managing the application version
        /// </summary>
        #region Constructor
        public VersionManagement()
        {
            CreateFile();
        }
        #endregion

        /// <summary>
        /// Updates the ProductInfo file with a new major version
        /// </summary>
        #region Public Methods
        public void Start(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("consoleapp.log")
                .CreateLogger();

            try
            {
                // Get passed in value
                var release = args[0];

                // Process version if valid action is found
                if (String.Compare(release, "feature", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var newVersion = IncrementMajorVersion();
                }
                else if (String.Compare(release, "bug fix", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    var newVersion = IncrementMinorVersion();
                }
                else
                {
                    Console.WriteLine("The value passed in, is not recognizable. Accepted values are \"Feature\" or \"Bug Fix\".");
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong with the program. Check logs for additional details.");
                Log.Error(e, "Application failed to process");
            }
        }

        public string IncrementMajorVersion()
        {
            string version = File.ReadAllText(_filePath);
            var versionArr = version.Split('.');
            string newVersion;
            int newMajorVersion = int.Parse(versionArr[2]) + 1;

            newVersion = $"{versionArr[0]}.{versionArr[1]}.{newMajorVersion}.0";

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.WriteLine(newVersion);
            }

            return newVersion;
        }

        /// <summary>
        /// Updates the ProductInfo file with a new minor version
        /// </summary>
        /// <returns></returns>
        public string IncrementMinorVersion()
        {
            string version = File.ReadAllText(_filePath);
            var versionArr = version.Split('.');
            string newVersion;
            int newMinorVersion = int.Parse(versionArr[3]) + 1;

            newVersion = $"{versionArr[0]}.{versionArr[1]}.{versionArr[2]}.{newMinorVersion}";

            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                writer.WriteLine(newVersion);
            }

            return newVersion;
        }
        #endregion

        /// <summary>
        /// Creates a new ProductInfo file if one does not already exist and updates it with the current application version
        /// </summary>
        #region Private Methods
        private void CreateFile()
        {
            if (!File.Exists(_filePath))
            {
                if (!Directory.Exists(_directory))
                {
                    Directory.CreateDirectory(_directory);
                }

                File.Create(_filePath).Close();

                var appInfo = System.Reflection.Assembly.GetExecutingAssembly();
                var fieVersionInfo = FileVersionInfo.GetVersionInfo(appInfo.Location);
                var version = fieVersionInfo.FileVersion;

                using (StreamWriter writer = new StreamWriter(_filePath))
                {
                    writer.WriteLine(version);
                }
            }
        }
        #endregion
    }
}
