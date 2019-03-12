using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GenderPrediction.Models
{
    public class ApplicationObject
    {
        public LibLinearDotNet.Model Model { get; set; }

        public FANNCSharp.Float.NeuralNet NeuralNetModel { get; set; }
        public string[] Lines { get; set; }

        Dictionary<string, Dictionary<string, string>> _dictionary = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<int, ArrayList> ParamDictionary = new Dictionary<int, ArrayList>();
        public FileInfo[] Files;

       
        public void Set(string key, Dictionary<string, string> value)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary[key] = value;
            }
            else
            {
                _dictionary.Add(key, value);
            }
        }

        public Dictionary<string, string> Get(string key)
        {
            Dictionary<string, string> result = null;

            if (_dictionary.ContainsKey(key))
            {
                result = _dictionary[key];
            }

            return result;
        }


    }
}