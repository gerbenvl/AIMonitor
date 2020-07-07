using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AIMonitor
{
    public interface IAIInstanceChecker
    {
        IEnumerable<AppStatus> CheckAiInstances(AIInstanceSetting[] aiInstancesToCheck);
    }
}
