# Speech Synth Endpoint

## API Access

```
GET /api/sound/{lang}/{text}
```

```
POST /api/sound

{
  "Language": "en-US",
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
