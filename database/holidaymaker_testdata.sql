-- Holiday Maker – testdata
-- Kör efter att tabellerna har skapats. Kör bara en gång (annars blir det dubbletter).

USE holidaymaker;

-- 5 boenden på 4 orter, alla är platser från filmerna om Gudfadern
INSERT INTO boende (namn, ort, pool, restaurang, avstand_strand, avstand_centrum, omdome) VALUES
('Villa Sant''Andrea',      'Taormina', 1, 1,  100, 1500, 5),
('Vitelli Guesthouse',      'Savoca',   0, 0, 3000,   50, 3),
('Castello degli Schiavi',  'Catania',  1, 0, 1500, 4000, 4),
('Villa Malfitano',         'Palermo',  0, 1, 1200,  100, 4),
('Teatro Massimo Hotel',    'Palermo',  0, 0,  300,  900, 2);

-- 20 rum, kapacitet 1, 2 eller 4 personer
INSERT INTO rum (boende_id, rumsnummer, max_personer, pris_per_natt) VALUES
(1, '101', 2, 1450.00),
(1, '102', 2, 1450.00),
(1, '201', 4, 2400.00),
(1, '202', 1,  950.00),
(2, '1',   1,  550.00),
(2, '2',   2,  750.00),
(2, '3',   2,  750.00),
(2, '4',   4, 1200.00),
(3, 'A1',  2, 1100.00),
(3, 'A2',  2, 1250.00),
(3, 'B1',  4, 1900.00),
(3, 'B2',  1,  800.00),
(4, '11',  1,  700.00),
(4, '12',  2, 1050.00),
(4, '21',  4, 1600.00),
(4, '22',  2, 1050.00),
(5, '1',   2,  650.00),
(5, '2',   2,  650.00),
(5, '3',   4,  990.00),
(5, '4',   1,  450.00);

-- 5 fiktiva kunder
INSERT INTO kund (namn, telefon, epost) VALUES
('Marco Rossi',   '+39 320 111 2233', 'marco.rossi@esempio.it'),
('Anna Bruno',    '+39 331 222 3344', 'anna.bruno@esempio.it'),
('Sofia Ricci',   '+39 347 333 4455', 'sofia.ricci@esempio.it'),
('Luca Conti',    '+39 366 444 5566', NULL),
('Paolo Marino',  '+39 380 555 6677', 'paolo.marino@esempio.it');

-- 25 bokningar i december 2026
-- Rum 1 (Villa Sant'Andrea 101) är bokat 10–13 dec: bra för att testa
-- "boka samma dag som någon checkar ut" och överlappande bokningar.
-- Palermos 4-personersrum (rum 15 och 19) är båda bokade 23–27 dec:
-- en sökning i Palermo 24–26 dec för 4 personer ger inget resultat.
INSERT INTO bokning (kund_id, rum_id, incheckning, utcheckning, antal_personer) VALUES
(1,  1, '2026-12-10', '2026-12-13', 2),
(2,  1, '2026-12-20', '2026-12-24', 2),
(3,  2, '2026-12-05', '2026-12-08', 1),
(4,  2, '2026-12-18', '2026-12-22', 2),
(5,  3, '2026-12-01', '2026-12-07', 4),
(1,  3, '2026-12-27', '2026-12-31', 3),
(2,  4, '2026-12-12', '2026-12-14', 1),
(3,  5, '2026-12-03', '2026-12-05', 1),
(4,  6, '2026-12-10', '2026-12-17', 2),
(5,  7, '2026-12-22', '2026-12-26', 2),
(1,  8, '2026-12-15', '2026-12-19', 4),
(2,  9, '2026-12-01', '2026-12-04', 2),
(3, 10, '2026-12-24', '2026-12-28', 2),
(4, 11, '2026-12-08', '2026-12-12', 3),
(5, 12, '2026-12-19', '2026-12-21', 1),
(1, 13, '2026-12-06', '2026-12-09', 1),
(2, 14, '2026-12-11', '2026-12-16', 2),
(3, 15, '2026-12-23', '2026-12-27', 4),
(4, 16, '2026-12-02', '2026-12-05', 2),
(5, 17, '2026-12-14', '2026-12-18', 2),
(1, 18, '2026-12-28', '2026-12-31', 1),
(2, 19, '2026-12-23', '2026-12-27', 3),
(3, 20, '2026-12-09', '2026-12-10', 1),
(4,  1, '2026-12-26', '2026-12-30', 1),
(5, 11, '2026-12-20', '2026-12-23', 4);
