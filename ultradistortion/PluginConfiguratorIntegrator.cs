using PluginConfig.API;
using PluginConfig.API.Fields;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ultradistortion
{
    public class PluginConfiguratorIntegrator
    {
        public static BoolField isMusicOnlyField;
        public static FloatField intensityField;
        private PluginConfigurator config;
        public bool PluginConfigulatorIntegration()
        {
            if (Plugin.isthereconfigManager)
            {
                config = PluginConfigurator.Create("UltraDistortion", "ultra_distortion");
                isMusicOnlyField = new BoolField(config.rootPanel, "MusicOnly", "enabler", true);
                intensityField = new FloatField(config.rootPanel, "Intensity (Earrape Amount)", "intensity", 0.0f);
                config.postConfigChange += Config_postConfigChange;
                GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
                Plugin.EarRapeApply(rootObjects);
                return true;
            }
            return false;
        }

        private void Config_postConfigChange()
        {
            Plugin.isOnlyMusicValue = isMusicOnlyField.value;
            Plugin.audioIntensityValue = intensityField.value;
            Plugin.log.LogDebug("Attempting to apply config value.");
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            Plugin.EarRapeApply(rootObjects);
        }
    }
}
