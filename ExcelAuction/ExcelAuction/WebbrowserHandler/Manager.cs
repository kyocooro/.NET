using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ExcelAuction.WebbrowserHandler
{
    public class Manager
    {
        public List<YahooItem> itemsInfo = new List<YahooItem>();
        protected WebForm webForm;
        public virtual void didGetPaymentInfo(string itemID, string paymentInfo)
        { }

        public virtual void didGetQA(string itemID, System.Windows.Forms.HtmlElement qaInfo)
        { }

        public virtual void didSendPaymentMsg(string itemID, string info)
        { }

        public virtual void didSendFirstMsg(string itemID, string info)
        { }

        public virtual void failToSendFirstMsg(string itemID, string info)
        { }

        public virtual void StoreFormInputingStatus(string itemID, string info)
        { }

        public virtual void didPayKantan(string itemID, string info)
        { }

        public virtual void didGetSellerInfo(string itemID, string info)
        { }
    }
}
