using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DotNetOpenAuth.AspNet.Clients;
using DotNetOpenAuth.Messaging;
using Newtonsoft.Json.Linq;

namespace YahooAuction
{
	internal static class HttpWebResponseExtensions {
		public static string GetResponseBody(this HttpWebResponse response) {
			using(var stream = response.GetResponseStream())
			using(var reader = new StreamReader(stream)) {
				return reader.ReadToEnd();
			}
		}
	}

	internal static class DictionaryExtensions {
		public static IDictionary<string, string> AddIfNotEmpty(this IDictionary<string, string> dictionary, string key, string value) {
			if(!string.IsNullOrEmpty(value)) dictionary[key] = value;
			return dictionary;
		}
	}

	public class YahooClient {
		private static string Escape(string target) {
			return Uri.EscapeDataString(target);
		}


		private readonly string _appId;
		private readonly string _appSecret;


		public YahooClient(string appId, string appSecret) {

			_appId = appId;
			_appSecret = appSecret;
		}


		// AuthorizationエンドポイントのURIを取得
        public Uri GetServiceLoginUrl(Uri returnUrl)
        {
			var builder = new UriBuilder("https://auth.login.yahoo.co.jp/yconnect/v1/authorization");
			builder.Query = string.Join("&",
				"response_type=code",
				"client_id=" + Escape(_appId),
				"redirect_uri=" + Escape(returnUrl.AbsoluteUri),
				"scope=" + Escape("openid profile"));
			return builder.Uri;
		}

		// 取得したアクセストークンを使ってユーザ情報を取得
        public IDictionary<string, string> GetUserData(string accessToken)
        {
			// UserInfo API
			var builder = new UriBuilder("https://userinfo.yahooapis.jp/yconnect/v1/attribute");
			builder.Query = string.Join("&", "schema=openid");

			var request = WebRequest.Create(builder.Uri);

			// Authorizationヘッダーにアクセストークンを含める
			request.Headers.Add( "Authorization", "Bearer " + Escape(accessToken));

			// リクエスト送信 => ユーザ情報の取得
			using(var response = (HttpWebResponse)request.GetResponse()) {
				if(response.StatusCode != HttpStatusCode.OK) return null;

				var json = response.GetResponseBody();
				var userData = JObject.Parse(json);
				if(userData == null) return null;

				// "id"と"username"は必須
				// todo:
				return new Dictionary<string, string>()
					.AddIfNotEmpty("id", (string)userData["user_id"])
					.AddIfNotEmpty("username", (string)userData["name"]);
			}
		}


		// Tokenエンドポイントへリクエストを送信して、アクセストークンを取得
        public string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
			var request = WebRequest.Create("https://auth.login.yahoo.co.jp/yconnect/v1/token");
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";



			// POSTデータ
			var postData = string.Join("&",
				"grant_type=authorization_code",
				"code=" + Escape(authorizationCode),
                "client_id=" + Escape(_appId),
				"redirect_uri=" + Escape(returnUrl.AbsoluteUri));

			// POSTデータをリクエストに書き込む
			request.ContentLength = postData.Length;
			using(var stream = request.GetRequestStream())
			using(var writer = new StreamWriter(stream)) {
				writer.Write(postData);
				writer.Flush();
			}

			// リクエスト送信 => アクセストークン取得
			using(var response = (HttpWebResponse)request.GetResponse()) {
				if(response.StatusCode != HttpStatusCode.OK) return null;

				var json = response.GetResponseBody();
				var tokenData = JObject.Parse(json);
				if(tokenData == null) return null;

				return (string)tokenData["access_token"];
			}
		}

        private String jsonSuccessfulItemsAtPage(int page)
        {
            var builder = new UriBuilder("https://auctions.yahooapis.jp/AuctionWebService/V2/myWonList");
            builder.Query = string.Join("&", "access_token=" + Global.accessCode,
                "output=json",
                "start=" + page
                );

            var request = WebRequest.Create(builder.Uri);

            // リクエスト送信 => ユーザ情報の取得
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode != HttpStatusCode.OK) return null;

                String json = response.GetResponseBody();
                json = json.Substring(7, json.Length - 8);
                return (String)json;
            }
        }
        public List<string> GetSuccessfulItemsAtPage(int page, ref int total, ref int end)
        {
            JObject json = JObject.Parse(jsonSuccessfulItemsAtPage(page));
            if (json != null)
            {
                List<string> itemsID = new List<string>();
                total = (int)json["ResultSet"]["@attributes"]["totalResultsAvailable"];
                end = (int)json["ResultSet"]["@attributes"]["totalResultsReturned"] + (int)json["ResultSet"]["@attributes"]["firstResultPosition"];
                IEnumerable<string> ids = json["ResultSet"]["Result"].Children()["AuctionID"].Values<string>();
                itemsID.AddRange(ids);
                return itemsID;
            }
            return null;
        }

        public JToken GetJsonItemInfo(string id)
        {
            var builder = new UriBuilder("http://auctions.yahooapis.jp/AuctionWebService/V2/auctionItem");
            builder.Query = string.Join("&",
                "appid=" + Escape(_appId),
                "output=json",
                "auctionID=" + id
                );

            var request = WebRequest.Create(builder.Uri);

            // リクエスト送信 => ユーザ情報の取得
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK) return null;

                    String json = response.GetResponseBody();
                    json = json.Substring(7, json.Length - 8);
                    return JObject.Parse(json)["ResultSet"]["Result"];
                }
            }
            catch (System.Exception ex)
            {
                return null;
            }
            
        }
	}
}