using System;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using System.IO;

namespace VerimlilikUygulamasi
{
    internal static class Program
    {
        private static readonly string DbFile = "verimlilik.db";
        private static readonly string ConnectionString = $"Data Source={DbFile}";

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }

        static void InitializeDatabase()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Gmail TEXT UNIQUE NOT NULL,
                    KullAdi TEXT NOT NULL,
                    Sifre TEXT NOT NULL,
                    Kilogram REAL,
                    BoyCm REAL,
                    HareketDurumu TEXT
                );
                CREATE TABLE IF NOT EXISTS BeslenmeVerileri (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    Tarih TEXT,
                    Kalori REAL,
                    Protein REAL,
                    Karbonhidrat REAL,
                    Yag REAL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );
                CREATE TABLE IF NOT EXISTS UykuVerileri (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    Tarih TEXT,
                    BaslamaSaati TEXT,
                    BitisSaati TEXT,
                    UykudaKesilme INTEGER,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );
                CREATE TABLE IF NOT EXISTS SuVerileri (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    Tarih TEXT,
                    SuLitresi REAL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );
                CREATE TABLE IF NOT EXISTS ElektrikVerileri (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    Tarih TEXT,
                    ElektrikKWh REAL,
                    KisiSayisi INTEGER,
                    CihazSayisi INTEGER,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );
                CREATE TABLE IF NOT EXISTS TelefonKullanimi (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    Tarih TEXT,
                    IsSaatleri REAL,
                    SosyalMedyaSaat REAL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );
                CREATE TABLE IF NOT EXISTS SporVerileri (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UserId INTEGER,
                    Tarih TEXT,
                    KasGrubu TEXT,
                    Hareketler TEXT,
                    AntrenmanSuresi REAL,
                    FOREIGN KEY(UserId) REFERENCES Users(UserId)
                );
            ";
            command.ExecuteNonQuery();

            transaction.Commit();
        }
    }
}
