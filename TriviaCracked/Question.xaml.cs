using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Diagnostics;

namespace TriviaCracked {
    public partial class Question : UserControl {
        public delegate void callback(JToken answer);

        private callback cb;

        private string id;
        private string category;
        private string type;

        public Question(JObject jo, callback _cb) {
            InitializeComponent();

            cb = _cb;

            JToken question = jo["spins_data"]["spins"][0]["questions"][0]["question"];
            type = jo["spins_data"]["spins"][0]["type"].Value<string>();

            string _text = question["text"].Value<string>();
            category = question["category"].Value<string>();
            id = question["id"].Value<string>();

            questionBox.Text = _text;
            categoryBox.Text = category;
            spinTypeBox.Text = type;

            List<JToken> answers = question["answers"].ToList();

            for(int i = 0; i < answers.Count(); ++i) {
                answerList.Items.Add(answers[i].Value<string>());
            }
            answerList.SelectedIndex = question["correct_answer"].Value<int>();
        }

        private void answerButton_Click(object sender, RoutedEventArgs e) {
            JToken answer = new JObject();
            answer["answers"] = new JArray();
            ((JArray)answer["answers"]).Add(new JObject());
            answer["answers"][0] = new JObject();
            answer["answers"][0]["id"] = id;
            answer["answers"][0]["category"] = category;
            answer["answers"][0]["answer"] = answerList.SelectedIndex;
            answer["type"] = type;

            cb(answer);
        }
    }
}
