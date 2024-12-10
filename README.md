# Slime Rancher Input Mod

Enables compatibility for newer controllers on the Steam version of slime rancher.

## Explanation

Despite Slime Rancher using Steam Input on the Steam version of the game. It does not show icons for unrecognised controllers! Including:

- Nintendo Switch Controllers
- Steam Deck
- Newer controllers than these

This mod should be future-proof as Steam Input itself is designed to be future-proof.

## Requirements

- [SRML](https://www.nexusmods.com/slimerancher/mods/2)
- A **Steam** copy of the game


## Installation

Extract the SteamInputFixMod.dll file into your SRML/Mods folder in the installation directory.

## Configuration

The config file for the mod can be found in <Username>/AppData/LocalLow/Monomi Park/Slime Rancher/SRML/Config/neo.steam-input-fix
(You must run the game once with the mod installed for this to be generated!)

Here are all the settings you can change:
- useAlways (True/False) - Always make the mod load icons instead of using the game's (Only required if you wish to use a specific style even if you're controller is normally supported)
- preferredStyle (Steam/SlimeRancher) - Art style to use for the buttons.
- replaceSteamWithXboxButtons (True/False) - Only applies when using SlimeRancher style! Use the Xbox artwork instead of the old Steam client artwork the game ships with for Steam Controller and Steam Deck.

## Bugs/Maintenance

I am a full time student and am quite busy in general so it's unlikely I'll be actively monitoring the repository.