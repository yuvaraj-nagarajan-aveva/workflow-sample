using Aveva.Platform.Common.DotNet.Extensions;
using System.Collections.Generic;

/// <summary>
/// Values pertaining to Backup
/// </summary>
internal static class BackupValues
{
    internal const string InstanceUniqueIdKey = "InstanceUniqueId";
    internal const string BackupIdKey = "BackupId";
    internal const string BackupStatusKey = "BackupStatus";
    internal const string BackupRecordKey = "BackupRecord";
    internal const string StoragePartitionCountKey = "StoragePartitionCount";
    internal const string ContainerBackupSnapshotsKey = "ContainerBackupSnapshots";
    internal const string ContainerNameKey = "ContainerName";
    internal const string BlobCopySnapshotsKey = "BlobCopySnapshots";

    internal static string GetInstanceUniqueId(IReadOnlyDictionary<string, object> values) => values.GetValue<string>(InstanceUniqueIdKey);
    internal static string GetBackupId(IReadOnlyDictionary<string, object> values) => values.GetValue<string>(BackupIdKey);
    internal static string GetKubernetesNamespace(IReadOnlyDictionary<string, object> values) => GetKubernetesNamespace(GetInstanceUniqueId(values));
    internal static string GetKubernetesNamespace(string instanceUniqueId) => $"sds-{instanceUniqueId}";
}
