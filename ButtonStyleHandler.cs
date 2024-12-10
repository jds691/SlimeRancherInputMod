using System.IO;
using Steamworks;
using UnityEngine;
using ButtonStyle = SteamInputFixMod.ModConfig.StyleConfig.ButtonStyle;

namespace SteamInputFixMod
{
    internal abstract class ButtonStyleHandler
    {
        private static ButtonStyleHandler _currentHandler;

        public abstract Sprite GetSprite(string actionString, bool isPauseAction);

        public static ButtonStyleHandler Get()
        {
            return _currentHandler;
        }

        public static void OnPreferredStyleChanged(object newPreferredStyle)
        {
            var style = (ButtonStyle)newPreferredStyle;
            switch (style)
            {
                case ButtonStyle.SlimeRancher:
                    _currentHandler = new SlimeRancherButtonStyleHandler();
                    break;
                case ButtonStyle.Steam:
                    _currentHandler = new SteamButtonStyleHandler();
                    break;
            }
        }

        public EControllerActionOrigin ReassignOriginIfNeeded(EControllerActionOrigin oldOrigin)
        {
            switch (oldOrigin)
            {
                // Nintendo Switch
                case EControllerActionOrigin.k_EControllerActionOrigin_Switch_A:
                    return EControllerActionOrigin.k_EControllerActionOrigin_Switch_B;

                case EControllerActionOrigin.k_EControllerActionOrigin_Switch_B:
                    return EControllerActionOrigin.k_EControllerActionOrigin_Switch_A;

                case EControllerActionOrigin.k_EControllerActionOrigin_Switch_X:
                    return EControllerActionOrigin.k_EControllerActionOrigin_Switch_Y;

                case EControllerActionOrigin.k_EControllerActionOrigin_Switch_Y:
                    return EControllerActionOrigin.k_EControllerActionOrigin_Switch_X;

                default:
                    return oldOrigin;
            }
        }

        public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f,
            SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

            var SpriteTexture = LoadTexture(FilePath);
            var NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),
                new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

            return NewSprite;
        }

        public Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f,
            SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference

            var NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0),
                PixelsPerUnit, 0, spriteType);

            return NewSprite;
        }

        public Texture2D LoadTexture(string FilePath)
        {
            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath))
            {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2); // Create new "empty" texture
                if (Tex2D.LoadImage(FileData)) // Load the imagedata into the texture (size is set automatically)
                    return Tex2D; // If data = readable -> return texture
            }

            return null; // Return null if load failed
        }
    }
}