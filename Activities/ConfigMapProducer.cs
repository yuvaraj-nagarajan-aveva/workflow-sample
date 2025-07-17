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
    internal class ConfigMapProducer : SimpleActivity
    {
        protected async override Task<IReadOnlyDictionary<string, object>> ExecuteAsync(IReadOnlyDictionary<string, object> input, CancellationToken cancellationToken)
        {
            var dict = new Dictionary<string, string> {
                { "CM1", "Data for CM1"},
                { "CM2", "Data for CM2"} };

            return new Dictionary<string, object> { { "ConfigMapsBlob", dict } };
        }
    }
}
