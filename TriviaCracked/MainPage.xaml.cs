using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using Newtonsoft.Json.Linq;

namespace TriviaCracked {
    public partial class MainPage : PhoneApplicationPage {

        private string session_id = "";
        private string user_id = "";

        private string game_id = "";

        private UIElement original_content;


        void showLoginPrompt() {
            //when initialized ask for a password
            PasswordPrompt prompt = new PasswordPrompt(this.Content, loginCallback);
            this.Content = prompt;
        }

        void loginCallback(string result) {
            JObject jo = JObject.Parse(result);
            //Debug.WriteLine(jo.ToString());

            session_id = jo["session"].Value<string>("session");
            user_id = jo.Value<string>("id");
            //got the session id and user id
            //now we need to load the active games
            CrackApi.dashboard(user_id, session_id, dashboardCallback);
        }

        void dashboardCallback(string result) {
            JObject jo = JObject.Parse(result);
            //Debug.WriteLine(jo.ToString());

            var jgames = jo["list"].ToList();
            for(int i = 0; i < jgames.Count; ++i) {
                gamesList.Items.Add(new Game(jgames[i]));
                ((Game)gamesList.Items[i]).Click += gameClicked;
            }
        }

        void gameClicked(object sender, RoutedEventArgs e) {
            Game g = (Game)sender;
            game_id = g.id;
            CrackApi.games(user_id, session_id, game_id, gameCallback);
        }

        void gameCallback(string result) {
            JObject jo = JObject.Parse(result);
            //Debug.WriteLine(jo.ToString());
            if(jo["game_status"].Value<string>() == "ENDED") {
                //we win go home
                InitializeComponent();
                //reload active games
                CrackApi.dashboard(user_id, session_id, dashboardCallback);
            } else {
                Question q = new Question(jo, questionDone);
                this.Content = q;
            }
        }

        void questionDone(JToken answer) {
            //send answer to server
            CrackApi.answer(user_id, session_id, game_id, answer, gotAnswer);
        }

        void gotAnswer(string result) {
            //get next question
            CrackApi.games(user_id, session_id, game_id, gameCallback);
        }

        // Constructor
        public MainPage() {
            InitializeComponent();
            original_content = this.Content;

            showLoginPrompt();
        }
    }
}