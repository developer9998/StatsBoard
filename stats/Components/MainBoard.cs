using BananaHook;
using Photon.Pun;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace stats.Components
{
    public class MainBoard : MonoBehaviour
    {
        public bool loaded;

        // Text
        public Text NameTex;
        public Text CerrntTime;
        public Text date;
        public Text timeplayedLifetieme;
        public Text Timeplayedsessoin;
        public Text RGBcoler;
        public Text GtagRGBcolor;
        public Text tags;
        public Text tagged;
        public Text matches;

        // UI
        public Image pfp;
        public Image colorimg;

        // Data
        public int Tags;
        public int Tagged;
        public int roundswon;
        public bool istagedd;

        // Time
        public double time_;
        public double Timelifetime;

        // Instance
        public static MainBoard Instance;
      
        // Use this for initialization
        public void Awake()
        {
            Instance = this;

            Events.OnPlayerTagPlayer += PlayerTagEvent;
            Events.OnLocalNicknameChange += OnNameChanged;
            Events.OnRoundEndPre += OnRoundEnd;
            Events.OnRoomDisconnected += OnDisconnect;
            Events.OnRoomJoined += OnRoomConnect;
        }

        public void Start()
        {
            LoadData();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "pfp.png");

            // UI grab
            NameTex = gameObject.transform.Find("stat/Name").gameObject.GetComponent<Text>();
            RGBcoler = gameObject.transform.Find("stat/RGB").gameObject.GetComponent<Text>();
            GtagRGBcolor = gameObject.transform.Find("stat/RGBgtag").gameObject.GetComponent<Text>();
            tags = gameObject.transform.Find("stat/tags").gameObject.GetComponent<Text>();
            tagged = gameObject.transform.Find("stat/taged").gameObject.GetComponent<Text>();
            matches = gameObject.transform.Find("stat/matches").gameObject.GetComponent<Text>();
            colorimg = gameObject.transform.Find("stat/colorImg").gameObject.GetComponent<Image>();
            pfp = gameObject.transform.Find("stat/pfp").GetComponent<Image>();
            Timeplayedsessoin = gameObject.transform.Find("stat/Time played S").gameObject.GetComponent<Text>();
            timeplayedLifetieme = gameObject.transform.Find("stat/Time played T").gameObject.GetComponent<Text>();
            CerrntTime = gameObject.transform.Find("stat/Time").gameObject.GetComponent<Text>();
            date = gameObject.transform.Find("stat/date").gameObject.GetComponent<Text>();

            // UI changes
            pfp.sprite = LoadPNG(path);
            NameTex.text = string.Concat("NAME: ", PhotonNetwork.LocalPlayer.NickName);

            // Colour logic
            float R = PlayerPrefs.GetFloat("redValue");
            float G = PlayerPrefs.GetFloat("greenValue");
            float B = PlayerPrefs.GetFloat("blueValue");

            // Move object
            gameObject.transform.position = new Vector3(-62.8345f, 12.334f, -83.214f);
            gameObject.transform.rotation = Quaternion.Euler(0.1f, 180f, 0.1f);
            gameObject.transform.localScale = new Vector3(0.0224f, 0.0248f, 0.0269f);

            // Finishing UI
            RGBcoler.text = $"R: {R * 255.0f} G: {G * 255.0f} G: {B * 255.0f}";
            GtagRGBcolor.text = $"R: {R * 9.0f} G: {G * 9.0f} G: {B * 9.0}";
            colorimg.sprite = null;
            colorimg.material = GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0];

            var lvlScreen = FindObjectOfType(typeof(GorillaLevelScreen)) as GorillaLevelScreen;
            Material localMaterial = new Material(Shader.Find("Legacy Shaders/Diffuse"));
            localMaterial.mainTexture = lvlScreen.goodMaterial.mainTexture;
            localMaterial.SetColor("_Color", Color.green);
            gameObject.transform.Find("mesh/Plane").GetComponent<Renderer>().material = localMaterial;

            tagged.text = $"TAGGED: {Tagged}";
            tags.text = $"TAGS: {Tags}";
            matches.text = $"HUNT MATCHES WON: {roundswon}";
            loaded = true;

            InvokeRepeating(nameof(SaveData), 0, 10);
            InvokeRepeating(nameof(UpdateTimer), 0.5f, 0.5f);
        }

        public void UpdateColour()
        {
            float R = PlayerPrefs.GetFloat("redValue");
            float G = PlayerPrefs.GetFloat("greenValue");
            float B = PlayerPrefs.GetFloat("blueValue");
            RGBcoler.text = $"R: {R * 255.0f} G: {G * 255.0f} G: {B * 255.0f}";
            GtagRGBcolor.text = $"R: {R * 9.0f} G: {G * 9.0f} G: {B * 9.0}";
        }

        public void Update()
        {
            time_ += Time.deltaTime;
            Timelifetime += (double)Time.deltaTime;

            string cerrnttime = DateTime.Now.ToString("h:mm tt");
            string Date = DateTime.Now.ToString("D").ToUpper();

            CerrntTime.text = cerrnttime;
            date.text = Date;
        }

        public void UpdateTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time_);
            string session = GetCorrectTime(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            Timeplayedsessoin.text = $"TIME [SESSION]: {session}";

            TimeSpan _timeSpan = TimeSpan.FromSeconds(Timelifetime);
            string lifetime = GetCorrectTime(_timeSpan.Days, _timeSpan.Hours, _timeSpan.Minutes, _timeSpan.Seconds);
            timeplayedLifetieme.text = $"TIME [LIFETIME]: {lifetime}";
        }

        public string GetCorrectTime(float day, float hour, float minute, float second)
        {
            int totalDaySlot = 3;
            int fixedDaySlot = totalDaySlot - day.ToString().Length;
            int totalHourSlot = 2;
            int fixedHourSlot = totalHourSlot - hour.ToString().Length;
            int totalMinuteSlot = 2;
            int fixedMinuteSlot = totalMinuteSlot - minute.ToString().Length;
            int totalSecondSlot = 2;
            int fixedSecondSlot = totalSecondSlot - second.ToString().Length;

            bool dayNeeded = day > 0;
            bool hourNeeded = hour > 0;

            string output = "";

            if (dayNeeded)
            {
                if (fixedDaySlot != 0) output += string.Concat(Enumerable.Repeat("0", fixedDaySlot));
                output += day;
                output += ":";
            }

            if (hourNeeded)
            {
                if (fixedHourSlot != 0) output += string.Concat(Enumerable.Repeat("0", fixedHourSlot));
                output += hour;
                output += ":";
            }

            if (fixedMinuteSlot != 0) output += string.Concat(Enumerable.Repeat("0", fixedMinuteSlot));
            output += minute;
            output += ":";

            if (fixedSecondSlot != 0) output += string.Concat(Enumerable.Repeat("0", fixedSecondSlot));
            output += second;

            return output;
        }

        void PlayerTagEvent(object sender, PlayerTaggedPlayerArgs player)
        {
            if (player.victim.IsLocal)
            {
                Tagged++;
                tagged.text = $"TAGGED: {Tagged}";
                istagedd = true;
            }

            if (player.tagger.IsLocal)
            {
                Tags++;
                tags.text = $"TAGS: {Tags}";
            }
        }

        void OnRoundEnd(object sender, EventArgs e)
        {
            if (!istagedd)
            {
                roundswon++;
                matches.text = $"HUNT MATCHES WON: {roundswon}";
            }

            istagedd = false;
        }

        void OnNameChanged(object sender, PlayerNicknameArgs args)
        {
            NameTex.text = string.Concat("NAME: ", PhotonNetwork.LocalPlayer.NickName);
        }

        void OnDisconnect(object senderm,  EventArgs args)
        {
            istagedd = false;
        }

        void OnRoomConnect(object sender, EventArgs args)
        {
            istagedd = true;
        }

        public void OnApplicationQuit()
        {
            SaveSystem.SaveData(this);
        }

        public void SaveData()
        {
            SaveSystem.SaveData(this);
            UpdateColour();
        }

        public void LoadData()
        {
            StatsData data = SaveSystem.LoadPlayer();

            Tags = data.Tags;
            Tagged = data.Tagged;
            roundswon = data.HuntWins;
            Timelifetime = data.Lifetime;
        }

        private static Sprite LoadPNG(string filePath)
        {
            Texture2D tex;
            Sprite sprite = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);

                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
                tex.filterMode = FilterMode.Point;

                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
            }

            return sprite;
        }
    }
}