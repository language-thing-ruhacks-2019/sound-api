﻿using System;
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
    [Route("api/trans")]
    [ApiController]
    public class TranslatedController : ControllerBase
    {
        private TranslationClient TransClient { get; }

        public TranslatedController()
        {
            this.TransClient = TranslationClient.Create();
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "Hello World Translated";
        }

        [HttpGet("{inlang}/{olang}/{content}")]
        public async Task<ActionResult<string>> Get(string inlang, string olang, string content)
        {

            var translatedContent = await this.TransClient.TranslateTextAsync(content, olang.Split('-')[0], inlang.Split('-')[0]);
            return JsonConvert.SerializeObject(translatedContent);
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<string>> Post([FromBody] string body)
        {
            var param = JsonConvert.DeserializeObject<TextToSpeechTranslatedParameters>(body);

            var translatedContent = await this.TransClient.TranslateTextAsync(param.Content, param.OutputLang.Split('-')[0], param.InputLang.Split('-')[0]);
            return JsonConvert.SerializeObject(translatedContent);

        }
    }
}
