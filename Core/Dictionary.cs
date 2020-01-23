using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ChatBot
{
    partial class Core
    {
        private class Dictionary
        {
            private readonly string[] questions = new string[]
            {
                "cześć", "hej", "elo"
            };

            private Regex _regex
            {
                get => new Regex(string.Join("|", questions));
            }

            public string GetAnswer(string question, string username)
            {
                var match = _regex.Match(question);
                string answer = "";

                if (match.Captures.Count > 0) 
                {
                    answer = "it matches";
                }

                return answer;
            }
        }
    }
}
