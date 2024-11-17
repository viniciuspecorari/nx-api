using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using nx_api.Domain.Context;

namespace nx_api.Data.Context
{
    public class NxContext : ContextBase, INXDBContext
    {
        public NxContext(ILogger<NxContext> logger, IConfiguration configuration) : base(logger, configuration)
        {
            
        }
    }
}
