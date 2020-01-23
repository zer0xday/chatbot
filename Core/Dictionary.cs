using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;

namespace ChatBot
{
    partial class Core
    {
        private class Dictionary
        {
            private Dictionary<List<string>, string> questions = new Dictionary<List<string>, string>()
            {
                { new List<string>() { "czesc", "elo", "hej" }, "test" },
                { new List<string>() { "pogoda" }, "WeatherAnswer" },
            };
            // fixme: all
            private string GetMethodToInvoke(string questionMessage)
            {
                string method = "";

                foreach (var list in questions)
                {
                    string regexString = "";

                    foreach (var item in list.Key)
                    {
                        regexString += item + "|";
                    }

                    regexString = regexString.Remove(regexString.Length - 1);
                    Regex regex = new Regex(regexString);
                    var match = regex.Match(questionMessage);

                    if (match.Captures.Count > 0)
                    {
                        method = list.Value;
                        break;
                    }
                }

                return method;
            }

            private string WeatherAnswer()
            {
                return "TO DZIALA";
            }

            public string GetAnswer(string questionMessage, string username)
            {
                string methodName = GetMethodToInvoke(questionMessage);
                string answer = "";

                if (methodName.Length > 0)
                {
                    Type type = GetType();
                    // something breaks here
                    //MethodInfo method = type.GetMethod(methodName);
                    //var answerObject = method.Invoke(this, null);

                    return "testedd";
                }

                return answer;
            }
        }
    }
}
