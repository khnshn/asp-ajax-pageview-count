using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Analytics for page visits
/// </summary>
public class Analytics
{
    private static string connectionString;
    /// <summary>
    /// create an instance and initialize connectionString
    /// </summary>
    public Analytics()
    {
        connectionString = "your connection string";
    }
    /// <summary>
    /// records a visit to current page from current client
    /// </summary>
    /// <param name="client">client ip address</param>
    /// <param name="page">current page address</param>
    public void recordVisit(string client, string page)
    {
        string pageName = page.ToLower().Replace("http://", "");
        pageName = pageName.ToLower().Replace("www.", "");
        string[] pages = pageName.Split('/');
        if (pages[pages.Length - 1].Equals("default.aspx") && pages.Length == 2)
        {
            pageName = pageName.Replace("default.aspx", "");
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "insert into log (host,timestamp,page) values (@client, GETDATE(), @page)";
            cmd.Parameters.AddWithValue("client", client);
            cmd.Parameters.AddWithValue("page", pageName);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    /// <summary>
    /// gets the number of visits to current page
    /// </summary>
    /// <param name="page">current page</param>
    /// <returns>number of visits of the current page</returns> 
    public string getPageVisit(string page)
    {
        string count = string.Empty;
        string pageName = page.ToLower().Replace("http://", "");
        pageName = pageName.ToLower().Replace("www.", "");
        string[] pages = pageName.Split('/');
        if (pages[pages.Length - 1].Equals("default.aspx") && pages.Length == 2)
        {
            pageName = pageName.Replace("default.aspx", "");
        }
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandText = "select sum(c) from (select count(*) as c from log where page like @page group by page) as tbl";
            cmd.Parameters.AddWithValue("page", "%" + pageName);
            count = cmd.ExecuteScalar().ToString();
            connection.Close();
        }
        return count.Length > 0 ? count : "0";
    }
}