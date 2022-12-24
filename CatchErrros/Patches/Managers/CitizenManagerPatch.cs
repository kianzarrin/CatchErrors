using CatchErrors;
using CatchErrors.Util;
using ColossalFramework.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

[HarmonyPatch(typeof(CitizenManager), "SimulationStepImpl")]
class CitizenManagerPatch {
    internal static class Delegates {
        internal delegate void SimulationStep(uint citizenID, ref Citizen data);
    }

    static Exception Finalizer(Exception __exception) {
        string name = "CitizenManager";
        var ex = new HealkitException(name + "Simulation Error", __exception);
        ex.m_uniqueData = name;
        ex.m_supperessMsg = "Suppress similar exceptions caused by this manager";
        ex.LogAndForward();
        return null; // suppress exception
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        var fromSimulationStep = typeof(CitizenAI).Method<Delegates.SimulationStep>();
        var toSimulationStep = typeof(CitizenManagerPatch).GetMethod(nameof(SimulationStep));
        var list = codes.ToList();
        PatchExtensions.ReplaceCalls(list, fromSimulationStep, toSimulationStep);
        return list;
    }

    static void SimulationStep(CitizenAI _this, uint citizenID, ref Citizen data) {
        try {
            _this.SimulationStep(citizenID, ref data);
        } catch (Exception e) {
            string info = $"An exception occurred during CitizenAI simulation step.\nAsset: {_this.m_info.name}" +
                $"\nCitizenID: {citizenID}\nType: {_this.GetType().Name}\nSeverity: High";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_uniqueData = _this.m_info.name;
            e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
            e2.LogAndForward();
        }
    }
}

