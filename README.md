you can now bring the CS:GO flashbang directly to your desktop.
connect it to your twitch account and setup a dedicated reward in your channel, to be used by your viewers.
this program does not "block" mouse clicks, so you can continue to use your computer while getting flashbanged.
it doesn't, however, work with certain fullscreen applications/games. borderless is fine.

## setup
setting up has to be done manually for now.

if you need help, you may contact me on discord, the username is `puncia`.

you need a `twitchflashbang.json` file alongside the executable that looks like this (yes the values are made up):

```json
{
  "testMode": "false",
  "enableAfterimage": "true",
  "perAppOpacity": {
    "LeagueClientUx": 0.5,
    "osu!.exe": 0.9,
    "chrome": 0.1,
    "explorer": 0.5
  },
  "twitchConfig": {
    "channelID": "60773607",
    "clientID": "l86yi9sesagn1pnkv9526v0gtkxfgu",
    "secret": "t3w2t8nsou4rrcwqm0r5k72fus55t6",
    "rewardIDs": [ "237b9695-fdd6-bd0e-3a671-0a4ac2f4b5a0" ]
  }
}
```
explaination:
- `testMode`: flashes a 500x500 portion instead of the whole screen;
- `enableAfterimage`: a screenshot of the desktop before flashing is taken, and is displayed right when the flash starts to fade;
- `perAppOpacity`: you can define per-app opacity, so for example you could set 50% transparency when playing a game, if you do not want to be completely blinded;
- `twitchConfig`: you have to retrieve the values yourself: use [twitchtokengenerator.com](https://twitchtokengenerator.com/) and populate the fields;
- `rewardIDs`: the reward ID: you can acquire it by debugging my program and look at the value in the right place. If you know any other method let me know.

## how
the whole functionality resides here:

https://github.com/Puncia/TwitchFlashbang/blob/e0694127a781414d4270d303e76babe2372cd61e/TwitchFlashbang/WinAPI.cs#L59-L72

read msdn if you want to learn more about the flags.

## credits
a friend asked me to do it, based on the idea of this: https://twitter.com/frooty_looms/status/1649965566965215234
his repo: https://github.com/narekb8/Simple-Desktop-Flashbang
