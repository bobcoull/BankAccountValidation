using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Interfaces;
using Entities;

namespace ModuleChecksLibrary
{
    public class ModulusCheck
    {
        private readonly IModulusWeightRepository modulusWeightRepository;

        public ModulusCheck()
        {
            modulusWeightRepository = new ModulusWeightRepository();
        }

        public ModulusCheck(IModulusWeightRepository modulusCheckRepository)
        {
            this.modulusWeightRepository = modulusCheckRepository;
        }

        public ModulusCheckValidationResult ModulusCheckValidation(int sortCode, int accountNo)
        {
            ModulusCheckValidationResult result = new ModulusCheckValidationResult
            {
                IsCheckValid = false,
                ExceptionNotProcessed = null
            };

            // check if parameters are out of range
            if (sortCode < 100000 || sortCode > 999999 || accountNo < 10000000 || accountNo > 99999999)
            {
                return result;
            }

            bool? isExceptionProcessed = null;
            var modulusWeight = modulusWeightRepository.GetBySortCode(sortCode);

            if (modulusWeight != null)
            {
                int modulusValue = (modulusWeight.ModCheck == "MOD11") ? 11 : 10;

                string fullCode = string.Format("{0}{1}", sortCode, accountNo);
                int total = 0;

                PreCheckWeightAdjustmentsForExceptions(modulusWeight, ref isExceptionProcessed);

                string weightDigitsString = modulusWeight.WeightString;
                
                for (var i = 0; i < 14; i++)
                {
                    int fullCodeDigit = int.Parse(fullCode.Substring(i, 1));
                    int weightDigit = int.Parse(weightDigitsString.Substring(i, 1));

                    int digitTotal = fullCodeDigit * weightDigit;

                    if (modulusWeight.ModCheck == "DBLAL")
                    {
                        //add sum of (up to)two digits to total
                        total += (digitTotal / 10) + (digitTotal % 10);
                    }
                    else
                    {
                        // MOD10, MOD11 - just sum up values        
                        total += digitTotal;
                    }
                }

                PostCheckWeightAdjustmentsForExceptions(modulusWeight.ModCheck, modulusWeight.Exception, ref isExceptionProcessed);

                // Check if exception processed
                if (modulusWeight.Exception != null)
                {
                    if (isExceptionProcessed == null || !isExceptionProcessed.Value)
                    {
                        result.ExceptionNotProcessed = modulusWeight.Exception;
                    }
                }

                result.IsCheckValid = ((total % modulusValue) == 0) ? true : false;
                return result;
            }

            return result;
            }

        private void PreCheckWeightAdjustmentsForExceptions(ModulusWeight modulusWeight, ref bool? isExceptionProcessed)
        {
            if (modulusWeight.Exception != null)
            {
                if (modulusWeight.Exception == 7)
                {
                    // Perform the check as specified, except if g = 9 zeroise weighting positions u-b.
                    if (modulusWeight.WeightG == 9)
                    {
                        modulusWeight.WeightU = 0;
                        modulusWeight.WeightV = 0;
                        modulusWeight.WeightW = 0;
                        modulusWeight.WeightX = 0;
                        modulusWeight.WeightY = 0;
                        modulusWeight.WeightZ = 0;
                        modulusWeight.WeightA = 0;
                        modulusWeight.WeightB = 0;
                    }

                    isExceptionProcessed = true;
                }
            }

        }

        private void PostCheckWeightAdjustmentsForExceptions(string modCheck, int? exception, ref bool? isExceptionProcessed)
        {
            if (exception != null)
            {
                if (exception == 4)
                {
                    // Perform the standard modulus 11 check.
                    // After you have finished the check, ensure that the remainder is the same as the two - digit
                    // checkdigit; the checkdigit for exception 4 is gh from the original account number.
                    isExceptionProcessed = true;
                }
            }

        }
    }
}
