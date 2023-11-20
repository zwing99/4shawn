using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using Dapper;
using Microsoft.Data.Sqlite;

struct Contact {
    public int contact_id { get; set; }
    public string first_name { get; set; }

    public override string ToString()
    {
        return string.Format("({0}, \"{1}\")", this.contact_id, this.first_name);
    }
};

partial class Program {
    static void Main(string[] args)
    {
        using (var connection = new SqliteConnection("Data Source=hello.db"))
        {
            connection.Open();
            connection.Execute(
                @"CREATE TABLE IF NOT EXISTS contacts (
                        contact_id INTEGER PRIMARY KEY AUTOINCREMENT,
                        first_name TEXT NOT NULL
                );"
            );

            Console.WriteLine("What name?");
            var newName = Console.ReadLine();
            if (newName != null && newName.Length > 0) {
                connection.Execute(
                    "INSERT INTO contacts (first_name) values (@name);",
                    new { name = newName}
                );
            }

            var contacts = connection.Query<Contact>("select contact_id, first_name from contacts;");
            foreach (var contact in contacts)
            {
                Console.WriteLine(contact);
            }
        }
    }
}
