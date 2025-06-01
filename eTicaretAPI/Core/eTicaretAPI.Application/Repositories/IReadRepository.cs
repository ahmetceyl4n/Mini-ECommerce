using eTicaretAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eTicaretAPI.Application.Repositories
{
    public interface IReadRepository<T> : IRepository<T> where T : BaseEntity  //Burada da T'nin class(entity) olduğunu belirtmemiz lazım
    {                               //Bu sınıfta okuma sorgularını yazıyoruz(SELECT)
        IQueryable<T> GetAll(bool tracking = true);    //Sorgu üzerinde çalışmak istiyorsak IQueryable kullanmamız lazım. GetAll ile hangi entity üzerinde çalışıyorsak her şeyi getirir
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);   //Filtreleme yapmak için kullanılacak methodu tanımlıyoruz. SQLdeki where yani.
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);         //Tekil durumlar için kullanılır. Şarta uygun olan ilkini getirir. Öncekisi çoğuldu.
        Task<T> GetByIdAsync(string id, bool tracking = true);       //id'ye uygun olanı getirir.
                                    //Bu sınıfta genellikle bu dört sorgu tanımlanır. İsteğe göre arttırıp azaltılabilinir ama.
                                    //GetSingle ve GetById asyncron çalıştığı için belirtilir.
    }
}


//Tracking DbContext üzerindeki değişikliği takip ederek Fiziksel database'de değişiklik yapmayı sağlar. Ama bunun maliyeti vardır.
//Biz burada bunu default olarak true yaparız ve gerekirse false yaparak gereksiz yerlerde bunu kapatabiliriz.