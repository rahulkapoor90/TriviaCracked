using System;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace TriviaCracked {
    public class CrackApi {
        public const string baseURL = @"http://api.preguntados.com/api/";

        public delegate void RequestFinished(string result);

        public static RequestFinished callback;

        public static void login(string email, string password, RequestFinished _callback) {
            callback = _callback;

            JObject jo = new JObject();
            jo["email"] = email;
            jo["password"] = password;

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.UploadStringCompleted += doneUpload;
            wc.UploadStringAsync(new Uri(baseURL + "login"), jo.ToString());

        }

        public static void dashboard(string user_id, string session_id, RequestFinished _callback) {
            callback = _callback;

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.Cookie] = "ap_session=" + session_id + "; ";
            wc.DownloadStringCompleted += doneDownload;
            wc.DownloadStringAsync(new Uri(baseURL+"users/"+user_id+"/dashboard"));
        }

        public static void games(string user_id, string session_id, string game_id, RequestFinished _callback) {
            callback = _callback;

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.Cookie] = "ap_session=" + session_id + "; ";
            wc.DownloadStringCompleted += doneDownload;
            wc.DownloadStringAsync(new Uri(baseURL + "users/" + user_id + "/games/" + game_id));
        }

        public static void answer(string user_id, string session_id, string game_id, JToken answer, RequestFinished _callback) {
            callback = _callback;

            WebClient wc = new WebClient();
            wc.Headers[HttpRequestHeader.Cookie] = "ap_session=" + session_id + "; ";
            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
            wc.UploadStringCompleted += doneUpload;
            wc.UploadStringAsync(new Uri(baseURL + "users/" + user_id + "/games/" + game_id+"/answers"), answer.ToString());
        }

        private static void doneDownload(Object sender, DownloadStringCompletedEventArgs e) {
            string result = "error";
            try {
                result = (string)e.Result;
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
            callback(result);
        }

        private static void doneUpload(Object sender, UploadStringCompletedEventArgs e) {
            string result = "error";
            try {
                 result = (string)e.Result;
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
            callback(result);
        }

    }
}
