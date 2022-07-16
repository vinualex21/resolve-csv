using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResolveCSV.CustomParser
{
    public static class Assert
    {
        public static void That<T>(bool condition, string msg) where T : Exception, new()
        {
            if (!condition)
            {
                var ex = Activator.CreateInstance(typeof(T), new object[] { msg }) as T;
                throw ex;
            }
        }
    }
}
