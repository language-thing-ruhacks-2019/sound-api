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
    public class ValuesController : ControllerBase
    {
        private TextToSpeechClient Client { get; }
        public ValuesController()
        {
            this.Client =  TextToSpeechClient.Create();
            
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World";
        }

        private IActionResult GetGoogleTtsResult(string lang,
            string content,
            SsmlVoiceGender gender,
            AudioEncoding encoding)
        {
            var input = new SynthesisInput
            {
                Text = content,
            };

            var voiceSelection = new VoiceSelectionParams
            {
                LanguageCode = lang,
                SsmlGender = gender
            };

            var audioConfig = new AudioConfig
            {
                AudioEncoding = encoding
            };
            
            var response = this.Client.SynthesizeSpeech(input, voiceSelection, audioConfig);

            var fStream = new MemoryStream();
            response.AudioContent.WriteTo(fStream);
            fStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(fStream, "audio/mpeg");
            
        }

        // GET api/values
        [HttpGet("{language}/{echo}")]
        public IActionResult Get(string language, string echo)
        {
            return GetGoogleTtsResult(language, echo, SsmlVoiceGender.Female, AudioEncoding.Mp3);
        }
        
        [HttpGet("{language}/{echo}/{gender}")]
        public IActionResult Get(string language, string echo, string gender)
        {
            return GetGoogleTtsResult(language, echo, ToGender(gender), AudioEncoding.Mp3);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string body)
        {
            var (lang, content, gender) = JsonConvert.DeserializeObject<TextToSpeechParameters>(body);
            
            return GetGoogleTtsResult(lang, content, ToGender(gender), AudioEncoding.Mp3);
        }

        public static SsmlVoiceGender ToGender(string gender)
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
                    return SsmlVoiceGender.Female;
            }
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
