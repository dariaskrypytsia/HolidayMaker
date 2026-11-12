-- Holiday Maker – SQL-frågor som jag har testat i DBeaver innan jag använde dem i C#

USE holidaymaker;

-- Sök lediga rum
-- Ett rum är ledigt om det INTE finns någon bokning som överlappar perioden.
-- Två perioder överlappar om: gammal incheckning < ny utcheckning
--                         OCH gammal utcheckning > ny incheckning
-- Utcheckningsdagen räknas inte som upptagen (därför > och inte >=).

-- Test A: Taormina 13–15 dec, 2 personer
-- Väntat: 101, 102, 201 (101 är bokat 10–13, men utcheckning 13 dec stör inte)
SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt
FROM rum r
JOIN boende bo ON bo.id = r.boende_id
WHERE bo.ort = 'Taormina'
  AND r.max_personer >= 2
  AND NOT EXISTS (
      SELECT 1
      FROM bokning bk
      WHERE bk.rum_id = r.id
        AND bk.incheckning < '2026-12-15'
        AND bk.utcheckning > '2026-12-13'
  )
ORDER BY r.pris_per_natt;

-- Test B: Taormina 11–12 dec, 2 personer (bokningen ligger inuti en annan)
-- Väntat: 102, 201 (inte 101)
SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt
FROM rum r
JOIN boende bo ON bo.id = r.boende_id
WHERE bo.ort = 'Taormina'
  AND r.max_personer >= 2
  AND NOT EXISTS (
      SELECT 1
      FROM bokning bk
      WHERE bk.rum_id = r.id
        AND bk.incheckning < '2026-12-12'
        AND bk.utcheckning > '2026-12-11'
  )
ORDER BY r.pris_per_natt;

-- Test C: Taormina 9–14 dec, 2 personer (perioden omsluter en bokning)
-- Väntat: 102, 201 (inte 101)
SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt
FROM rum r
JOIN boende bo ON bo.id = r.boende_id
WHERE bo.ort = 'Taormina'
  AND r.max_personer >= 2
  AND NOT EXISTS (
      SELECT 1
      FROM bokning bk
      WHERE bk.rum_id = r.id
        AND bk.incheckning < '2026-12-14'
        AND bk.utcheckning > '2026-12-09'
  )
ORDER BY r.pris_per_natt;

-- Test D: Palermo 24–26 dec, 4 personer
-- Väntat: inget resultat (båda 4-personersrummen i Palermo är bokade 23–27)
SELECT r.id, bo.namn AS boende, bo.ort, r.rumsnummer, r.max_personer, r.pris_per_natt
FROM rum r
JOIN boende bo ON bo.id = r.boende_id
WHERE bo.ort = 'Palermo'
  AND r.max_personer >= 4
  AND NOT EXISTS (
      SELECT 1
      FROM bokning bk
      WHERE bk.rum_id = r.id
        AND bk.incheckning < '2026-12-26'
        AND bk.utcheckning > '2026-12-24'
  )
ORDER BY r.pris_per_natt;
