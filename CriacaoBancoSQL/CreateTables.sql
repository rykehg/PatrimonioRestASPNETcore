CREATE DATABASE Empresa;

CREATE TABLE Marca (
    MarcaId INT IDENTITY(1,1) PRIMARY KEY,
    Nome CHAR(80) NOT NULL UNIQUE
);

CREATE TABLE Patrimonio (
    PatrimonioId INT IDENTITY(1,1) PRIMARY KEY,
    MarcaId INT NOT NULL,
    Nome CHAR(80) NOT NULL,
    Descricao TEXT,
    NumeroTombo INT
);

ALTER TABLE Patrimonio ADD CONSTRAINT fk_patrimonio_marca
    FOREIGN KEY(MarcaId) REFERENCES Marca (MarcaId)
    ON DELETE CASCADE;

INSERT INTO Marca (Nome) 
VALUES 
	('Marca1'),
	('Marca2');

INSERT INTO Patrimonio (MarcaId, Nome, Descricao, NumeroTombo)
VALUES
	(1, 'PatrimonioI', 'Descricao PatrimonioI', '1001'),
	(1, 'PatrimonioII', 'Descricao PatrimonioII', '1002'),
	(2, 'PatrimonioIII', 'Descricao PatrimonioIII', '1003'),
	(2, 'PatrimonioIV', 'Descricao PatrimonioIV', '1004');
