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
using uv_playground.Activities;
using static Aveva.Platform.Common.Workflow.Definitions.ActivityDefinitionFactory;

namespace uv_playground;

internal class SampleWorkflow
{
    internal async Task<string> ExecuteWorkflowAsync(IHost host)
    {
        string workflowId = "wf-" + DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
        var engine = host.Services.GetRequiredService<IWorkflowEngine<ActivityExecutionDefinition>>();
        await engine.StartAsync();
        IWorkflow workflow;
        try
        {
            workflow = await engine.GetWorkflowAsync(workflowId);
        }
        catch (KeyNotFoundException)
        {
            var definition = GetWorkflowpActivityDefinition(workflowId);
            workflow = await engine.StartWorkflowExecutionAsync(definition, null);
        }

        await workflow.WaitForCompletionAsync<long>(TimeSpan.FromHours(4));
        await engine.StopAsync();
        return workflowId;
    }

    internal ActivityDefinition GetWorkflowpActivityDefinition(string workflowId)
    {
        return CreateSequential($"{workflowId}",
            Create<ConfigMapProducer>(out string configMapProducer,
                CreateConstantBinding("ConstantParam1", "ParamValue")),
            Create<CRDProducer>(out string crdProducer,
                CreateConstantBinding("ConstantParam1", "ParamValue")),
            Create<ResourceConsumer>(
                CreateConstantBinding("ConstantParam2", "ParamValue"),
                CreateActivityResultBinding("ConfigMapsBlob", configMapProducer, "ConfigMapsBlob"),
                CreateActivityResultBinding("CRDInstance", crdProducer, "CRDInstance")));
    }
}
