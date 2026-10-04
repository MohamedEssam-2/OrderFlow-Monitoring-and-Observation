using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OrderFlow.Application.Monitoring;

public static class OrderFlowTracing
{
    public const string SourceName = "OrderFlow";

    public static readonly ActivitySource ActivitySource =new(SourceName, "1.0.0");
}
