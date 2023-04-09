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
		User user;
		if (userRepository.IsExistUser(username)) {
			user = userRepository.GetUser(username) ?? throw new Exception("계정 정보를 가져오는데 실패했습니다.");
			Console.WriteLine($"반갑습니다, {username}님!");
		} else {
			var result = userRepository.AddUser(username);
			if (!result) {
				Console.WriteLine("계정 생성에 실패했습니다.");
				Environment.Exit(-1);
				return;
			}
			Console.WriteLine($"처음 뵙겠습니다, {username}님!");
			user = userRepository.GetUser(username) ?? throw new Exception("계정 정보를 가져오는데 실패했습니다.");
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
				case "help": {
					PrintHelp();
					break;
				}
				case "add": {
					var content = string.Join(" ", args.Skip(1));
					var result = todoRepository.AddTodo(user, content);
					Console.WriteLine(result ? "할 일을 추가했습니다." : "할 일을 추가하는데 실패했습니다.");
					break;
				}
				case "list": {
					var items = todoRepository.GetAllTodo(user);
					if (items.Count > 0) {
						Console.WriteLine("할 일 목록:");
						foreach (Todo item in items) {
							Console.WriteLine($" - [{item.ID}] {item.Content}: {(item.Completed ? "O" : "X")}");
						}
					} else {
						Console.WriteLine("할 일이 없습니다.");
					}

					break;
				}
				case "done": {
					var result = todoRepository.SetTodoDone(uint.Parse(args[1]));
					Console.WriteLine(result ? "할 일을 완료했습니다." : "할 일을 완료하는데 실패했습니다.");
					break;
				}
				case "delete": {
					var result = todoRepository.DeleteTodo(uint.Parse(args[1]));
					Console.WriteLine(result ? "할 일을 삭제했습니다." : "할 일을 삭제하는데 실패했습니다.");
					break;
				}
				case "exit":
					return;
			}
		}
	}

	private static string GetEnvironmentVariable(string key)
		=> Environment.GetEnvironmentVariable(key) ?? throw new Exception($"{key} is not set");
}
