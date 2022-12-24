namespace CatchErrors.Util;
using KianCommons;
using System;
using ColossalFramework.UI;
internal static class Extensions {
    internal static void LogAndForward(this Exception ex) {
        ex.Log(false);
        UIView.ForwardException(ex);
    }
}
