# joybox

A kid-friendly touch-controlled library media player on the Raspberry Pi

## Targetted Hardware (i.e. only tested with)

* Raspberry Pi 3 Model A+ running Raspbian Stretch
* Pimoroni HyperPixel 4.0 Touch Screen
* Adafruit Mini External USB Stereo Speaker

## Requirements

### Frontend

* Node.js v14.x (LTS):  
[https://github.com/nodesource/distributions/blob/master/README.md#debinstall](https://github.com/nodesource/distributions/blob/master/README.md#debinstall)
* Yarn 1.22.x:  
[https://yarnpkg.com/lang/en/docs/install/#debian-stable](https://yarnpkg.com/lang/en/docs/install/#debian-stable)  
**Important on Raspberry Pi: Execute `yarn config set child-concurrency 1` before executing `yarn install`.  
Otherwise the SDCard will get hammered by 5 parallel build processes and the system will overload!**

### Backend

* .NET Core 3.1 SDK for Linux: [dotnet-core-3.1]

### System

* Raspbian Stretch Packages (additional to Raspbian Lite)
  * `omxplayer` for media playback
  * `chromium-browser` for the Web UI
  * An X desktop environment (I used LXDE) with autologin enabled.
* .NET Core 3.1 Runtime for Linux (ARM32): [dotnet-core-3.1]

## Setup (OUTDATED)

1. Fork/Clone

2. Install dependencies with `yarn install`

3. Create the SQLite database and tables with `npm run initdb` (or `npm run initdb:prod`)

4. Fill the library SQLite database with `npm run scan` (or `npm run scan:prod`)  
   This will execute `knex seed:run --env development` to seed the database with the contents of the `media` folder.

5. Fire up the server with `npm start`

6. Start the front-end with `DISPLAY=:0 chromium-browser --kiosk http://localhost:3000`

## Install Dependencies

Go to the `joybox2` folder and install client App dependencies with:

    $ yarn install

If your IDE doesn't automatically install your .NET NuGet Dependencies, you can manually install them with:

    $ dotnet restore

## Dev Workflow

Start a [watched .NET Core build](https://docs.servicestack.net/templates-websites#watched-net-core-builds) in the background from the command-line with:

    $ dotnet watch run
    watch : Started
    Hosting environment: Development
    Content root path: C:\Users\simon\git\joybox2\joybox2
    Now listening on: https://localhost:5001
    Application started. Press Ctrl+C to shut down.

In a new terminal window start a watched nuxt dev server build with:

    $ yarn dev

Then open [http://localhost:3000](http://localhost:3000) in your browser to view your App served directly from Nuxt.js dev server and will proxy all Server requests to ServiceStack Server running on [https://localhost:5001](https://localhost:5001).

## Disclaimer

Most of the REST API & DB backend code is based on the examples in [https://github.com/mjhea0/node-koa-api](https://github.com/mjhea0/node-koa-api).  
It was of great help to have such a nice template to implement my own requirements.

## Notes

Volume control with amixer

```shell
amixer cset numid=3,iface=MIXER,name='PCM Playback Volume' 10%
```

Playback control with omxplayer

```shell
omxplayer -o alsa media/one_love.mp3
```

Other omxplayer options

```shell
-l  --pos n                 Start position (hh:mm:ss)
    --no-osd                Do not display status information on screen
    --no-keys               Disable keyboard input (prevents hangs for certain TTYs)
```

[dotnet-core-3.1]: <https://dotnet.microsoft.com/download/dotnet-core/3.1> "Download .NET Core 3.1"
