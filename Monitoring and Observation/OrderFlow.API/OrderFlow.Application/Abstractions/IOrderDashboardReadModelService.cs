using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Abstractions
{
    public interface IOrderDashboardReadModelService
    {
        Task RefreshAsync(CancellationToken cancellationToken = default);
    }
}
