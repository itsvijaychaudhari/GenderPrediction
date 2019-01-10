using System;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenderPrediction.Models;
using LibLinearDotNet;
using Newtonsoft.Json;

namespace GenderPrediction.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PredictionModel predictionModel = new PredictionModel();
            //{
            //    SexList = new List<Sex>
            //    {
            //        new Sex{ID="1" , Type = "Male"},
            //        new Sex{ID="2" , Type = "Female"},
            //        new Sex{ID="3",Type="Both"}
            //    }
            //};
            return View(predictionModel);
        }


        //public ActionResult GetGender(PredictionModel gender)
        [HttpGet]
        public JsonResult Predict(string gender)
        {
            if (gender!= string.Empty)
            {
                gender = gender.ToLower();

                PredictionModel Result = GetPrediction_Fusion(gender);
                return Json(Result, JsonRequestBehavior.AllowGet);


                //gender.SexList = new List<Sex>
                //{
                //    new Sex {ID = "1", Type = "Male"},
                //    new Sex {ID = "2", Type = "Female"}

                //};
                //gender.GenderType = new GenderType();
                //Eventhandle actionToPerform = Predict != null ? Eventhandle.PREDICT : Eventhandle.CANCEL;
                //PredictionModel tempObj = new PredictionModel();
                //tempObj = gender;

                //gender = gender.GetPredict(gender);

                //switch (actionToPerform)
                //{
                //    case Eventhandle.PREDICT:
                //        gender = gender.GetPredict(gender);
                //        break;
                //    case Eventhandle.CANCEL:
                //        if (cancel!=null)
                //        {
                //            gender.result = "";
                //            ModelState.Clear();
                //        }

                //        break;
                //}



                //if (gender.genderType =="Female")
                //{
                //    //log here
                //}
                //else if(gender.genderType == "Male")
                //{
                //    //log
                //}
                //gender = JsonConvert.SerializeObject(predictionModel);
                //return View("Index", gender);
                return Json(gender,JsonRequestBehavior.AllowGet);

            }
            //return View("Index", gender);
            return Json(gender, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private PredictionModel GetPrediction_Fusion(string name)
        {
            PredictionModel gender = new PredictionModel();
            string test_data = "";
            ApplicationObject appObj = new ApplicationObject();
            appObj = System.Web.HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject;
            //I
            //Shifted_trigram_model shift = new Shifted_trigram_model(name);
            //test_data = shift.getTestData();
            //string prediction_shifted = shift.predictFusion(appObj.Model, test_data, 75, 78);

            TrigramOnlyModel trigramOnlyModel = new TrigramOnlyModel(name);
            test_data = trigramOnlyModel.getTestData_trigram_only();
            double maleCoeff = 0.524, femaleCoeff = 0.522;
            string prediction_trigram = trigramOnlyModel.predictFusion(appObj.Model, test_data, maleCoeff, femaleCoeff);


            //II
            NeuralNet_model net = new NeuralNet_model(name);
            net.calculateFeaturesForNN();
            string prediction_NN = net.predictFusionSVM_NN(appObj.NeuralNetModel);




            string prediction_shifted_gender ="" ;
            double prediction_shifted_score =0;
            string prediction_NN_gender = "";
            double prediction_NN_score = 0;



            if (prediction_trigram != "Can't predict")
            {
                prediction_shifted_gender = prediction_trigram.Split(' ')[0];
                prediction_shifted_score = double.Parse(prediction_trigram.Split(' ')[1]);

            }
            if (prediction_NN != "")
            {
                prediction_NN_gender = prediction_NN.Split(' ')[0];
                prediction_NN_score = double.Parse(prediction_NN.Split(' ')[1]) * 100;
            }



            if (prediction_shifted_gender == "M" && prediction_NN_gender == "M")
                gender.Suggestedgender = "Male";
            else if (prediction_shifted_gender == "M" && prediction_NN_gender == "Both")
                gender.Suggestedgender = "Male";
            //else if (prediction_shifted_gender == "M" && prediction_NN_gender == "Both")
            //    gender.Suggestedgender = "Male";
            else if (prediction_shifted_gender == "Both" && prediction_NN_gender == "M")
                gender.Suggestedgender = "Male";
            else if (prediction_shifted_gender == "Both" && prediction_NN_gender == "Both")
                gender.Suggestedgender = "Both";
            //else if (prediction_shifted_gender == "MF" && prediction_NN_gender == "FM")
            //    gender.Suggestedgender = "Both";
            else if (prediction_shifted_gender == "Both" && prediction_NN_gender == "F")
                gender.Suggestedgender = "Female";
            //else if (prediction_shifted_gender == "FM" && prediction_NN_gender == "M")
            //    gender.Suggestedgender = "Male";
 
            else if (prediction_shifted_gender == "F" && prediction_NN_gender == "Both")
                gender.Suggestedgender = "Female";
            else if (prediction_shifted_gender == "F" && prediction_NN_gender == "F")
                gender.Suggestedgender = "Female";
            else if (prediction_shifted_gender == "F" && prediction_NN_gender == "M")
            {
                if (prediction_NN_score > prediction_shifted_score)
                    gender.Suggestedgender = "Male";
                else if (prediction_shifted_score > prediction_NN_score)
                    gender.Suggestedgender = "Female";
            }
            else if (prediction_shifted_gender == "M" && prediction_NN_gender == "F")
            {
                if (prediction_NN_score > prediction_shifted_score)
                    gender.Suggestedgender = "Female";
                else if (prediction_shifted_score > prediction_NN_score)
                    gender.Suggestedgender = "Male";
            }
            else if (prediction_trigram == "Can't predict")
            {
                if (prediction_NN_gender == "M")
                    gender.Suggestedgender = "Male";
                else if (prediction_NN_gender == "F")
                    gender.Suggestedgender = "Female";
                else if (prediction_NN_gender == "Both")
                    gender.Suggestedgender = "Both";
            }
            return gender;
        }


        [HttpPost]
        public void SaveCorrectedAns(PredictionModel gender)
        {
            string CorrectPrediction;
            if (gender.Currectedgender == "Male")
                CorrectPrediction = "Male";
            else
                CorrectPrediction = "Female";

            if (gender.Suggestedgender != CorrectPrediction)
            {

                string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = Request.ServerVariables["REMOTE_ADDR"];
                }
                Logger.LogMsg(LogLevel.INFO, gender.Name +"\t"+gender.Suggestedgender+"\t"+ CorrectPrediction+"\t"+ ip +"\t" );
                

            }

        }
    }
}