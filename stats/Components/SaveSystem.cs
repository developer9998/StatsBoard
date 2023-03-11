using System.IO;
using System.Reflection;
using UnityEngine;

namespace stats.Components
{
    public static class SaveSystem
    {
        public static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string saveLocation = Path.Combine(Application.streamingAssetsPath, "StatsBoardMod.txt");

        public static void SaveData(MainBoard bFManager)
        {
            StatsData data = new StatsData(bFManager);
            File.WriteAllText(saveLocation, JsonUtility.ToJson(data));
        }

        public static StatsData LoadPlayer()
        {
            if (!File.Exists(saveLocation)) MainBoard.Instance.SaveData();
            StatsData data = JsonUtility.FromJson<StatsData>(File.ReadAllText(saveLocation));

            return data;
        }
    }
}
