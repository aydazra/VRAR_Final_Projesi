using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Data;            // SQL için gerekli
using System.Data.SqlClient;  // SQL Server komutlarý için gerekli

public class LoginManager : MonoBehaviour
{
    [Header("Öðretmen Alanlarý")]
    public TMP_InputField ogretmenAd;
    public TMP_InputField ogretmenSoyad;
    public TMP_InputField ogretmenSifre;

    [Header("Öðrenci Alanlarý")]
    public TMP_InputField ogrenciAd;
    public TMP_InputField ogrenciSoyad;
    public TMP_InputField ogrenciNo;

    // Baðlantý Adresi (Localhost: Senin bilgisayarýn)
    // "Server=" yerine "Data Source=" ve "Database=" yerine "Initial Catalog=" yazdýk.
    // Unity'nin en sevdiði format budur.
    string connectionString =
 @"Data Source=DESKTOP-9TDC980\SQLKODLAB;
Initial Catalog=VRAR_DB;
Integrated Security=True;
Encrypt=True;
TrustServerCertificate=True;";
    // ÖÐRETMEN GÝRÝÞ BUTONU
    public void OgretmenGiris()
    {
        string gelenAd = ogretmenAd.text.Trim();
        string gelenSifre = ogretmenSifre.text.Trim();

        // Boþ mu diye bak
        if (string.IsNullOrEmpty(gelenAd) || string.IsNullOrEmpty(gelenSifre))
        {
            Debug.LogError("Lütfen Ad ve Þifre giriniz!");
            return;
        }

        // Veritabanýna Sor
        CheckDatabase(gelenAd, "", gelenSifre, true);
    }

    // ÖÐRENCÝ GÝRÝÞ BUTONU
    public void OgrenciGiris()
    {
        string gelenAd = ogrenciAd.text.Trim();
        string gelenNo = ogrenciNo.text.Trim();

        if (string.IsNullOrEmpty(gelenAd) || string.IsNullOrEmpty(gelenNo))
        {
            Debug.LogError("Lütfen Ad ve Okul No giriniz!");
            return;
        }

        // Veritabanýna Sor
        CheckDatabase(gelenAd, gelenNo, "", false);
    }

    // ORTAK KONTROL FONKSÝYONU
    void CheckDatabase(string ad, string numara, string sifre, bool ogretmenMi)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open(); // Kapýyý çal
                string sql = "";

                if (ogretmenMi)
                {
                    // Öðretmen ise Ad ve Þifre kontrol et
                    sql = "SELECT COUNT(*) FROM Tbl_Ogretmen WHERE Ad='" + ad + "' AND Sifre='" + sifre + "'";
                }
                else
                {
                    // Öðrenci ise Ad ve OkulNo kontrol et
                    sql = "SELECT COUNT(*) FROM Tbl_Ogrenci WHERE Ad='" + ad + "' AND OkulNo='" + numara + "'";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                int sonuc = (int)cmd.ExecuteScalar(); // Kaç kiþi buldun?

                if (sonuc > 0)
                {
                    Debug.Log("GÝRÝÞ BAÞARILI! Hoþgeldin: " + ad);
                    SceneManager.LoadScene("MainMenuScene");
                }
                else
                {
                    Debug.LogError("Kullanýcý Bulunamadý! Bilgileri kontrol et.");
                }
            }
        }
        catch (System.Exception hata)
        {
            Debug.LogError("Veritabaný Baðlantý Hatasý: " + hata.Message);
        }
    }
}