namespace CatchErrors;
using System;
public class HealkitException : Exception {
    public string m_uniqueData;
    public string m_supperessMsg;
    public HealkitException(string message, Exception innerException) : base(message, innerException) { }
}
