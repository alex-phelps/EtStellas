using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG
{
    public static class SoundManager
    {
        private static List<SoundEffect> sfx = new List<SoundEffect>();

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(SoundManager), "Begin loading sounds");

            foreach (string se in Directory.EnumerateFiles("Content/Sounds/"))
                sfx.Add(content.Load<SoundEffect>(se.Replace("Content/", "").Replace(".xnb", "")));

            MainGame.eventLogger.Log(typeof(SoundManager), "Finished loading sounds");
        }

        public static SoundEffectInstance GetEffectInstance(string assetName)
        {
            return sfx.First(x => x.Name.Replace("Sounds/", "").Replace(".xnb", "").ToLower() == assetName.ToLower()).CreateInstance();
        }
    }
}
