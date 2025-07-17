using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Aveva.Platform.Sds.Kubernetes.Communication.KubernetesSpecs;
using k8s;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using YamlDotNet.Serialization;

namespace Aveva.Platform.Sds.BackupProcessor.Services;

/// <inheritdoc/>
public class KubernetesCrdProvider(IKubernetes kubernetesClient) : IKubernetesCrdProvider
{
    /// <inheritdoc/>
    public async Task<string> GetSdsInstanceCrdAsync(string crdInstanceName, string kubernetesNamespace, CancellationToken cancellationToken)
    {
        var response = await kubernetesClient.CustomObjects.GetNamespacedCustomObjectAsync<JsonElement>(
                group: V2SdsInstance.KubeGroup,
                version: V2SdsInstance.KubeApiVersion,
                namespaceParameter: kubernetesNamespace,
                plural: V2SdsInstance.KubePluralName,
                name: crdInstanceName,
                cancellationToken: cancellationToken);

        dynamic deserializedObject = JsonConvert.DeserializeObject<ExpandoObject>(response.ToString(), new ExpandoObjectConverter());

        // Wrap the spec as the root element
        var sdsInstanceSpec = new Dictionary<string, object>
        {
            { "spec", deserializedObject.spec },
        };
        var sdsInstanceSpecYaml = new Serializer().Serialize(sdsInstanceSpec);
        return sdsInstanceSpecYaml;
    }
}
