using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimFeedback.telemetry.mc
{
    public sealed class MCTelemetryValue : AbstractTelemetryValue
    {

        public MCTelemetryValue(string name, object value)
            : base()
        {
            Name = name;
            Value = value;
        }

        public override object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.Value, this.Unit);
        }
    }
}
