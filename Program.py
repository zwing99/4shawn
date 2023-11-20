import sqlalchemy
from sqlalchemy import text as satext

engine = sqlalchemy.create_engine(r"sqlite+pysqlite:///hello.db")

with engine.connect() as conn:
    conn.execute(satext("""
        CREATE TABLE IF NOT EXISTS contacts (
            contact_id INTEGER PRIMARY KEY AUTOINCREMENT,
            first_name TEXT NOT NULL
        );""")
    );

    new_name = input("What name? ")
    if new_name:
        conn.execute(satext("INSERT INTO contacts (first_name) values (:name);"), 
            dict(name=new_name),
        )
        conn.commit()

    rows = conn.execute(satext("select contact_id, first_name from contacts;"))
    for row in rows:
        print(row)

