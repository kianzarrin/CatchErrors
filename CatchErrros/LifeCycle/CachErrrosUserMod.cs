namespace CatchErrors.LifeCycle;
using System;
using JetBrains.Annotations;
using ICities;
using CitiesHarmony.API;
using KianCommons;
using KianCommons.IImplict;

public class CachErrrosMod : IModWithSettings, IMod {
    static CachErrrosMod() {
        Log.Debug("ExperimentMod.UserMod static constructor called!" + Environment.StackTrace);
    }

    public static Version ModVersion => typeof(CachErrrosMod).Assembly.GetName().Version;
    public static string VersionString => ModVersion.ToString(2);
    public string Name => "Catch Errors " + VersionString;
    public string Description => "Caches, Handles, and Suppresses errors. Can help with broken save.";
    const string HARMONY_ID = "CS.CachErrros";

    public void OnEnabled() {
        Log.Buffered = false;
        Log.VERBOSE = false;
        Log.Stack();
        LoadingManager.instance.m_levelPreLoaded += Load;
        LoadingManager.instance.m_levelUnloaded += Unload;

    }

    public void OnDisabled() {
        Log.Buffered = false;
        LoadingManager.instance.m_levelPreLoaded -= Load;
        LoadingManager.instance.m_levelUnloaded -= Unload;
    }

    public void OnSettingsUI(UIHelper helper) => CatchErrorsSettings.OnSettingsUI(helper);
    
    public static void Load() {
        try {
            HarmonyUtil.InstallHarmony(HARMONY_ID);
        }
        catch (Exception ex) { ex.Log(); }
    }

    public static void Unload() {
        try {
            HarmonyUtil.UninstallHarmony(HARMONY_ID);
        }
        catch (Exception ex) { ex.Log(); }
    }
}
