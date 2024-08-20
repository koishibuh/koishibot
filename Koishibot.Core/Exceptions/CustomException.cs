namespace Koishibot.Core.Exceptions;

public class CustomException(string message, Exception? inner = null)
	: Exception(message, inner);

public class JsonDeserializeException(string json, Exception? innerException = null)
: Exception($"Json deserialize failed: {json}", innerException);