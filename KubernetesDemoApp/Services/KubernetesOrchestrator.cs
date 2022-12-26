using k8s;
using k8s.Models;
using KubernetesDemoApp.K8sConfig;

namespace KubernetesDemoApp.Services
{
    public class KubernetesOrchestrator : IKubernetesOrchestrator
    {
        private readonly ILogger<KubernetesOrchestrator> _logger;
        private readonly Kubernetes _kubernetesClient;
        private readonly KubernetesConfig _kubernetesConfig;

        private const string _namespaceName = "demo-app";
        
        public KubernetesOrchestrator(ILogger<KubernetesOrchestrator> logger, IConfiguration configuration)
        {
            _logger = logger;

            // TODO:: Sort Helm env overrides
            _kubernetesConfig = configuration.GetSection("Kubernetes").Get<KubernetesConfig>();
            KubernetesClientConfiguration config;

            if (_kubernetesConfig.IsLocal)
            {
                // TODO:: Sort this, doesn't work currrently
                _logger.LogInformation("Building configuration from file.");
                config = KubernetesClientConfiguration.BuildConfigFromConfigFile(
                    "../KubernetesDemoApp/K8sConfig/kubeconfig-local.yml");
            }
            else
            {
                _logger.LogInformation("Building in cluster configuration.");
                config = KubernetesClientConfiguration.InClusterConfig();
            }
            
            _kubernetesClient = new k8s.Kubernetes(config);
        }

        public async Task<V1PodList> GetPods()
        {
            try
            {
                return await _kubernetesClient.ListNamespacedPodAsync("demo-app");
            }
            catch (Exception exception)
            {
                _logger.LogError("Error when creating getting K8s: {message}", exception.Message);
            }

            return new V1PodList();
        }

        public async Task CreateJob()
        {
            var id = Guid.NewGuid().ToString();
            var jobName = $"demo-app-job-{id}";
            var jobWorkerName = $"demo-app-worker-job-{id}";

            var job = new V1Job
            {
                Metadata = new V1ObjectMeta
                {
                    Name = jobName
                },
                Spec = new V1JobSpec
                {
                    TtlSecondsAfterFinished = 0,
                    Template = new V1PodTemplateSpec
                    {
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new V1Container
                                {
                                    Name = jobWorkerName,
                                    Image = _kubernetesConfig.JobConsoleImage,
                                    //Args = args,
                                    Env = new List<V1EnvVar>
                                    {
                                        // TODO :: move to config map
                                        //new V1EnvVar("Example-key", Example-value)
                                    }
                                }
                            },
                            RestartPolicy = "Never"
                        }
                    }
                }
            };

            job.Validate();

            try
            {
                _logger.LogDebug("Attempting to create job with name: {jobName}", jobName);
                await _kubernetesClient.CreateNamespacedJobAsync(job, _namespaceName);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error when creating K8s job: {message}", exception.Message);
            }
        }
    }
}
