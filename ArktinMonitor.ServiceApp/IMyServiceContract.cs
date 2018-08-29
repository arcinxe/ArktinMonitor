using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.ServiceApp
{
    public interface IMyServiceContract
    {
        void Start();

        void Stop();

        void SessionChanged(Topshelf.SessionChangedArguments args);
    }
}
