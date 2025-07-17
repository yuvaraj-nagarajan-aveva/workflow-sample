using Aveva.Platform.Sds.BackupProcessor.Activities;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace uv_playground.Models
{
    /// <summary>
    /// Contents of SDS Backup Record
    /// </summary>
    public class SdsBackupRecordContents()
    {
        /// <summary>
        /// Backup record for Kubernetes Resources
        /// </summary>
        [JsonProperty("k8sResources")]
        public KubernetesResources KubernetesResources;
    }

    /// <summary>
    /// Contains Blob names for the backued up Kubernetes Resources
    /// </summary>
    /// <param name="configMapsBlobLocation">Dictionary with blob names for backed up config maps</param>
    /// <param name="sdsIntanceBlobLocation">Blob name for backed up SDS Instance specification</param>
    public class KubernetesResources(IReadOnlyDictionary<string, string> configMapsBlobLocation, string sdsIntanceBlobLocation)
    {
        /// <summary>
        /// Blob names for backed up Kubernetes config maps
        /// </summary>
        [JsonProperty("configMaps")]
        public IReadOnlyDictionary<string, string> ConfigMapsBlob = configMapsBlobLocation;
        
        /// <summary>
        /// Blob name for SDS Instance specification
        /// </summary>
        [JsonProperty("sdsInstanceSpec")]
        public string SdsInstanceSpecBlob = sdsIntanceBlobLocation;
    }
}
