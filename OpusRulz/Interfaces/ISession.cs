using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusRulz.Interfaces
{
    public interface ISession : IDisposable
    {
        IDictionary<string, object> Instances { get; }

        IDictionary<string, object> Outputs { get; }

        IWorkspace Workspace { get; }
 
        void Insert(string name, object instance);

        void Execute();

        void SetOutput(string name, object output);

        T GetOutput<T>(string name);
    }
}
