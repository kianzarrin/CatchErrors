namespace CatchErrors.Patches;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using HarmonyLib;
using ICities;
using KianCommons.Plugins;
using System;
using System.Collections.Generic;
using System.Reflection;
using KianCommons;

[HarmonyPatch(typeof(ThreadingWrapper))]
static class ThreadingWrapperPatch {
    public static void Handle(Exception ex, IThreadingExtension ext, string method) {
        Assembly assembly = ext.GetType().Assembly;
        PluginManager.PluginInfo pluginInfo = PluginManager.instance.FindPluginInfo(assembly);
        string modName = pluginInfo?.GetModName();
        string text = $"An error has occurred in mod's {method} method.\n";
        if (modName == null) {
            text +=
                $"Mod name: <Unknown>\n" +
                $"Assembly: {assembly.FullName}\nSeverity: Medium";
        } else {
            text += $"Mod name: {modName ?? "<Unknown>"}";
        }
        HealkitException ex2 = new HealkitException($"The Mod '{modName}' has caused an error", ex);

        Log.Exception(ex2, showInPanel:false);
        UIView.ForwardException(e: ex2);
        ex2.m_uniqueData = modName;
        ex2.m_supperessMsg = "Suppress similar exceptions caused by this mod";
        UIView.ForwardException(ex2);
    }

    [HarmonyPrefix, HarmonyPatch("OnUpdate")]
    static bool UpdatePrefix(float realTimeDelta, float simulationTimeDelta, List<IThreadingExtension> ___m_ThreadingExtensions) {
        for (int i = 0; i < ___m_ThreadingExtensions.Count; i++) {
            try {
                ___m_ThreadingExtensions[i].OnUpdate(realTimeDelta, simulationTimeDelta);
            } catch (Exception e) {
                Handle(e, ___m_ThreadingExtensions[i], "OnUpdate");
            }
        }
        return false;
    }

    [HarmonyPrefix, HarmonyPatch("OnBeforeSimulationTick")]
    static bool OnBeforeSimulationTickPrefix(List<IThreadingExtension> ___m_ThreadingExtensions) {
        for (int i = 0; i < ___m_ThreadingExtensions.Count; i++) {
            try {
                ___m_ThreadingExtensions[i].OnBeforeSimulationTick();
            } catch (Exception e) {
                Handle(e, ___m_ThreadingExtensions[i], "OnBeforeSimulationTick");
            }
        }
        return false;
    }

    [HarmonyPrefix, HarmonyPatch("OnAfterSimulationTick")]
    static bool OnAfterSimulationTickPrefix(List<IThreadingExtension> ___m_ThreadingExtensions) {
        for (int i = 0; i < ___m_ThreadingExtensions.Count; i++) {
            try {
                ___m_ThreadingExtensions[i].OnAfterSimulationTick();
            } catch (Exception e) {
                Handle(e, ___m_ThreadingExtensions[i], "OnAfterSimulationTick");
            }
        }
        return false;
    }

    [HarmonyPrefix, HarmonyPatch("OnBeforeSimulationFrame")]
    static bool OnBeforeSimulationFramePrefix(List<IThreadingExtension> ___m_ThreadingExtensions) {
        for (int i = 0; i < ___m_ThreadingExtensions.Count; i++) {
            try {
                ___m_ThreadingExtensions[i].OnBeforeSimulationFrame();
            } catch (Exception e) {
                Handle(e, ___m_ThreadingExtensions[i], "OnBeforeSimulationFrame");
            }
        }
        return false;
    }

    [HarmonyPrefix, HarmonyPatch("OnAfterSimulationFrame")]
    static bool OnAfterSimulationFramePrefix(List<IThreadingExtension> ___m_ThreadingExtensions) {
        for (int i = 0; i < ___m_ThreadingExtensions.Count; i++) {
            try {
                ___m_ThreadingExtensions[i].OnAfterSimulationFrame();
            } catch (Exception e) {
                Handle(e, ___m_ThreadingExtensions[i], "OnAfterSimulationFrame");
            }
        }
        return false;
    }

}

