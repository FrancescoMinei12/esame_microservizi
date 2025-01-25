-- Creazione del database Pagamenti
CREATE DATABASE Pagamenti;

-- Uso del database Pagamenti
USE Pagamenti;

-- Creazione della tabella MetodiPagamento
CREATE TABLE MetodiPagamento (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Identificativo univoco del metodo di pagamento
    Nome NVARCHAR(50) NOT NULL        -- Nome del metodo di pagamento (es. Carta di Credito, PayPal, Bonifico)
);

-- Creazione della tabella Pagamenti
CREATE TABLE Pagamenti (
    Id INT PRIMARY KEY IDENTITY(1,1),         -- Identificativo univoco del pagamento
    Importo DECIMAL(10,2) NOT NULL,           -- Totale pagato
    DataPagamento DATETIME NOT NULL,          -- Data e ora del pagamento
	Fk_Ordine INT NOT NULL,                   -- ID dell'ordine associato
    Fk_MetodoPagamento INT NOT NULL,          -- Metodo di pagamento utilizzato
    CONSTRAINT FK_MetodoPagamento FOREIGN KEY (Fk_MetodoPagamento) REFERENCES MetodiPagamento(Id)
);

-- Inserimento di metodi di pagamento
INSERT INTO MetodiPagamento (Nome) VALUES ('Carta di Credito');
INSERT INTO MetodiPagamento (Nome) VALUES ('PayPal');
INSERT INTO MetodiPagamento (Nome) VALUES ('Bonifico Bancario');
INSERT INTO MetodiPagamento (Nome) VALUES ('Google Pay');
INSERT INTO MetodiPagamento (Nome) VALUES ('Apple Pay');

-- Inserimento di pagamenti
INSERT INTO Pagamenti (Importo, DataPagamento, Fk_Ordine, Fk_MetodoPagamento) VALUES (100.50, GETDATE(), 1, 1);
INSERT INTO Pagamenti (Importo, DataPagamento, Fk_Ordine, Fk_MetodoPagamento) VALUES (250.75, GETDATE(), 2, 2);
INSERT INTO Pagamenti (Importo, DataPagamento, Fk_Ordine, Fk_MetodoPagamento) VALUES (500.00, GETDATE(), 3, 3);
INSERT INTO Pagamenti (Importo, DataPagamento, Fk_Ordine, Fk_MetodoPagamento) VALUES (75.00, GETDATE(), 4, 4);
INSERT INTO Pagamenti (Importo, DataPagamento, Fk_Ordine, Fk_MetodoPagamento) VALUES (300.00, GETDATE(), 5, 5);
