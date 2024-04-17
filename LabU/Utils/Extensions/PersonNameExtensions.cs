using LabU.Core.Entities;

namespace LabU.Utils.Extensions;

public static class PersonNameExtensions
{
    private static string ShortName(string? lastName, string? firstName, string? middleName)
    {
        return $"{lastName} {(!string.IsNullOrEmpty(firstName) ? firstName[0] + '.' : "")} {(!string.IsNullOrEmpty(middleName) ? middleName[0] + '.' : "")}";
    }
    
    private static string FullName(string lastName, string? firstName, string? middleName)
    {
        return $"{lastName} {firstName ?? ""} {middleName ?? ""}".Trim();
    }

    public static string ToShortName(this BasePersonEntity entity)
    {
        return ShortName(entity.LastName, entity.FirstName, entity.MiddleName);
    } 
    
    public static string ToFullName(this BasePersonEntity entity)
    {
        return FullName(entity.LastName, entity.FirstName, entity.MiddleName);
    } 
    
    public static IEnumerable<string> ToShortNames(this IEnumerable<BasePersonEntity> entities)
    {
        return entities.Select(ToShortName).ToArray();
    }
    
    public static IEnumerable<string> ToFullNames(this IEnumerable<BasePersonEntity> entities)
    {
        return entities.Select(ToFullName).ToArray();
    }
}