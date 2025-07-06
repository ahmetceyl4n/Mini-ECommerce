using eTicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Domain.Entities
{
    public class File : BaseEntity
    {
        [NotMapped]  // NotMapped özelliği, bu özelliğin veritabanında bir sütun olarak saklanmamasını sağlar.
        public override DateTime UpdatedDate { get => base.UpdatedDate; set => base.UpdatedDate = value; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }     
}
