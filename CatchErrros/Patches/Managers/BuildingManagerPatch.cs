namespace CatchErrors.Patches.Managers;
using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using KianCommons;
using KianCommons.Patches;
using CatchErrors.Util;

[HarmonyPatch(typeof(BuildingManager),"SimulationStepImpl")]
class BuildingManagerPatch {
    internal static class Delegates {
        internal delegate void SimulationStep(ushort buildingID, ref Building data);
    }
    static Exception Finalizer(Exception __exception) {
        string name = "BuildingManager";
        var ex = new HealkitException(name + "Simulation Error", __exception);
        ex.m_uniqueData = name;
        ex.m_supperessMsg = "Suppress similar exceptions caused by this manager";
        ex.LogAndForward();
        return null; // suppress exception
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        var fromSimulationStep = typeof(BuildingAI).Method<Delegates.SimulationStep>();
        var toSimulationStep = typeof(BuildingManagerPatch).GetMethod(nameof(SimulationStep));
        var fromCheckUnlocking = typeof(BuildingAI).Method(nameof(BuildingAI.CheckUnlocking));
        var toCheckUnlocking = typeof(BuildingManagerPatch).GetMethod(nameof(CheckUnlocking));

        var list = codes.ToList();
        PatchExtensions.ReplaceCalls(list, fromSimulationStep, toSimulationStep);
        PatchExtensions.ReplaceCalls(list, fromCheckUnlocking, toCheckUnlocking);
        return list;
    }

    static void SimulationStep(BuildingAI _this, ushort buildingID, ref Building data) {
        try {
            _this.SimulationStep(buildingID, ref data);
        } catch (Exception e) {
            string info = $"An exception occurred during BuildingAI simulation step.\nAsset: {_this.m_info.name}" +
                $"\nBuildingID: {buildingID}\nType: {_this.GetType().Name}\nSeverity: High";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_uniqueData = _this.m_info.name;
            e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
            e2.LogAndForward();
        }
    }

    static bool CheckUnlocking(BuildingAI _this) {
        try {
            return _this.CheckUnlocking();
        } catch (Exception e) {
            string info = $"An exception occurred during BuildingAI CheckUnlocking() method.\nAsset: {_this.m_info.name}" +
                $"\nBuildingID: ??\nType: {_this.GetType().Name}\nSeverity: High";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_uniqueData = _this.m_info.name;
            e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
            e2.LogAndForward();
        }

        return false;
    }
}
