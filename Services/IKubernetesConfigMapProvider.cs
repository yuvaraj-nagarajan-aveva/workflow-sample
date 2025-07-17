using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Aveva.Platform.Sds.BackupProcessor.Services;

/// <summary>  
/// Provides a mechanism to resolve a unique identifier for a backup.  
/// </summary>  
public interface IKubernetesConfigMapProvider
{
    /// <summary>  
    /// Retrieves the unique identifier for the backup.  
    /// </summary>  
    /// <returns>A string representing the backup identifier.</returns>  
    /// <returns>Dictionary with each config map represented as a seprate entry</returns>  
    /// <returns>Dictionary with each config map represented as a seprate entry</returns>  
    Task<IReadOnlyDictionary<string, string>> GetConfigMapResourcesAsync(string kubernetesNamespace, CancellationToken cancellationToken);
}
