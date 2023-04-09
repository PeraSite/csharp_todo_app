using System.Collections;
using MySql.Data.MySqlClient;
using todo_app.database;
using todo_app.entity;

namespace todo_app.repository;

public class UserRepository {
	private readonly MySqlDatabase _database;

	public UserRepository(MySqlDatabase database) {
		_database = database;
	}

	public bool IsExistUser(string username) {
		var usernameParam = (new MySqlParameter("@name", MySqlDbType.VarChar, 256), username);
		using MySqlDataReader reader = _database.Execute("SELECT 1 FROM user WHERE name = @name", usernameParam);
		return reader.HasRows;
	}

	public bool AddUser(string username) {
		if (IsExistUser(username))
			return false;

		var usernameParam = (new MySqlParameter("@name", MySqlDbType.VarChar, 256), username);
		using MySqlDataReader reader = _database.Execute("INSERT INTO user (name) VALUES (@name)", usernameParam);
		return reader.HasRows;
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
