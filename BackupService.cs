using Aveva.Platform.Common.Workflow;
using Aveva.Platform.Common.Workflow.Definitions;
using Aveva.Platform.Sds.BackupProcessor.Activities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aveva.Platform.Common.Workflow.Definitions.ActivityDefinitionFactory;

namespace uv_playground;

internal class BackupService
{
    internal async Task<string> ExecuteWorkflowAsync(IHost host)
    {
        string backupId = "backup-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
        var engine = host.Services.GetRequiredService<IWorkflowEngine<ActivityExecutionDefinition>>();
        await engine.StartAsync();
        IWorkflow workflow;
        try
        {
            workflow = await engine.GetWorkflowAsync(backupId);
        }
        catch (KeyNotFoundException)
        {
            var definition = GetBackupActivityDefinition("f1f60f72-ec8b-403c-b0e6-460e3a241751", backupId, 1);
            workflow = await engine.StartWorkflowExecutionAsync(definition, null);
        }

        await workflow.WaitForCompletionAsync<long>(TimeSpan.FromHours(4));
        await engine.StopAsync();
        return backupId;
    }

    internal ActivityDefinition GetBackupActivityDefinition(string instanceUniqueId, string backupId, int storagePartitionCount)
    {
        return CreateSequential($"BackupSdsInstance-{instanceUniqueId}-{backupId}",
            Create<BackupKubernetesConfigMaps>(
                CreateConstantBinding(BackupValues.BackupIdKey, backupId),
                CreateConstantBinding(BackupValues.InstanceUniqueIdKey, instanceUniqueId)),
            Create<BackupKubernetesSdsInstance>(
                CreateConstantBinding(BackupValues.BackupIdKey, backupId),
                CreateConstantBinding(BackupValues.InstanceUniqueIdKey, instanceUniqueId)
                ));
    }
}
