using NAudio.Midi;

namespace PartyMixCompanionProxy
{
    public partial class Window : Form
    {
        public readonly VirtualMidiDevice VirtualMidiDevice;

        public Window()
        {
            InitializeComponent();
            Logger.Log("Window Host Loaded!");

            Thread.Sleep(500);
            VirtualMidiDevice = new VirtualMidiDevice();

            setText(StatusMessageState.WAITING_FOR_INPUT_DEVICE);

            for (var device = 0; device < MidiIn.NumberOfDevices; device++)
            {
                var deviceName = MidiIn.DeviceInfo(device).ProductName;
                if (deviceName == EmulationMappings.DeviceName) continue;

                comboBox1.Items.Add(deviceName);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MidiProxy.updateInputDevice(comboBox1.SelectedItem.ToString());
        }

        public void setText(StatusMessageState state)
        {
            var text = state switch
            {
                StatusMessageState.WAITING_FOR_INPUT_DEVICE => "Waiting for input device..",
                StatusMessageState.RUNNING =>
                    $"Running midi proxy from {MidiProxy.selectedDeviceName} to emulated {EmulationMappings.DeviceName} using driver version {TeVirtualMIDI.driverVersionString}",
                _ => ""
            };
            label1.Text = text;
        }

        public enum StatusMessageState
        {
            WAITING_FOR_INPUT_DEVICE,
            RUNNING,
        }
    }
}