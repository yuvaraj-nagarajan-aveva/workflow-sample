using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aveva.Platform.Common.DotNet.Attributes;
using Aveva.Platform.Common.Workflow;
using Aveva.Platform.Sds.BackupProcessor.Services;

namespace Aveva.Platform.Sds.BackupProcessor.Activities;

/// <inheritdoc/>
[Name(nameof(BackupKubernetesConfigMaps))]
[Description("Activity for backing up Kubernetes Config Maps")]
public class BackupKubernetesConfigMaps(IKubernetesConfigMapProvider configMapProvider, IBackupResourceUploader backupResourceUploader) : SimpleActivity
{
    /// <inheritdoc/>
    protected override async Task<IReadOnlyDictionary<string, object>> ExecuteAsync(IReadOnlyDictionary<string, object> input, CancellationToken cancellationToken)
    {
        string backupId = BackupValues.GetBackupId(input);
        string instanceNamespace = BackupValues.GetKubernetesNamespace(input);

        // Get config maps with backup annotation from Kubernetes
        var configMapsToBackup = await configMapProvider.GetConfigMapResourcesAsync(instanceNamespace, cancellationToken);

        // Store retrieved config maps inside backup-record-repository container in Backup Storage Account
        var backedUpConfigMaps = await backupResourceUploader.UploadResourcesAsync(backupId, "configMap", configMapsToBackup, cancellationToken);

        return backedUpConfigMaps.ToDictionary(
            kvp => kvp.Key,
            kvp => (object)kvp.Value
        );
    }
}
