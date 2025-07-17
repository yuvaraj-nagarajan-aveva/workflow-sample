using Aveva.Platform.Common.DotNet.Extensions;
using Aveva.Platform.Common.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using uv_playground.Models;
using YamlDotNet.Serialization;

namespace uv_playground.Activities
{
    internal class ResourceConsumer : SimpleActivity
    {
        protected override async Task<IReadOnlyDictionary<string, object>> ExecuteAsync(
            IReadOnlyDictionary<string, object> input, CancellationToken cancellationToken)
        {
            var configMaps = input["ConfigMapsBlob"];
            var instanceYaml = input["CRDInstance"];




            return new Dictionary<string, object> {};
        }
    }
}
