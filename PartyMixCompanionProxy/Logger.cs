namespace PartyMixCompanionProxy
{
    public static class Logger
    {
        public static void Log(string message, Type type = Type.Info)
        {
            var formattedMessage = $@"[{type.ToString()}] {message}";
            Console.WriteLine(formattedMessage);
        }

        public enum Type
        {
            Info,
            Success,
            Warning,
            Error,
            MIDI,
            PROXY,
        }
    }
}