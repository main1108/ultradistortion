using PluginConfig.API;
using PluginConfig.API.Fields;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ultradistortion
{
    public class PluginConfiguratorIntegrator
    {
        public static BoolField isMusicOnlyField;
        public static FloatSliderField intensityField;
        private PluginConfigurator config;
        public bool PluginConfigulatorIntegration()
        {
            if (Plugin.isthereconfigManager)
            {
                config = PluginConfigurator.Create("UltraDistortion", "ultra_distortion");
                isMusicOnlyField = new BoolField(config.rootPanel, "MusicOnly", "enabler", false);
                intensityField = new FloatSliderField(config.rootPanel, "Intensity (Earrape Amount)", "intensity", new Tuple<float, float>(0.0f, 1.0f), 1.0f, 3, true, true);
                isMusicOnlyField.onValueChange += (evt) => Config_postConfigChange(null, evt.value);
                intensityField.postValueChangeEvent += (value, bounds) => Config_postConfigChange(value, null);
                GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                Plugin.EarRapeApply(rootObjects);
                return true;
            }
            return false;
        }

        private void Config_postConfigChange(float? a = null, bool? b = null)
        {
            Plugin.audioIntensityValue = a == null ? intensityField.value : a.Value;
            Plugin.isOnlyMusicValue = b == null ? isMusicOnlyField.value : b.Value;
            Plugin.log.LogDebug("Attempting to apply config value.");
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            Plugin.EarRapeApply(rootObjects);
        }
    }
}
