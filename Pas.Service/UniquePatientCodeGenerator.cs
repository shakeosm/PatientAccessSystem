using Pas.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Service
{
    public class UniquePatientCodeGenerator : IUniquePatientCodeGenerator
    {
        /// <summary>
        /// This will generate a Unique key for a Patient- in the PAS app
        /// </summary>
        /// <param name="mobileNumber">Full Mobile number</param>
        /// <param name="districtId">District Id</param>
        /// <returns></returns>
        public string Get(string mobileNumber, int districtId)
        {
            //## Get Operator Code first
            IDictionary<string, string> OperatorList = new Dictionary<string, string>();
            OperatorList.Add("015", "1");   
            OperatorList.Add("016", "2");   //## Robi- Old
            OperatorList.Add("017", "3");   //## Grameen
            OperatorList.Add("018", "4");   //## Robi - New
            OperatorList.Add("019", "5");   //## BanglaLink

            string mobileOperator = mobileNumber.Substring(0, 3);

            var operatorId = OperatorList[mobileOperator];

            //## Get last 4 digits from Mobile
            string mobileLastDigits = mobileNumber.Substring(mobileNumber.Length - 4, 4);


            return $"{operatorId}{districtId.ToString()}{mobileLastDigits}";    //## ie: 1 10 4993, display format: "110-4993"
        }
    }
}
