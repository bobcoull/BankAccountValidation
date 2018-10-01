using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using Moq;
using Entities;

namespace ModuleChecksLibrary.Tests.ModuleCheckTests
{
    [TestFixture]
    public class ModulusCheckValidation_Should
    {
        [Test]
        public void Return_False_If_Sort_Code_Is_Null()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation(null, "12345678");

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(false, res.IsSortCodeValid);
        }

        [Test]
        public void Return_False_If_Account_No_Is_Null()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation("123456", null);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(false, res.IsAccountNoValid);
        }

        [Test]
        public void Return_False_If_Sort_Code_Less_Than_6_Digits()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation("99999", "12345678");

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(false, res.IsSortCodeValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_If_Sort_Code_Greater_Than_6_Digits()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation("1000000", "12345678");

            //assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(false, res.IsSortCodeValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_If_Account_No_Less_Than_8_Digits()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation("999999", "9999999");

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(false, res.IsAccountNoValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_If_Account_No_Greater_Than_8_Digits()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation("999999", "100000000");

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
            Assert.AreEqual(false, res.IsAccountNoValid);
        }

        [Test]
        public void Return_Exception_Not_Processed_For_Exception_Other_Than_4_7()
        {
            // assign
            string sortCode = "499273";
            string accountNo = "12345678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 400000,
                SortCodeEnd = 500000,
                ModCheck = "DBLAL",
                WeightU = 2, WeightV = 1, WeightW = 2, WeightX = 1, WeightY = 2, WeightZ = 1,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 2, WeightH = 1,
                Exception = 5
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(5, res.ExceptionNotProcessed.Value);
        }

        [Test]
        public void Return_Exception_Not_Processed_Is_Null_For_Exception_4()
        {
            // assign
            string sortCode = "499273";
            string accountNo = "12345678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 400000,
                SortCodeEnd = 500000,
                ModCheck = "DBLAL",
                WeightU = 2, WeightV = 1, WeightW = 2, WeightX = 1, WeightY = 2, WeightZ = 1,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 2, WeightH = 1,
                Exception = 4
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_Exception_Not_Processed_Is_Null_For_Exception_7()
        {
            // assign
            string sortCode = "499273";
            string accountNo = "12345678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 400000,
                SortCodeEnd = 500000,
                ModCheck = "DBLAL",
                WeightU = 2, WeightV = 1, WeightW = 2, WeightX = 1, WeightY = 2, WeightZ = 1,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 2, WeightH = 1,
                Exception = 7
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_True_For_All_Zero_Value_Weights()
        {
            // assign
            string sortCode = "000000";
            string accountNo = "00000000";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 100000,
                SortCodeEnd = 100000,
                ModCheck = "DBLAL",
                WeightU = 0, WeightV = 0, WeightW = 0, WeightX = 0, WeightY = 0, WeightZ = 0,
                WeightA = 0, WeightB = 0, WeightC = 0, WeightD = 0, WeightE = 0, WeightF = 0, WeightG = 0, WeightH = 0,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(true, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_For_All_Nine_Value_Weights()
        {
            // This is to check all max values do not cause exceptions
            // assign
            string sortCode = "999999";
            string accountNo = "99999999";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 999999,
                SortCodeEnd = 999999,
                ModCheck = "DBLAL",
                WeightU = 9, WeightV = 9, WeightW = 9, WeightX = 9, WeightY = 9, WeightZ = 9,
                WeightA = 9, WeightB = 9, WeightC = 9, WeightD = 9, WeightE = 9, WeightF = 9, WeightG = 9, WeightH = 9,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_True_For_Valid_SortCode_And_AccountNumber_DBLAL()
        {
            // assign
            string sortCode = "499273";
            string accountNo = "12345678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 400000,
                SortCodeEnd = 500000,
                ModCheck = "DBLAL",
                WeightU = 2, WeightV = 1, WeightW = 2, WeightX = 1, WeightY = 2, WeightZ = 1,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 2, WeightH = 1,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(true, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_For_InValid_SortCode_And_AccountNumber_DBLAL()
        {
            // assign
            string sortCode = "499272";
            string accountNo = "12345678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 400000,
                SortCodeEnd = 500000,
                ModCheck = "DBLAL",
                WeightU = 2, WeightV = 1, WeightW = 2, WeightX = 1, WeightY = 2, WeightZ = 1,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 2, WeightH = 1,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_True_For_Valid_SortCode_AccountNumber_MOD11_Exception_7_And_G_9()
        {
            // assign
            string sortCode = "774110";
            string accountNo = "12335978";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD11",
                WeightU = 9, WeightV = 9, WeightW = 9, WeightX = 9, WeightY = 9, WeightZ = 9,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 9, WeightH = 1,
                Exception = 7
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(true, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_True_For_Valid_SortCode_AccountNumber_MOD11()
        {
            // assign
            string sortCode = "774110";
            string accountNo = "12335178";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD11",
                WeightU = 9, WeightV = 9, WeightW = 9, WeightX = 9, WeightY = 9, WeightZ = 9,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 9, WeightH = 1,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(true, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_For_InValid_SortCode_AccountNumber_MOD11()
        {
            // assign
            string sortCode = "774110";
            string accountNo = "12335179";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD11",
                WeightU = 9, WeightV = 9, WeightW = 9, WeightX = 9, WeightY = 9, WeightZ = 9,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 9, WeightH = 1,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_True_For_Valid_SortCode_AccountNumber_MOD10()
        {
            // assign
            string sortCode = "774110";
            string accountNo = "12335678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD10",
                WeightU = 9, WeightV = 9, WeightW = 9, WeightX = 9, WeightY = 9, WeightZ = 9,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 9, WeightH = 1,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(true, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_For_InValid_SortCode_AccountNumber_MOD10()
        {
            // assign
            string sortCode = "774110";
            string accountNo = "12135678";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD10",
                WeightU = 9, WeightV = 9, WeightW = 9, WeightX = 9, WeightY = 9, WeightZ = 9,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 9, WeightH = 1,
                Exception = null
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_True_For_Valid_SortCode_AccountNumber_MOD11_Exception_4_Reminder_Matches_GH()
        {
            //Exception 4:
            // After you have finished the check, ensure that the remainder is the same as the two-digit
            // checkdigit; the checkdigit for exception 4 is gh from the original account number

            // assign
            string sortCode = "774110";
            string accountNo = "12335504";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD11",
                WeightU = 5, WeightV = 8, WeightW = 6, WeightX = 2, WeightY = 3, WeightZ = 4,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 7, WeightH = 8,
                Exception = 4
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(true, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_For_Valid_SortCode_AccountNumber_MOD11_Exception_4_Reminder_Doesnt_Matche_GH()
        {
            //Exception 4:
            // After you have finished the check, ensure that the remainder is the same as the two-digit
            // checkdigit; the checkdigit for exception 4 is gh from the original account number

            // assign
            string sortCode = "774110";
            string accountNo = "12335104";
            Mock<IModulusWeightRepository> modulusWeightRepository = new Mock<IModulusWeightRepository>();

            ModulusWeight modulusWeight = new ModulusWeight
            {
                SortCodeStart = 774100,
                SortCodeEnd = 774599,
                ModCheck = "MOD11",
                WeightU = 5, WeightV = 8, WeightW = 6, WeightX = 2, WeightY = 3, WeightZ = 4,
                WeightA = 2, WeightB = 1, WeightC = 2, WeightD = 1, WeightE = 2, WeightF = 1, WeightG = 7, WeightH = 8,
                Exception = 4
            };

            modulusWeightRepository.Setup(x => x.GetBySortCode(sortCode)).Returns(modulusWeight);

            var modulusCheck = new ModulusCheck(modulusWeightRepository.Object);

            // act
            var res = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }
    }
}
