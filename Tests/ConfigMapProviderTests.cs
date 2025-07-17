using Aveva.Platform.Sds.BackupProcessor.Services;
using k8s;
using k8s.Autorest;
using k8s.Models;
using Moq;

public class ConfigMapProviderTests
{
    public ConfigMapProviderTests()
    {
    }

    [Fact]
    public async Task GetConfigMapResources_ReturnsOnlyConfigMapsWithBackupAnnotation()
    {
        // Arrange
        var configMaps = new V1ConfigMapList
        {
            Items = new List<V1ConfigMap>
            {
                new (
                    metadata: new V1ObjectMeta(
                        name: "include-in-backup",
                        annotations: new Dictionary<string, string>
                        {
                            { "sds.connect.aveva.com/backup", "true" }
                        }
                    ),
                    data: new Dictionary<string, string>
                    {
                        { "key1", "value1" }
                    }
                ),
                new(
                    metadata: new V1ObjectMeta(
                        name: "do-not-include-in-backup"
                    ),
                    data: new Dictionary<string, string>
                    {
                        { "key1", "value1" }
                    }
                )
            }
        };

        var coreV1Mock = new Mock<ICoreV1Operations>();
        coreV1Mock
            .Setup(c => c.ListNamespacedConfigMapWithHttpMessagesAsync(
                It.IsAny<string>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new HttpOperationResponse<V1ConfigMapList> { Body = configMaps });

        var k8sMock = new Mock<IKubernetes>();
        k8sMock.Setup(k => k.CoreV1).Returns(coreV1Mock.Object);

        var provider = new KubernetesConfigMapProvider(k8sMock.Object);

        // Act
        var result = await provider.GetConfigMapResourcesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>());

        // Assert
        Assert.Single(result);
        Assert.Contains("include-in-backup", result.Keys);
    }
}