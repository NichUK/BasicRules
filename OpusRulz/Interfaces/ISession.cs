using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusRulz.Interfaces
{
    public interface ISession
    {
        IWorkspace Workspace { get; }
    }
}
