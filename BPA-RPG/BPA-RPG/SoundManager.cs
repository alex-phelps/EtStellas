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
        private static List<SoundEffectInstance> instances = new List<SoundEffectInstance>();

        public static void LoadContent(ContentManager content)
        {
            MainGame.eventLogger.Log(typeof(SoundManager), "Begin loading sounds");

            foreach (string se in Directory.EnumerateFiles("Content/Sounds/"))
                sfx.Add(content.Load<SoundEffect>(se.Replace("Content/", "").Replace(".xnb", "")));

            MainGame.eventLogger.Log(typeof(SoundManager), "Finished loading sounds");
        }

        public static SoundEffectInstance GetEffectInstance(string assetName)
        {
            SoundEffectInstance effect = sfx.First(x => x.Name.Replace("Sounds/", "").Replace(".xnb", "").ToLower() == assetName.ToLower()).CreateInstance();

            //Add new instance to list and update the list
            UpdateSfxInstances();
            instances.Add(effect);

            return effect;
        }

        /// <summary>
        /// Changes volume of all SFX instances
        /// </summary>
        /// <param name="volume"></param>
        public static void ChangeSFXVolume(float volume)
        {
            instances.ForEach(x => x.Volume = volume);
        }

        /// <summary>
        /// Removes any disposed instances from 
        /// </summary>
        private static void UpdateSfxInstances()
        {
            instances.RemoveAll(x => x.IsDisposed);
        }
    }
}
