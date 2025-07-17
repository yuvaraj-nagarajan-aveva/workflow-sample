using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using YamlDotNet.Serialization;

namespace Aveva.Platform.Sds.BackupProcessor.Services;

/// <inheritdoc/>
public class KubernetesConfigMapProvider(IKubernetes kubernetesClient) : IKubernetesConfigMapProvider
{
    /// <summary>
    /// Key used to annotate kubernetes resources that should be backed up.
    /// </summary>
    private const string BackupAnnotationKey = "sds.connect.aveva.com/backup";

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<string, string>> GetConfigMapResourcesAsync(string kubernetesNamespace, CancellationToken cancellationToken)
    {
        var configMaps = await kubernetesClient.CoreV1.ListNamespacedConfigMapAsync(namespaceParameter: kubernetesNamespace, cancellationToken: cancellationToken);
        var result = new Dictionary<string, string>();
        var serializer = new SerializerBuilder().Build();

        foreach (var configMap in configMaps.Items)
        {
            var annotations = configMap.Metadata.Annotations;
            if (annotations != null &&
                annotations.TryGetValue(BackupAnnotationKey, out var backupValue) &&
                string.Equals(backupValue, "true", StringComparison.OrdinalIgnoreCase))
            {
                var yaml = serializer.Serialize(configMap);

                // TODO: GZip the YAML content. Keeping it Yaml here for development simplicity.
                byte[] yamlBytes = System.Text.Encoding.UTF8.GetBytes(yaml);
                await using var compressedYamlStream = new MemoryStream();
                await using (var gzipStream = new System.IO.Compression.GZipStream(
                    compressedYamlStream,
                    System.IO.Compression.CompressionLevel.Optimal, leaveOpen: true))
                {
                    await gzipStream.WriteAsync(yamlBytes, 0, yamlBytes.Length, cancellationToken);
                }

                compressedYamlStream.Position = 0;
                result[configMap.Metadata.Name] = Convert.ToBase64String(compressedYamlStream.ToArray());
            }
        }

        return result;
    }
}
