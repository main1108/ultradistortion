using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ultradistortion
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource log = BepInEx.Logging.Logger.CreateLogSource("ULTRADistortion");
        public static ConfigEntry<float> audiodistortionlevel;
        public static ConfigEntry<bool> onlymusic;
        private const string PluginGuid = "susinopo.ULTRADistortion";      //MODを識別するID
        private const string PluginName = "ULTRADistortion";                   //MODの名前
        private const string PluginVer = "0.0.1";                      //MODのバージョン
        private void Awake()
        {
            audiodistortionlevel = Config.Bind("Audio", "AudioDistortionLevel", 0.5f, "Value of AudioDistortionFilter.distortionLevel");
            onlymusic = Config.Bind("Audio", "OnlyMusic", false, "Only music to distorted.");

            try
            {
                Harmony harmony = new Harmony(PluginGuid);
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Logger.LogFatal("ERROR ERROR ERROR");
                Logger.LogFatal(e.ToString());
            }

        }
    }

    [HarmonyPatch(typeof(SceneHelper), "OnSceneLoaded"),]
    public class OSLpatch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (GameObject obj in rootObjects)
            {
                if (Plugin.onlymusic.Value)
                {
                    try
                    {
                        if (obj.name == "Beats")
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (aus.gameObject.GetComponent<GetMusicVolume>() != null)
                                {
                                    UnityEngine.Object.Destroy(aus.GetComponent<GetMusicVolume>());
                                }
                                if (aus != null)
                                {
                                    AudioDistortionFilter a = aus.gameObject.AddComponent<AudioDistortionFilter>();
                                    a.distortionLevel = Plugin.audiodistortionlevel.Value;
                                }
                            }
                        }
                        AudioSource[] musics = obj.GetComponentInChildren<MusicManager>(true).gameObject.GetComponentsInChildren<AudioSource>(true);
                        foreach (AudioSource music in musics)
                        {
                            GameObject mm = music.gameObject;
                            if (mm != null)
                            {
                                AudioDistortionFilter a = mm.AddComponent<AudioDistortionFilter>();
                                a.distortionLevel = Plugin.audiodistortionlevel.Value;
                            }
                        }
                    }
                    catch { }
                }
                if (!Plugin.onlymusic.Value)
                {
                    try
                    {
                        AudioListener listener = obj.GetComponentInChildren<AudioListener>();
                        try
                        {
                            AudioDistortionFilter a = listener.gameObject.AddComponent<AudioDistortionFilter>();
                            a.distortionLevel = Plugin.audiodistortionlevel.Value;
                        }
                        catch { }
                    }
                    catch { }
                }

            }
        }
    }
}
