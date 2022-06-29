using BananaHook;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
namespace stats.conpontes
{
    public class statsbored : MonoBehaviour
    {
        Text name;
        Text timeplayedLifetieme;
        Text Timeplayedsessoin;
        Text RGBcoler;
        Text tags;
        Text tagged;
        Text matches;
        Image pfp;
        Image colorimg;
       public int Tags;
       public int Tagged;
       public int roundswon;
        bool istagedd;
        public double time_;
        public double Timelifetime;
        public static statsbored Instins = new statsbored();
        // Use this for initialization
        void Awake()
        {
            BananaHook.Events.OnPlayerTagPlayer += Playertagged;
            BananaHook.Events.OnLocalNicknameChange += Onnamechanged;
            BananaHook.Events.OnRoundEndPre += Onroundend;
            BananaHook.Events.OnRoomDisconnected += Ondisconect;
            BananaHook.Events.OnRoomJoined += Onroom;
            
            
        }
        void Start()
        {
            LoadData();
            string path = statsbored.fileLocation + "\\pfp.png";
            if(!File.Exists(path))
            {
                print("NO pfp found in:" + path);
            }
            name = gameObject.transform.Find("stat/Name").gameObject.GetComponent<Text>();
            RGBcoler = gameObject.transform.Find("stat/RGB").gameObject.GetComponent<Text>();
            tags = gameObject.transform.Find("stat/tags").gameObject.GetComponent<Text>();
            tagged = gameObject.transform.Find("stat/taged").gameObject.GetComponent<Text>();
            matches = gameObject.transform.Find("stat/matches").gameObject.GetComponent<Text>();
             colorimg = gameObject.transform.Find("stat/colorImg").gameObject.GetComponent<Image>();
             pfp = gameObject.transform.Find("stat/pfp").GetComponent<Image>();
            pfp.sprite = LoadPNG(path);
            Timeplayedsessoin = gameObject.transform.Find("stat/Time played S").gameObject.GetComponent<Text>();
            timeplayedLifetieme = gameObject.transform.Find("stat/Time played T").gameObject.GetComponent<Text>();
            base.gameObject.transform.position = new Vector3(-62.8345f, 12.334f, -83.214f);
            base.gameObject.transform.rotation = Quaternion.Euler(0.1f, 180f, 0.1f);
            base.gameObject.transform.localScale = new Vector3(0.0224f, 0.0248f, 0.0269f);
            name.text = $"NAME: {Photon.Pun.PhotonNetwork.LocalPlayer.NickName}";
            float R = PlayerPrefs.GetFloat("redValue") / 1.0f * 255.0f;
            float G = PlayerPrefs.GetFloat("greenValue") / 1.0f * 255.0f;
            float B =PlayerPrefs.GetFloat("blueValue") / 1.0f * 255.0f;
            byte R_ = Convert.ToByte(R);
            byte G_ = Convert.ToByte(G);
            byte B_ = Convert.ToByte(B);
            
            
            RGBcoler.text = $"R: {PlayerPrefs.GetFloat("redValue")/ 1.0f * 255.0f} G: {PlayerPrefs.GetFloat("greenValue") / 1.0f * 255.0f} G: {PlayerPrefs.GetFloat("blueValue") / 1.0f * 255.0f}";
            colorimg.color = new Color32(R_, G_, B_, 255);
            tagged.text = $"TAGGED: {Tagged}";
            tags.text = $"TAGS: {Tags}";
            matches.text = $"HUNT MATCHES WON: {roundswon}";
            InvokeRepeating("adtosave", 0, 10);
        }
        void Update()
        {
            time_ += (double)Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(time_);
            Timeplayedsessoin.text = $"TIME PLAYED (SESSION): {timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
            Timelifetime += (double)Time.deltaTime;
            TimeSpan _timeSpan = TimeSpan.FromSeconds(Timelifetime);
            timeplayedLifetieme.text = $"TIME PLAYED (LIFETIME): {_timeSpan.Hours}:{_timeSpan.Minutes}:{_timeSpan.Seconds}";
        }
        void Playertagged(object sender, PlayerTaggedPlayerArgs player)
        {
            if(Plugin.inRoom == false)
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
            
        }
        void Onroundend(object sender, EventArgs e)
        {
            print("round end");
            if (istagedd == false)
            {
                print("You won");
                roundswon++;
                matches.text = $"HUNT MATCHES WON: {roundswon}";
            }
            istagedd = false;
        }

        void Onnamechanged(object sender, PlayerNicknameArgs args)
        {
            name.text = $"NAME: {Photon.Pun.PhotonNetwork.LocalPlayer.NickName}";
        }

        /*public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled

            print(gamemode);
            istagedd = true;
        } */
        void Ondisconect(object senderm,  EventArgs args)
        {
            istagedd = false;
        }
        void Onroom(object sender, EventArgs args)
        {
            
            istagedd = true;
        }
         void OnApplicationQuit()
        {
            savesysyem.SaveData(this);
        }
       public void adtosave()
        {
            savesysyem.SaveData(this);
        }
        public void LoadData()
        {
            statsData data = savesysyem.LoadPlayer();

            Tags = data.Tags;
            Tagged = data.Tagged;
            roundswon = data.huntwins;
            Timelifetime = data.TodleTimne;
         

            Debug.Log("Yay Loaded A Save File! " + tags);
        }
        private static Sprite LoadPNG(string filePath)
        {

            Texture2D tex = null;
            Sprite sprite = null;
            byte[] fileData;

            if (System.IO.File.Exists(filePath))
            {
                fileData = System.IO.File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
               tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
            }
            return sprite;
        }
        public static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}