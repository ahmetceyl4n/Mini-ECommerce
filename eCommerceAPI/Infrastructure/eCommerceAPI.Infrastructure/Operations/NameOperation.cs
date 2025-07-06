using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.StaticServices
{
    public static class NameOperation
    {
        public static string CharacterRegulatory(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            // Replace Turkish characters with their English equivalents
            name = name.Replace("ç", "c")
                       .Replace("ğ", "g")
                       .Replace("ı", "i")
                       .Replace("ö", "o")
                       .Replace("ş", "s")
                       .Replace("ü", "u")
                       .Replace("Ç", "C")
                       .Replace("Ğ", "G")
                       .Replace("İ", "I")
                       .Replace("Ö", "O")
                       .Replace("Ş", "S")
                       .Replace("Ü", "U");
            // Remove special characters and replace spaces with underscores
            return new string(name.Where(c => char.IsLetterOrDigit(c) || c == ' ').ToArray()).Trim().Replace(' ', '_');
        }
    }
}
