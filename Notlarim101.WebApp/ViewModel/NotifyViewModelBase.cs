using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notlarim101.WebApp.ViewModel
{
    public class NotifyViewModelBase<T>//bütün alanlar için kullanabilir yapmak için generic yaptık.
    {
        public List<T> Items { get; set; }
        public string Header { get; set; }
        public string Title { get; set; }
        public bool IsRedirecting { get; set; }
        public string RedirectingUrl { get; set; }
        public int RedirectingTimeout { get; set; }
        public NotifyViewModelBase()
        {
            Header = "Yönlendiriliyorsunuz...";
            Title = "Geçersiz işlem";
            IsRedirecting = true;
            RedirectingUrl = "/Home/Index";
            RedirectingTimeout = 10000;
            Items = new List<T>();

        }
    }
}