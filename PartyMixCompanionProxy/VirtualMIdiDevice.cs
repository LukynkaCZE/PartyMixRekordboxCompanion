using System.Diagnostics;
using NAudio.Midi;

namespace PartyMixCompanionProxy
{
    public class VirtualMidiDevice
    {
        public static TeVirtualMIDI Port;
        public static MidiIn MidiIn;
        
        public static Guid Manufacturer = new Guid( "aa4e075f-3504-4aab-9b06-9a4104a91cf0" );
        public static Guid Product = new Guid( "bb4e075f-3504-4aab-9b06-9a4104a91cf0" );

        public static List<PadModeHandler> PadModeHandlers = new List<PadModeHandler>();

        public static void CommunicationWorkerThread() 
        {
            byte[] command;

            try {
                while ( true ) {
                    command = Port.getCommand();
                    Port.sendCommand( command );
                    Logger.Log($@"Command: {Utils.ByteArrayToString(command)}", Logger.Type.MIDI);
                }
            } catch ( Exception ex ) {
                Logger.Log( $@"thread aborting: {ex.Message}", Logger.Type.Error);
            }
        }

        public VirtualMidiDevice()
        {
            Logger.Log($"Initializing virtual midi device under name {EmulationMappings.DeviceName}");
            Port = new TeVirtualMIDI(EmulationMappings.DeviceName, 65535, TeVirtualMIDI.TE_VM_FLAGS_PARSE_RX, ref Manufacturer, ref Product);
            Logger.Log($"Starting communication worker thread");
            var thread = new Thread(CommunicationWorkerThread);
            
            Logger.Log("Starting MidiOut and MidiIn channels..");
            
            var selfIndex = Utils.GetSelfDeviceIndex();
            if (selfIndex == null)
            {
                Application.Exit();
                throw new Exception($"Something went wrong: Handle to Virtual Midi Device \"{EmulationMappings.DeviceName}\"was not found!");
            }
            MidiIn = new MidiIn(selfIndex.Value);
            MidiIn.MessageReceived += MidiInOnMessageReceived;
            MidiIn.Start();
            
            Logger.Log($"Started virtual midi device {EmulationMappings.DeviceName}!");
            
            PadModeHandlers.Add(new PadModeHandler(1));
            PadModeHandlers.Add(new PadModeHandler(2));
        }

        private void MidiInOnMessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if (e.MidiEvent is not NoteOnEvent @event) return;
            
            var midiIn = @event;
            Debug.Assert(midiIn != null, nameof(midiIn) + " != null");
            Logger.Log($"(IN -->) {e.MidiEvent} (RAW: {@event.GetAsShortMessage()}) (Note Number {midiIn.NoteNumber})", Logger.Type.MIDI);
            MidiProxy.inputDeviceMidiOut.Send(@event.GetAsShortMessage());

        }

        public void ProxyThrough(MidiEvent noteEvent)
        {

            var eventOut = noteEvent.GetAsShortMessage() switch
            {
                8326036 => PadModeSwitch.handle(noteEvent),
                8326804 => PadModeSwitch.handle(noteEvent),
                8323220 => PadModeSwitch.handle(noteEvent),
                8327060 => PadModeSwitch.handle(noteEvent),
                
                8323221 => PadModeSwitch.handle(noteEvent),
                8326805 => PadModeSwitch.handle(noteEvent),
                8326037 => PadModeSwitch.handle(noteEvent),
                8327061 => PadModeSwitch.handle(noteEvent),
                
                8336031 => null, 
                _ => noteEvent
            };


            if (eventOut == null) return;
            
            // Pad buttons handling
            if ((noteEvent.Channel is 5 or 6) && noteEvent is NoteOnEvent notOnEvent)
            {
                var deck = notOnEvent.Channel switch
                {
                    5 => 1,
                    _ => 2,
                };
                var handler = PadModeHandlers[deck - 1];
                eventOut = handler.handle(notOnEvent);
            }
            
            Port.sendCommand(BitConverter.GetBytes(eventOut.GetAsShortMessage()));
            Logger.Log($"(PROXY ->) {eventOut}", Logger.Type.PROXY);
        }
    }
}