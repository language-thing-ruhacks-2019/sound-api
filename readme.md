# Speech Synth Endpoint

## API Access

```
GET /api/sound/{lang}/{text}/{gender?}
```

```
POST /api/sound

{
  "Language": "en-US",
  "Content": "Hello World",
  "Gender": "female" // "male" | "female" | "neutral"
}
```


```
GET /api/sound/trans/{inlang}/{outlang}/{text}/{gender?}
```

```
POST /api/sound/trans

{
  "InputLang": "en-US",
  "OutputLang": "fr-FR",
  "Content": "Hello World",
  "Gender": "female"
}
```



```
GET /api/trans/{inlang}/{outlang}/{text}
```

```
POST /api/sound/trans

{
  "InputLang": "en-US",
  "OutputLang": "fr-FR",
  "Content": "Hello World",
}
```

## Deploy
Get .NET Core 2.2 from here

https://aka.ms/dotnet-download

Finally, from the root of the directory: 

```
$ export GOOGLE_APPLICATION_CREDENTIALS=NewAgent-a0dd6e8b24b6.json
$ dotnet run
```
