using CatchErrors;
using CatchErrors.Util;
using ColossalFramework.UI;
using HarmonyLib;
using KianCommons;
using System;
using System.Collections.Generic;
using System.Linq;

[HarmonyPatch(typeof(TransportManager), "SimulationStepImpl")]
class TransportManagerManagerPatch {
    internal static class Delegates {
        internal delegate void SimulationStep(ushort lineID);
    }
    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes) {
        var fromSimulationStep = typeof(TransportLine).Method<Delegates.SimulationStep>();
        var toSimulationStep = typeof(TransportManagerManagerPatch).GetMethod(nameof(SimulationStep));
        var list = codes.ToList();
        PatchExtensions.ReplaceCalls(list, fromSimulationStep, toSimulationStep);
        return list;
    }

    static void SimulationStep(ref TransportLine _this, ushort lineID) {
        try {
            _this.SimulationStep(lineID);
        } catch (Exception e) {
            string info = $"An exception occurred during TransportLine simulation step.\n" +
                $"Line: {Enum.GetName(typeof(TransportInfo.TransportType), _this.Info.m_transportType)} {_this.m_lineNumber}\n" +
                $"LineID: {lineID}\nSeverity: High";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_supperessMsg = "Suppress this exception";
            e2.Log();
            e2.Display();
        }
    }
}

