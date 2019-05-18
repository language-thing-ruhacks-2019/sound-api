namespace speech_synth.Controllers
{
    public sealed class TextToSpeechParameters
    {
        public string Language { get; set; }
        public string Content { get; set; }
        public string Gender { get; set; }

        public void Deconstruct(out string lang, out string content, out string gender)
        {
            lang = Language;
            content = Content;
            gender = Gender;
        }
    }
}
