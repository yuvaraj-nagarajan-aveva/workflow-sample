using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aveva.Platform.Sds.BackupProcessor.Services;

/// <summary>
/// Provides a mechanism to upload backup resources (such as config maps, sds instance crd yaml)
/// to azure storage location, with each resource stored as a separate entry.
/// </summary>
public interface IBackupResourceUploader
{
    /// <summary>
    /// Uploads the provided resources to azure backup record repository.
    /// Each entry in the dictionary represents a separate resource to be uploaded.
    /// </summary>
    /// <param name="backupId">Backup Id</param>
    /// <param name="resourceType">The type of resource being uploaded (e.g., "ConfigMap").</param>
    /// <param name="resources">A dictionary where each key is the resource name and the value is the resource content.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>Dictionary with resource name as key and blob name as value</returns>
    Task<IReadOnlyDictionary<string, string>> UploadResourcesAsync(string backupId, string resourceType, IReadOnlyDictionary<string, string> resources, CancellationToken cancellationToken);
}
