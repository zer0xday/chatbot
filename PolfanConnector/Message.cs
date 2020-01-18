using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace PolfanConnector
{
    public class Message
    {
        public int[] Ints 
        { 
            set { } 
            get { return this.ints.ToArray(); } 
        }
        public string[] Strings 
        { 
            set { } 
            get { return this.strings.ToArray(); } 
        }

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

        public int? GetInt(int index)
        {
            try
            {
                return this.Ints[index];
            } catch (IndexOutOfRangeException) { }

            return null;
        }

        public string GetString(int index)
        {
            try
            {
                return this.Strings[index];
            }
            catch (IndexOutOfRangeException) { }

            return null;
        }

        public object GetFrame()
        {
            return new { strings = Strings, numbers = Ints };
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(GetFrame());
        }

        public Message SetJson(string json)
        {
            var parsed = JsonConvert.DeserializeObject<Frame>(json);
            this.strings = parsed.strings;
            this.ints = parsed.numbers;
          
            return this;
        }

        public override string ToString()
        {
            return GetJson();
        }

        private class Frame
        {
            public List<string> strings = new List<string>();
            public List<int> numbers = new List<int>();
        }
    }
}
