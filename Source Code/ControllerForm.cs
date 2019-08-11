using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimFeedback.telemetry.mc
{
    public partial class ControllerForm : Form
    {
        List<KeyValuePair<Guid, string>> joystickList = new List<KeyValuePair<Guid, string>>();
        
        private MCTelemetryProvider provider;
        
        public KeyValuePair<Guid, string> selectedJoystick = new KeyValuePair<Guid, string>(Guid.Empty, null);

        public ControllerForm(MCTelemetryProvider provider)
        {
            this.provider = provider;
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (provider.IsMotionEnabled)
            {
                comboBox1.Enabled = true;
                provider.IsMotionEnabled = false;
                button1.Text = "Enable Plugin";
            }
            else
            {
                comboBox1.Enabled = false;
                provider.IsMotionEnabled = true;
                button1.Text = "Disable Plugin";
            }
        }

        private void ControllerForm_Load(object sender, EventArgs e)
        {
        }
        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;
            base.OnLoad(e);
        }
        
        private void ControllerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }
        private void ListInputDevices()
        {
            // Find a Joystick Guid
            joystickList.Clear();
            var joystickGuid = Guid.Empty;
            ///foreach (var deviceInstance in directInput.GetDevices(DeviceType.Driving, DeviceEnumerationFlags.AllDevices))
            ///{
            ///    joystickGuid = deviceInstance.InstanceGuid;
            ///    joystickList.Add(
            ///        new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
            ///    MessageBox.Show("Driving Added!");
            ///}
            ///
            ///foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            ///{
            ///    joystickGuid = deviceInstance.InstanceGuid;
            ///    joystickList.Add(
            ///        new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
            ///    //MessageBox.Show("Joystick Added!");
            ///}
            ///
            ///foreach (var deviceInstance in directInput.GetDevices(DeviceType.FirstPerson, DeviceEnumerationFlags.AllDevices))
            ///{
            ///    joystickGuid = deviceInstance.InstanceGuid;
            ///    joystickList.Add(
            ///        new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
            ///    //MessageBox.Show("Joystick Added!");
            ///}
            ///
            ///foreach (var deviceInstance in directInput.GetDevices(DeviceType.Flight, DeviceEnumerationFlags.AllDevices))
            ///{
            ///    joystickGuid = deviceInstance.InstanceGuid;
            ///    joystickList.Add(
            ///        new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
            ///    //MessageBox.Show("Joystick Added!");
            ///}
            ///
            ///foreach (var deviceInstance in directInput.GetDevices(DeviceType.Keyboard, DeviceEnumerationFlags.AllDevices))
            ///{
            ///    joystickGuid = deviceInstance.InstanceGuid;
            ///    joystickList.Add(
            ///        new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
            ///    //MessageBox.Show("Joystick Added!");
            ///}
            ///
            ///foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AllDevices))
            ///{
            ///    joystickGuid = deviceInstance.InstanceGuid;
            ///    joystickList.Add(
            ///        new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
            ///    //MessageBox.Show("Gamepad Added!");
            ///}

            KeyValuePair<Guid, string> selectedJoystick = new KeyValuePair<Guid, string>(Guid.Empty, null);
            
            foreach (var deviceInstance in provider.directInput.GetDevices())
            {
                if (deviceInstance.Type == SharpDX.DirectInput.DeviceType.Mouse)
                {
                    continue;
                }
                joystickGuid = deviceInstance.InstanceGuid;
                joystickList.Add(
                    new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName));
                //byte[] guidBytes = joystickGuid.ToByteArray();
                //for (int i = 0; i < 16; i++)
                //{
                //    if (guidBytes[i] != savedGuidBytes[i])
                //    {
                //        break;
                //    }
                //    else if (i == 15)
                //    {
                //        //MessageBox.Show(i + " " + deviceInstance.InstanceName);
                //        selectedJoystick = new KeyValuePair<Guid, string>(joystickGuid, deviceInstance.InstanceName);
                //    }
                //}
                //MessageBox.Show("Gamepad Added!");
            }

            comboBox1.DataSource = new BindingSource(joystickList, null);
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";
            
            if (joystickList.Count > 0)
            {
                this.selectedJoystick = joystickList[0];
            }
            
            if (selectedJoystick.Key != Guid.Empty && selectedJoystick.Value != null)
            {
                
                //MessageBox.Show(selectedJoystick.Key + "");

                this.selectedJoystick = selectedJoystick;
                comboBox1.SelectedItem = this.selectedJoystick;
                //MessageBox.Show(selectedJoystick.Value);
            }

            // Default set the first Guid to be used
            // Autostart Mouse if Fanatec is detected
            //if (joystickList.Count > 0)
            //{
            //    wheelGuid = joystickList.First().Key;
            //    if (joystickList.First().Value.IndexOf("FANATEC") != -1)
            //    {
            //        StartStopToggle();
            //    }
            //}
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            ListInputDevices();
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            selectedJoystick = (KeyValuePair<Guid, string>)comboBox1.SelectedItem;
        }
    }
}
