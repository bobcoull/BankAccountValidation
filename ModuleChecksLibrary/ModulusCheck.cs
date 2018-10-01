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
    public interface IModulusCheck
    {
        ModulusCheckValidationResult ModulusCheckValidation(string sortCode, string accountNo);
    }

    public class ModulusCheck : IModulusCheck
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

        public ModulusCheckValidationResult ModulusCheckValidation(string sortCode, string accountNo)
        {
            ModulusCheckValidationResult result = new ModulusCheckValidationResult
            {
                IsCheckValid = false,
                ExceptionNotProcessed = null,
                IsAccountNoValid = true,
                IsSortCodeValid = true
            };

            // check if parameters are valid
            if (sortCode == null)
            {
                result.IsSortCodeValid = false;
            }

            if (accountNo == null)
            {
                result.IsAccountNoValid = false;
            }

            int sortCodeValidated;
            int accountNoValidated;

            if (!int.TryParse(sortCode, out sortCodeValidated))
            {
                result.IsSortCodeValid = false;
            }

            if (!int.TryParse(accountNo, out accountNoValidated))
            {
                result.IsAccountNoValid = false;
            }

            if (sortCode != null && sortCode.Length != 6)
            {
                result.IsSortCodeValid = false;
            }

            if (accountNo != null && accountNo.Length != 8)
            {
                result.IsAccountNoValid = false;
            }

            if (!result.IsSortCodeValid || !result.IsAccountNoValid)
            {
                return result;
            }

            result.IsSortCodeValid = true;
            result.IsAccountNoValid = true;

            bool? isExceptionProcessed = null;
            var modulusWeight = modulusWeightRepository.GetBySortCode(sortCode);

            if (modulusWeight != null)
            {
                int modulusValue = (modulusWeight.ModCheck == "MOD11") ? 11 : 10;

                string fullCode = string.Format("{0}{1}", sortCode, accountNo);
                int total = 0;

                PreCheckWeightAdjustmentsForExceptions(modulusWeight, ref isExceptionProcessed);

                //TODO: WeightString belowe assumes weight numbers are single digits.
                // However some are more than one digit and some are negative.
                // Change this to be an array of ints to solve the problem

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

                result.Remainder = total % modulusValue;

                int expectedRemainder = 0;

                PostCheckResultForExceptions(modulusWeight, fullCode, ref expectedRemainder, ref isExceptionProcessed);

                result.IsCheckValid = (result.Remainder == expectedRemainder) ? true : false;

                // Check if exception processed
                if (modulusWeight.Exception != null)
                {
                    if (isExceptionProcessed == null || !isExceptionProcessed.Value)
                    {
                        result.ExceptionNotProcessed = modulusWeight.Exception;
                    }
                }

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

        private void PostCheckResultForExceptions(ModulusWeight modulusWeight, string fullCode, ref int expectedRemainder, ref bool? isExceptionProcessed)
        {
            if (modulusWeight.Exception != null)
            {
                if (modulusWeight.Exception == 4)
                {
                    // Perform the standard modulus 11 check.
                    // After you have finished the check, ensure that the remainder is the same as the two - digit
                    // checkdigit; the checkdigit for exception 4 is gh from the original account number.

                    expectedRemainder = int.Parse(fullCode.Substring(12, 2));
                    isExceptionProcessed = true;
                }
            }
        }
    }
}
