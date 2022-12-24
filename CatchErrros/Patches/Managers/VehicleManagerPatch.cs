using CatchErrors;
using CatchErrors.Util;
using ColossalFramework.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[HarmonyPatch(typeof(VehicleManager), "SimulationStepImpl")]
class VehicleManagerPatch {
    internal static class Delegates {
        internal delegate void SimulationStep(ushort vehicleID, ref Vehicle data, Vector3 physicsLodRefPos);
    }
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        var fromSimulationStep = typeof(VehicleAI).Method<Delegates.SimulationStep>();
        var toSimulationStep = typeof(VehicleManagerPatch).GetMethod(nameof(SimulationStep));
        var list = codes.ToList();
        PatchUtil.ReplaceCalls(list, fromSimulationStep, toSimulationStep);
        return list;
    }

    static void SimulationStep(VehicleAI _this, ushort vehicleID, ref Vehicle data, Vector3 physicsLodRefPos) {
        try {
            _this.SimulationStep(vehicleID, ref data, physicsLodRefPos);
        }
        catch (Exception e) {
            string info = $"An exception occurred during VehicleAI simulation step.\nAsset: {_this.m_info.name}" +
                $"\nVehicleID: {vehicleID}\nType: {_this.GetType().Name}\nSeverity: High";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_uniqueData = _this.m_info.name;
            e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
            UIView.ForwardException(e2);
        }
    }
}
