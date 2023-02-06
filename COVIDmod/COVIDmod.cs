using ICities;
using UnityEngine;
using CitiesHarmony.API;


namespace COVIDmod
{
    public class COVIDmod : IUserMod
    {
        public string Name
        {
            get
            {
                return "COVID mod";
            }
        }

        public string Description
        {
            get { return "it kills everyone"; }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group = helper.AddGroup("COVID mod");
            group.AddCheckbox("enabled", ModSettings.ModEnabled, (isChecked) => SetActive(isChecked));
            group.AddSlider("how much die you want? left = no die. right = lots die.", 0f, 1f, 0.01f, ModSettings.DieRate, (value) => SetDieAmount(value));
        }

        public void SetActive(bool Status)
        {
            ResidentAISimulationStepPatch.keepDying = Status;
            ModSettings.ModEnabled = Status; //Save the enabled status
        }

        public void SetDieAmount(float DieRate)
        {
            ResidentAISimulationStepPatch.dieRate = DieRate;
            ModSettings.DieRate = DieRate; //Save the dierate setting
        }

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if(HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
            ModSettings.EraseSettings();
        }
    }
}
