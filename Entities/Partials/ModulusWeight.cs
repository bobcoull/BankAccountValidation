
namespace Entities
{
    public partial class ModulusWeight
    {
        public string WeightString
        {
            get
            {
                return string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}",
                    WeightU, WeightV, WeightW, WeightX, WeightY, WeightZ,
                    WeightA, WeightB, WeightC, WeightD, WeightE, WeightF, WeightG, WeightH);
            }
        }
    }
}
