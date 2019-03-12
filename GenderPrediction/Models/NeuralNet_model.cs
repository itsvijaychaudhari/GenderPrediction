using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace GenderPrediction.Models
{
    public class NeuralNetModel
    {
        private readonly string _name;
        /*
                private readonly Dictionary<string, Dictionary<string, string>> _dict = new Dictionary<string, Dictionary<string, string>>();
        */
        /*
                private readonly FileInfo[] _files;
        */
        private float[] _inputFeatures;
        /*
                private readonly double[] _inputFeaturesDouble;
        */
        //private readonly List<string> _allFeatures = new List<string>();
        /*
                private readonly Dictionary<int, ArrayList> _dictionary = new Dictionary<int, ArrayList>();
        */
        private int _cnt = 1;


        public NeuralNetModel(string name)
        {
            _name = name;

        }

        //calculate features using NN
        public void CalculateFeaturesForNn()
        {
            //name = textBox.Text;
            //List<string> features = new List<string>();
            string feature = "";
            char[] charArray = _name.ToCharArray();
            Array.Reverse(charArray);
            int index = 0;
            Array.Sort(((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).Files, (f1, f2) => string.Compare(f2.Name, f1.Name, StringComparison.Ordinal));
            string filename = "";
            string loadfile = "";
            Dictionary<string, string> temp = null;
            _inputFeatures = new float[10];

            for (int i = 0; i < 2; i++)
            {
                #region try

                float featureValue;
                if (i == 0)
                {
                    for (int t = i; t < i + 3; t++)
                    {
                        if (t == 0)
                        {
                            filename = "Uni";
                        }
                        else if (t == 1)
                        {
                            filename = "Bi";
                        }
                        else if (t == 2)
                        {
                            filename = "Tri0";
                        }

                        for (int k = 0; k < 2; k++)
                        {
                            if (k == 0)
                            {
                                loadfile = filename + "_M.txt";
                            }
                            else if (k == 1)
                            {
                                loadfile = filename + "_F.txt";
                            }

                            for (int j = 0; j < t + 1; j++)
                            {

                                if (j == charArray.Length)
                                {
                                    if (feature.Length == 1)
                                    {
                                        feature = "**" + feature;
                                    }
                                    else if (feature.Length == 2)
                                    {
                                        feature = "*" + feature;
                                    }

                                    break;
                                }

                                loadfile = string.Join("", ((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).Files.Where(f => f.Name.Equals(loadfile)).ToList());
                                temp = ((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).Get(loadfile);
                                feature = charArray[j] + feature;
                            }
                            //_allFeatures.Add(feature);
                            Debug.Assert(temp != null, nameof(temp) + " != null");
                            if (temp.ContainsKey(feature))
                            {
                                featureValue = float.Parse(temp[feature]);
                                ScaleFeature(ref featureValue);
                                _inputFeatures[index] = featureValue;
                            }
                            else
                            {
                                featureValue = 0.0f;
                                ScaleFeature(ref featureValue);
                                _inputFeatures[index] = featureValue;
                            }

                            feature = "";
                            index++;

                        }

                    }

                }
                else
                {
                    for (int t = i; t < i + 2; t++)
                    {
                        if (t == 1)
                        {
                            filename = "Tri1";
                        }
                        else if (t == 2)
                        {
                            filename = "Tri2";
                        }

                        for (int k = 0; k < 2; k++)
                        {
                            if (k == 0)
                            {
                                loadfile = filename + "_M.txt";
                            }
                            else if (k == 1)
                            {
                                loadfile = filename + "_F.txt";
                            }

                            for (int j = t; j < t + 3; j++)
                            {

                                if (j == charArray.Length)
                                {
                                    if (feature.Length == 1)
                                    {
                                        feature = "**" + feature;
                                    }
                                    else if (feature.Length == 2)
                                    {
                                        feature = "*" + feature;
                                    }

                                    break;
                                }

                                loadfile = string.Join("", ((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).Files.Where(f => f.Name.Equals(loadfile)).ToList());
                                temp = ((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).Get(loadfile);
                                feature = charArray[j] + feature;

                            }
                            //_allFeatures.Add(feature);
                            Debug.Assert(temp != null, nameof(temp) + " != null");
                            if (temp.ContainsKey(feature))
                            {
                                featureValue = float.Parse(temp[feature]);
                                ScaleFeature(ref featureValue);
                                _inputFeatures[index] = featureValue;
                            }
                            else
                            {
                                featureValue = 0.0f;
                                ScaleFeature(ref featureValue);
                                _inputFeatures[index] = featureValue;
                            }

                            feature = "";
                            index++;
                        }
                    }
                }
                #endregion
            }
        }

        private void ScaleFeature(ref float index)
        {
            int upper = 1;
            int lower = -1;
            ArrayList l = ((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).ParamDictionary[_cnt];
            l.Sort();
            float min = (float)Convert.ToDouble(((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).ParamDictionary[_cnt][0]);
            float max = (float)Convert.ToDouble(((ApplicationObject)HttpContext.Current.Application["GobleObjectSVM"]).ParamDictionary[_cnt][1]);

            //index = (index - min) / (max - min);
            if (index == min)
            {
                min = lower;
                // return;

            }
            else if (index == max)
            {
                max = upper;
                //  return;
            }
            //if(index != upper && index !=lower)
            index = lower + (((upper - lower) * (index - min)) / (max - min));

            _cnt++;

        }

        //perdict gender using  NN

        public string predictFusionSVM_NN(FANNCSharp.Float.NeuralNet netCsharp)
        {
            float[] calcOutFloat = netCsharp.Run(_inputFeatures);
            //double dMaleCoefficient = 0.42;
            //double dFemaleCoefficient = 0.58;
            double dMaleCoefficient = 0.4625;
            double dFemaleCoefficient = 0.5375;
            calcOutFloat[0] = calcOutFloat[0] / (calcOutFloat[0] + calcOutFloat[1]);
            calcOutFloat[1] = calcOutFloat[1] / (calcOutFloat[0] + calcOutFloat[1]);
            if (calcOutFloat[0] > calcOutFloat[1])
            {
                if (calcOutFloat[0] >= dMaleCoefficient)
                {
                    return "M " + calcOutFloat[0];
                }

                return "Both " + calcOutFloat[0];
            }
            if (calcOutFloat[1] > calcOutFloat[0])
            {
                if (calcOutFloat[1] >= dFemaleCoefficient)
                {
                    return "F " + calcOutFloat[1];
                }
                return "Both " + calcOutFloat[1];
            }
            return "";

        }


    }
}