namespace CatchErrors.Util; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KianCommons.Patches;
using HarmonyLib;
using System.Reflection.Emit;
using KianCommons;
internal static class PatchUtil {
    internal static MethodInfo Method<TDelegate>
        (this Type type, bool throwOnError = true, bool instance = false) where TDelegate : Delegate =>
        TranspilerUtils.DeclaredMethod<TDelegate>(type, throwOnError, instance);
    internal static MethodInfo Method(this Type type, string methodName) =>
        ReflectionHelpers.GetMethod(type, methodName, throwOnError: true);

    internal static void ReplaceCalls(this List<CodeInstruction> instructions, MethodBase from, MethodBase to) {
        for(int i = 0; i < instructions.Count; i++) {
            var code = instructions[i];
            if (code.Calls(from)) {
                instructions[i] = new CodeInstruction(OpCodes.Call, to);
            }
        }
    }

}
