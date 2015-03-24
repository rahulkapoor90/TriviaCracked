using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Navigation;
using System;

namespace TriviaCracked {
    public class Game : Button {
        public string opponent_user_name, opponent_name;

        public string status;
        public string id;

        public bool my_turn;

        public bool active {
            get {
                return status == "ACTIVE" || status == "PENDING_APPROVAL";
            }
        }

        public Game(JToken j)
        {
            opponent_user_name = j["opponent"].Value<string>("username");
            opponent_name = j["opponent"].Value<string>("facebook_name");

            status = j.Value<string>("game_status");
            id = j.Value<string>("id");

            my_turn = j.Value<bool>("my_turn");

            Content = opponent_user_name +"\t"+status + "\n" + opponent_name;
            if(active && my_turn) {
                Visibility = System.Windows.Visibility.Visible;
            } else {
                Visibility = System.Windows.Visibility.Collapsed;
            }
            Width = 441;
            HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
        }
    }
}
