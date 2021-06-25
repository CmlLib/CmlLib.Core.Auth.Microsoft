using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmlLib.Core.Auth.Microsoft
{
    public interface ICacheManager<T>
    {
        T GetDefaultObject();
        T ReadCache();
        void SaveCache(T obj);
    }
}
