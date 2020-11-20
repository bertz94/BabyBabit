using System;
using System.Net;
using Amazon.Lambda.Core;
using System.IO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BabyBabit
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(FunctionInput input, ILambdaContext context)
        {

            try
            {
                DateTime anchor = new DateTime(2020, 8, 18);
                DateTime now = DateTime.Now;
                TimeSpan span = now - anchor;
                if ((span.Days / 7) < 43)
                {
                    SendRequest(GetMonthString, span);
                    SendRequest(GetWeekString, span);
                    SendRequest(GetDayString, span);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        private int GetWeeklyBabyWeigh(int weekNumber)
        {
            switch(weekNumber)
            {
                case 13 : return 28;
                case 14 : return 43;
                case 15 : return 70;
                case 16 : return 100;
                case 17 : return 140;
                case 18 : return 200;
                case 19 : return 240;
                case 20 : return 300;
                case 21 : return 300;
                case 22 : return 450;
                case 23 : return 500;
                case 24 : return 620;
                case 25 : return 680;
                case 26 : return 760;
                case 27 : return 1000;
                case 28 : return 1000;
                case 29 : return 1150;
                case 30 : return 1400;
                case 31 : return 1500;
                case 32 : return 1700;
                case 33 : return 1800;
                case 34 : return 2200;
                case 35 : return 2400;
                case 36 : return 2700;
                case 37 : return 3000;
                default : return -1;
            }
        }

        private void SendRequest(Func<TimeSpan, string> interval, TimeSpan span)
        {
            string intervalString = interval(span);
            if (intervalString != null)
            {
                var request = (HttpWebRequest)WebRequest.Create("http://api.telegram.org/bot1079609971:AAHyvQQBnvJ2X3WGRdjD2ISQ3Ixfd--iXww/sendMessage?chat_id=-449591202&text=" + intervalString);
                var content = string.Empty;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private string GetDayString(TimeSpan span)
        {
            return ((span.Days % 2) == 0) ? "baby" : "babit";
        }
        
        private string GetWeekString(TimeSpan span)
        {
            string week = "Starting week ";
            if(span.Days % 7 == 0) {
                week += span.Days / 7;
                week += " %F0%9F%91%B6";
                int babyWeight = GetWeeklyBabyWeigh(span.Days / 7);
                week += (babyWeight > 0) ? "%0AWe're at about " + babyWeight + " grams" :  "%0Over 3kg!!!";
            }
            else
            {
                week = null;
            }
            return week;
        }

        private string GetMonthString(TimeSpan span)
        {
            return ((span.Days % 28) == 0) ? "Starting month " + span.Days / 28 : null;
        }
    }

    public class FunctionInput
    {
        public string Prop { get; set; }
    }
}
