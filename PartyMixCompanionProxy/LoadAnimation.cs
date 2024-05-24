using NAudio.Midi;

namespace PartyMixCompanionProxy;

public static class LoadAnimation
{
    public static void Animation()
    {
        // Reset
        for (var channel = 1; channel < 16; channel++)
        {
            for (var note = 0; note < 127; note++)
            {
                var midi = new NoteEvent(0, channel, MidiCommandCode.NoteOff, note, 0);
                MidiProxy.inputDeviceMidiOut?.Send(midi.GetAsShortMessage());
            }
        }
        Thread.Sleep(100);
        
        // Pads
        for (var note = 20; note < 24; note++)
        {
            var midi1 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, note, 127);
            var midi2 = new NoteEvent(0, 6, MidiCommandCode.NoteOn, note, 127);
            MidiProxy.inputDeviceMidiOut?.Send(midi1.GetAsShortMessage());
            MidiProxy.inputDeviceMidiOut?.Send(midi2.GetAsShortMessage());
            Thread.Sleep(100);
        }
        
        // Deck Controls
        for (var note = 3; note > -1; note--)
        {
            Logger.Log($"{note}");
            var midi1 = new NoteEvent(0, 1, MidiCommandCode.NoteOn, note, 127);
            var midi2 = new NoteEvent(0, 2, MidiCommandCode.NoteOn, note, 127);
            MidiProxy.inputDeviceMidiOut?.Send(midi1.GetAsShortMessage());
            MidiProxy.inputDeviceMidiOut?.Send(midi2.GetAsShortMessage());
            Thread.Sleep(100);
        }
    }
}