using Aveva.Platform.Common.DotNet.Attributes;
using Aveva.Platform.Common.DotNet.Extensions;
using Aveva.Platform.Common.Workflow;
using Aveva.Platform.Sds.BackupProcessor.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aveva.Platform.Sds.BackupProcessor.Activities;

/// <inheritdoc/>
[Name(nameof(BackupKubernetesSdsInstance))]
[Description("Activity for backing up Kubernetes SDS Instance specification")]
public class BackupKubernetesSdsInstance(IKubernetesCrdProvider crdProvider, IBackupResourceUploader backupResourceUploader) : SimpleActivity
{
    /// <inheritdoc/>
    protected override async Task<IReadOnlyDictionary<string, object>> ExecuteAsync(IReadOnlyDictionary<string, object> input, CancellationToken cancellationToken)
    {
        string backupId = BackupValues.GetBackupId(input);
        string instanceNamespace = BackupValues.GetKubernetesNamespace(input);

        // Get SDS Instance Custom Resource Definition Specification from Kubernetes
        string sdsInstanceSpec = await crdProvider.GetSdsInstanceCrdAsync(instanceNamespace, instanceNamespace, cancellationToken);

        // Store retrieved instance spec yaml inside backup-record-repository container in Backup Storage Account
        var backedUpSdsInstanceSpec = await backupResourceUploader.UploadResourcesAsync(backupId, "sdsInstance", 
            new Dictionary<string, string>() { { "sdsInstance" , sdsInstanceSpec } }, cancellationToken);

        return backedUpSdsInstanceSpec.ToDictionary(
            kvp => kvp.Key,
            kvp => (object)kvp.Value
        );
    }
}
