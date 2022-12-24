namespace CatchErrors.LifeCycle;
using ColossalFramework;
using System;
using KianCommons;
using UnityEngine;
using System.IO;

internal static class CatchErrorsSettings {
    public const string SETTINGS_FILE_NAME = "CatchErrors";
    public static readonly SettingsBool sb_SuppressAllExceptions = new SettingsBool(
        description: "Suppress all exceptions",
        tooltip: "Not recommended",
        name: "suppressAllExceptions",
        defaultValue: false);

    static CatchErrorsSettings() {
        try {
            // Creating setting file - from SamsamTS
            if (GameSettings.FindSettingsFileByName(SETTINGS_FILE_NAME) == null) {
                GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = SETTINGS_FILE_NAME } });
            }
        } catch (Exception ex) {
            ex.Log("Couldn't load/create the setting file.");
        }
    }

    internal static void OnSettingsUI(UIHelper helper) {
        try {
            sb_SuppressAllExceptions.Draw(helper);
            helper.AddSpace(10);
            helper.AddButton("Clear list of suppressed exceptions", ExceptionTemplate.ResetSuppressing);
            helper.AddSpace(10);
            helper.AddButton("Open log folder", () => {
                Utils.OpenInFileBrowser(Path.Combine(Application.dataPath, "Logs"));
            });
        } catch (Exception ex) { ex.Log(); }
    }
}
