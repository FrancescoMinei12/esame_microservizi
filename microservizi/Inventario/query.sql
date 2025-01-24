-- Creazione del database se non esiste
CREATE DATABASE Inventario;

-- Selezione del database
USE Inventario;

-- Creazione della tabella Fornitori
CREATE TABLE Fornitori (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Chiave primaria
    Nome VARCHAR(255) NOT NULL, -- Nome del Fornitore
    Indirizzo VARCHAR(255), -- Indirizzo del Fornitore
    Telefono VARCHAR(15), -- Numero di telefono
    Email VARCHAR(100) NOT NULL -- Email del Fornitore
);

-- Creazione della tabella Articoli con chiave esterna per Fornitori
CREATE TABLE Articoli (
    Id INT PRIMARY KEY IDENTITY(1,1), -- Chiave primaria con incremento automatico
    Nome VARCHAR(255) NOT NULL, -- Nome dell'Articolo
    Descrizione TEXT, -- Descrizione dell'Articolo
    Prezzo DECIMAL(10, 2) NOT NULL, -- Prezzo con precisione di 2 decimali
    QuantitaDisponibile INT NOT NULL, -- Quantità disponibile
    CodiceSKU VARCHAR(50) NOT NULL UNIQUE, -- Codice SKU univoco
    Categoria VARCHAR(100), -- Categoria dell'Articolo
    DataInserimento DATETIME2 NOT NULL DEFAULT GETDATE(), -- Data di inserimento con valore predefinito corrente
    Fk_fornitore INT NOT NULL, -- Chiave esterna per Fornitori
    CONSTRAINT FK_Fornitore FOREIGN KEY (Fk_fornitore) REFERENCES Fornitori(Id) -- Definizione della chiave esterna
);

-- Inserimento di 5 fornitori nella tabella Fornitori
INSERT INTO Fornitori (Nome, Indirizzo, Telefono, Email)
VALUES
('Tech Solutions Srl', 'Via Roma 10, Milano', '0255551234', 'info@techsolutions.com'),
('Distribuzione Elettronica Spa', 'Corso Italia 22, Torino', '0113334455', 'contatti@distr.elettronica.it'),
('Accessori Innovativi Srl', 'Via Veneto 5, Roma', '0687654321', 'vendite@accessorinnov.it'),
('Home & Office Supplies', 'Via Napoli 14, Firenze', '0551234567', 'supporto@homeandoffice.it'),
('Outdoor Tech Group', 'Piazza Duomo 3, Napoli', '0819876543', 'sales@outdoortech.it');

-- Inserimento di articoli nella tabella Articoli
INSERT INTO Articoli (Nome, Descrizione, Prezzo, QuantitaDisponibile, CodiceSKU, Categoria, fk_fornitore)
VALUES
('Notebook Pro X15', 'Notebook ad alte prestazioni con display 15 pollici.', 1299.99, 25, 'NBX15-001', 'Elettronica', 1),
('Smartphone Edge 5G', 'Smartphone 5G con fotocamera avanzata.', 899.50, 100, 'SE5G-002', 'Telefonia', 2),
('Cuffie Over-Ear', 'Cuffie con cancellazione attiva del rumore.', 199.99, 50, 'COE-003', 'Audio', 3),
('Monitor UHD 27"', 'Monitor 4K UHD con supporto HDR.', 349.99, 30, 'MUHD27-004', 'Periferiche', 1),
('Sedia Ergonomica', 'Sedia da ufficio con supporto lombare.', 249.90, 40, 'SEDERG-005', 'Arredamento', 4),
('Tastiera Meccanica', 'Tastiera RGB con switch meccanici.', 89.99, 75, 'TMECH-006', 'Periferiche', 2),
('Mouse Wireless', 'Mouse ergonomico con alta precisione.', 49.99, 120, 'MOUSE-007', 'Periferiche', 5),
('Smartwatch Sport', 'Smartwatch impermeabile con GPS integrato.', 179.50, 60, 'SWS-008', 'Telefonia', 1),
('Tablet Plus 10"', 'Tablet con 64GB di memoria espandibile.', 299.00, 45, 'TABP10-009', 'Elettronica', 3),
('Zaino Tech', 'Zaino per laptop resistente all acqua.', 59.90, 80, 'ZTECH-010', 'Accessori', 4),
('Caricatore Wireless', 'Caricatore rapido compatibile con Qi.', 39.99, 100, 'CARIQ-011', 'Accessori', 2),
('Action Camera 4K', 'Camera resistente all acqua con 4K.', 249.00, 30, 'ACTC-012', 'Fotografia', 5),
('Lampada LED', 'Lampada da scrivania con luminosità regolabile.', 39.50, 90, 'LEDL-013', 'Arredamento', 1),
('Auricolari True Wireless', 'Auricolari compatti con custodia ricaricabile.', 79.99, 150, 'ATWS-014', 'Audio', 2),
('Hard Disk Esterno 1TB', 'Disco rigido USB 3.0.', 89.50, 70, 'HDE1TB-015', 'Periferiche', 3),
('Router Wi-Fi 6', 'Router con tecnologia Wi-Fi 6 avanzata.', 139.00, 35, 'RWF6-016', 'Elettronica', 4),
('Stampante Laser', 'Stampante laser monocromatica.', 199.00, 25, 'STL-017', 'Periferiche', 5),
('Webcam HD', 'Webcam 1080p con microfono integrato.', 59.99, 110, 'WHD-018', 'Periferiche', 1),
('Cornice Digitale', 'Cornice digitale per foto con display HD.', 119.00, 40, 'CORND-019', 'Accessori', 2),
('Drone Mini', 'Drone con videocamera HD e controllo remoto.', 149.50, 20, 'DRMINI-020', 'Elettronica', 3),
('Set di Cacciaviti', 'Set con impugnatura ergonomica.', 29.99, 200, 'SCACC-021', 'Fai-da-te', 4),
('Telecamera di Sicurezza', 'Telecamera con rilevamento movimento.', 99.50, 65, 'TSEC-022', 'Elettronica', 5),
('Altoparlante Bluetooth', 'Altoparlante portatile con suono potente.', 69.99, 75, 'ALBT-023', 'Audio', 1),
('Batteria Portatile', 'Power bank da 10.000 mAh.', 39.99, 150, 'BATT-024', 'Accessori', 2),
('Giacca Termica', 'Giacca riscaldante per esterni.', 249.00, 30, 'GIAT-025', 'Abbigliamento', 3),
('Occhiali VR', 'Occhiali per realtà virtuale con controller.', 399.00, 15, 'OVR-026', 'Elettronica', 4),
('Cintura Fitness', 'Cintura per il monitoraggio della postura.', 49.99, 80, 'CFIT-027', 'Salute', 5),
('Microfono USB', 'Microfono cardioide per streaming.', 129.50, 40, 'MICU-028', 'Audio', 1),
('Tappetino Mouse XXL', 'Tappetino per mouse antiscivolo.', 19.99, 100, 'TMOXXL-029', 'Periferiche', 2),
('Forno a Microonde', 'Forno con funzione grill e timer.', 149.90, 20, 'FMICRO-030', 'Elettrodomestici', 3);
