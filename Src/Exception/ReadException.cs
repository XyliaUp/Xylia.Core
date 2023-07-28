using System.Runtime.Serialization;

namespace Xylia.CustomException;

[Serializable]
public class ReadException : ApplicationException
{
	public ReadException() { }
	public ReadException(string message) : base(message) { }
	public ReadException(string message, Exception inner) : base(message, inner) { }
	public ReadException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
