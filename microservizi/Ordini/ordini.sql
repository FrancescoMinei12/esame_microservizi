-- Creazione del database OrdiniDB
CREATE DATABASE Ordini;

-- Selezione del database OrdiniDB
USE Ordini;

-- Creazione della tabella Clienti
CREATE TABLE Clienti (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Chiave primaria
    Nome VARCHAR(255) NOT NULL, -- Nome del Cliente
    Cognome VARCHAR(255) NOT NULL, -- Cognome del Cliente
    Email VARCHAR(100) NOT NULL UNIQUE, -- Email del Cliente
    Telefono VARCHAR(15), -- Numero di telefono
    Indirizzo VARCHAR(255) -- Indirizzo del Cliente
);

-- Creazione della tabella Ordini
CREATE TABLE Ordini (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Chiave primaria
    DataOrdine DATETIME2 NOT NULL DEFAULT GETDATE(), -- Data dell'ordine
    Totale DECIMAL(10, 2) NOT NULL, -- Totale dell'ordine
	Fk_cliente INT NOT NULL, -- Chiave esterna per Clienti
    CONSTRAINT FK_Cliente FOREIGN KEY (Fk_cliente) REFERENCES Clienti(Id) -- Definizione della chiave esterna
);

-- Creazione della tabella OrdiniProdotti per la relazione molti-a-molti
CREATE TABLE OrdiniProdotti (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Chiave primaria
    Quantita INT NOT NULL DEFAULT 1, -- Quantità del prodotto nell'ordine
	Fk_ordine INT NOT NULL, -- Chiave esterna per Ordini
    Fk_prodotto INT NOT NULL, -- Chiave esterna per Prodotti
    CONSTRAINT FK_Ordine FOREIGN KEY (Fk_ordine) REFERENCES Ordini(Id), -- Definizione della chiave esterna per Ordini
);

-- Inserimento dati nella tabella Clienti
INSERT INTO Clienti (Nome, Cognome, Email, Telefono, Indirizzo)
VALUES 
('Mario', 'Rossi', 'mario.rossi@example.com', '1234567890', 'Via Roma 10, Milano'),
('Luisa', 'Bianchi', 'luisa.bianchi@example.com', '0987654321', 'Via Napoli 20, Roma'),
('Giulia', 'Verdi', 'giulia.verdi@example.com', '1122334455', 'Via Firenze 15, Torino');

-- Inserimento dati nella tabella Ordini
INSERT INTO Ordini (DataOrdine, Totale, Fk_cliente)
VALUES 
('2025-01-20', 150.50, 1), -- Ordine di Mario Rossi
('2025-01-21', 200.00, 2), -- Ordine di Luisa Bianchi
('2025-01-22', 120.75, 3); -- Ordine di Giulia Verdi

-- Inserimento dati nella tabella OrdiniProdotti
INSERT INTO OrdiniProdotti (Quantita, Fk_ordine, Fk_prodotto)
VALUES 
(1, 1, 1), -- Mario Rossi ha acquistato un Laptop
(2, 1, 4), -- Mario Rossi ha acquistato 2 Cuffie
(1, 2, 2), -- Luisa Bianchi ha acquistato uno Smartphone
(3, 3, 5), -- Giulia Verdi ha acquistato 3 Mouse
(1, 3, 3); -- Giulia Verdi ha acquistato un Tablet
