using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SimFeedback.telemetry.mc
{
    
    
    
    
    public sealed class MCTelemetryInfo : EventArgs, TelemetryInfo
    {
        private readonly MCAPI _telemetryData;
        private readonly MCAPI _lastTelemetryData;

        public MCTelemetryInfo(MCAPI telemetryData, MCAPI lastTelemetryData)
        {
            _telemetryData = telemetryData;
            _lastTelemetryData = lastTelemetryData;
        }

        private float Rumble
        {
            get
            {
                if (_lastTelemetryData.SuspensionTravel == null) return 0;

                const float x = 1000.0f;
                float[] data =
                    {
                    _telemetryData.SuspensionTravel[0] - _lastTelemetryData.SuspensionTravel[0],
                    _telemetryData.SuspensionTravel[1] - _lastTelemetryData.SuspensionTravel[1],
                    _telemetryData.SuspensionTravel[2] - _lastTelemetryData.SuspensionTravel[2],
                    _telemetryData.SuspensionTravel[3] - _lastTelemetryData.SuspensionTravel[3]
                };
                return data.Max() * x;
            }
        }

        public TelemetryValue TelemetryValueByName(string name)
        {
            object data;

            TelemetryValue tv;
            switch (name)
            {
                case "Rumble":
                    data = Rumble;
                    break;

                default:
                    int arrayIndexPos = -1;
                    int squareBracketPos = name.IndexOf('[');
                    if (squareBracketPos != -1)
                    {
                        int.TryParse(name.Substring(squareBracketPos + 1, 1), out arrayIndexPos);
                        name = name.Substring(0, squareBracketPos);
                    }
                    Type eleDataType = typeof(MCAPI);
                    PropertyInfo propertyInfo;
                    FieldInfo fieldInfo = eleDataType.GetField(name);
                    if (fieldInfo != null)
                    {
                        data = fieldInfo.GetValue(_telemetryData);
                        if (arrayIndexPos != -1 && data.GetType().IsArray)
                        {
                            float[] array = (float[])data;
                            data = array[arrayIndexPos];
                        }

                    }
                    else if ((propertyInfo = eleDataType.GetProperty(name)) != null)
                    {
                        data = propertyInfo.GetValue(_telemetryData, null);
                    }
                    else
                    {
                        throw new UnknownTelemetryValueException(name);
                    }
                    break;
            }

            tv = new MCTelemetryValue(name, data);
            object value = tv.Value;
            if (value == null)
            {
                throw new UnknownTelemetryValueException(name);
            }
            return tv;
        }
    }
}
