using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aveva.Platform.Sds.Backup;
using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Aveva.Platform.Sds.BackupProcessor.Services;

/// <inheritdoc/>
public class BackupResourceUploader([FromKeyedServices("backup")] BlobServiceClient blobServiceClient, ILogger<BackupResourceUploader> logger) : IBackupResourceUploader
{
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly IAsyncPolicy _retryPolicy = BlobClientRetryPolicy.Create(logger);

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<string, string>> UploadResourcesAsync(string backupId, string resourceType, IReadOnlyDictionary<string, string> resources, CancellationToken cancellationToken)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient("backup-record-repository");
        await _retryPolicy.ExecuteAsync(async () => await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken));
        var uploadedBlobs = new Dictionary<string, string>();
        foreach (var kvp in resources)
        {
            string blobName = $"{backupId}/k8sResources/{resourceType}/{kvp.Key}";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            byte[] contentBytes = Convert.FromBase64String(kvp.Value);
            using var stream = new MemoryStream(contentBytes);
            await _retryPolicy.ExecuteAsync(async () => await blobClient.UploadAsync(stream, overwrite: true, cancellationToken));
            uploadedBlobs.Add(kvp.Key, blobName);
        }

        return uploadedBlobs;
    }
}
