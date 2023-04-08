using MySql.Data.MySqlClient;

namespace todo_app.database;

public class MySqlDatabase {
	private readonly MySqlConnection _connection;

	public MySqlDatabase(
		string server,
		uint port,
		string database,
		string user,
		string password
	) {
		MySqlConnectionStringBuilder connectionString = new MySqlConnectionStringBuilder {
			Server = server,
			Port = port,
			Database = database,
			UserID = user,
			Password = password
		};
		_connection = new MySqlConnection(connectionString.ToString());
		_connection.Open();
	}

	~MySqlDatabase() {
		_connection.Close();
	}

	public MySqlDataReader Execute(string query) {
		MySqlCommand cmd = new MySqlCommand(query, _connection);
		MySqlDataReader rdr = cmd.ExecuteReader();
		return rdr;
	}

	public int ExecuteNonQuery(string query) {
		MySqlCommand cmd = new MySqlCommand(query, _connection);
		return cmd.ExecuteNonQuery();
	}
}
