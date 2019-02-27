using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGenerateForm.Core.Repository
{
   public interface IRepository<T> where T :class,new()
    {
        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        void SaveChanges();
        List<T> GetAll();

        T Get(object id);


        

    }
}
