using k8s.Models;
using KubernetesDemoApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KubernetesDemoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrchestratorController : Controller
    {
        private readonly ILogger<OrchestratorController> _logger;
        private readonly IKubernetesOrchestrator _kubernetesOrchestrator;

        public OrchestratorController(ILogger<OrchestratorController> logger, IKubernetesOrchestrator kubernetesOrchestrator)
        {
            _logger = logger;
            _kubernetesOrchestrator = kubernetesOrchestrator;
        }

        [HttpPost]
        public async Task<HttpStatusCode> CreateJob()
        {
            await _kubernetesOrchestrator.CreateJob();

            return HttpStatusCode.OK;
        }

        [HttpGet]
        public async Task<V1PodList> GetPods()
        {
            return await _kubernetesOrchestrator.GetPods();
        }
    }
}
