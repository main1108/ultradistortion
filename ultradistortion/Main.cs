using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ultradistortion
{
    public static class a
    {
        public static PluginConfiguratorIntegrator integrator = new PluginConfiguratorIntegrator();
        public static void CallPluginConfigurator()
        {
            if (!integrator.PluginConfigulatorIntegration())
            {
                Plugin.log.LogError("[ERROR] Failed to Initialize config manager!");
            }

        }
    }
    [BepInPlugin(Guid, InternalName, InternalVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource log = BepInEx.Logging.Logger.CreateLogSource("ULTRADistortion");
        public static ConfigEntry<float> audiodistortionlevel;
        public static ConfigEntry<bool> onlymusic;
        public static float audioIntensityValue = 0.5f;
        public static bool isOnlyMusicValue = false;
        public static bool isthereconfigManager;
        private const string Guid = "susinopo.ULTRADistortion";
        private const string InternalName = "ULTRADistortion";
        private const string InternalVersion = "0.0.1";
        public void SetConfig()
        {
            audioIntensityValue = audiodistortionlevel.Value;
            isOnlyMusicValue = onlymusic.Value;
        }
        private void Awake()
        {
            log.LogWarning("UltraDistortion Loading... | Version v." + InternalVersion);
            isthereconfigManager = File.Exists(
                Path.Combine(
                    Paths.BepInExRootPath,
                    "plugins",
                    "EternalsTeam-PluginConfigurator",
                    "PluginConfigurator",
                    "PluginConfigurator.dll"
                )
            );
            if (isthereconfigManager)
            {
                log.LogInfo($"PluginConfigurator Detected!");
                log.LogDebug($"[DEBUG] Calling PluginConfigurator Integration");
                a.CallPluginConfigurator();
            }
            else
            {
                audiodistortionlevel = Config.Bind("Audio", "AudioDistortionLevel", 0.5f, "Intensity. a.k.a Earrape Amount");
                onlymusic = Config.Bind("Audio", "OnlyMusic", false, "Only music to be distorted.");
                SetConfig();
            }
            try
            {
                Harmony harmony = new Harmony(InternalName);
                harmony.PatchAll();
            }
            catch (Exception e)
            {
                Logger.LogFatal("ERROR ERROR ERROR");
                Logger.LogFatal(e.ToString());
            }
        }
        public static void EarRapeApply(GameObject[] rootObjects)
        {
            foreach (GameObject obj in rootObjects)
            {
                log.LogDebug("Current Config Value");
                log.LogDebug("OnlyMusic: "+ isOnlyMusicValue);
                log.LogDebug("Intensity: " + audioIntensityValue);
                if (isOnlyMusicValue)
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
                                    Destroy(aus.GetComponent<GetMusicVolume>());
                                }
                                if (aus != null)
                                {
                                    if (aus.gameObject.GetComponent<AudioDistortionFilter>() == null)
                                    {
                                        AudioDistortionFilter a = aus.gameObject.AddComponent<AudioDistortionFilter>();
                                        a.distortionLevel = audioIntensityValue;
                                    }
                                    else
                                    {
                                        aus.gameObject.GetComponent<AudioDistortionFilter>().distortionLevel = audioIntensityValue;
                                    }
                                }
                            }
                        }
                        AudioSource[] musics = obj.GetComponentInChildren<MusicManager>(true).gameObject.GetComponentsInChildren<AudioSource>(true);
                        foreach (AudioSource music in musics)
                        {
                            GameObject mm = music.gameObject;
                            if (mm != null)
                            {
                                if (mm.GetComponent<AudioDistortionFilter>() == null)
                                {
                                    AudioDistortionFilter a = mm.AddComponent<AudioDistortionFilter>();
                                    a.distortionLevel = audioIntensityValue;
                                }
                                else
                                {
                                    mm.GetComponent<AudioDistortionFilter>().distortionLevel = audioIntensityValue;
                                }
                            }
                        }
                    }
                    catch (Exception e) { }
                }
                try
                {
                    AudioListener listener = obj.GetComponentInChildren<AudioListener>();
                    try
                    {
                        float audioIntensityValueI = audioIntensityValue;
                        if (isOnlyMusicValue)
                        {
                            audioIntensityValueI = 0;
                        }
                        if (listener.gameObject.GetComponent<AudioDistortionFilter>() == null)
                        {
                            AudioDistortionFilter a = listener.gameObject.AddComponent<AudioDistortionFilter>();
                            a.distortionLevel = audioIntensityValueI;
                        }
                        else
                        {
                            listener.gameObject.GetComponent<AudioDistortionFilter>().distortionLevel = audioIntensityValueI;
                        }
                    }
                    catch (Exception e)
                    {
                        log.LogDebug("Failed to Add AudioDistortionFilter!" + e.Message);
                    }
                }
                catch (Exception e)
                {
                    log.LogDebug("Failed to Get AudioListner!" + e.Message);
                }
                
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
            Plugin.EarRapeApply(rootObjects);
        }
    }
}
