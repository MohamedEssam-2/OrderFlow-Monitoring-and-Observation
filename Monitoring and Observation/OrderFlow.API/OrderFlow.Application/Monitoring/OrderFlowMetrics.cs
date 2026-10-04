using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Application.Monitoring
{
    public class OrderFlowMetrics
    {
        public const string MeterName = "OrderFlow";

        private readonly Meter _meter;

        public Counter<long> OrdersCreated { get; }

        public ObservableGauge<int> PendingOrders { get; }

        public Counter<long> WorkerExecutions { get; }

        public Counter<long> WorkerProcessedOrders { get; }

        private int _pendingOrders;

        public OrderFlowMetrics()
        {
            _meter = new Meter(MeterName, "1.0.0");

            OrdersCreated = _meter.CreateCounter<long>("orderflow.orders.created",
                description: "Number of orders created.");

            WorkerExecutions = _meter.CreateCounter<long>("orderflow.worker.executions",
                description: "Number of background worker executions.");

            WorkerProcessedOrders = _meter.CreateCounter<long>("orderflow.worker.processed_orders",
                description: "Number of orders processed by the background worker.");

            PendingOrders = _meter.CreateObservableGauge("orderflow.orders.pending",
                () => _pendingOrders,
                description: "Current number of pending orders.");
        }

        public void SetPendingOrders(int count)
        {
            _pendingOrders = count;
        }
    }
}
