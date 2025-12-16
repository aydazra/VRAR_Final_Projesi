// ==================== CelestialDataManager.cs ====================
// Bu script tüm gök cisimlerinin bilgilerini ve hikayelerini yönetir
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StorySection
{
    public string title;           // Bölüm başlığı
    public int pageNumber;         // Kitap sayfa numarası
    [TextArea(5, 15)]
    public string content;         // Hikaye metni
    public AudioClip audioClip;    // Sesli anlatım
    public float displayDuration;  // Ekranda kalma süresi (saniye)
    public Sprite illustrationImage; // İllüstrasyon görseli (opsiyonel)
}

[System.Serializable]
public class CelestialObjectData
{
    public string objectName;      // Nesne ID'si (Sun, Earth, Moon)
    public string displayName;     // Ekranda görünen isim
    public string characterVoice;  // Karakter sesi tipi (derin, yumuşak, neşeli)
    public Color themeColor;       // Tema rengi
    public Sprite characterIcon;   // Karakter avatarı

    [Header("Hikaye Bölümleri")]
    public List<StorySection> storySections = new List<StorySection>();

    [Header("Etkileşimli Özellikler")]
    public bool hasAnimation;      // Animasyon var mı?
    public string animationType;   // Animasyon tipi (rotation, orbit, glow)
    public bool hasQuiz;           // Quiz var mı?
    public List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
}

[System.Serializable]
public class QuizQuestion
{
    public string question;
    public string[] options;       // 4 şık
    public int correctAnswer;      // Doğru cevap indexi (0-3)
    public string explanation;     // Cevap açıklaması
}

public class CelestialDataManager : MonoBehaviour
{
    public static CelestialDataManager Instance { get; private set; }

    [Header("Gök Cisimleri Veritabanı")]
    public List<CelestialObjectData> celestialDatabase = new List<CelestialObjectData>();

    private Dictionary<string, CelestialObjectData> dataDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDatabase()
    {
        dataDict = new Dictionary<string, CelestialObjectData>();

        // GÜNEŞ VERİLERİ
        CreateSunData();

        // AY VERİLERİ
        CreateMoonData();

        // DÜNYA VERİLERİ
        CreateEarthData();

        // Dictionary'ye ekle
        foreach (var data in celestialDatabase)
        {
            dataDict[data.objectName] = data;
        }
    }

    void CreateSunData()
    {
        CelestialObjectData sun = new CelestialObjectData
        {
            objectName = "Sun",
            displayName = "Güneş",
            characterVoice = "derin_ve_güçlü",
            themeColor = new Color(1f, 0.75f, 0f), // Turuncu-sarı
            hasAnimation = true,
            animationType = "glow_and_rotate",
            hasQuiz = true
        };

        // BÖLÜM 1: Tanışma
        sun.storySections.Add(new StorySection
        {
            title = "🌟 Merhaba! Ben Güneş!",
            pageNumber = 20,
            displayDuration = 8f,
            content =
                "Merhaba küçük astronot! Ben Güneş! 🌞\n\n" +
                "Senin güzel gezegenin Dünya'yı her sabah selamlarım. Işıklarımla uyanırsın, " +
                "sıcaklığımla ısınırsın. Ben olmasam her yer buz gibi soğuk ve kapkaranlık olurdu!\n\n" +
                "Dünya'dan bana baktığında çok küçük görünürüm değil mi? Ama aslında... " +
                "İÇİME 1 MİLYON 300 BİN TANE DÜNYA SIĞAR! 😱\n\n" +
                "Hadi, beni daha yakından tanımak ister misin? O zaman ekrana dokunup devam et!"
        });

        // BÖLÜM 2: Güneşin Yapısı (Sayfa 21)
        sun.storySections.Add(new StorySection
        {
            title = "☀️ Benim Süper Güçlerim!",
            pageNumber = 21,
            displayDuration = 15f,
            content =
                "Şimdi sana süper güçlerimi anlatayım! 💪\n\n" +
                "🔥 SICAKLIĞIM:\n" +
                "Yüzeyim 5.500°C! Ama merkezimde tahmin et kaç derece? TAM 15 MİLYON DERECE! 🌡️\n" +
                "Bu sıcaklık o kadar fazla ki, demiri bile saniyeler içinde buharlaştırır!\n\n" +
                "📏 BOYUTUM:\n" +
                "Çapım 1.392.000 kilometre! Dünya'nın 109 katı büyüklüğündeyim.\n" +
                "Eğer Dünya bir portakal kadar olsaydı, ben bir basketbol sahası büyüklüğünde olurdum!\n\n" +
                "⚡ ENERJİM:\n" +
                "İçimde sürekli hidrojen bombası gibi patlamalar oluyor! " +
                "Hidrojen atomları birleşip helyuma dönüşüyor ve MUAZZAM enerji açığa çıkıyor.\n" +
                "Her saniye 4 milyon ton kütlemi enerjiye çeviriyorum! ⚛️\n\n" +
                "💡 Bir tek ben 1 saniyede ürettiğim enerjiyle dünyadaki tüm insanların " +
                "500.000 yıl boyunca kullanacağı elektriği üretebilirim!"
        });

        // BÖLÜM 3: Dönme Hareketi (Sayfa 21)
        sun.storySections.Add(new StorySection
        {
            title = "🌀 Ben de Dönüyorum!",
            pageNumber = 21,
            displayDuration = 10f,
            content =
                "Sana bir sır vereyim: Ben de senin gibi dönüyorum! 🎡\n\n" +
                "Ama senin Dünya'n gibi katı bir top değilim. Ben dev bir gaz topuyum! ☁️\n" +
                "Bu yüzden farklı yerlerim farklı hızlarda döner. Tuhaf değil mi?\n\n" +
                "🔄 EKVATORDA: Bir tur 25 günde\n" +
                "🔄 KUTUPLARDA: Bir tur 35 günde\n\n" +
                "Düşün ki, göbeğim daha hızlı koşuyor, tepem ve dipim ise yavaş yavaş takip ediyor! 😄\n\n" +
                "Şimdi beni ekranda çevir ve dönüşümü izle! Parmaklarınla beni döndürebilirsin. 👆"
        });

        // BÖLÜM 4: Güneş Sistemindeki Yerim
        sun.storySections.Add(new StorySection
        {
            title = "👑 Güneş Sisteminin Kralı",
            pageNumber = 22,
            displayDuration = 12f,
            content =
                "Ben Güneş Sisteminin kralıyım! 👑⭐\n\n" +
                "Etrafımda 8 gezegen dans ediyor:\n" +
                "🪐 Merkür (en hızlısı - yakınımda koşuyor!)\n" +
                "🌍 Venüs (en sıcak gezegen)\n" +
                "🌎 Dünya (senin evin! 🏠)\n" +
                "🔴 Mars (kırmızı gezegen)\n" +
                "🌪️ Jüpiter (dev gezegen - fırtınalı!)\n" +
                "💍 Satürn (halkası çok güzel!)\n" +
                "💎 Uranüs (yan yatan gezegen)\n" +
                "🌊 Neptün (en uzaktaki mavi dev)\n\n" +
                "Hepsini çekim gücümle tutuyorum! Eğer ben olmasam, " +
                "gezegenler uzaya savrulur giderdi! 🚀💨\n\n" +
                "Işığım ve ısım sayesinde Dünya'da bitkiler büyüyor, " +
                "hayvanlar yaşıyor, sen nefes alıyorsun! 🌱🦋\n\n" +
                "Yani aslında ben bir DEV YAŞAM MAKİNESİYİM! 💚"
        });

        // Quiz Soruları
        sun.quizQuestions.Add(new QuizQuestion
        {
            question = "Güneş'in merkezindeki sıcaklık kaç derecedir?",
            options = new string[] { "5.500°C", "100.000°C", "15 milyon °C", "1 milyon °C" },
            correctAnswer = 2,
            explanation = "Güneş'in merkezinde tam 15 milyon derece sıcaklık var! Yüzeyi ise 5.500 derece."
        });

        sun.quizQuestions.Add(new QuizQuestion
        {
            question = "Güneş'in içine kaç tane Dünya sığar?",
            options = new string[] { "100 bin", "500 bin", "1 milyon 300 bin", "10 milyon" },
            correctAnswer = 2,
            explanation = "Güneş o kadar büyük ki içine 1 milyon 300 bin tane Dünya sığar!"
        });

        celestialDatabase.Add(sun);
    }

    void CreateMoonData()
    {
        CelestialObjectData moon = new CelestialObjectData
        {
            objectName = "Moon",
            displayName = "Ay",
            characterVoice = "yumuşak_ve_sakin",
            themeColor = new Color(0.8f, 0.8f, 0.9f), // Gümüş-beyaz
            hasAnimation = true,
            animationType = "orbit_earth",
            hasQuiz = true
        };

        // BÖLÜM 1: Tanışma
        moon.storySections.Add(new StorySection
        {
            title = "🌙 Selam! Ben Ay!",
            pageNumber = 26,
            displayDuration = 8f,
            content =
                "Merhaba! Ben Ay! 🌙✨\n\n" +
                "Geceleri seni hep seyrederim. Bazen dolgun ve yuvarlağım, " +
                "bazen ince bir hilal gibi görünürüm. Ama aslında hep aynıyım! 😊\n\n" +
                "Dünya'nın en yakın arkadaşıyım. 384.000 kilometre uzaktayım. " +
                "Bu çok mu uzak? Aslında uzayda çok yakın sayılır! 🚀\n\n" +
                "Dünya'nın etrafında dans eder gibi dönüyorum. Hiç yalnız bırakmam onu!\n\n" +
                "Hadi benimle bir ay yolculuğuna çıkmaya hazır mısın? 🌠"
        });

        // BÖLÜM 2: Krater Hikayem
        moon.storySections.Add(new StorySection
        {
            title = "💫 Yüzümdeki İzler",
            pageNumber = 27,
            displayDuration = 12f,
            content =
                "Yüzüme bak... Ne görüyorsun? Bir sürü çukur var değil mi? 🌑\n\n" +
                "Bunlara KRATER diyoruz. Bunlar benim savaş yaralarım gibi! 💪\n\n" +
                "📖 HİKAYEM:\n" +
                "Milyarlarca yıl önce, uzayda dev kayalar (meteorlar) " +
                "gezinip dururdu. Bazıları bana çarptı - BAM! 💥\n\n" +
                "Her çarpma bende dev çukurlar bıraktı. En büyük kraterlerimin " +
                "bazıları 300 kilometre genişliğinde! Türkiye'nin yarısı kadar! 😱\n\n" +
                "🛡️ Ama bilir misin? Ben aslında Dünya'nın KALIKANIM!\n" +
                "Dünya'ya çarpacak meteorların çoğunu ben durduruyorum. " +
                "Yani Dünya'yı koruyorum! 🦸‍♂️\n\n" +
                "Yüzümdeki her krater bir hikaye anlatır. " +
                "En eski kraterlerde 4 milyar yaşında!"
        });

        // BÖLÜM 3: Atmosfersiz Dünya
        moon.storySections.Add(new StorySection
        {
            title = "🚫 Hava Yok, Ses Yok!",
            pageNumber = 27,
            displayDuration = 10f,
            content =
                "Bende ATMOSFERİM YOK! 🌑\n\n" +
                "Atmosfer ne mi? Dünya'yı saran hava tabakası işte! " +
                "Senin nefes aldığın şey. Ben onu kaybettim... 😔\n\n" +
                "📢 SES: Olmaz! Bağırsan bile kimse duymaz.\n" +
                "🌈 GÖKYÜZÜ: Hep siyah! Gündüz bile.\n" +
                "☄️ METEORLAR: Yanmadan düşer!\n\n" +
                "🌡️ SICAKLIK ÇILGINLIĞI:\n" +
                "Gündüz: +127°C (su kaynar!) ☀️🔥\n" +
                "Gece: -173°C (don dondurucudan soğuk!) 🌙❄️\n\n" +
                "300 derece fark var! Dünya'da en soğuk ve en sıcak yer " +
                "arasında bile sadece 140 derece fark var.\n\n" +
                "👨‍🚀 Bu yüzden astronotların özel kıyafetlere ihtiyacı var!\n\n" +
                "Ama güzel tarafı: Yıldızları HER ZAMAN net görebilirsin! ✨"
        });

        // BÖLÜM 4: Çekim Gücü
        moon.storySections.Add(new StorySection
        {
            title = "⚖️ Hafif Kanat Gibiyim!",
            pageNumber = 27,
            displayDuration = 9f,
            content =
                "Benim çekim gücüm Dünya'nın ALTIDA BİRİ kadar! 🪶\n\n" +
                "Bu ne demek?\n\n" +
                "Diyelim ki Dünya'da 60 kilo geliyorsun. " +
                "Bende sadece 10 kilo çekersin! 🤸‍♀️\n\n" +
                "SÜPER GÜÇLER KAZANIRSIN:\n" +
                "🦘 6 kat daha yüksek zıplarsın!\n" +
                "💪 6 kat daha fazla ağırlık kaldırırsın!\n" +
                "🏃 Çok daha hızlı koşarsın!\n\n" +
                "Astronotlar bende tavşan gibi zıplayarak gezerler. " +
                "Çünkü normal yürümek çok yorucu! 🐰\n\n" +
                "🏀 Dünya'da basketbol potasına uzanamayan biri, " +
                "bende kolayca smaç basabilir!\n\n" +
                "Ama dikkatli ol! Çok zıplarsan çok uzağa gidebilirsin! 😄"
        });

        // BÖLÜM 5: Senkronize Dans
        moon.storySections.Add(new StorySection
        {
            title = "💃 İki Hareketim, Bir Ritim",
            pageNumber = 27,
            displayDuration = 11f,
            content =
                "Benim çok özel bir dansım var! 🎭\n\n" +
                "İKİ HAREKET BİRDEN:\n" +
                "1️⃣ Kendi etrafımda dönüyorum (topac gibi)\n" +
                "2️⃣ Dünya'nın etrafında dönüyorum (döner dolap gibi)\n\n" +
                "✨ BÜYÜK SIR:\n" +
                "Her iki hareket de TAM 27 GÜN 8 SAAT sürüyor!\n\n" +
                "Bu demek oluyor ki... Dünya'dan bana baktığında " +
                "HEP AYNI YÜZÜMÜ GÖRÜRSÜN! 🌙\n\n" +
                "🎭 KARTAL İK YÜZ:\n" +
                "Ön yüz: Hep görürsün (kraterlerle dolu)\n" +
                "Arka yüz: HİÇ göremezsin! (karanlık taraf)\n\n" +
                "İnsanlar 1959'a kadar arka yüzümü hiç görmemişti. " +
                "Sonra bir uzay aracı gitti ve fotoğraf çekti! 📸\n\n" +
                "Bu senkronize dansa 'Kilitli Rotasyon' denir. " +
                "Dünya'nın çekim gücü beni böyle kilitldi!"
        });

        // BÖLÜM 6: Evrelerim
        moon.storySections.Add(new StorySection
        {
            title = "🌑🌒🌓🌔🌕 Şekil Değiştiriyorum!",
            pageNumber = 35,
            displayDuration = 15f,
            content =
                "Hadi en eğlenceli kısmına gelelim: EVRELERİM! 🎭\n\n" +
                "Aslında hiç değişmiyorum! Ama Güneş ışığı beni " +
                "farklı açılardan aydınlattığı için farklı görünüyorum.\n\n" +
                "🌑 YENİ AY (1. Gün):\n" +
                "'Merhaba! Görünmez oldum! Karanlık yüzüm size dönük.'\n\n" +
                "🌒 HİLAL (3-7. Gün):\n" +
                "'İnce bir gülümseme gibi görünüyorum! İlk ışıklarım belirdi.'\n\n" +
                "🌓 İLK DÖRDÜN (7-8. Gün):\n" +
                "'Yarıma ulaştım! Yarım daire gibiyim.'\n\n" +
                "🌔 ŞIŞMAN AY (8-14. Gün):\n" +
                "'Dolunaya yaklaşıyorum! Şişmanlıyorum! 😊'\n\n" +
                "🌕 DOLUNAY (14-15. Gün):\n" +
                "'TAM DOLDUM! En parlak halim! Gökyüzünü aydınlatıyorum!' ✨\n\n" +
                "🌖 AZALAN AY (15-22. Gün):\n" +
                "'Tekrar küçülmeye başlıyorum...'\n\n" +
                "🌗 SON DÖRDÜN (22-23. Gün):\n" +
                "'Yine yarım oldum, ama ters taraftan!'\n\n" +
                "🌘 SON HİLAL (23-29. Gün):\n" +
                "'İnce bir çizgi kaldı... Yakında yok olacağım.'\n\n" +
                "Ve... 🌑 tekrar YENİ AY!\n\n" +
                "Bu döngü 29.5 günde bir tekrarlanır! 🔄"
        });

        // BÖLÜM 7: Gelgit Gücüm
        moon.storySections.Add(new StorySection
        {
            title = "🌊 Denizleri Hareket Ettiriyorum!",
            pageNumber = 35,
            displayDuration = 10f,
            content =
                "Küçük bir sırrım daha var: DENİZLERİ KONTROL EDİYORUM! 🌊\n\n" +
                "Çekim gücüm sayesinde deniz suyu hareket ediyor:\n\n" +
                "📈 GELGİT (Kabarma):\n" +
                "Bana bakan tarafta deniz kabarıyor! Su seviyesi yükseliyor.\n" +
                "Sahilde kumdan kaleler yapıyorsun, sonra... ŞLAP! 🌊\n" +
                "Gelgit geliyor ve kaleleri yıkıyor!\n\n" +
                "📉 CEZIR (Çekilme):\n" +
                "Diğer tarafta su çekiliyor. Kayalıklar ortaya çıkıyor.\n" +
                "Sahil genişliyor!\n\n" +
                "🔄 Her gün 2 kez gelgit, 2 kez cezir olur!\n\n" +
                "🌕 DOLUNAYDA:\n" +
                "Güneş ve ben aynı hizadayız. Güçlerimiz birleşir!\n" +
                "En BÜYÜK gelgitler oluşur! Dev dalgalar! 🌊🌊🌊\n\n" +
                "Bazı yerlerde gelgit o kadar güçlü ki, deniz 15 metre kabarıyor!\n" +
                "5 katlı bir bina yüksekliğinde! 😱\n\n" +
                "Balıkçılar beni izleyerek denize açılır. " +
                "Çünkü gelgit zamanlarını bilirim! 🎣"
        });

        // Quiz Soruları
        moon.quizQuestions.Add(new QuizQuestion
        {
            question = "Ay'ın Dünya'ya uzaklığı yaklaşık kaç kilometredir?",
            options = new string[] { "150 milyon km", "384 bin km", "1 milyon km", "50 bin km" },
            correctAnswer = 1,
            explanation = "Ay, Dünya'ya yaklaşık 384 bin kilometre uzaklıkta. Bu mesafeyi arabayla hiç durmadan gitsek, 160 gün sürer!"
        });

        moon.quizQuestions.Add(new QuizQuestion
        {
            question = "Ay'ın evrelerinin bir döngüsü kaç gün sürer?",
            options = new string[] { "7 gün", "14 gün", "29.5 gün", "365 gün" },
            correctAnswer = 2,
            explanation = "Ay'ın evreleri 29.5 günde bir tekrarlanır. Yeni aydan yeni aya kadar geçen süre!"
        });

        celestialDatabase.Add(moon);
    }

    void CreateEarthData()
    {
        CelestialObjectData earth = new CelestialObjectData
        {
            objectName = "Earth",
            displayName = "Dünya",
            characterVoice = "neşeli_ve_sıcak",
            themeColor = new Color(0.2f, 0.6f, 1f), // Mavi
            hasAnimation = true,
            animationType = "rotate_and_orbit",
            hasQuiz = true
        };

        // BÖLÜM 1: Tanışma
        earth.storySections.Add(new StorySection
        {
            title = "🌍 Evine Hoş Geldin!",
            pageNumber = 44,
            displayDuration = 8f,
            content =
                "Merhaba sevgili arkadaşım! Ben Dünya! 🌍💙\n\n" +
                "Senin, benim, kedilerin, ağaçların, okyanusların... " +
                "HERKESIN EVİ! 🏠🌳🐱\n\n" +
                "Bilinen evrende YAŞAM BARINDIRAN TEK GEZEGENİM! ✨\n\n" +
                "Güneş Sistemi'nde üçüncü sıradayım. " +
                "Ne çok yakın Güneş'e (yanar biteriz), " +
                "ne çok uzak (donarız). TAM ORTA NOKTADAYIM! 👌\n\n" +
                "Bu yüzden su'yum var, hava'yum var, yaşam var! 💧🌬️🌱\n\n" +
                "Geleceğimi mi öğrenmek istiyorsun? Hadi başlayalım! 🚀"
        });

        // BÖLÜM 2: Mavi Gezegen
        earth.storySections.Add(new StorySection
        {
            title = "🌊 Mavi Gezegenin Sırrı",
            pageNumber = 44,
            displayDuration = 12f,
            content =
                "Uzaydan bana baksaydın ne görürdün? MAVİ BİR TOP! 💙\n\n" +
                "Çünkü yüzeyimin %71'i SU ile kaplı! 🌊\n" +
                "Sadece %29'u kara. Aslında adım 'DÜNYA' değil 'OKYANUSLAR' olmalıydı! 😄\n\n" +
                "💧 SU HİKAYEM:\n" +
                "- 5 büyük okyanusumuz var\n" +
                "- Binlerce göl ve nehir\n" +
                "- Kutuplarda buzullar\n" +
                "- Gökyüzünde bulutlar\n\n" +
                "Suyun %97'si TUZLU (okyanuslarda)\n" +
                "Sadece %3'ü içilebilir tatlı su! 🚰\n\n" +
                "🐋 OKYANUSLAR:\n" +
                "En derin yerim: Mariana Çukuru → 11.000 metre!\n" +
                "Everest Dağı'nı içine atsak, üstü bile görünmez! 😱\n\n" +
                "Okyanuslarımda milyonlarca canlı türü yaşar:\n" +
                "Balinalar, köpek balıkları, ahtapotlar, balıklar... 🐠🦈🐙\n\n" +
                "Su hayat demektir! Su olmasaydı, hiçbir canlı olmazdı! 💚"
        });

        // BÖLÜM 3: Atmosfer Kalkanım
        earth.storySections.Add(new StorySection
        {
            title = "🛡️ Görünmez Kalkanım: Atmosfer",
            pageNumber = 45,
            displayDuration = 13f,
            content =
                "Beni saran görünmez bir kalkanım var: ATMOSFER! 🌫️\n\n" +
                "Atmosfer nedir? Etrafımı saran HAVA TABAKASI! " +
                "100 kilometre kalınlığında dev bir battaniye gibi! 🛡️\n\n" +
                "🌟 SÜPER GÜÇLERİ:\n\n" +
                "1️⃣ NEFES ALDIRIR:\n" +
                "%78 Azot + %21 Oksijen = Yaşam! 🫁\n" +
                "Sen şu anda oksijen soluyorsun! 💨\n\n" +
                "2️⃣ SESİ TAŞIR:\n" +
                "Konuşabilir, müzik dinleyebilirsin! 🎵\n" +
                "Ay'da ses olmaz ama bende var! 📢\n\n" +
                "3️⃣ SICAKLIĞI DENGELER:\n" +
                "Gece çok soğumamı engeller! 🌡️\n" +
                "Gündüz çok ısınmamı önler!\n\n" +
                "4️⃣ METEOR YAKAR:\n" +
                "Uzaydan gelen kayalar sürtünmeyle yanar! 💥\n" +
                "Yıldız kayması dediğin şey aslında yanan meteor!\n\n" +
                "5️⃣ GÜNEŞ'İN ZARARLI IŞINLARINI FİLTRELER:\n" +
                "Ozon tabakası seni korur! ☀️🛡️\n\n" +
                "Eğer atmosfer olmasaydı:\n" +
                "❌ Nefes alamazdın\n" +
                "❌ Konuşamazdın\n" +
                "❌ Meteorlar başına düşerdi\n" +
                "❌ Güneş seni yakardı\n\n" +
                "Atmosfer = Süper kahraman kalkanı! 🦸‍♀️"
        });

        // BÖLÜM 4: İçimdeki Katmanlar
        earth.storySections.Add(new StorySection
        {
            title = "🍰 İçim Soğan Gibi Katmanlı!",
            pageNumber = 45,
            displayDuration = 11f,
            content =
                "İçime bir röntgen çeksek ne görürdük? 🔍\n\n" +
                "Bir pasta gibi katmanlardan oluşuyorum! 🎂\n\n" +
                "1️⃣ KABUK (En dışta):\n" +
                "Üzerinde yürüdüğün yer! 🏔️\n" +
                "Kalınlık: 5-70 km (ince bir kabuk)\n" +
                "Burada dağlar, ovalar, denizler var.\n\n" +
                "2️⃣ MANTO (Ortada - en kalın tabaka):\n" +
                "2.900 km kalınlığında! 🌋\n" +
                "Çok sıcak ve yarı-akışkan (macun gibi)\n" +
                "Yavaş yavaş hareket eder (yılda birkaç cm)\n" +
                "Volkanlardan çıkan lavalar buradan gelir!\n\n" +
                "3️⃣ DIŞ ÇEKİRDEK:\n" +
                "Sıvı halde! 💧\n" +
                "Demir ve nikelden oluşur\n" +
                "Çok sıcak: 4.000 - 5.000°C! 🔥\n" +
                "Sürekli hareket eder, bu sayede...\n\n" +
                "4️⃣ İÇ ÇEKIRDEK (Tam merkezde):\n" +
                "Katı metal top! 🔩\n" +
                "Demir ve nikel\n" +
                "Sıcaklık: 5.000 - 6.000°C! (Güneş'in yüzeyi kadar!)\n" +
                "O kadar yüksek basınç var ki, erimez!\n\n" +
                "🧲 MANYETİK SIRIM:\n" +
                "Dış çekirdeğin hareketi dev bir mıknatıs oluşturur!\n" +
                "Bu sayede pusulalar çalışır! 🧭\n" +
                "Kuşlar yön bulur! 🐦\n" +
                "Güzel kutup ışıkları oluşur! 🌌"
        });

        // BÖLÜM 5: Günlük Hareket - Gece Gündüz Dansı
        earth.storySections.Add(new StorySection
        {
            title = "☀️🌙 Gece-Gündüz Dansım",
            pageNumber = 45,
            displayDuration = 14f,
            content =
                "Şimdi en eğlenceli kısmına gelelim: DÖNÜŞÜM! 🎡\n\n" +
                "Kendi etrafımda topaç gibi dönüyorum! Buna GÜNLÜK HAREKET denir.\n\n" +
                "⏰ BİR TUR: Tam 24 saat!\n" +
                "🔄 YÖN: Batıdan Doğuya (sola doğru)\n\n" +
                "Bu dönüş sayesinde:\n" +
                "🌅 SABAH: Güneş doğar\n" +
                "☀️ ÖĞLEN: Güneş tepede\n" +
                "🌇 AKŞAM: Güneş batar\n" +
                "🌙 GECE: Yıldızlar çıkar\n\n" +
                "🏃 HIZ:\n" +
                "Ekvatorda dönerken saatte 1.670 km gidiyorum!\n" +
                "Bir jet uçağından bile hızlıyım! ✈️💨\n\n" +
                "Ama sen bunu hissetmiyorsun çünkü:\n" +
                "- Hep aynı hızda dönüyorum\n" +
                "- Sen de benimle birlikte dönüyorsun\n" +
                "- Atmosfer de benimle dönüyor\n\n" +
                "Düşün ki: Şu anda otururken aslında 1.670 km/saat hızla uzayda süzülüyorsun! 🚀\n\n" +
                "🌍 İLGİNÇ GERÇEK:\n" +
                "Ekvator'da daha hızlı dönüyorum.\n" +
                "Kutuplar'da daha yavaşım.\n" +
                "İstanbul'da: 1.200 km/saat hızla dönüyorsun!\n\n" +
                "🌅 GÜNEŞ DOĞUYOR MU?\n" +
                "Aslında hayır! Güneş hiç hareket etmez.\n" +
                "BEN dönüyorum! Ama biz 'Güneş doğdu' deriz. 😊\n\n" +
                "Ekranımda beni çevir, gece-gündüzün nasıl oluştuğunu gör! 👆"
        });

        // BÖLÜM 6: Yıllık Hareket - Mevsimler Dansı
        earth.storySections.Add(new StorySection
        {
            title = "🌸☀️🍂❄️ Mevsimler Sihri",
            pageNumber = 45,
            displayDuration = 16f,
            content =
                "İkinci dansım: Güneş'in etrafında dönmek! 🌞🔄\n\n" +
                "Buna YILLIK HAREKET denir. Bir tur atmam tam 365 gün 6 saat!\n\n" +
                "❓ HER 4 YILDA BİR ARTIK YIL NEDEN?\n" +
                "Her yıl 6 saat artar: 6+6+6+6 = 24 saat = 1 gün!\n" +
                "4 yılda bir Şubat 29 çeker! 📅\n\n" +
                "🎯 BÜYÜK SIR: EĞİM!\n" +
                "Ben dümdüz dönmüyorum! 23.5 derece EĞİĞİM! 📐\n\n" +
                "Bu eğiklik MEVSİMLERİ YARATIR! 🎨\n\n" +
                "🌸 İLKBAHAR:\n" +
                "Kuzey Yarımküre Güneş'e doğru eğilmeye başlar\n" +
                "Hava ısınır, karlar erir, çiçekler açılır! 🌷\n" +
                "Kuşlar döner, arılar vızıldar! 🐝\n\n" +
                "☀️ YAZ (En sıcak!):\n" +
                "Kuzey Yarımküre Güneş'e TAM eğik!\n" +
                "Güneş ışınları dik gelir → Çok ısı! 🔥\n" +
                "Gündüzler uzun (14-16 saat)\n" +
                "Geceler kısa (8-10 saat)\n" +
                "Plaja gitme zamanı! 🏖️\n\n" +
                "🍂 SONBAHAR:\n" +
                "Güneş'ten uzaklaşmaya başlarız\n" +
                "Yapraklar renk değiştirir: sarı, kırmızı, turuncu! 🍁\n" +
                "Hasat zamanı! 🌾\n\n" +
                "❄️ KIŞ (En soğuk!):\n" +
                "Kuzey Yarımküre Güneş'ten en uzak!\n" +
                "Güneş ışınları yatay gelir → Az ısı! 🌡️\n" +
                "Gündüzler kısa (8-10 saat)\n" +
                "Geceler uzun (14-16 saat)\n" +
                "Kar yağar, kardan adam yapılır! ☃️\n\n" +
                "🌍🌏 İLGİNÇ GERÇEK:\n" +
                "Türkiye'de yaz iken → Avustralya'da kış!\n" +
                "Türkiye'de kış iken → Brezilya'da yaz!\n\n" +
                "Ekvator'da mevsim yok! Hep sıcak! 🥵\n" +
                "Kutuplar'da ekstrem mevsimler:\n" +
                "- 6 ay gündüz (gece yok!)\n" +
                "- 6 ay gece (gündüz yok!) 😱"
        });

        // BÖLÜM 7: Üçlü Dans - Güneş, Dünya, Ay
        earth.storySections.Add(new StorySection
        {
            title = "🌞🌍🌙 Kozmik Dans Üçlüsü",
            pageNumber = 46,
            displayDuration = 15f,
            content =
                "Şimdi en büyük sırrı söyleyeyim: ÜÇLÜ DANSIMIZ! 💃🕺\n\n" +
                "Güneş, Ben ve Ay birlikte muhteşem bir dans ediyoruz! 🎭\n\n" +
                "🌞 GÜNEŞ:\n" +
                "Merkezde duruyor (dans pistinin kralı!)\n" +
                "Hepimizi çekim gücüyle tutuyor\n\n" +
                "🌍 BEN (DÜNYA):\n" +
                "Güneş'in etrafında dönüyorum (365 gün)\n" +
                "Aynı zamanda kendi etrafımda da dönüyorum (24 saat)\n\n" +
                "🌙 AY:\n" +
                "Benim etrafımda dönüyor (29.5 gün)\n" +
                "Hiç beni bırakmıyor, sadık arkadaşım! 💙\n\n" +
                "⭐ ÖZEL ANLAR:\n\n" +
                "🌑 GÜNEŞ TUTULMASI:\n" +
                "Ay tam Güneş ile benim aramda!\n" +
                "Güneş → Ay → Dünya (düz çizgi)\n" +
                "Ay Güneş'i gizler!\n" +
                "Gündüz karanlığa gömülür! 😱\n" +
                "2-7 dakika sürer\n" +
                "Çok nadir görülür (50 yılda 1-2 kez aynı yerde)\n\n" +
                "🌕 AY TUTULMASI:\n" +
                "Ben Ay ile Güneş'in arasındayım!\n" +
                "Güneş → Dünya → Ay (düz çizgi)\n" +
                "Gölgem Ay'ın üstüne düşer!\n" +
                "Ay kırmızımsı olur (Kanlı Ay) 🩸\n" +
                "Saatlerce sürebilir\n" +
                "Daha sık görülür\n\n" +
                "🌊 GELGİTLER:\n" +
                "Ay beni çeker → Denizler kabarır!\n" +
                "Dolunay'da en güçlü gelgitler olur\n\n" +
                "Bu dans milyarlarca yıldır devam ediyor! 🎶\n" +
                "Ve milyarlarca yıl daha devam edecek! ⏳"
        });

        // BÖLÜM 8: Yaşam Gezegeni
        earth.storySections.Add(new StorySection
        {
            title = "💚 Evrendeki Mucize: Yaşam!",
            pageNumber = 50,
            displayDuration = 12f,
            content =
                "Biliyor musun? Evrende milyarlarca gezegen var.\n" +
                "Ama bildiğimiz tek yaşam barındıran gezegen BENİM! 🌍💚\n\n" +
                "❓ NEDEN?\n\n" +
                "1️⃣ TAM MESAFE:\n" +
                "Güneş'e ne çok yakın, ne çok uzağım!\n" +
                "Su donmuyor, kaynamıyor → Sıvı halde! 💧\n\n" +
                "2️⃣ ATMOSFERİM VAR:\n" +
                "Oksijen → Nefes al! 🫁\n" +
                "Ozon → Koruma kalkanı! 🛡️\n\n" +
                "3️⃣ SU'YUM VAR:\n" +
                "Yaşamın kaynağı!\n" +
                "Her canlının suya ihtiyacı var! 🌊\n\n" +
                "4️⃣ MANYETİK ALANIM VAR:\n" +
                "Güneş rüzgarlarından koruyor! 🧲\n\n" +
                "5️⃣ AY'IM VAR:\n" +
                "Beni dengede tutuyor!\n" +
                "Gelgitler yaratıyor! 🌙\n\n" +
                "🌱 ÜZERİMDE:\n" +
                "- 8.7 milyon tür canlı\n" +
                "- 8 milyar insan\n" +
                "- Trilyonlarca ağaç 🌳\n" +
                "- Katrilyonlarca karınca 🐜\n\n" +
                "Hepiniz benim çocuklarımsınız! 👨‍👩‍👧‍👦\n\n" +
                "💔 TEHLİKE:\n" +
                "İnsanlar beni hasta ediyor:\n" +
                "- Plastikler okyanuslarımı kirletiyor\n" +
                "- Gazlar atmosferimi bozuyor\n" +
                "- Ormanlarım kesiliyor\n\n" +
                "💚 LÜTFEN BENİ KORU:\n" +
                "- Çöplerini geri dönüştür ♻️\n" +
                "- Ağaç dik 🌱\n" +
                "- Su ve elektrik tasarrufu yap 💡\n" +
                "- Hayvanları koru 🦁\n\n" +
                "Sen benim kahramanımsın! 🦸‍♀️\n" +
                "Birlikte geleceği kurtaracağız! 🌈"
        });

        // Quiz Soruları
        earth.quizQuestions.Add(new QuizQuestion
        {
            question = "Dünya kendi etrafında bir tur kaç saatte döner?",
            options = new string[] { "12 saat", "24 saat", "48 saat", "365 gün" },
            correctAnswer = 1,
            explanation = "Dünya kendi etrafında 24 saatte bir tam tur atar. Bu sayede gece ve gündüz oluşur!"
        });

        earth.quizQuestions.Add(new QuizQuestion
        {
            question = "Dünya'nın eksen eğikliği kaç derecedir?",
            options = new string[] { "0 derece", "15 derece", "23.5 derece", "45 derece" },
            correctAnswer = 2,
            explanation = "Dünya'nın 23.5 derece eksen eğikliği sayesinde mevsimler oluşur!"
        });

        earth.quizQuestions.Add(new QuizQuestion
        {
            question = "Dünya'nın yüzeyinin yüzde kaçı su ile kaplıdır?",
            options = new string[] { "%50", "%61", "%71", "%85" },
            correctAnswer = 2,
            explanation = "Dünya'nın yüzeyinin %71'i su ile kaplıdır. Bu yüzden 'Mavi Gezegen' denir!"
        });

        celestialDatabase.Add(earth);
    }

    public CelestialObjectData GetData(string objectName)
    {
        if (dataDict.ContainsKey(objectName))
        {
            return dataDict[objectName];
        }
        return null;
    }

    public List<CelestialObjectData> GetAllData()
    {
        return celestialDatabase;
    }
}