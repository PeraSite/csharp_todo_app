using todo_app.database;
using todo_app.entity;
using todo_app.repository;

namespace todo_app;

internal static class Program {
	private static void Main() {
		// 환경변수 가져오기
		var server = GetEnvironmentVariable("MYSQL_SERVER");
		var port = uint.Parse(GetEnvironmentVariable("MYSQL_PORT"));
		var databaseName = GetEnvironmentVariable("MYSQL_DATABASE");
		var sqlUser = GetEnvironmentVariable("MYSQL_USER");
		var password = GetEnvironmentVariable("MYSQL_PASSWORD");

		// 필수 로직 수행 오브젝트 생성
		using MySqlDatabase database = new MySqlDatabase(server, port, databaseName, sqlUser, password);
		TodoRepository todoRepository = new TodoRepository(database);
		UserRepository userRepository = new UserRepository(database);

		// 이름 입력받기
		Console.Write("이름을 입력하세요: ");
		var username = Console.ReadLine();
		if (username is null) {
			Console.WriteLine("이름을 입력하지 않았습니다.");
			return;
		}

		// 계정 정보 확인
		if (userRepository.IsExistUser(username)) {
			Console.WriteLine($"반갑습니다, {username}님!");
		} else {
			var result = userRepository.AddUser(username);
			if (!result) {
				Console.WriteLine("계정 생성에 실패했습니다.");
				Environment.Exit(-1);
				return;
			}
			Console.WriteLine($"처음 뵙겠습니다, {username}님!");
		}

		// 메인 루프 시작
		void PrintHelp() {
			Console.WriteLine("명령어 목록:");
			Console.WriteLine("- help: 명령어 목록을 확인합니다.");
			Console.WriteLine("- list : 할 일 목록을 확인합니다.");
			Console.WriteLine("- add [할 일] : 할 일을 추가합니다.");
			Console.WriteLine("- done [할 일 번호] : [할 일 번호]를 완료합니다.");
			Console.WriteLine("- delete [할 일 번호] : [할 일 번호]를 삭제합니다.");
			Console.WriteLine("- exit : 프로그램을 종료합니다.");
		}

		PrintHelp();
		while (true) {
			Console.Write("입력: ");
			var args = Console.ReadLine()?.Split(" ");
			if (args is null || args.Length == 0)
				break;
			switch (args[0]) {
				case "help":
					PrintHelp();
					break;
				case "add":
					todoRepository.AddTodoItem(username, args[1]);
					break;
				case "list":
					var items = todoRepository.GetTodoItems(username);
					foreach (Todo item in items) {
						Console.WriteLine($"{item.ID}: {item.Content}");
					}
					break;
				case "done":
					todoRepository.SetTodoItemDone(username, uint.Parse(args[1]));
					break;
				case "delete":
					todoRepository.DeleteTodoItem(username, uint.Parse(args[1]));
					break;
				case "exit":
					return;
			}
		}
	}

	private static string GetEnvironmentVariable(string key)
		=> Environment.GetEnvironmentVariable(key) ?? throw new Exception($"{key} is not set");
}
