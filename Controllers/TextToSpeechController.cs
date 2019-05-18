using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.TextToSpeech.V1;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace speech_synth.Controllers
{
    [Route("api/sound")]
    [ApiController]
    public class TextToSpeechController : ControllerBase
    {
        private TextToSpeechClient Client { get; }
        public TextToSpeechController()
        {
            this.Client =  TextToSpeechClient.Create();
            
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World";
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

            var response = await this.Client.SynthesizeSpeechAsync(input, voiceSelection, audioConfig);

            var fStream = new MemoryStream();
            response.AudioContent.WriteTo(fStream);
            fStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(fStream, "audio/mpeg");
        }


        [HttpGet("{language}/{echo}/{gender?}")]
        public Task<IActionResult> Get(string language, string echo, string gender = "female")
        {
            return GetGoogleTtsResult(language, echo, gender, AudioEncoding.Mp3);
        }

        // POST api/values
        [HttpPost]
        public Task<IActionResult> Post([FromBody] string body)
        {
            var (lang, content, gender) = JsonConvert.DeserializeObject<TextToSpeechParameters>(body);
            return GetGoogleTtsResult(lang, content, gender, AudioEncoding.Mp3);
        }

        #region template
        /*
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/

        #endregion
    }
}
