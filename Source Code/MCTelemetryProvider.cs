using SharpDX.DirectInput;
using SimFeedback.log;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace SimFeedback.telemetry.mc
{
    public sealed class MCTelemetryProvider : AbstractTelemetryProvider
    {
        private bool isStopped = true;
        private Thread t;

        public bool IsMotionEnabled { get; set; } = false;
        
        public DirectInput directInput;
        
        private Joystick joystick = null;
        private Guid controllerGuid = Guid.Empty;

        private ControllerForm controllerForm;
        
        public MCTelemetryProvider() : base()
        {
            Author = "S4nder & HoiHman";
            Version = "v0.1.0";
            BannerImage = @"img\banner_mc.png";
            IconImage = @"img\mc.jpg";
            TelemetryUpdateFrequency = 100;
            //MessageBox.Show(System.AppContext.BaseDirectory);
            //Assembly.LoadFile(@".\provider\motioncontroller\WindowsInput.dll");
            //Assembly.LoadFile(@".\provider\motioncontroller\SharpDX.dll");
            //Assembly.LoadFile(@".\provider\motioncontroller\SharpDX.DirectInput.dll");
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve1;
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve2;
            //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve3;
            directInput = new DirectInput();
        }

        public override string Name { get { return "mc"; } }

        public override void Init(ILogger logger)
        {
            base.Init(logger);
            Log("Initializing MCTelemetryProvider");
            controllerForm = new ControllerForm(this);
            //controllerForm.Visible = false;
            new Thread(new ThreadStart(() => Application.Run(controllerForm))).Start();
        }

        public override string[] GetValueList()
        {
            return GetValueListByReflection(typeof(MCAPI));
        }
        
        public override void Start()
        {
            if (isStopped)
            {
                LogDebug("Starting MCTelemetryProvider");
                isStopped = false;
                t = new Thread(Run);
                t.Start();
                if (controllerGuid != Guid.Empty)
                {
                    joystick = new Joystick(directInput, controllerGuid);
                }
            }
            controllerForm.Show();
        }

        public override void Stop()
        {
            if (joystick != null)
            {
                joystick.Dispose();
            }
            LogDebug("Stopping MCTelemetryProvider");
            isStopped = true;
            if (t != null) t.Join();
            controllerForm.Hide();
        }
        
        private void Run()
        {
            MCAPI lastTelemetryData = new MCAPI();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (!isStopped)
            {
                try
                {
                    MCAPI telemetryData = new MCAPI();//(MCAPI)readSharedMemory(typeof(MCAPI), sharedMemoryFile);

                    Guid key = controllerForm.selectedJoystick.Key;
                    if (controllerGuid != key)
                    {
                        if (key == Guid.Empty)
                        {
                            if (joystick != null)
                            {
                                joystick.Dispose();
                                joystick = null;
                            }
                        }
                        else
                        {
                            if (joystick != null)
                            {
                                joystick.Dispose();
                            }
                            joystick = new Joystick(directInput, key);
                        }
                    }
                    controllerGuid = key;
                    
                    joystick.Acquire();
                    joystick.Poll();

                    telemetryData.Pitch = 0;
                    telemetryData.Roll = 0;

                    {
                        JoystickState state = new JoystickState();
                        joystick.GetCurrentState(ref state);
                        
                        if (IsMotionEnabled)
                        {
                            //if(axes.Length > 5)
                            //{
                            //telemetryData.Roll = axes[5];
                            //if (state.Buttons.Length > 1 && state.Buttons[1])
                            //{
                            //    MessageBox.Show("STATE: " + state.ToString());
                            //}
                            telemetryData.AccG = new float[3];
                            telemetryData.Roll = (state.RotationX - 32768.0f) / 32768.0f;
                            telemetryData.Pitch = (state.Y - 32768.0f) / 32768.0f;
                            telemetryData.AccG[1] = (state.Z - 32768.0f) / 32768.0f * 5.0f;
                            //}

                            //X = LEFT THUMB X AXIS
                            //Y = LEFT THUMB Y AXIS

                            //ROTATION X = RIGHT THUMB X AXIS
                            //ROTATION Y = RIGHT THUMB Y AXIS

                            //POINT OF VIEW CONTROLLERS = DPAD

                            //RIGHT TRIGGER = -Z
                            //LEFT TRIGGER = Z
                        }
                    }
                    
                    IsConnected = true;
                    
                    IsRunning = true;

                    sw.Restart();

                    TelemetryEventArgs args = new TelemetryEventArgs(
                        new MCTelemetryInfo(telemetryData, lastTelemetryData));
                    RaiseEvent(OnTelemetryUpdate, args);

                    lastTelemetryData = telemetryData;
                    
                    Thread.Sleep(SamplePeriod);
                }
                catch (Exception e)
                {
                    LogError("MCTelemetryProvider Exception while processing data", e);
                    IsConnected = false;
                    IsRunning = false;
                    Thread.Sleep(1000);
                }
            }

            IsConnected = false;
            IsRunning = false;
        }

    }
}
