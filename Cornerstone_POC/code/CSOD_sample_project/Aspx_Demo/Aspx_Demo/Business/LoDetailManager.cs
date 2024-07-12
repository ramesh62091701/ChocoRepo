using Aspx_Demo.DataRepo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Aspx_Demo.Business
{
    public class LoDetailManager
    {
        private readonly string connectionString;

        public LoDetailManager()
        {
            connectionString = GetConnectionString();
        }

        private string GetConnectionString()
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration.GetConnectionString("DefaultConnection");
        }

        public string GetLoDetail()
        {
            DatabaseConn dbConn = new DatabaseConn(connectionString);
            var assignUsers = dbConn.GetAssignUserData();
            var userCount = dbConn.GetAssignUserCount();

            return $"{assignUsers}\nTotal Users: {userCount}";
        }

        public string AddNewLoDetail(string userName)
        {
            DatabaseConn dbConn = new DatabaseConn(connectionString);
            bool success = dbConn.InsertAssignUser(userName);

            return success ? "User added successfully." : "Failed to add user.";
        }

        public string RemoveLoDetail(int userId)
        {
            DatabaseConn dbConn = new DatabaseConn(connectionString);
            bool success = dbConn.DeleteAssignUser(userId);

            return success ? "User removed successfully." : "Failed to remove user.";
        }
    }
}