# Tester

## 1. Ett ledigt rum visas och ett upptaget rum filtreras bort
Jag sökte i Taormina 11–12 december för 2 personer.
Rum 101 syns inte, eftersom det är bokat 10–13 december. Rum 102 och 201 syns.

## 2. Ett rum kan bokas samma dag som en tidigare kund checkar ut
Jag sökte i Taormina 13–15 december. Rum 101 syns nu, fast en bokning slutar den 13:e.
Jag bokade rummet och fick bokningsnummer och totalpris.

## 3. Överlappande bokningar nekas, även när en period omsluter en annan
Jag sökte i Taormina 9–14 december. Den perioden omsluter bokningen 10–13 december.
Rum 101 syns inte. Programmet kollar också en gång till i databasen innan bokningen sparas.

## 4. Ett för stort sällskap kan inte boka rummet
Jag sökte i Palermo 24–26 december för 4 personer. Inga rum hittades,
eftersom rummen för 4 är bokade och de andra är för små.
Rum 202 i Taormina (för 1 person) syns aldrig när man söker för 2.

## 5. Kombinerade sökfilter och båda sorteringarna fungerar
Jag sökte i Palermo 1–3 december för 2 personer.
Med filtret restaurang syns bara Villa Malfitano. Sortering på pris visar billigast först,
och sortering på omdöme visar högst omdöme först.

## 6. Ändring av antal personer följer rummets kapacitet
Jag försökte ändra en bokning i rum 101 (max 2 personer) till 3 personer.
Programmet sa att det inte får plats.

## 7. Avbokning frigör rummet
Jag avbokade bokningen och såg i DBeaver att den togs bort. Rummet går att boka igen.

## 8. Uppgifterna finns kvar efter omstart
Jag gjorde en bokning, stängde programmet och startade det igen. Bokningen fanns kvar.

## Felaktig inmatning
Bokstäver i menyn, fel datum, utcheckning före incheckning och bokstäver i antal personer:
programmet säger till och kraschar inte.

## Fel jag hittade
Om man skriver fel rumsnummer när man bokar kommer man tillbaka till menyn
och måste söka igen. Programmet kraschar inte, men det är inte så smidigt.
