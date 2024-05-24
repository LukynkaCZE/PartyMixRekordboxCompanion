using NAudio.Midi;

namespace PartyMixCompanionProxy;

public class PadModeHandler(int channel)
{
    public readonly int Channel = channel;
    public PadMode PadMode = PadMode.HOT_CUE;

    public void CyclePadMode(PadMode mode)
    {
        var old = PadMode;
        PadMode = mode;
        Logger.Log($"Cycled to new pad value on channel {Channel}: {old} -> {PadMode}");
    }
    
    public NoteEvent handle(NoteOnEvent noteOnEvent)
    {
        Logger.Log($"{noteOnEvent.NoteNumber} | {PadMode}");
        var dataOut = PadMode switch
        {
            PadMode.HOT_CUE => noteOnEvent.NoteNumber switch
            {
                20 => PadModeData.PAD_HOT_CUE_1,
                21 => PadModeData.PAD_HOT_CUE_2,
                22 => PadModeData.PAD_HOT_CUE_3,
                23 => PadModeData.PAD_HOT_CUE_4,
                _ => noteOnEvent
            },
            PadMode.LOOP => noteOnEvent.NoteNumber switch
            {
                20 => PadModeData.PAD_LOOP_1,
                21 => PadModeData.PAD_LOOP_2,
                22 => PadModeData.PAD_LOOP_3,
                23 => PadModeData.PAD_LOOP_4,
                _ => noteOnEvent
            },
            PadMode.SAMPLE => noteOnEvent.NoteNumber switch
            {
                20 => PadModeData.PAD_SAMPLE_1,
                21 => PadModeData.PAD_SAMPLE_2,
                22 => PadModeData.PAD_SAMPLE_3,
                23 => PadModeData.PAD_SAMPLE_4,
                _ => noteOnEvent
            },
            PadMode.EFFECT => noteOnEvent.NoteNumber switch
            {
                20 => PadModeData.PAD_EFFECT_1,
                21 => PadModeData.PAD_EFFECT_2,
                22 => PadModeData.PAD_EFFECT_3,
                23 => PadModeData.PAD_EFFECT_4,
                _ => noteOnEvent
            },
            _ => noteOnEvent
        };
        dataOut.Channel = Channel switch
        {
            1 => 5,
            2 => 6,
            _ => noteOnEvent.Channel
        };

        Logger.Log($"Returning {dataOut} | {dataOut.NoteNumber}");
        return dataOut;
    }
}

public enum PadMode
{
    HOT_CUE = 0,
    LOOP = 1,
    SAMPLE = 2,
    EFFECT = 3
}

public static class PadModeData
{
    public static readonly NoteEvent PAD_HOT_CUE_1 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 20, 127);
    public static readonly NoteEvent PAD_HOT_CUE_2 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 21, 127);
    public static readonly NoteEvent PAD_HOT_CUE_3 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 22, 127);
    public static readonly NoteEvent PAD_HOT_CUE_4 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 23, 127);
    
    public static readonly NoteEvent PAD_LOOP_1 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 24, 127);
    public static readonly NoteEvent PAD_LOOP_2 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 25, 127);
    public static readonly NoteEvent PAD_LOOP_3 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 26, 127);
    public static readonly NoteEvent PAD_LOOP_4 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 27, 127);
    
    public static readonly NoteEvent PAD_SAMPLE_1 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 28, 127);
    public static readonly NoteEvent PAD_SAMPLE_2 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 29, 127);
    public static readonly NoteEvent PAD_SAMPLE_3 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 30, 127);
    public static readonly NoteEvent PAD_SAMPLE_4 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 40, 127);
    
    public static readonly NoteEvent PAD_EFFECT_1 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 41, 127);
    public static readonly NoteEvent PAD_EFFECT_2 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 42, 127);
    public static readonly NoteEvent PAD_EFFECT_3 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 43, 127);
    public static readonly NoteEvent PAD_EFFECT_4 = new NoteEvent(0, 5, MidiCommandCode.NoteOn, 45, 127);
}