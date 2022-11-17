using System;
using System.Runtime.Serialization;

namespace AplombTech.WMS.Persistence.Repositories
{
  [Serializable]
  internal class ConfigureationValidationException : Exception
  {
    public ConfigureationValidationException()
    {
    }

    public ConfigureationValidationException(string message) : base(message)
    {
    }

    public ConfigureationValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ConfigureationValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}