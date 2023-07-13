you can now bring the CS:GO flashbang directly to your desktop.
connect it to your twitch account and setup a dedicated reward in your channel, to be used by your viewers.
this program does not "block" mouse clicks, so you can continue to use your computer while getting flashbanged.
it doesn't, however, work with certain fullscreen applications/games. borderless is fine.

## setup

A Twitch Developer application is required. Go to [https://dev.twitch.tv/console/apps](https://dev.twitch.tv/console/apps) and create your personal application. Then copy over `Client ID` and `Client Secret` into the program. Category is irrelevant.
The default Redirect URL is `http://localhost:8758/`. You can select a different one by modifying the `TwitchFlashbang.json` file in the program folder.

if you need help, you may contact me on discord, the username is `puncia`.

explaination of the settings:

- `TestMode`: flashes a 500x500 portion instead of the whole screen;
- `TestKey`: a combination of keys that fires a flashbang. This is not mandatory and you can remove it;
- `EnableAfterimage`: a screenshot of the desktop before flashing is taken, and is displayed right when the flash starts to fade;
- `PerAppOpacity`: you can define per-app opacity, so for example you could set 50% transparency when playing a game, if you do not want to be completely blinded;
- `TwitchConfig`: contains the credentials of your twitch account and twitch dev app;
- `ChannelName`/`ChannelID`: self explanatory. `ChannelID` is retrieved using twitch API;
- `RewardIDs`: the reward ID: you can acquire it by debugging my program and look at the value in the right place. If you know any other method let me know;
- `Token`/`RefreshToken`: the tokens used to connect to Helix;
- `MinFlashbangBits`: the minimum bits required to get a flashbang;

## how

the whole functionality resides here:

<https://github.com/Puncia/TwitchFlashbang/blob/e0694127a781414d4270d303e76babe2372cd61e/TwitchFlashbang/WinAPI.cs#L59-L72>

read msdn if you want to learn more about the flags.

## credits

a friend asked me to do it, based on the idea of this: <https://twitter.com/frooty_looms/status/1649965566965215234>
his repo: <https://github.com/narekb8/Simple-Desktop-Flashbang>
