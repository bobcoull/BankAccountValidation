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
        public void Return_False_If_Sort_Code_Less_Than_100000()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation(99999, 12345678);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_If_Sort_Code_Greater_Than_999999()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation(1000000, 12345678);

            //assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_If_Account_No_Less_Than_10000000()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation(999999, 9999999);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_False_If_Account_No_Greater_Than_99999999()
        {
            // assign
            var modulusCheck = new ModulusCheck();

            // act
            var res = modulusCheck.ModulusCheckValidation(999999, 100000000);

            // assert
            Assert.AreEqual(false, res.IsCheckValid);
            Assert.AreEqual(null, res.ExceptionNotProcessed);
        }

        [Test]
        public void Return_Exception_Not_Processed_For_Exception_Other_Than_4_7()
        {
            // assign
            int sortCode = 499273;
            int accountNo = 12345678;
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
            int sortCode = 499273;
            int accountNo = 12345678;
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
            int sortCode = 499273;
            int accountNo = 12345678;
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
            int sortCode = 100000;
            int accountNo = 10000000;
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
            int sortCode = 999999;
            int accountNo = 99999999;
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
            int sortCode = 499273;
            int accountNo = 12345678;
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
            int sortCode = 499272;
            int accountNo = 12345678;
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
            int sortCode = 774110;
            int accountNo = 12335978;
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
            int sortCode = 774110;
            int accountNo = 12335178;
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
            int sortCode = 774110;
            int accountNo = 12335179;
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
            int sortCode = 774110;
            int accountNo = 12335678;
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
            int sortCode = 774110;
            int accountNo = 12135678;
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

    }
}
