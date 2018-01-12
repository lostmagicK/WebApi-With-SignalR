using Dependency2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Dependency2.DAL
{
    public class DataAccess
    {
        public static IEnumerable<Employee> GetData()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [ID],[FirstName],[LastName],[Email],[Gender] FROM [dbo].[Employee]", connection))
                {
                    command.Notification = null;
                    var dependency = new SqlDependency(command);
                    command.ExecuteNonQuery();
                    dependency.OnChange += new OnChangeEventHandler(Dependency_OnChange);
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    
                    using (var reader = command.ExecuteReader())
                        return reader.Cast<IDataRecord>()
                            .Select(x => new Employee()
                            {
                                ID = x.GetInt32(0),
                                FirstName = x.GetString(1),
                                LastName = x.GetString(2),
                                Gender = x.GetString(3),
                                Email = x.GetString(4)
                            }).ToList();
                }
            }
        }

        private static void Dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if(e.Info == SqlNotificationInfo.Insert)
            MyHub.Show();
            GetData();
        }
    }
}