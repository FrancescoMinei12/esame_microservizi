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
