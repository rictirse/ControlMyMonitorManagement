using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace CMM.Library.Helpers
{
    static class JsonSerializerExtensions
    {
        public static JsonSerializerOptions defaultSettings = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IgnoreNullValues = true,
            PropertyNamingPolicy = null,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
    }

    internal static class JsonHelper
    {
        /// <summary>
        /// 複製整個obj全部結構
        /// </summary>
        public static T DeepCopy<T>(T RealObject) =>
            JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(RealObject, JsonSerializerExtensions.defaultSettings));

        public static string JsonFormResource(this string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                        assembly.GetManifestResourceNames().
                        Where(str => str.Contains(fileName)).FirstOrDefault();
            if (resourceName == null) return "";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        public static T JsonFormResource<T>(this string fileName) =>
            JsonFormString<T>(JsonFormResource(fileName));

        public static T JsonFormFile<T>(this string fileName) =>
            JsonFormString<T>(Load(fileName));

        public static T JsonFormString<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json);

        public static void FileToJson<T>(this T payload, string savePath) =>
            Save(savePath, payload.ToJson());

        public static string ToJson<T>(this T payload) =>
            JsonSerializer.Serialize(payload, JsonSerializerExtensions.defaultSettings);

        /// <summary>
        /// 從Embedded resource讀string
        /// </summary>
        /// <param name="aFileName">resource位置，不含副檔名</param>
        public static string GetResource(this Assembly assembly, string aFileName)
        {
            var resourceName = assembly
                .GetManifestResourceNames()
                .Where(str => str.Contains(aFileName))
                .FirstOrDefault();
            if (resourceName == null) return "";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var sr = new StreamReader(stream, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        public static string Load(string aFileName) =>
            Load(new FileInfo(aFileName));

        public static string Load(FileInfo aFi)
        {
            if (aFi.Exists)
            {
                string _Json = string.Empty;

                try
                {
                    var sr = new StreamReader(aFi.FullName);
                    _Json = sr.ReadToEnd();
                    sr.Close();
                }
                catch (IOException) { throw; }
                catch (Exception) { throw; }

                return _Json;
            }

            throw new Exception("開檔失敗。");
        }

        public static void Save(string filePath, string content) =>
            Save(new FileInfo(filePath), content);

        public static void Save(FileInfo aFi, string aContent)
        {
            if (!aFi.Directory.Exists)
            {
                aFi.Directory.Create();
            }

            if (aFi.Exists)
            {
                aFi.Delete();
            }

            aFi.Refresh();
            if (aFi.Exists) throw new Exception("寫檔失敗，檔案已存在或已開啟。");

            try
            {
                File.WriteAllText(aFi.FullName, aContent);
            }
            catch (IOException) { throw; }
            catch (Exception) { throw; }
        }
    }
}
