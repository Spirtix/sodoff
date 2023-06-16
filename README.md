# SoD-Off - School of Dragons, Offline

On 7th June, 2023, School of Dragons announced they were "sunsetting" the game, and turning the servers off on the 30th of June.

## Getting started

For the first time setup, run the following command:

```
dotnet restore
```

Then run the server as follows:

```
# run mitmproxy to redirect requests to the app
mitmproxy -s mitm-redirect.py

# run the server
dotnet run --project src/sodoff.csproj
```

Then run School of Dragons.

## Status

### What works

- Registration
- Login

### What doesn't

Everything else

## Credits

A huge thanks to [BlazingTwist](https://github.com/BlazingTwist) for their [initial work](https://github.com/BlazingTwist/SoD_OfflineServer) and XSD creation.
