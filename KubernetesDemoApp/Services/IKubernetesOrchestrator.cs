using k8s.Models;

namespace KubernetesDemoApp.Services
{
    public interface IKubernetesOrchestrator
    {
        Task CreateJob();
        Task<V1PodList> GetPods();
    }
}
