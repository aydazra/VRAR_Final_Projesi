-- TABLOLAR VARSA ÖNCE SÝL (HATA VERMESÝN)
IF OBJECT_ID('dbo.Tbl_Ogrenci', 'U') IS NOT NULL DROP TABLE dbo.Tbl_Ogrenci;
IF OBJECT_ID('dbo.Tbl_Ogretmen', 'U') IS NOT NULL DROP TABLE dbo.Tbl_Ogretmen;

-- 1. ÖÐRETMEN TABLOSU
CREATE TABLE Tbl_Ogretmen (
    OgretmenID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Soyad NVARCHAR(50),
    Sifre NVARCHAR(20)
);

-- 2. ÖÐRENCÝ TABLOSU
CREATE TABLE Tbl_Ogrenci (
    OgrenciID INT IDENTITY(1,1) PRIMARY KEY,
    Ad NVARCHAR(50),
    Soyad NVARCHAR(50),
    OkulNo NVARCHAR(20)
);

-- 3. TEST ÝÇÝN VERÝ EKLEME
-- Öðretmen: Admin / 1234
INSERT INTO Tbl_Ogretmen (Ad, Soyad, Sifre) VALUES ('Admin', 'Teacher', '1234');
INSERT INTO Tbl_Ogretmen (Ad, Soyad, Sifre) VALUES ('Nisa', 'Kopuz', '1234');
INSERT INTO Tbl_Ogretmen (Ad, Soyad, Sifre) VALUES ('Hasret', 'Ýþler', '1234');
-- Öðrenci: Ali / 101
INSERT INTO Tbl_Ogrenci (Ad, Soyad, OkulNo) VALUES ('Ali', 'Yilmaz', '101');
INSERT INTO Tbl_Ogrenci (Ad, Soyad, OkulNo) VALUES ('Beyzanur', 'Toysöz', '207');
INSERT INTO Tbl_Ogrenci (Ad, Soyad, OkulNo) VALUES ('Defne', 'Gödelekoðlu', '105');

-- SONUCU GÖSTER
SELECT * FROM Tbl_Ogretmen;
SELECT * FROM Tbl_Ogrenci;