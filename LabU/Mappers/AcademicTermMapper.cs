using LabU.Core;

namespace LabU.Mappers
{
    public class AcademicTermMapper
    {
        public static string Map(AcademicTerms term)
        {
            return term switch
            {
                AcademicTerms.Spring => "Весенний",
                AcademicTerms.Autumn => "Осенний",
                _ => ""
            };
        }
    }
}
