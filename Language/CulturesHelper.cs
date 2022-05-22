using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace CMM.Language
{
    public class CulturesHelper
    {
        private bool _isFoundInstalledCultures = false;

        const string LanguageFolder = "/Language;component";
        const string ResourceLocalPath = "pack://application:,," + LanguageFolder;
        const string LanguageFileName = "StringResources";

        public List<CultureInfo> SupportedCultures { get; private set; } = new List<CultureInfo>();

        public CulturesHelper() => Init();

        private void Init()
        {
            if (!_isFoundInstalledCultures)
            {
                var cultureInfo = new CultureInfo("");

                var Languages = GetAllLanguageResource();

                GetAllLanguageResource().ForEach(file =>
                {
                    try
                    {
                        string cultureName = file.Substring(file.IndexOf(".") + 1).Replace(".xaml", "");

                        cultureInfo = CultureInfo.GetCultureInfo(cultureName);

                        if (cultureInfo != null)
                        {
                            SupportedCultures.Add(cultureInfo);
                        }
                    }
                    catch (ArgumentException) { }
                });

                _isFoundInstalledCultures = true;
            }
        }

        /// <summary>
        /// 增加Language的XMAL檔
        /// </summary>
        private static List<string> GetAllLanguageResource()
        {
            var Languages = new List<string>();
            string uriPath = ResourceLocalPath + "/" + LanguageFileName;

            Languages.Add(uriPath + ".en-US.xaml");
            Languages.Add(uriPath + ".zh-TW.xaml");

            return Languages;
        }

        public void ChangeCulture(string cultureName)
        {
            var cultureInfo = CultureInfo.GetCultureInfo(cultureName);

            if (cultureInfo != null)
            {
                ChangeCulture(cultureInfo);
            }
        }

        /// <summary>
        /// 切換語系
        /// </summary>
        public void ChangeCulture(CultureInfo culture)
        {
            if (SupportedCultures.Contains(culture))
            {
                var existsRD = Application.Current.Resources.MergedDictionaries
                        .Where(x => x.Source.OriginalString.StartsWith(ResourceLocalPath, StringComparison.CurrentCultureIgnoreCase) ||
                                    x.Source.OriginalString.StartsWith(LanguageFolder, StringComparison.CurrentCultureIgnoreCase))
                        .FirstOrDefault();

                if (existsRD == null) return;

                var resourceFile = $"{ResourceLocalPath}/{LanguageFileName}.{culture.Name}.xaml";
                var res = new ResourceDictionary()
                {
                    Source = new Uri(resourceFile, UriKind.Absolute)
                };

                Application.Current.Resources.MergedDictionaries.Remove(existsRD);
                Application.Current.Resources.MergedDictionaries.Add(res);
            }
        }
    }

    public static class Lang
    {
        public static string Find(string key)
        {
            if (Application.Current == null) return null;

            try
            {
                return (string)Application.Current.FindResource(key);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
