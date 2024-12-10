using System.Collections.Generic;
using InControl;
using Steamworks;
using UnityEngine;

namespace SteamInputFixMod
{
    internal class SlimeRancherButtonStyleHandler : ButtonStyleHandler
    {
        private static readonly Dictionary<int, Sprite> _glyphSprites = new Dictionary<int, Sprite>();

        public override Sprite GetSprite(string actionString, bool isPauseAction)
        {
            var uiTemplates = SRSingleton<GameContext>.Instance.UITemplates;
            var activeSteamInputDevice = (SteamInputDevice)InputManager.ActiveDevice;
            var handle = activeSteamInputDevice.GetController();
            var inputType = SteamController.GetInputTypeForHandle(handle);

            Sprite result;
            var origin = activeSteamInputDevice.GetOrigin(actionString, isPauseAction);
            var originalOrigin = origin;

            if (_glyphSprites.TryGetValue(origin, out result)) return result;

            origin = (int)ReassignOriginIfNeeded((EControllerActionOrigin)origin);

            var translatedOrigin = SteamController.TranslateActionOrigin(ESteamInputType.k_ESteamInputType_Unknown,
                (EControllerActionOrigin)origin);
            origin = (int)translatedOrigin;

            origin = (int)ReassignSteamButtonsIfNeeded(origin);

            if (uiTemplates.steamButtonIcons[origin] != null) return uiTemplates.steamButtonIcons[origin];

            Debug.Log(
                $"Replacing origin {(EControllerActionOrigin)originalOrigin} ({originalOrigin}) with origin {(EControllerActionOrigin)origin} ({origin})");


            // Fallback to Steam if needed (shouldn't but you never know)
            return new SteamButtonStyleHandler().GetSprite(actionString, isPauseAction);
        }

        private EControllerActionOrigin ReassignSteamButtonsIfNeeded(int origin)
        {
            if (!ModConfig.SlimeRancherStyleConfig.replaceSteamWithXboxButtons)
                return (EControllerActionOrigin)origin;

            switch ((EControllerActionOrigin)origin)
            {
                case EControllerActionOrigin.k_EControllerActionOrigin_A:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_A:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_A;
                case EControllerActionOrigin.k_EControllerActionOrigin_B:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_B:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_B;
                case EControllerActionOrigin.k_EControllerActionOrigin_X:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_X:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_X;
                case EControllerActionOrigin.k_EControllerActionOrigin_Y:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_Y:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_Y;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftBumper:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftBumper:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftBumper_Pressure:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftBumper;
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightBumper:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightBumper_Pressure:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightBumper;

                case EControllerActionOrigin.k_EControllerActionOrigin_Start:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_Start:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_Start;
                case EControllerActionOrigin.k_EControllerActionOrigin_Back:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_Back:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_Back;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadNorth:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftPad_DPadNorth:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_DPad_North;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadSouth:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftPad_DPadSouth:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_DPad_South;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadWest:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftPad_DPadWest:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_DPad_West;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftPad_DPadEast:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftPad_DPadEast:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_DPad_East;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_Click:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightPad_Click:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightStick_Click;
                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadNorth:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightPad_DPadNorth:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightStick_DPadNorth;
                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadSouth:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightPad_DPadSouth:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightStick_DPadSouth;
                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadWest:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightPad_DPadWest:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightStick_DPadWest;
                case EControllerActionOrigin.k_EControllerActionOrigin_RightPad_DPadEast:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightPad_DPadEast:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightStick_DPadEast;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftTrigger_Pull:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftTrigger_Pull:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftTrigger_Pull;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftTrigger_Click:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftTrigger_Click:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftTrigger_Click;

                case EControllerActionOrigin.k_EControllerActionOrigin_RightTrigger_Pull:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightTrigger_Pull:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightTrigger_Pull;
                case EControllerActionOrigin.k_EControllerActionOrigin_RightTrigger_Click:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_RightTrigger_Click:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_RightTrigger_Click;

                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_Move:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftStick_Move:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftStick_Move;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_Click:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftStick_Click:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftStick_Click;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadNorth:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftStick_DPadNorth:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftStick_DPadNorth;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadSouth:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftStick_DPadSouth:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftStick_DPadSouth;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadWest:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftStick_DPadWest:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftStick_DPadWest;
                case EControllerActionOrigin.k_EControllerActionOrigin_LeftStick_DPadEast:
                case EControllerActionOrigin.k_EControllerActionOrigin_SteamV2_LeftStick_DPadEast:
                    return EControllerActionOrigin.k_EControllerActionOrigin_XBox360_LeftStick_DPadEast;


                default:
                    return (EControllerActionOrigin)origin;
            }
        }
    }
}