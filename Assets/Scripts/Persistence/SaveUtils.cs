using System.IO;
using UnityEngine;

namespace Assets.Scripts.Persistence
{
    public static class SaveUtils
    {
        private static readonly string DirectoryPath = Application.persistentDataPath + "/Save";
        private static readonly string FilePath = DirectoryPath + "/save.dat";

        public static void Save(Snapshot snapshot)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            var data = JsonUtility.ToJson(snapshot);
            using var writer = new StreamWriter(FilePath);
            writer.Write(data);
        }

        public static Snapshot Load()
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            using var reader = new StreamReader(FilePath);
            var data = reader.ReadToEnd();
            return (Snapshot) JsonUtility.FromJson(data, typeof(Snapshot));
        }
    }
}
