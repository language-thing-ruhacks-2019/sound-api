namespace speech_synth.Controllers
{
    public sealed class TextToSpeechTranslatedParameters
    {
        public string InputLang { get; set; }
        public string OutputLang { get; set; }

        public string Content { get; set; }

        public string Gender { get; set; }
    }
}