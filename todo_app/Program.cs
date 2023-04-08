using todo_app.database;
using todo_app.entity;
using todo_app.repository;

namespace todo_app;

internal static class Program {
	private static void Main() {
		// 환경변수 가져오기
		var server = GetEnvironment("MYSQL_SERVER");
		var port = uint.Parse(GetEnvironment("MYSQL_PORT"));
		var databaseName = GetEnvironment("MYSQL_DATABASE");
		var sqlUser = GetEnvironment("MYSQL_USER");
		var password = GetEnvironment("MYSQL_PASSWORD");

		// 필수 로직 수행 오브젝트 생성
		MySqlDatabase database = new MySqlDatabase(server, port, databaseName, sqlUser, password);
		TodoRepository repository = new TodoRepository(database);

		foreach (User user in repository.GetAllUsers()) {
			Console.WriteLine(user);
		}
	}

	private static string GetEnvironment(string key) {
		var value = Environment.GetEnvironmentVariable(key);
		if (value == null) {
			throw new Exception($"{key} is not set");
		}
		return value;
	}
}
