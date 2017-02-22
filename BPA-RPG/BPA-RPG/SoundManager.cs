using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EtStellas
{
    public static class SoundEffectManager
    {
        private static List<SoundEffect> sfx = new List<SoundEffect>();

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(SoundEffectManager), "Begin loading sounds");

            foreach (string se in Directory.EnumerateFiles("Content/Sounds/"))
                sfx.Add(content.Load<SoundEffect>(se.Replace("Content/", "").Replace(".xnb", "")));

            MainGame.eventLogger.Log(typeof(SoundEffectManager), "Finished loading sounds");
        }

        public static SoundEffectInstance GetEffectInstance(string assetName)
        {
            return sfx.First(x => x.Name.Replace("Sounds/", "").Replace(".xnb", "").ToLower() == assetName.ToLower()).CreateInstance();
        }
    }
}
