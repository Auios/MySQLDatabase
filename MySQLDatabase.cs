using System;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;

public static class MySQLDatabase
{
    private static MySqlConnection conn;
    public static DataTable table;


    public static bool Connect(string server, int port, string database, string userID, string password)
    {
        if (IsConnected())
            return false;

        string connStr =
            "Server=" + server + ";" +
            "Port=" + port + ";" +
            "Database=" + database + ";" +
            "Uid=" + userID + ";" +
            "Pwd=" + password + ";" +
            "Persistsecurityinfo=True;" +
            "Convert Zero Datetime=True;" +
            "SSLMode=None";

        conn = new MySqlConnection(connStr);
        conn.Open();

        return IsConnected();
    }

    public static bool IsConnected()
    {
        return conn != null && conn.State == ConnectionState.Open;
    }

    public static void Disconnect()
    {
        if (IsConnected())
        {
            conn.Close();
            conn.Dispose();
            GC.Collect();
        }
    }

    public static int Query(string statement)
    {
        if (!IsConnected())
            return 0;

        MySqlCommand cmd = new MySqlCommand(statement, conn);
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

        if (table != null)
            table.Dispose();

        table = new DataTable();
        da.Fill(table);

        return table.Rows.Count;
    }

    public static string DateTime(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }
    
    public static string CleanString(string input)
    {
        return MySqlHelper.EscapeString(input);
    }
}
