using NAudio.Midi;

namespace PartyMixCompanionProxy;

public static class PadModeSwitch
{
    public static NoteEvent? handle(MidiEvent noteEvent)
    {
        NoteOnEvent noteOnEvent = (NoteOnEvent)noteEvent;
        var deck = noteEvent.Channel switch
        {
            5 => 1,
            _ => 2,
        };
        var handler = VirtualMidiDevice.PadModeHandlers[deck - 1];
        var mode = noteOnEvent.NoteNumber switch
        {
            0 => PadMode.HOT_CUE,
            14 => PadMode.LOOP,
            11 => PadMode.SAMPLE,
            _ => PadMode.EFFECT
        };
        
        handler.CyclePadMode(mode);
        return null;
    }
}