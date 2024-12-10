using System.Collections.Generic;
using InControl;
using Steamworks;
using UnityEngine;
using Console = SRML.Console.Console;

namespace SteamInputFixMod
{
    internal class SteamButtonStyleHandler : ButtonStyleHandler
    {
        private static readonly Console.ConsoleInstance console;
        private static readonly Dictionary<int, Sprite> _glyphSprites = new Dictionary<int, Sprite>();

        static SteamButtonStyleHandler()
        {
            if (console == null) console = new Console.ConsoleInstance(typeof(SteamButtonStyleHandler).Name);
        }

        public override Sprite GetSprite(string actionString, bool isPauseAction)
        {
            var uiTemplates = SRSingleton<GameContext>.Instance.UITemplates;
            var activeSteamInputDevice = (SteamInputDevice)InputManager.ActiveDevice;
            var handle = activeSteamInputDevice.GetController();
            var inputType = SteamController.GetInputTypeForHandle(handle);

            var origin = activeSteamInputDevice.GetOrigin(actionString, isPauseAction);
            var originalOrigin = origin;

            if (_glyphSprites.TryGetValue(origin, out var sprite)) return sprite;

            var glyphPath = SteamController.GetGlyphForActionOrigin((EControllerActionOrigin)origin);
            var originSprite = LoadNewSprite(glyphPath);

            Debug.Log(
                $"Replacing origin {(EControllerActionOrigin)originalOrigin} ({originalOrigin}) with origin {(EControllerActionOrigin)origin} ({origin})");

            if (originSprite != null)
            {
                _glyphSprites.Add(origin, originSprite);
                return originSprite;
            }

            Debug.LogError(string.Format("Unable to find a suitable button replacement for origin '{0}' (ordinal: {1})",
                (EControllerActionOrigin)origin, origin));
            return uiTemplates.unknownButtonIcon;
        }
    }
}