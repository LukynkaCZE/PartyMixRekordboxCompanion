using NAudio.Midi;

namespace PartyMixCompanionProxy
{
    public static class Utils
    {
        public static string ByteArrayToString( byte[] ba ) {
            var hex = BitConverter.ToString( ba );
            return hex.Replace( "-" , ":" );
        }
        
        public static int? GetSelfDeviceIndex()
        {
            int? self = null;
            for (var i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                var deviceInfo = MidiIn.DeviceInfo(i);
                if (deviceInfo.ProductName == EmulationMappings.DeviceName)
                {
                    self = i;
                }
            }
            return self;
        }
    }
}