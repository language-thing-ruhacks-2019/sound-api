using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.TextToSpeech.V1;
using Google.Cloud.Translation.V2;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace speech_synth.Controllers
{
    [Route("api/sound/trans")]
    [ApiController]
    public class TextToSpeechTranslatedController : ControllerBase
    {
        private TextToSpeechClient TtsClient { get; }
        private TranslationClient TransClient { get; }

        public TextToSpeechTranslatedController()
        {
            this.TtsClient =  TextToSpeechClient.Create();
            this.TransClient = TranslationClient.Create();
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World Translated";
        }

         private async Task<IActionResult> GetGoogleTtsResult(string lang,
            string content,
            string wavenet,
            AudioEncoding encoding)
        {
            var input = new SynthesisInput
            {
                Text = content,
            };

            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = lang,
            };

            if (wavenet?.ToGender() != SsmlVoiceGender.Unspecified)
            {
                voiceSelection.SsmlGender = wavenet?.ToGender() ?? "female".ToGender();
            }
            else
            {
                voiceSelection.Name = wavenet;
            }

            var audioConfig = new AudioConfig
            {
                AudioEncoding = encoding
            };

            var response = await this.TtsClient.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

            var fStream = new MemoryStream();
            response.AudioContent.WriteTo(fStream);
            fStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(fStream, "audio/mpeg");
        }


        [HttpGet("{inlang}/{olang}/{content}/{gender?}")]
        public async Task<IActionResult> Get(string inlang, string olang, string content, string gender = "female")
        {
            string lang = olang.Split('-')[0];
            if (lang == "zh") lang = olang; // special case chinese
            var translatedContent = await this.TransClient.TranslateTextAsync(content, lang, inlang.Split('-')[0]);
            return await GetGoogleTtsResult(olang, translatedContent.TranslatedText, gender, AudioEncoding.Mp3);

        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string body)
        {
            var param = JsonConvert.DeserializeObject<TextToSpeechTranslatedParameters>(body);

            string lang = param.OutputLang.Split('-')[0];
            if (lang == "zh") lang = param.OutputLang; // special case chinese

            var translatedContent = await this.TransClient.TranslateTextAsync(param.Content, lang, param.InputLang.Split('-')[0]);
            return await GetGoogleTtsResult(param.OutputLang, translatedContent.TranslatedText, param.Gender, AudioEncoding.Mp3);

        }
    }
}
