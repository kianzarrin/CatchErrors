namespace CatchErrors;
using System;
using System.Collections.Generic;

public class ExceptionTemplate {
    private static Dictionary<int, ExceptionTemplate> templates = new Dictionary<int, ExceptionTemplate>();

    public Exception Exception { get; private set; }
    public int RaisedCount { get; set; } = 0;
    public bool Suppressed { get; set; }

    private ExceptionTemplate(Exception e) {
        Exception = e;
    }

    public static void ResetSuppressing() {
        foreach (var templatePair in templates) {
            templatePair.Value.Suppressed = false;
        }
    }

    public static ExceptionTemplate RegisterException(Exception e) {
        ExceptionTemplate val;
        int hash = GetExceptionHash(e);
        if (!templates.TryGetValue(hash, out val)) {
            val = new ExceptionTemplate(e);
            templates.Add(hash, val);
        }
        return val;
    }

    private static int GetExceptionHash(Exception e) {
        int ret = e.ToString().GetHashCode();
        if ((e as HealkitException)?.m_uniqueData is string data) {
            ret ^= data.GetHashCode();
        }
        return ret;
    }
}
