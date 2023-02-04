using ICities;
using CitiesHarmony;
using CitiesHarmony.API;
using HarmonyLib;

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
            group.AddCheckbox("enabled", true, (isChecked) => SetActive(isChecked));
            group.AddSlider("how much die you want? left = no die. right = lots die.", 0f, 1f, 0.01f, 1f, (value) => SetDieAmount(value));
        }

        public void SetActive(bool Status)
        {
            ResidentAISimulationStepPatch.keepDying = Status;
        }

        public void SetDieAmount(float DieRate)
        {
            ResidentAISimulationStepPatch.dieRate = DieRate;
        }

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if(HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
    }
}
