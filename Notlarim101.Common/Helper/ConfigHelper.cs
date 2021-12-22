using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim101.Common.Helper
{
    public class ConfigHelper
    {
        //public static string Get(string key)
        //{
        //    //Configuration manager Web.config dosyası içinde appSettings içinde oluşturdğumuz mail dosyalarının keylerine ulaşmak için kullanıcaz.
        //    return ConfigurationManager.AppSettings[key];
        //}

        public static T Get<T>(string key)
        {
            //port numarası gibi int bir geri dönğş istenirse bunun için metodu generic hale getirerek gelen tipi istenen tipe değiştirerek göndeririz.
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
