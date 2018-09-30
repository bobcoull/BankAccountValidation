using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using DataAccess.Interfaces;
using Entities;

namespace DataAccess
{
    public class ModulusWeightRepository : IModulusWeightRepository
    {
        public ModulusWeight GetBySortCode(string sortCode)
        {
            string root = HostingEnvironment.ApplicationPhysicalPath;
            string sortCodeString = string.Format("{0:000000}", sortCode);

            string filename = root + @"\ModulusCheckData.txt";
            foreach (var line in File.ReadAllLines(filename))
            {
                string startSortCode = line.Substring(0, 6);
                string endSortCode = line.Substring(7, 6);

                if (string.Compare(sortCodeString, startSortCode) >= 0 && string.Compare(sortCodeString, endSortCode) <= 0)
                {
                    ModulusWeight modulusWeight = new ModulusWeight();

                    modulusWeight.SortCodeStart = int.Parse(line.Substring(0, 6));
                    modulusWeight.SortCodeEnd = int.Parse(line.Substring(7, 6));
                    modulusWeight.ModCheck = line.Substring(14, 5);
                    modulusWeight.WeightU = int.Parse(line.Substring(19, 5));
                    modulusWeight.WeightV = int.Parse(line.Substring(24, 5));
                    modulusWeight.WeightW = int.Parse(line.Substring(29, 5));
                    modulusWeight.WeightX = int.Parse(line.Substring(34, 5));
                    modulusWeight.WeightY = int.Parse(line.Substring(39, 5));
                    modulusWeight.WeightZ = int.Parse(line.Substring(44, 5));
                    modulusWeight.WeightA = int.Parse(line.Substring(49, 5));
                    modulusWeight.WeightB = int.Parse(line.Substring(54, 5));
                    modulusWeight.WeightC = int.Parse(line.Substring(59, 5));
                    modulusWeight.WeightD = int.Parse(line.Substring(64, 5));
                    modulusWeight.WeightE = int.Parse(line.Substring(69, 5));
                    modulusWeight.WeightF = int.Parse(line.Substring(74, 5));
                    modulusWeight.WeightG = int.Parse(line.Substring(79, 5));
                    modulusWeight.WeightH = int.Parse(line.Substring(84, 5));

                    if (line.Length >= 93)
                    {
                        modulusWeight.Exception = int.Parse(line.Substring(89, 4));
                    }
                    else
                    {
                        modulusWeight.Exception = null;
                    }

                    return modulusWeight;
                }
            }

            return null;
        }
    }
}
