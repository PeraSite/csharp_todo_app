using MySql.Data.MySqlClient;
using todo_app.database;
using todo_app.entity;

namespace todo_app.repository;

public class TodoRepository {
	private readonly MySqlDatabase _database;

	public TodoRepository(MySqlDatabase database) {
		_database = database;
	}

	public List<User> GetAllUsers() {
		using MySqlDataReader reader = _database.Execute("SELECT * FROM todo");
		var users = new List<User>();
		while (reader.Read()) {
			users.Add(User.FromSql(reader));
		}
		return users;
	}
}
