using System;
using UnityEngine;

namespace COVIDmod
{
    public static class ModSettings
    {
        private const string enabled = "COVIDMOD_ENABLED";
        private const string dierate = "COVIDMOD_DIE_RATE";

        public static bool ModEnabled
        {
            get
            {
                //If the enabled key string exists in the registry, return the proper value, if it doesn't, then create it
                if (PlayerPrefs.HasKey(enabled))
                {
                    if (PlayerPrefs.GetInt(enabled) == 1)
                    {
                        return true;
                    } else
                    {
                        return false;
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(enabled, 1);
                    return true;
                }
            }

            //Code to set if the mod is enabled in the registry
            set
            {
                if (value == true)
                {
                    PlayerPrefs.SetInt(enabled, 1);
                }
                else
                {
                    PlayerPrefs.SetInt(enabled, 0);
                }
            }
        }

        public static float DieRate
        {
            get
            {
                if (PlayerPrefs.HasKey(dierate))
                {
                    return PlayerPrefs.GetFloat(dierate);
                }
                else
                {
                    PlayerPrefs.SetFloat(dierate, 1.0f);
                    return 1.0f;
                }
            }

            set
            {
                value = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat(dierate, value);
            }
        }

        //Erase the settings to cleanup the user's PC after they are done with the mod
        public static void EraseSettings()
        {
            PlayerPrefs.DeleteKey(enabled);
            PlayerPrefs.DeleteKey(dierate);
        }
    }
}
