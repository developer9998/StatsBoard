using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace stats.Components
{
    public static class SaveSystem
    {
        public static string fileLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string saveLocation = Path.Combine(Application.streamingAssetsPath, "StatsBoard.log");
        public static void SaveData(MainBoard bFManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = saveLocation;

            FileStream fileStream = new FileStream(path, FileMode.Create);
            StatsData data = new StatsData(bFManager);

            formatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public static StatsData LoadPlayer()
        {
            string path = saveLocation;
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                StatsData data = binaryFormatter.Deserialize(fileStream) as StatsData;
                fileStream.Close();

                return data;

            }
            else
            {
                MainBoard.Instance.SaveData();

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);

                StatsData data = binaryFormatter.Deserialize(fileStream) as StatsData;
                fileStream.Close();

                return data;
            }
        }
    }
}
