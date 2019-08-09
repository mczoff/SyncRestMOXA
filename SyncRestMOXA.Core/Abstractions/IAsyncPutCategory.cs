using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncRestMOXA.Core.Abstractions
{
    public interface IAsyncPutCategory<TModel>
    {
        Task PutAsync(TModel model);
    }
}
