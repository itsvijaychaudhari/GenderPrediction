using LibLinearDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GenderPrediction.Models
{
    public class ApplicationObject
    {
        public LibLinearDotNet.Model Model { get; set; }

        public FANNCSharp.Float.NeuralNet NeuralNetModel { get; set; }
        public string[] lines { get; set; }

        Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
        public Dictionary<int, ArrayList> paramDictionary = new Dictionary<int, ArrayList>();
        public FileInfo[] files;

       
        public void Set(string key, Dictionary<string, string> value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public Dictionary<string, string> Get(string key)
        {
            Dictionary<string, string> result = null;

            if (dictionary.ContainsKey(key))
            {
                result = dictionary[key];
            }

            return result;
        }


    }
}