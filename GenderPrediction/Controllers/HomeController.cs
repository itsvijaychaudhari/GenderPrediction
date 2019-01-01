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
            PredictionModel predictionModel = new PredictionModel
            {
                SexList = new List<Sex>
                {
                    new Sex{ID="1" , Type = "Male"},
                    new Sex{ID="2" , Type = "Female"}
                }
            };
            return View(predictionModel);
        }


        //public ActionResult GetGender(PredictionModel gender)
        [HttpPost]
        public JsonResult Predict(PredictionModel gender)
        {
            if (ModelState.IsValid)
            {
                gender.SexList = new List<Sex>
                {
                    new Sex {ID = "1", Type = "Male"},
                    new Sex {ID = "2", Type = "Female"}
                };
                //gender.GenderType = new GenderType();
                //Eventhandle actionToPerform = Predict != null ? Eventhandle.PREDICT : Eventhandle.CANCEL;
                //PredictionModel tempObj = new PredictionModel();
                //tempObj = gender;
                gender = gender.GetPredict(gender);

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

       

        [HttpPost]
        public void SaveCorrectedAns(PredictionModel gender)
        {
            string CorrectPrediction;
            if (gender.Currectedgender == "1")
                CorrectPrediction = "M";
            else
                CorrectPrediction = "F";

            if (gender.Suggestedgender != CorrectPrediction)
            {
                Logger.LogMsg(LogLevel.INFO, gender.Name +" "+gender.Suggestedgender+" Changed To:"+ CorrectPrediction);
                

            }

        }
    }
}