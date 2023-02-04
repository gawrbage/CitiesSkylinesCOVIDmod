using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace COVIDmod
{
    public static class Patcher
    {
        private const string HarmonyID = "gawrbage.COVIDmod";
        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched) return;

            patched = true;

            var harmony = new Harmony(HarmonyID);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

        }

        public static void UnpatchAll()
        {
            if (!patched) return;

            var harmony = new Harmony(HarmonyID);
            harmony.UnpatchAll(HarmonyID);

            patched = false;
        }
    }

    [HarmonyPatch(typeof(ResidentAI))]
    public static class ResidentAISimulationStepPatch
    {
        public static bool keepDying = true;
        public static float dieRate = 1.0f;

        [HarmonyPatch(nameof(ResidentAI.SimulationStep), new[] { typeof(ushort), typeof(Citizen) }, new[] { ArgumentType.Normal, ArgumentType.Ref })]
        static void Postfix(uint citizenID, ref Citizen data)
        {
            if(!data.Dead && keepDying)
            {
                if (UnityEngine.Random.Range(0.0f, 1.0f) > dieRate)
                {
                    return;
                }

                data.Sick = false;
                data.Dead = true;
                data.SetParkedVehicle(citizenID, 0);
                if ((data.m_flags & Citizen.Flags.MovingIn) != 0)
                {
                    return;
                }

                ushort num = data.GetBuildingByLocation();
                if (num == 0)
                {
                    num = data.m_homeBuilding;
                }

                if (num != 0)
                {
                    DistrictManager instance = Singleton<DistrictManager>.instance;
                    Vector3 position = Singleton<BuildingManager>.instance.m_buildings.m_buffer[num].m_position;
                    byte district = instance.GetDistrict(position);
                    instance.m_districts.m_buffer[district].m_deathData.m_tempCount++;
                    if (IsSenior(citizenID))
                    {
                        instance.m_districts.m_buffer[district].m_deadSeniorsData.m_tempCount++;
                        instance.m_districts.m_buffer[district].m_ageAtDeathData.m_tempCount += (uint)data.Age;
                    }
                }
            }
        }

        private static bool IsSenior(uint citizenID)
        {
            return Citizen.GetAgeGroup(Singleton<CitizenManager>.instance.m_citizens.m_buffer[citizenID].Age) == Citizen.AgeGroup.Senior;
        }
    }
}