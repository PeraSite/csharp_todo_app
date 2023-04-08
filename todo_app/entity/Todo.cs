using MySql.Data.MySqlClient;

namespace todo_app.entity;

public record Todo(
	int ID,
	int UserID,
	string Content,
	bool Completed
) {
	public static Todo FromSql(MySqlDataReader reader) {
		return new Todo(
			reader.GetInt32(0),
			reader.GetInt32(1),
			reader.GetString(2),
			reader.GetBoolean(3)
		);
	}
};
