using CatchErrors;
using CatchErrors.Util;
using ColossalFramework.UI;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

[HarmonyPatch(typeof(NetManager), "SimulationStepImpl")]
class NetManagerPatch {
    internal static class Delegates {
        internal delegate void SimulationStep(ushort segmentID, ref NetSegment data);
    }
    static Exception Finalizer(Exception __exception) {
        string name = "NetManager";
        var ex = new HealkitException(name + "Simulation Error", __exception);
        ex.m_uniqueData = name;
        ex.m_supperessMsg = "Suppress similar exceptions caused by this manager";
        ex.LogAndForward();
        return null; // suppress exception
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        var fromSimulationStep = typeof(NetAI).Method<Delegates.SimulationStep>();
        var toSimulationStep = typeof(NetManagerPatch).GetMethod(nameof(SimulationStep));
        var list = codes.ToList();
        PatchExtensions.ReplaceCalls(list, fromSimulationStep, toSimulationStep);
        return list;
    }

    static void SimulationStep(NetAI _this, ushort segmentID, ref NetSegment data) {
        try {
            _this.SimulationStep(segmentID, ref data);
        } catch (Exception e) {
            string info = $"An exception occurred during NetAI simulation step.\nAsset: {_this.m_info.name}" +
                $"\nSegmentID: {segmentID}\nType: {_this.GetType().Name}\nSeverity: High";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_uniqueData = _this.m_info.name;
            e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
            e2.LogAndForward();
        }
    }
}
