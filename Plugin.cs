using BepInEx;
using System;
using UnityEngine;
using Utilla;
using BananaHook;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;

namespace stats
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInDependency("net.rusjj.gtlib.bananahook", "1.3.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
      public static bool inRoom;
        int Tags;
        int Tagged;
        GameObject statsbord;
        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
            BananaHook.Events.OnPlayerTagPlayer += Playertagged;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
            BananaHook.Events.OnPlayerTagPlayer -= Playertagged;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
            StartCoroutine(KTstart());
        }
        IEnumerator KTstart()
        {
            var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("stats.asset.stat");
            Debug.Log("1");
            var bundleLoadRequest = AssetBundle.LoadFromStreamAsync(fileStream);
            Debug.Log("2");
            yield return bundleLoadRequest;
            Debug.Log("2");
            var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            Debug.Log("3");
            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                Debug.Log("4");
                yield break;
            }
            Debug.Log("5");
            var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("stats");
            Debug.Log("6");
            yield return assetLoadRequest;
            Debug.Log("7");
            GameObject stat = assetLoadRequest.asset as GameObject;
            Debug.Log("8");
            
            statsbord = Instantiate(stat);
            Debug.Log("9");
            statsbord.AddComponent<conpontes.statsbored>();
            statsbord.GetComponent<conpontes.statsbored>().enabled = true;
            Debug.Log("10 You made it");
        }

        void Playertagged(object sender, PlayerTaggedPlayerArgs player)
        {
            print("Tag event");
            if (player.victim.IsLocal)
            {
                print("lol you got tagged");
                Tagged++;
                print($"Tagged: {Tagged}");
            }
            if (player.tagger.IsLocal)
            {
                print("You Taged somebody good job");
                Tags++;
                print($"Tags: {Tags}");
            }
        }
        void Update()
        {
            /* Code here runs every frame when the mod is enabled */
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
    public static class savesysyem
    {
        public static void SaveData(conpontes.statsbored bFManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.streamingAssetsPath + "/Stas.kt";

            FileStream fileStream = new FileStream(path, FileMode.Create);
            statsData data = new statsData(bFManager);


            formatter.Serialize(fileStream, data);
            fileStream.Close();

            Debug.Log("SAVED BANANA FIEND DATA TO " + path);
        }
        public static statsData LoadPlayer()
        {
            string path = Application.streamingAssetsPath + "/Stas.kt";
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                statsData data = binaryFormatter.Deserialize(fileStream) as statsData;
                fileStream.Close();

                return data;

            }
            else
            {
                Debug.Log("No savefile found :( path  was " + path);
                conpontes.statsbored.Instins.adtosave();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                statsData data = binaryFormatter.Deserialize(fileStream) as statsData;
                fileStream.Close();

                return data;

            }


        }
            public static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
    } 
    
 


