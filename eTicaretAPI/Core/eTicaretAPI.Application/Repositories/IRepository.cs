using eTicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Application.Repositories
{
    public interface IRepository<T> where T : BaseEntity   //Repository Design Pattern. Birden çok database kullanıyosak kullanmak mantıklı. İstediğimiz tipte sorgu atmayı falan sağlayan genel bir yapı
    {                                                 //Burada T ile generic uygulanır. Bu sayede hangi entity ile çalıştığımız bilgisini alır ona göre işlem yaparız
        DbSet<T> Table {  get; }                     //DbSet'te T'yi kullanmak için T'nin class(entity) olduğunu belirtmemiz lazım. Bu yüzden yukarıda where yapısını kullandık. Ve DbSet ile table'ı çektik
    }
}
//Merhaba