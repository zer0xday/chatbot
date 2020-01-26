using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;
using System.IO;
using RestSharp;
using Newtonsoft.Json;

namespace ChatBot
{
    partial class Core
    {
        private class Questions
        {
            public List<QuestionRecords> questions { get; set; }
        }

        private class QuestionRecords
        {
            public List<string> table { get; set; }
            public string methodName { get; set; }
        }

        private class Answer
        {
            private const string DICTIONARIES_DIRECTORY_NAME = @"dictionaries\";
            private const string DICTIONARY_JSON_FILE = "dictionary.json";

            private Questions dictionary { get; set; }

            public Answer()
            {
                dictionary = GetDictionary();
            }

            private Questions GetDictionary()
            {
                Questions dictionary;

                try
                {
                    StreamReader streamReader = new StreamReader(GetDictionaryLocation());
                    string fileContent = streamReader.ReadToEnd();
                    dictionary = JsonConvert.DeserializeObject<Questions>(fileContent);
                } catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return dictionary;
            }
                
            private string GetDictionaryLocation()
            {
                string baseDir = AppContext.BaseDirectory;
                string[] explodedPath = baseDir.Split("GuiClient");

                StringBuilder dictionariesPath = new StringBuilder();
                dictionariesPath.Append(explodedPath[0]);
                dictionariesPath.Append(DICTIONARIES_DIRECTORY_NAME);
                dictionariesPath.Append(DICTIONARY_JSON_FILE);

                return dictionariesPath.ToString();
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
                        throw new Exception("Metoda dla wybranego zbioru pytań nie istnieje.");
                    }

                    var answerObject = method.Invoke(this, parameters);

                    return answerObject.ToString();
                }
                return answer;
            }

            private string GetMethodToInvoke(string questionMessage)
            {
                string method = "";

                foreach (var question in dictionary.questions)
                {
                    string pattern = GetRegexPattern(question.table);

                    Regex regex = new Regex(pattern);
                    var match = regex.Match(questionMessage);

                    if (match.Captures.Count > 0)
                    {
                        method = question.methodName;
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

            private string WelcomeAnswer(string question, string username)
            {
                List<string> list = dictionary.questions.First().table;
                Random rnd = new Random();
                int randomIndex = rnd.Next(list.Count);

                return $"{list[randomIndex]}, {username}!";
            }

            private string WeatherAnswer(string question, string username)
            {
                const string httpsRequest = "https://api.openweathermap.org/data/2.5/weather?q=Krakow,pl&appid=d488bec22a52041e1f7455123c831df3&units=metric";
                var client = new RestClient(httpsRequest);
                var request = new RestRequest(Method.GET);

                IRestResponse response = client.Execute(request);
                var responseJson = JsonConvert.DeserializeObject<Weather>(response.Content);

                StringBuilder responseStringBuilder = new StringBuilder();
                responseStringBuilder.Append("Pogoda w mieście ");
                responseStringBuilder.Append(responseJson.name);
                responseStringBuilder.Append(" wynosi: ");
                responseStringBuilder.Append(responseJson.main.temp);
                responseStringBuilder.Append("°C");

                return responseStringBuilder.ToString();
            }
        }

        private class Weather
        {
            public string name { get; set; }
            public WeatherMain main { get; set; }
        }

        private class WeatherMain
        {
            public float temp { get; set; }
        }
    }
}
