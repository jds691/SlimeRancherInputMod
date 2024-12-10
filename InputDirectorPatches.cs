using System.Collections.Generic;
using HarmonyLib;
using InControl;
using Steamworks;
using UnityEngine;

namespace SteamInputFixMod
{
    [HarmonyPatch(typeof(InputDirector))]
    [HarmonyPatch("GetSteamDeviceIcon")]
    internal class GetSteamDeviceIconPatch
    {
        private static Dictionary<int, Sprite> additionalIconSprites = new Dictionary<int, Sprite>();

        private static string currentActionStr;
        private static bool currentIsPauseAction;

        private static void Prefix(string actionStr, bool isPauseAction)
        {
            currentActionStr = actionStr;
            currentIsPauseAction = isPauseAction;
        }

        private static void Postfix(ref Sprite __result)
        {
            var uiTemplates = SRSingleton<GameContext>.Instance.UITemplates;
            var activeSteamInputDevice = (SteamInputDevice)InputManager.ActiveDevice;
            var handle = activeSteamInputDevice.GetController();
            var inputType = SteamController.GetInputTypeForHandle(handle);

            var origin = activeSteamInputDevice.GetOrigin(currentActionStr, currentIsPauseAction);
            var translatedOrigin = SteamController.TranslateActionOrigin(ESteamInputType.k_ESteamInputType_Unknown,
                (EControllerActionOrigin)origin);

            if (__result == uiTemplates.unknownButtonIcon || ModConfig.GeneralConfig.useAlways)
                __result = ButtonStyleHandler.Get().GetSprite(currentActionStr, currentIsPauseAction);
        }
    }
}