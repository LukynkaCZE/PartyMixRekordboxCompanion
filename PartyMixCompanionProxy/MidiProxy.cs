using NAudio.Midi;

namespace PartyMixCompanionProxy;

public static class MidiProxy
{
    public static MidiIn? inputDeviceMidiIn = null;
    public static MidiOut inputDeviceMidiOut;
    public static string selectedDeviceName;

    public static void updateInputDevice(string deviceName)
    {
        if (inputDeviceMidiIn != null)
        {
            Program.WindowHost.setText(Window.StatusMessageState.WAITING_FOR_INPUT_DEVICE);
            inputDeviceMidiIn.Stop();
            inputDeviceMidiIn.Dispose();
            inputDeviceMidiOut.Close();
            inputDeviceMidiOut.Dispose();
        }
        for (var device = 0; device < MidiIn.NumberOfDevices; device++)
        {
            var deviceInfo = MidiIn.DeviceInfo(device);
            if (deviceInfo.ProductName != deviceName) continue;
            
            inputDeviceMidiIn = new MidiIn(device);
            inputDeviceMidiOut = new MidiOut(device);
            selectedDeviceName = deviceName;
            Program.WindowHost.setText(Window.StatusMessageState.RUNNING);
            inputDeviceMidiIn.Start();
        }
        
        for (var device = 0; device < MidiOut.NumberOfDevices; device++)
        {
            var deviceInfo = MidiOut.DeviceInfo(device);
            if (deviceInfo.ProductName == deviceName)
            {
                inputDeviceMidiOut = new MidiOut(device);
            }
        }

        if (inputDeviceMidiIn == null)
        {
            MessageBox.Show("ERROR: invalid device selected");
            Program.WindowHost.setText(Window.StatusMessageState.WAITING_FOR_INPUT_DEVICE);
        }

        LoadAnimation.Animation();
        if (inputDeviceMidiIn != null) inputDeviceMidiIn.MessageReceived += InputDeviceMidiInOnMessageReceived;
    }

    private static void InputDeviceMidiInOnMessageReceived(object? sender, MidiInMessageEventArgs e)
    {
        Program.WindowHost.VirtualMidiDevice.ProxyThrough(e.MidiEvent);
    }
}