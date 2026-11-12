-- Holiday Maker – databasstruktur (MySQL 8.0.16+)
-- Kör hela skriptet en gång i DBeaver (Alt+X = Execute script).

CREATE DATABASE IF NOT EXISTS holidaymaker;
USE holidaymaker;

-- Kunder
CREATE TABLE kund (
    id       INT AUTO_INCREMENT PRIMARY KEY,
    namn     VARCHAR(100) NOT NULL,
    telefon  VARCHAR(20)  NOT NULL,
    epost    VARCHAR(100) UNIQUE
);

-- Boenden (hotell, pensionat)
CREATE TABLE boende (
    id               INT AUTO_INCREMENT PRIMARY KEY,
    namn             VARCHAR(100) NOT NULL,
    ort              VARCHAR(100) NOT NULL,
    pool             BOOLEAN      NOT NULL,
    restaurang       BOOLEAN      NOT NULL,
    avstand_strand   INT          NOT NULL CHECK (avstand_strand >= 0),
    avstand_centrum  INT          NOT NULL CHECK (avstand_centrum >= 0),
    omdome           INT          NOT NULL CHECK (omdome BETWEEN 1 AND 5)
);

-- Rum, varje rum hör till ett boende
CREATE TABLE rum (
    id             INT AUTO_INCREMENT PRIMARY KEY,
    boende_id      INT           NOT NULL,
    rumsnummer     VARCHAR(10)   NOT NULL,
    max_personer   INT           NOT NULL CHECK (max_personer > 0),
    pris_per_natt  DECIMAL(10,2) NOT NULL CHECK (pris_per_natt > 0),
    FOREIGN KEY (boende_id) REFERENCES boende(id),
    UNIQUE (boende_id, rumsnummer)
);

-- Bokningar, en kund och ett rum per bokning
CREATE TABLE bokning (
    id              INT AUTO_INCREMENT PRIMARY KEY,
    kund_id         INT  NOT NULL,
    rum_id          INT  NOT NULL,
    incheckning     DATE NOT NULL,
    utcheckning     DATE NOT NULL,
    antal_personer  INT  NOT NULL CHECK (antal_personer > 0),
    FOREIGN KEY (kund_id) REFERENCES kund(id),
    FOREIGN KEY (rum_id)  REFERENCES rum(id),
    CHECK (utcheckning > incheckning)
);
