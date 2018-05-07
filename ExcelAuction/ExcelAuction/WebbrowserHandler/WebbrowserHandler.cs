using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace ExcelAuction.WebbrowserHandler
{
    public class WebbrowserHandler
    {
        public string itemID;
        public Manager defaultManager;
        protected List<HtmlElement> ElementsByClass(HtmlElementCollection elements, string className)
        {
            List<HtmlElement> results = new List<HtmlElement>();
            foreach (HtmlElement e in elements)
                if (e.GetAttribute("className") == className)
                    results.Add(e);
            return results;
        }


        protected List<HtmlElement> ElementsByName(HtmlElementCollection elements, string value)
        {
            List<HtmlElement> results = new List<HtmlElement>();
            foreach (HtmlElement e in elements)
                if (e.GetAttribute("name") == value)
                    results.Add(e);
            return results;
        }

        protected HtmlElement FirstElementByClass(HtmlElementCollection elements, string className)
        {
           
            foreach (HtmlElement e in elements)
                if (e.GetAttribute("className") == className)
                    return e ;
            return null;
        }
        public virtual void WhenDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        { }

        public virtual void WhenNavigated(object sender, WebBrowserNavigatedEventArgs e) { }

    }
}
