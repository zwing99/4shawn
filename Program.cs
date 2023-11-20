using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;


string runCommand(SqliteConnection connection, string sql, Dictionary<string, object> addtionalParams = null) {
    var command = connection.CreateCommand();
    command.CommandText = sql;
    if (addtionalParams != null) {
        foreach(var item in addtionalParams) {
            command.Parameters.AddWithValue(item.Key, item.Value);
        }
    }

    var sb = new StringBuilder();
    using (var reader = command.ExecuteReader()) {
        while (reader.Read()) {
            for (int i=0; i < reader.FieldCount; i++) {
                sb.AppendFormat(@"{0}, ", reader.GetString(i));
            }
            sb.Length -= 2;
            sb.AppendLine();
        }
    } 

    return sb.ToString();
}

using (var connection = new SqliteConnection("Data Source=hello.db")) {
    connection.Open();
    runCommand(
        connection,
        @"
        CREATE TABLE IF NOT EXISTS contacts (
            contact_id INTEGER PRIMARY KEY,
            first_name TEXT NOT NULL
        );
        "
    );

    Console.WriteLine("What name?");
    var newName = Console.ReadLine();
    if (newName != null && newName.Length > 0) {
        var newParams = new Dictionary<string, object>
        {
            { "name", newName }
        };

        runCommand(
            connection,
            @"
            INSERT INTO contacts (first_name) values (:name);
            ",
            newParams
        );
    }

    var foo = runCommand(
        connection,
        @"
        select contact_id, first_name from contacts;
        "
    );

    Console.WriteLine(foo);
}
