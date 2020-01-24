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
        private class Answer
        {
            private Dictionary<List<string>, string> questions = new Dictionary<List<string>, string>()
            {
                { 
                    new List<string>() 
                    { 
                        "czesc", "cześć", "siemka", "siema", "elo", "hej", "witam" 
                    }, 
                    "WelcomeAnswer" 
                },
                { 
                    new List<string>() 
                    { 
                        "pogoda" 
                    }, 
                    "WeatherAnswer"
                }
            };

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

            public string GetAnswer(string questionMessage, string username)
            {
                string methodName = GetMethodToInvoke(questionMessage);
                string[] parameters = new string[2] 
                {
                    questionMessage, 
                    username 
                };
                string answer = "";

                if (methodName.Length > 0)
                {
                    Type type = GetType();
                    MethodInfo method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

                    if (method == null)
                    {
                        return "";
                    }

                    var answerObject = method.Invoke(this, parameters);

                    return answerObject.ToString();
                }
                return answer;
            }

            private string WelcomeAnswer(string question, string username)
            {
                return $"Dzień dobry, {username}!";
            }

            private string WeatherAnswer(string question, string username)
            {
                return "Pogoda - będzie zimno, będzie wiało, wszędzie będzie pizgało.";
            }
        }
    }
}
