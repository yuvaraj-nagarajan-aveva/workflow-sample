using Aveva.Platform.Common.Workflow;
using Aveva.Platform.Common.Workflow.Definitions;
using Aveva.Platform.Common.Workflow.Engine;
using Aveva.Platform.Sds.BackupProcessor.Activities;
using Aveva.Platform.Sds.BackupProcessor.Services;
using Azure.Storage.Blobs;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using uv_playground;
using uv_playground.Activities;
using uv_playground.Models;

BlobServiceClient cc = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=b6dbackup1254043140;AccountKey=uZnrkfVQK4DsyiJKvBTu88DDBoGPv/AEFffe4Y1JUsCA/KT/SG4c+HXCtkZyb+vWulfR9cUeemq/+AStDWl6sg==;BlobEndpoint=https://b6dbackup1254043140.z7.blob.storage.azure.net/;FileEndpoint=https://b6dbackup1254043140.z7.file.storage.azure.net/;TableEndpoint=https://b6dbackup1254043140.z7.table.storage.azure.net/;QueueEndpoint=https://b6dbackup1254043140.z7.queue.storage.azure.net/");

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Register your services here
        services.AddKeyedSingleton("backup", cc);
        services.AddTransient<IGreeter, Greeter>();
        services.AddSingleton<IKubernetes>(new Kubernetes(KubernetesClientConfiguration.BuildDefaultConfig()));
        services.AddSingleton<IKubernetesConfigMapProvider, KubernetesConfigMapProvider>();
        services.AddSingleton<IKubernetesCrdProvider, KubernetesCrdProvider>();
        services.AddSingleton<IBackupResourceUploader, BackupResourceUploader>();
        services.AddWorkflowEngine();
        services.AddSingleton<BackupService>();
        services.AddSingleton<SampleWorkflow>();
    })
    .Build();

var jsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    MaxDepth = 10
};

var jsonSerializerSettings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    NullValueHandling = NullValueHandling.Ignore,
};

SdsBackupRecordContents recordContents = new() { 
    KubernetesResources = new(new Dictionary<string, string>() { {"CM1", "Blob1" } }, "SpecBlob1")
};

string g2 = JsonConvert.SerializeObject(recordContents, jsonSerializerSettings);
string backupId = Guid.NewGuid().ToString();
string instanceNamespace = "sds-f1f60f72-ec8b-403c-b0e6-460e3a241751";
CancellationToken cancellationToken = new CancellationToken();
//var crdProvider = host.Services.GetRequiredService<IKubernetesCrdProvider>();
//var instanceYaml = await crdProvider.GetSdsInstanceCrdAsync(instanceNamespace, instanceNamespace, cancellationToken);

//var configMapProvider = host.Services.GetRequiredService<IKubernetesConfigMapProvider>();
//var op = await configMapProvider.GetConfigMapResourcesAsync(instanceNamespace, cancellationToken);

//// Upload the config maps to Azure Blob Storage
//var backupResourceUploader = host.Services.GetRequiredService<IBackupResourceUploader>();
//var bkOp = await backupResourceUploader.UploadResourcesAsync(backupId, "configmap", op, cancellationToken);
//var greeter = host.Services.GetRequiredService<IGreeter>();
//greeter.Greet();

// Use DI to get WorkflowStartup
//var workflowStartup = host.Services.GetRequiredService<BackupService>();
//workflowStartup.ExecuteWorkflowAsync(host).GetAwaiter().GetResult();

var workflowSample = host.Services.GetRequiredService<SampleWorkflow>();
workflowSample.ExecuteWorkflowAsync(host).GetAwaiter().GetResult();