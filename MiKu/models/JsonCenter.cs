using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MiKu.models
{
    public class JsonCenter
    {
        public static List<AppFile> ReadAppJson()
        {
            var jsonStr = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\app.json");
            return fastJSON.JSON.ToObject<List<AppFile>>(jsonStr);
        }

        public static List<LinkObject> ReadLinkJson()
        {
            var jsonStr = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\link.json");
            return fastJSON.JSON.ToObject<List<LinkObject>>(jsonStr);
        }

        public static SettingObject ReadSettingJson()
        {
            var jsonStr = System.IO.File.ReadAllText(Environment.CurrentDirectory + "\\setting.json");
            return fastJSON.JSON.ToObject<SettingObject>(jsonStr);
        }

        public static void InitAppJson(List<AppClassify> AppList)
        {
            fastJSON.JSONParameters parameters = new fastJSON.JSONParameters();
            parameters.EnableAnonymousTypes = true;
            parameters.UseEscapedUnicode = false;
            var jsonStr = fastJSON.JSON.ToJSON(AppList, parameters);

            var fileName = Environment.CurrentDirectory + "\\app.json";
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName + ".bak");
                System.IO.File.Move(fileName, fileName + ".bak");
            }
            System.IO.File.WriteAllText(fileName, jsonStr);
        }

        public  static void WriteAppJson(List<AppFile> AppList)
        {
            fastJSON.JSONParameters parameters = new fastJSON.JSONParameters();
            parameters.EnableAnonymousTypes = true;
            parameters.UseEscapedUnicode = false;
            var jsonStr = fastJSON.JSON.ToJSON(AppList, parameters);
            System.IO.File.WriteAllText(Environment.CurrentDirectory+"\\app.json",jsonStr);
        }

        public static void WriteSettingJson(SettingObject setting)
        {
            fastJSON.JSONParameters parameters = new fastJSON.JSONParameters();
            parameters.EnableAnonymousTypes = true;
            parameters.UseEscapedUnicode = false;
            var jsonStr = fastJSON.JSON.ToJSON(setting, parameters);
            System.IO.File.WriteAllText(Environment.CurrentDirectory + "\\setting.json", jsonStr);
        }

    }
}
