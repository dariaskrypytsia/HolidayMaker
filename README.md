# Holiday Maker

Ett konsolprogram i C# för en resebyrå. En operatör använder programmet när kunder ringer.
Operatören kan:

- registrera och välja kund
- söka lediga rum (ort, datum, antal personer) med filter och sortering
- boka ett rum direkt efter sökningen
- visa kundens bokningar
- ändra antal personer i en bokning
- avboka

Data sparas i en MySQL-databas.

## Det här behövs

- .NET 10
- MySQL 8 (jag kör MySQL i Docker)
- En databasklient, till exempel DBeaver

## Skapa databasen

1. Starta MySQL (i Docker: starta containern).
2. Anslut till MySQL i DBeaver.
3. Kör `database/holidaymaker_schema.sql`. Den skapar databasen och tabellerna.
4. Kör `database/holidaymaker_testdata.sql`. Den lägger in testdata.

Kör skripten bara en gång.

## Kör programmet

1. Skriv ditt användarnamn och lösenord till MySQL i `connectionString` i `Program.cs`.
2. Öppna en terminal i projektmappen.
3. Skriv `dotnet run`.

## Filer

- `database/holidaymaker_schema.sql` – skapar databasen och tabellerna
- `database/holidaymaker_testdata.sql` – testdata
- `database/fragor.sql` – SQL-frågor som jag testade i DBeaver
- `database/er-diagram.png` – E/R-diagram
- `Program.cs` – anslutning och huvudmeny
- `*Repository.cs` – all SQL, pratar med databasen
- `*Menu.cs` – pratar med användaren
- `TESTER.md` – hur jag har testat programmet
