using System.IO;
using UnityEngine;

namespace Assets.Scripts.Persistence
{
    public static class SaveUtils
    {
        private static readonly string DirectoryPath = Application.persistentDataPath + "/Save";
        private static readonly string FilePath = DirectoryPath + "/save.dat";

        public static void Save(SaveFile saveFile)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            var data = JsonUtility.ToJson(saveFile);
            using var writer = new StreamWriter(FilePath);
            writer.Write(data);
        }

        public static SaveFile Load()
        {
            if (!File.Exists(FilePath))
            {
                return null;
            }

            using var reader = new StreamReader(FilePath);
            var data = reader.ReadToEnd();
            return (SaveFile) JsonUtility.FromJson(data, typeof(SaveFile));
        }
    }
}
