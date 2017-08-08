using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LondonCalling.Helper
{
    [Flags]
    public enum SpeechletRequestValidationResult
    {
        OK = 0,
        NoSignatureHeader = 1,
        NoCertHeader = 2,
        InvalidSignature = 4,
        InvalidTimestamp = 8,
        InvalidJson = 16
    }
}