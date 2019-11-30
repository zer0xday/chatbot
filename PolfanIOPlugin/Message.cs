using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PolfanIOPlugin
{
    public class Message
    {
        private List<string> strings = new List<string>();
        private List<int> ints = new List<int>();

        public Message AddString(params string[] values)
        {
            foreach (var value in values)
            {
                strings.Add(value);
            }

            return this;
        }

        public Message AddInt(params int[] values)
        {
            foreach (var value in values)
            {
                ints.Add(value);
            }

            return this;
        }

        public string[] GetStrings()
        {
            return strings.ToArray();
        }

        public int[] GetInts()
        {
            return ints.ToArray();
        }

        public object GetFrame()
        {
            return new { strings = GetStrings(), numbers = GetInts() };
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(GetFrame());
        }

        public Message SetJson(string json)
        {
            var parsed = JsonConvert.DeserializeObject(json);
            var stringsProperty = parsed.GetType().GetProperty("strings");
            var numbersProperty = parsed.GetType().GetProperty("numbers");
            Console.WriteLine(parsed);
            //if (stringsProperty != null && stringsProperty.GetType().Name == "array")
            //{
            //    foreach (var text in stringsProperty.GetValue())
            //    {
            //        AddString(text);
            //    }
            //}

            //if (numbersProperty != null && stringsProperty.Name == "array")
            //{
            //    foreach (var number in parsed.numbers)
            //    {
            //        AddString(number);
            //    }
            //}

            return this;
        }

        public override string ToString()
        {
            return GetJson();
        }
    }
}
