using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helper
{
    public static class ErrorExtensions
    {
        public static void AddApplicaitonError(this HttpResponse resposne, string message)
        {
            resposne.Headers.Add("Application-Error", message);
            resposne.Headers.Add("Access-Control-Expose-Headers", "Applicaion-Error");
            resposne.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime)
        {
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}
