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
                    string pattern = GetRegexPattern(list.Key);

                    Regex regex = new Regex(pattern);
                    var match = regex.Match(questionMessage);

                    if (match.Captures.Count > 0)
                    {
                        method = list.Value;
                        break;
                    }
                }
                return method;
            }

            private string GetRegexPattern(List<string> questionsList)
            {
                string pattern = "";

                GetNormalPattern(questionsList, ref pattern);
                GetUpperCasePattern(questionsList, ref pattern);
                GetFirstUpperCasePattern(questionsList, ref pattern);

                pattern = pattern.Remove(pattern.Length - 1);

                return pattern;
            }

            private void GetNormalPattern(List<string> questionsList, ref string _pattern)
            {
                foreach (var item in questionsList)
                {
                    _pattern += item + "|";
                }
            }

            private void GetUpperCasePattern(List<string> questionsList, ref string _pattern)
            {
                foreach (var item in questionsList)
                {
                    _pattern += item.ToUpper() + "|";
                }
            }

            private void GetFirstUpperCasePattern(List<string> questionsList, ref string _pattern)
            {
                foreach (var item in questionsList)
                {
                    _pattern += item.First().ToString().ToUpper() + item.Substring(1) + "|";
                }
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
                        throw new ArgumentNullException();
                    }

                    var answerObject = method.Invoke(this, parameters);

                    return answerObject.ToString();
                }
                return answer;
            }

            private string WelcomeAnswer(string question, string username)
            {
                List<string> list = questions.Keys.First();
                Random rnd = new Random();
                int randomIndex = rnd.Next(list.Count);

                return $"{list[randomIndex]}, {username}!";
            }

            private string WeatherAnswer(string question, string username)
            {
                return "Pogoda - będzie zimno, będzie wiało, wszędzie będzie pizgało.";
            }
        }
    }
}
