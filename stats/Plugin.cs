using BepInEx;
using UnityEngine;
using System.Reflection;
using stats.Components;

namespace stats
{
    [BepInDependency("net.rusjj.gtlib.bananahook", "1.3.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public GameObject Board;

        public void Awake()
        {
            Instance = this;
            HarmonyPatches.ApplyHarmonyPatches();
        }

        public void Init()
        {
            var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("stats.Assets.stat");
            var bundle = AssetBundle.LoadFromStream(fileStream);
            var resource = bundle.LoadAsset("stats") as GameObject;

            Board = Instantiate(resource);
            Board.AddComponent<MainBoard>();
        }
    }
} 