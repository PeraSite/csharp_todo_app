using MySql.Data.MySqlClient;

namespace todo_app;

internal static class Program {
	private static void Main() {
		MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder {
			Server = Environment.GetEnvironmentVariable("MYSQL_SERVER"),
			Port = 8080,
			Database = "week5",
			UserID = "root",
			Password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD")
		};

		using MySqlConnection connection = new MySqlConnection(connectionString.ToString());
		connection.Open();

		MySqlCommand cmd = new MySqlCommand("SELECT * FROM todo", connection);
		MySqlDataReader rdr = cmd.ExecuteReader();
		while (rdr.Read()) {
			Console.WriteLine(rdr["content"]);
		}
		rdr.Close();
	}
}
