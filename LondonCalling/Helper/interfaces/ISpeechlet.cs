using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LondonCalling.Helper.interfaces
{
    public interface ISpeechlet
    {
        bool OnRequestValidation(
          AlexaResponse result, DateTime referenceTimeUtc, AlexaRequest requestEnvelope);

    }
}
