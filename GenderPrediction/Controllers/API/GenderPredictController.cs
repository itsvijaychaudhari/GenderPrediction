using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GenderPrediction.Controllers.API
{
    public class GenderPredictController : ApiController
    {
            
        [HttpGet]
        public IHttpActionResult GetGender(string Name)
        {

            return Ok();
        }
       
    }
}
