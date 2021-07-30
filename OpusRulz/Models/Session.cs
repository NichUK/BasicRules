using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpusRulz.Interfaces;

namespace OpusRulz.Models
{
    public class Session : ISession
    {
        public virtual IWorkspace Workspace { get; }

        public Session(IWorkspace workspace)
        {
            Workspace = workspace;
        }
    }
}
