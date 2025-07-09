using eCommerceAPI.Infrastructure.StaticServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Services.Storage
{
    public class Storage
    {
        protected delegate bool HasFile(string pathOrContainerName, string fileName);  // HasFile delegate, hangi concrete storage sınıfının kullanıldığını bilmeden, dosyanın var olup olmadığını kontrol etmek için kullanılır.(Local ve Azure'da mesela farklı şekilde kontrol edilmeli)
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool isFirst = true, int index = 0)
        {
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string newFileName;

            if (isFirst)
            {
                fileNameWithoutExtension = NameOperation.CharacterRegulatory(fileNameWithoutExtension);
                newFileName = $"{fileNameWithoutExtension}{fileExtension}";
            }
            else
            {
                newFileName = $"{fileNameWithoutExtension}({index}){fileExtension}";
            }

            // if (File.Exists(Path.Combine(path, newFileName)))
            if(hasFileMethod(pathOrContainerName, newFileName)) // hasFileMethod, LocalStorage ve AzureStorage'da farklı şekilde implement edilecektir.
            {
                return await FileRenameAsync(pathOrContainerName, fileName, hasFileMethod, false, index + 1);
            }

            return newFileName;
        }
    }
}
