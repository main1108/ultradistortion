using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ultradistortion
{
    public static class TransformExtensions
    {
        public static Transform ForceGetChild(this Transform t, int index)
        {
            try
            {
                Transform[] tlist = t.GetComponentsInChildren<Transform>(true).Where(t1 => t1.parent == t).ToArray();
                return tlist[index];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
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
        private const string InternalVersion = "1.0.1";
        public void SetConfig()
        {
            audioIntensityValue = audiodistortionlevel.Value;
            isOnlyMusicValue = onlymusic.Value;
        }
        private void Start()
        {
            log.LogWarning("UltraDistortion Loading... | Version v." + InternalVersion);
            if (isthereconfigManager = Chainloader.PluginInfos.ContainsKey("com.eternalUnion.pluginConfigurator"))
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
        private static void DistortApply(AudioSource aus)
        {
            if (aus != null)
            {
                if (aus.GetComponent<GetMusicVolume>() != null)
                {
                    Destroy(aus.GetComponent<GetMusicVolume>());
                }
                if (aus.GetComponent<AudioDistortionFilter>() == null)
                {
                    AudioDistortionFilter a = aus.gameObject.AddComponent<AudioDistortionFilter>();
                    a.distortionLevel = audioIntensityValue;
                }
                else
                {
                    aus.GetComponent<AudioDistortionFilter>().distortionLevel = audioIntensityValue;
                }
            }
        }
        public static void EarRapeApply(GameObject[] rootObjects)
        {
            log.LogDebug("Current Config Value");
            log.LogDebug("OnlyMusic: " + isOnlyMusicValue);
            log.LogDebug("Intensity: " + audioIntensityValue);
            string scenename = SceneHelper.CurrentScene;
            log.LogInfo("CurrentScene: " + "|" + scenename + "|");
            foreach (GameObject obj in rootObjects)
            {
                if (isOnlyMusicValue)
                {
                    musicDistort(obj, scenename);
                }
                AudioListener listener = obj.GetComponentInChildren<AudioListener>();
                try
                {
                    if (listener != null)
                    {
                        float audioIntensityValueI = audioIntensityValue;
                        if (isOnlyMusicValue)
                        {
                            audioIntensityValueI = 0;
                        }
                        if (listener.GetComponent<AudioDistortionFilter>() == null)
                        {
                            AudioDistortionFilter a = listener.gameObject.AddComponent<AudioDistortionFilter>();
                            a.distortionLevel = audioIntensityValueI;
                        }
                        else
                        {
                            listener.GetComponent<AudioDistortionFilter>().distortionLevel = audioIntensityValueI;
                        }
                    }
                }
                catch (Exception e)
                {
                    log.LogError("Failed to Add AudioDistortionFilter!" + e.Message);
                }
            }
        }
        private static void musicDistort(GameObject obj, string scenename)
        {
            try
            {
                //if (scenename == "" || scenename == "Bootstrap" || scenename == "Intro")
                //{ return; }
                if (obj.name == "Beats")
                {
                    AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                    if (music != null)
                    {
                        foreach (AudioSource aus in music)
                        {
                            DistortApply(aus);
                        }
                    }
                }
                switch (scenename)
                {
                    case string a when a.Contains("Main Menu"):
                        if (obj.name == "Music" || obj.name.Contains("Seasonal"))
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (aus != null)
                                {
                                    DistortApply(aus);
                                }
                            }
                        }
                        break;
                    case string a when a.Contains("2-S"):
                        //Canvas/PowerUpVignette/Panel/Music (1~3)
                        //Panel/Intro/Drone (1~2)
                        if (obj.name == "Canvas")
                        {
                            Transform panel = obj.transform.ForceGetChild(2)?.ForceGetChild(0);
                            if (panel != null)
                            {
                                AudioSource[] music = panel.GetComponentsInChildren<AudioSource>(true);
                                foreach (AudioSource aus in music)
                                {
                                    if (aus == null) { break; }
                                    if (aus.gameObject.name.Contains("Music"))
                                    {
                                        DistortApply(aus);
                                    }
                                }
                            }
                        }
                        break;
                    case string a when a.Contains("4-S"):
                        if (obj.name == "Music")
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                DistortApply(aus);
                            }
                        }
                        break;
                    case string a when a.Contains("5-S"):
                        if (obj.name == "Time of Day")
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (aus.gameObject.name.Contains("Jingle") || aus.gameObject.name == "Music")
                                {
                                    DistortApply(aus);
                                }
                            }
                        }
                        else if (obj.name == "Exit Lobby Interior")
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (aus.transform.parent.name.Contains("Screen"))
                                {
                                    DistortApply(aus);
                                }
                            }
                        }

                        break;
                    case string a when a.Contains("6-2"):
                        if ((obj.name == "IntroSounds") || (obj.name == "BossMusic"))
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (!aus.gameObject.name.Contains("Speech"))
                                {
                                    DistortApply(aus);
                                }
                            }
                        }
                        break;
                    case string a when a.Contains("7-S"):
                        if (obj.name == "Music")
                        {
                            AudioSource aus;
                            aus = obj.GetComponent<AudioSource>();
                            DistortApply(aus);
                        }
                        break;
                    case string a when a.Contains("8-2"):
                        if ((obj.name.Contains("Intro") && obj.name.Contains("Music")) || obj.name == "Boss Music")
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                DistortApply(aus);
                            }
                        }
                        if (obj.name == "Exteriors")
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (aus.name == "Undernap") DistortApply(aus);
                            }
                        }
                        break;
                    case string a when a.Contains("8-3"):
                        if (obj.name.Contains("IntroMusic") || (obj.name == "Space"))
                        {
                            AudioSource[] music = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in music)
                            {
                                if (aus.transform.parent == null || aus.gameObject.name.ToLower().Contains("romusic"))
                                {
                                    DistortApply(aus);
                                }
                            }
                        }
                        if (obj.name == "Pre-Space")
                        {
                            Transform[] startrooms = obj.transform.GetChild(1).GetComponentsInChildren<Transform>(true);
                            foreach (Transform t in startrooms)
                            {
                                if (!t.name.Contains("Starts"))
                                {
                                    AudioSource[] music = t.GetComponentsInChildren<AudioSource>(true);
                                    foreach (AudioSource aus in music)
                                    {
                                        if (aus.gameObject.name.Contains("Music"))
                                        {
                                            DistortApply(aus);
                                        }
                                    }
                                }
                            }

                        }
                        break;
                    case string a when a.Contains("P-1"):
                        if (obj.name.ToLower().Contains("music"))
                        {
                            AudioSource[] sounds = obj.GetComponentsInChildren<AudioSource>(true);

                            foreach (AudioSource aus in sounds)
                            {
                                DistortApply(aus);
                            }
                        }
                        break;
                    case string a when a.Contains("P-2"):
                        if (obj.name == "BossMusics")
                        {
                            AudioSource[] sounds = obj.GetComponentsInChildren<AudioSource>(true);
                            foreach (AudioSource aus in sounds)
                            {
                                DistortApply(aus);
                            }
                        }
                        if (obj.name == "Rain")
                        {
                            if (obj.transform.childCount >= 5)
                            {
                                AudioSource[] sounds = obj.transform.GetChild(5).GetComponentsInChildren<AudioSource>(true);
                                foreach (AudioSource aus in sounds)
                                {
                                    DistortApply(aus);
                                }
                            }
                        }
                        break;
                    case string a when a.Contains("Endless"):
                        if (obj.name == "Everything")
                        {
                            AudioSource[] sounds = obj.GetComponentsInChildren<AudioSource>(true);

                            foreach (AudioSource aus in sounds)
                            {
                                DistortApply(aus);
                            }
                        }
                        break;
                }
                AudioSource[] musics =
                    obj.GetComponentInChildren<MusicManager>(true)?.GetComponentsInChildren<AudioSource>(true);
                if (musics != null)
                {
                    foreach (AudioSource aus in musics)
                    {
                        DistortApply(aus);
                    }
                }
            }
            catch (Exception e)
            {
                log.LogError("Failed to Apply Distortion to Music! ");
                log.LogError(e.Message);
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
            if (Plugin.isthereconfigManager)
            {
                Plugin.audioIntensityValue = PluginConfiguratorIntegrator.intensityField.value;
                Plugin.isOnlyMusicValue = PluginConfiguratorIntegrator.isMusicOnlyField.value;
            }
            Plugin.EarRapeApply(rootObjects);
        }
    }
    [HarmonyPatch(typeof(AudioCopyVolumeAndTime), "Update")]
    public class ACVATpatch
    {
        [HarmonyPostfix]
        public static void Postfix(AudioCopyVolumeAndTime __instance, AudioSource ___aud)
        {
            if (__instance.GetComponent<AudioSource>() != null)
            {
                if (__instance.target.GetComponent<AudioDistortionFilter>() != null)
                {
                    if (___aud.GetComponent<AudioDistortionFilter>() == null)
                    {
                        AudioDistortionFilter a = ___aud.gameObject.AddComponent<AudioDistortionFilter>();
                        a.distortionLevel = Plugin.isOnlyMusicValue ? Plugin.audioIntensityValue : 0f;
                    }
                    ___aud.GetComponent<AudioDistortionFilter>().distortionLevel = Plugin.isOnlyMusicValue ? Plugin.audioIntensityValue : 0f;
                }
            }
        }
    }
}
