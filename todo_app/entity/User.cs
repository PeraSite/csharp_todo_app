using MySql.Data.MySqlClient;

namespace todo_app.entity;

public record User(
	int ID,
	string Name
) {

	public static User FromSql(MySqlDataReader reader) {
		return new User(
			reader.GetInt32(0),
			reader.GetString(1)
		);
	}
};
