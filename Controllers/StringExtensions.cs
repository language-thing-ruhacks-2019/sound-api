using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.TextToSpeech.V1;

namespace speech_synth.Controllers
{
    public static class StringExtensions
    {
        public static SsmlVoiceGender ToGender(this string gender)
        {
            switch (gender.ToLowerInvariant())
            {
                case "male":
                    return SsmlVoiceGender.Male;
                case "female":
                    return SsmlVoiceGender.Female;
                case "neutral":
                    return SsmlVoiceGender.Neutral;
                default:
                    return SsmlVoiceGender.Unspecified;
            }
        }
    }
}
