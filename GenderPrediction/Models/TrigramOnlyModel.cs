using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using LibLinearDotNet;

namespace GenderPrediction.Models
{
    public class TrigramOnlyModel
    {
        string _name;
        string[] _newLines;

        public TrigramOnlyModel(string name)
        {
            _name = name;
            ApplicationObject appObj = HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject;
            _newLines = appObj?.Lines;
        }

        public string getTestData_trigram_only()
        {
            List<int> indices = new List<int>();
            int a;
            List<string> features = new List<string>();
            string feature = "";
            string suffix = "";
            string testData = "0";
            var charArray = _name.ToCharArray();
            Array.Reverse(charArray);
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                    suffix = "_1";
                else if (i == 1)
                    suffix = "_2";
                else if (i == 2)
                    suffix = "_3";

                for (int j = i; j < i + 3; j++)
                {
                    var result = feature.Split('_');
                    if (j == charArray.Length)
                        break;
                    feature = charArray[j].ToString() + result[0];
                }
                feature = feature + suffix;
                features.Add(feature);
                feature = "";
            }
            foreach (var item in features)
            {
                if (_newLines.Contains(item))
                {
                    a = Array.IndexOf(_newLines, item);
                    indices.Add(a + 1);
                }
                else
                {
                    // write1.WriteLine(item.ToString() + "\tTrigram-Only");
                }
            }
            //if (vowels.Contains(last_one))
            //    d = 1;

            //   isVowel = "1942:"+d;

            indices.Sort();
            //if(skip==false)
            {
                foreach (int i in indices)
                {
                    if (i != 0)
                        testData = testData + " " + i + ":" + 1;
                }
            }
            // write1.Close();
            return testData;

        }

        private Problem ReadTestData(string testData, double bias)
        {
            var x = new List<FeatureNode[]>();
            var y = new List<double>();
            //  var lines = File.ReadAllLines(path);

            var testDataParts = testData.Split(' ');
            y.Add(double.Parse(testDataParts[0]));

            var nodes = new List<FeatureNode>();
            for (var i = 1; i <= testDataParts.Length - 1; i++)
            {
                var token = testDataParts[i].Trim().Split(':');
                nodes.Add(new FeatureNode
                {
                    Index = int.Parse(token[0]),
                    Value = int.Parse(token[1])
                });
            }

            x.Add(nodes.ToArray());
            return new Problem(x.ToArray(), y.ToArray(), bias);
        }


        public string PredictFusion(Model model, string testData, double dMaleCoefficient, double dFemaleCoefficient)
        {
            Problem test;
            double bias = -1.0;
            var predict = 0;
            double[] prob = null;
            if (testData != "0")
                test = ReadTestData(testData, bias);
            else
            {
                //if (!MainWindow.isLoading)
                //    ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Can't perdict");
                return "Can't predict";
            }

            var x = test.X;
            for (var i = 0; i < test.Length; i++)
            {
                var array = x[i];
                predict = (int)LibLinear.Predict(model, array, out prob);
            }

            double greater;
            //double dMaleCoefficient = 58;
            //double dFemaleCoefficient = 63;
            Debug.Assert(prob != null, nameof(prob) + " != null");
            if (prob[0] > prob[1])
                greater = Convert.ToInt16(prob[0] * 100);
            else
                greater = Convert.ToInt16(prob[1] * 100);

            if (predict == 2)
            {
                if (greater >= dMaleCoefficient * 100)
                {
                    return "M " + greater;
                }

                else
                {
                    //return "MF " + greater;
                    return "Both " + greater;
                }
            }

            else
            {
                if (greater >= dFemaleCoefficient * 100)
                {
                    return "F " + greater;
                }
                else
                {
                    //return "FM " + greater;
                    return "Both " + greater;
                }
            }
            //return "";
            // write1.Close();
        }
    }
}