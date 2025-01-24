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