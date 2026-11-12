# Holiday Maker

Ett enkelt konsolprogram i C# för en resebyrå. Operatören kan registrera kunder, söka lediga rum, boka, visa bokningar, ändra antal personer och avboka.

Data sparas i en MySQL-databas.

## Det här behövs

- .NET 10
- MySQL 8 (jag kör MySQL i Docker)
- En databasklient, till exempel DBeaver

## Skapa databasen

1. Starta MySQL.
2. Öppna DBeaver och anslut till MySQL.
3. Kör `database/holidaymaker_schema.sql`. Den skapar databasen och tabellerna.
4. Kör `database/holidaymaker_testdata.sql`. Den lägger in testdata.

Kör skripten bara en gång.

## Kör programmet

1. Skriv ditt lösenord till MySQL i `Program.cs`.
2. Öppna en terminal i projektmappen.
3. Skriv `dotnet run`.
