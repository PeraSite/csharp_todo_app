namespace todo_app.entity;

public record Todo(
	int ID,
	int UserID,
	string Content,
	bool Completed
);
