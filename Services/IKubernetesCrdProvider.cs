using System.Threading;
using System.Threading.Tasks;

namespace Aveva.Platform.Sds.BackupProcessor.Services;

/// <summary>  
/// Provides a mechanism to retrieve the SDS instance Custom Resource Definition (CRD) from SDS instance namespace  
/// </summary>  
public interface IKubernetesCrdProvider
{
    /// <summary>  
    /// Retrieves the SDS instance Custom Resource Definiiton.  
    /// </summary>  
    /// <returns>A string representing the SDS instance CRD instance yaml.</returns>  
    Task<string> GetSdsInstanceCrdAsync(string crdInstanceName, string kubernetesNamespace, CancellationToken cancellationToken);
}
