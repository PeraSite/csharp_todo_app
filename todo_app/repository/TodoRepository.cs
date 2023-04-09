using todo_app.database;
using todo_app.entity;

namespace todo_app.repository;

public class TodoRepository {
	private readonly MySqlDatabase _database;

	public TodoRepository(MySqlDatabase database) {
		_database = database;
	}

	public void AddTodoItem(string username, string s) {
		throw new NotImplementedException();
	}
	public IEnumerable<Todo> GetTodoItems(string username) {
		throw new NotImplementedException();
	}
	public void SetTodoItemDone(string username, uint parse) {
		throw new NotImplementedException();
	}
	public void DeleteTodoItem(string username, uint parse) {
		throw new NotImplementedException();
	}
}
