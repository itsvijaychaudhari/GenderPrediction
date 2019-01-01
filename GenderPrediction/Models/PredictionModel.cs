using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LibLinearDotNet;

namespace GenderPrediction.Models
{
    public class PredictionModel
    {
      
        [Required]
        public string Name { get; set; }
        public string Suggestedgender { get; set; }
        public string Currectedgender { get; set; }
        public string genderType { get; set; }
       
        public Sex Gender { get; set; }

        public List<Sex> SexList { get; set; }


        double[] probability;

        public PredictionModel GetPredict(PredictionModel gender)
        {
            //PredictionModel getGender = new PredictionModel();
            ApplicationObject appObj = new ApplicationObject();
            appObj = HttpContext.Current.Application["GobleObject"] as ApplicationObject;


            string test_data = "0";
            string last_three = "";
            string last_two = "";
            string last_one = "";
            string isVowel = "";
            string vowels = "aeiou";

            List<int> indices = new List<int>();
            int a = 0, b = 0, c = 0, d = -1;

            
            //getGender.Name = gender.Name;

            if (gender.Name.Length > 3)
                last_three = gender.Name.Substring(gender.Name.Length - 3);

            if (gender.Name.Length > 2)
                last_two = gender.Name.Substring(gender.Name.Length - 2);

            if (gender.Name.Length > 1)
                last_one = gender.Name.Substring(gender.Name.Length - 1);

            if (appObj != null && appObj.lines.Contains(last_three))
            {
                a = Array.IndexOf(appObj.lines, last_three);
                indices.Add(a + 1);
            }

            if (appObj.lines.Contains(last_two))
            {
                b = Array.IndexOf(appObj.lines, last_two);
                indices.Add(b + 1);
            }

            if (appObj.lines.Contains(last_one))
            {
                c = Array.IndexOf(appObj.lines, last_one);
                indices.Add(c + 1);
            }


            if (vowels.Contains(last_one))
                d = 1;

            isVowel = "1942:" + d;

            indices.Sort();
            foreach (int i in indices)
            {
                if (i != 0)
                    test_data = test_data + " " + i + ":" + 1;
            }

            test_data = test_data + " " + isVowel;

            int predict = getPrediction(test_data);

            double greater = 0.0;
            double dMaleCoefficient = 58;
            double dFemaleCoefficient = 63;
            if (probability[0] > probability[1])
                greater = Convert.ToInt16(probability[0] * 100);
            else
                greater = Convert.ToInt16(probability[1] * 100);

            if (predict == 2)
            {
                if (greater >= dMaleCoefficient)
                {
                    gender.genderType = "Male (" + greater + "%)";
                    gender.Suggestedgender = "M";
                  
                   
                }
                else
                {
                    gender.genderType = "Male/Female (" + greater + "%)";
                    gender.Suggestedgender= "M";
                   
                   
                }
            }
            else
            {
                if (greater >= dFemaleCoefficient)
                {
                    gender.genderType = "Female (" + greater + "%)";
                    gender.Suggestedgender = "F";
                  
                   
                }
                else
                {
                    gender.genderType = "Female/Male (" + greater + "%)";
                    gender.Suggestedgender = "F";
                }
            }
            






            //if (predict == 2)
            //{
            //    gender.genderType = "Male";
            //    //gender.GenderType.IsMale = true;
            //    //getGender.result = "Male";
            //    //ViewBag.Gender= "Male";
            //}
            //else
            //{
            //    gender.genderType = "Female";
            //    //gender.GenderType.IsFemale = true;
            //    //getGender.result = "Female";
            //    //ViewBag.Gender = "Female";
            //}
            indices.Clear();
            return gender;
        }

        public int getPrediction(string test_data)
        {

            double bias = -1.0;
            var predict = 0;
            // StreamWriter write = new StreamWriter("Prediction.txt");

            var test = ReadTestData(test_data, bias);
            // using (model)
            {
                var x = test.X;
                for (var i = 0; i < test.Length; i++)
                {
                    var array = x[i];
                   
                    predict = (int)LibLinear.Predict((HttpContext.Current.Application["GobleObject"] as ApplicationObject)?.Model, array,out probability);
                    // write.WriteLine(predict);
                }
            }

            //write.Close();
            return predict;
        }

        public Problem ReadTestData(string test_data, double bias)
        {
            var x = new List<FeatureNode[]>();
            var y = new List<double>();
            //  var lines = File.ReadAllLines(path);

            var test_dataParts = test_data.Split(' ');
            y.Add(double.Parse(test_dataParts[0]));

            var nodes = new List<FeatureNode>();
            for (var i = 1; i <= test_dataParts.Length - 1; i++)
            {
                var token = test_dataParts[i].Trim().Split(':');
                nodes.Add(new FeatureNode
                {
                    Index = int.Parse(token[0]),
                    Value = int.Parse(token[1])
                });
            }

            x.Add(nodes.ToArray());


            return new Problem(x.ToArray(), y.ToArray(), bias);
        }


    }
}