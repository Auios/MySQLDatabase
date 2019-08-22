using System;
using System.Data;
using MySql.Data.MySqlClient;

public class MySQLDatabase
{
    private MySqlConnection conn;
    public DataTable table;

    public bool Connect(string server, int port, string database, string userID, string password)
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
            "SSLMode=None;" +
            "charset=utf8";

        conn = new MySqlConnection(connStr);
        conn.Open();

        return IsConnected();
    }

    public bool IsConnected()
    {
        return conn != null && conn.State == ConnectionState.Open;
    }

    public void Disconnect()
    {
        if (IsConnected())
        {
            conn.Close();
            conn.Dispose();
            GC.Collect();
        }
    }

    public int Query(string statement)
    {
        if (!IsConnected())
            throw new Exception("MySQLDatabase is not connected");

        MySqlCommand cmd = new MySqlCommand(statement, conn);
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

        if (table != null)
            table.Dispose();

        table = new DataTable();
        da.Fill(table);

        return table.Rows.Count;
    }

    public string DateTime(DateTime dt)
    {
        return dt.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public string CleanString(string input)
    {
        return MySqlHelper.EscapeString(input);
    }
}
