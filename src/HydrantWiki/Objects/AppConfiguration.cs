using System;
using System.IO;
using System.Reflection;

namespace HydrantWiki.Objects
{
    public class AppConfiguration
    {
        public static AppConfiguration Load()
        {
            var assembly = typeof(AppConfiguration).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("HydrantWiki.AppConfiguration.json");
            string json = "";
            using (var reader = new StreamReader(stream))
            {
                json = reader.ReadToEnd();
            }

            AppConfiguration config = JsonConvert.DeserializeObject<AppConfiguration>(json);

            return config;
        }
    }
}
