using MySql.Data.MySqlClient;
using todo_app.database;
using todo_app.entity;

namespace todo_app.repository;

public class TodoRepository {
	private readonly MySqlDatabase _database;

	public TodoRepository(MySqlDatabase database) {
		_database = database;
	}

	public bool AddTodo(User user, string content) {
		(MySqlParameter, int ID) userParameter = (new MySqlParameter("@user_id", MySqlDbType.Int32), user.ID);
		(MySqlParameter, string content) contentParameter = (new MySqlParameter("@content", MySqlDbType.VarChar, 256), content);

		var affectedRow = _database.ExecuteNonQuery("INSERT INTO todo (user_id, content, completed) VALUES (@user_id, @content, false)",
			userParameter, contentParameter);

		return affectedRow == 1;
	}

	public List<Todo> GetAllTodo(User user) {
		(MySqlParameter, int ID) userParameter = (new MySqlParameter("@user_id", MySqlDbType.Int32), user.ID);

		using MySqlDataReader reader = _database.Execute("SELECT * FROM todo WHERE user_id = @user_id", userParameter);
		var todos = new List<Todo>();
		while (reader.Read()) {
			todos.Add(Todo.FromSql(reader));
		}
		return todos;
	}

	public bool SetTodoDone(uint todoId) {
		(MySqlParameter, uint todoId) todIdParameter = (new MySqlParameter("@todo_id", MySqlDbType.Int32), todoId);
		var affectedRow = _database.ExecuteNonQuery("UPDATE todo SET completed = true WHERE id = @todo_id", todIdParameter);
		return affectedRow == 1;
	}

	public bool DeleteTodo(uint todoId) {
		(MySqlParameter, uint todoId) todIdParameter = (new MySqlParameter("@todo_id", MySqlDbType.Int32), todoId);
		var affectedRow = _database.ExecuteNonQuery("DELETE FROM todo WHERE id = @todo_id", todIdParameter);
		return affectedRow == 1;
	}
}
