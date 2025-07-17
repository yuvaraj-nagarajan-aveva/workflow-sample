using Aveva.Platform.Common.DotNet.Extensions;
using Aveva.Platform.Common.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace uv_playground.Activities
{
    internal class CRDProducer : SimpleActivity
    {
        protected async override Task<IReadOnlyDictionary<string, object>> ExecuteAsync(IReadOnlyDictionary<string, object> input, CancellationToken cancellationToken)
        {
            var instanceCrd = "yaml for crd";

            return new Dictionary<string, object> { { "CRDInstance", instanceCrd } };
        }
    }
}
