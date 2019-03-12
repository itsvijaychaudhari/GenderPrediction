using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GenderPrediction.Models;
using Model = LibLinearDotNet.Model;

namespace GenderPrediction
{
    public class MvcApplication : HttpApplication
    {

        ApplicationObject _appObj = new ApplicationObject();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadModelSvm();
            LoadModelNn();
        }

        private void LoadModelSvm()
        {
            

            //string path = HttpContext.Current.Server.MapPath("~/PredictionModel/gram_features.txt");
            //StreamReader read = new StreamReader(path);
            ////StreamReader read = new StreamReader("gram_features.txt");
            //appObj.lines = new string[1945];
            //string line = read.ReadToEnd();
            //var lines = line.Split('\n');
            //for (int i = 0; i < lines.Count(); i++)
            //{
            //    appObj.lines[i] = lines[i].Trim();
            //}
            //read.Close();
            //string path2 = HttpContext.Current.Server.MapPath("~/PredictionModel/gender_detection_train.model");
            //appObj.Model = Model.Load(path2);


            string path = HttpContext.Current.Server.MapPath("~/PredictionModel/only_tri_grams.txt");
            StreamReader read = new StreamReader(path);
            //StreamReader read = new StreamReader("gram_features.txt");
            _appObj.Lines = new string[5679];
            string line = read.ReadToEnd();
            var lines = line.Split('\n');
            for (int i = 0; i < lines.Count(); i++)
            {
                _appObj.Lines[i] = lines[i].Trim();
            }
            read.Close();
            string path2 = HttpContext.Current.Server.MapPath("~/PredictionModel/LR_Only_trigram_143.model");
            _appObj.Model = Model.Load(path2);

            //appObj.Model = Model.Load("gender_detection_train.model");
            HttpContext.Current.Application["GobleObjectSVM"] = _appObj;
        }

        private void LoadModelNn()
        {
            string neuralNetModelPath = HttpContext.Current.Server.MapPath("~/PredictionModel/NN_10_prob_143.net");
            _appObj.NeuralNetModel = new FANNCSharp.Float.NeuralNet(neuralNetModelPath);
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Probabilities");
            DirectoryInfo directory = new DirectoryInfo(path);
            _appObj.Files = directory.GetFiles();
            foreach (var file in _appObj.Files)
            {
                Dictionary<string, string> fileDict = new Dictionary<string, string>();
                StreamReader read = new StreamReader(file.DirectoryName + "\\" + file);

                string line;
                while ((line = read.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    fileDict.Add(parts[0], parts[1]);
                }

                _appObj.Set(file.ToString(), fileDict);
            }
            ReadParam();
        }

        public void ReadParam()
        {
           
            string line;
            string path = HttpContext.Current.Server.MapPath("~/PredictionModel/Corr_Train_x.data.parameter");
            StreamReader read = new StreamReader(path);//"Train_x.data.parameter");
            int count = 0;
            while ((line = read.ReadLine()) != null)
            {
                count++;
                if (count < 3)
                    continue;
                string[] parameters = line.Split(' ');
                ArrayList list = new ArrayList(2);
                list.Add(parameters[1]);
                list.Add(parameters[2]);
                _appObj.ParamDictionary.Add(Convert.ToInt32(parameters[0]), list);

            }

        }

    }
}
