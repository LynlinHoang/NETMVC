using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020056.DataLayers
{
    public interface ICommonDAL<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        IList<T> List(int page=1, int pageSize=0, string searchValue= "");
        int Count(string searchValue="");
        int Add(T data);
        bool Update(T data);
        bool Delete(int id);
        T? Get(int id);
        bool IsUsed(int id);

    }
}
