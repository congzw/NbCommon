namespace NbPilot.Common.Serialize
{
    public class NbJsonSerializeConfig
    {
        public NbJsonSerializeConfig()
        {
            Formatting = NbJsonFormatting.None;
            CamelCase = false;
        }
        public NbJsonFormatting Formatting { get; set; }
        public bool CamelCase { get; set; }
    }

    public enum NbJsonFormatting
    {
        None = 0,
        Indented = 1,
    }
}