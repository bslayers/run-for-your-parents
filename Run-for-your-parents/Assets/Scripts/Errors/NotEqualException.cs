using System;

public class NotEqualException : Exception
{
    #region Constructor

    public NotEqualException() { }

    public NotEqualException(string message) : base(message) { }

    public NotEqualException(string message, Exception inner) : base(message, inner) { }

    public NotEqualException(string obj1Name, string obj2Name, int obj1Value, int obj2Value) :
    base(CreateMessage(obj1Name, obj2Name, obj1Value, obj2Value))
    { }

    public NotEqualException(string obj1Name, string obj2Name, float obj1Value, float obj2Value) :
    base(CreateMessage(obj1Name, obj2Name, obj1Value, obj2Value))
    { }

    protected NotEqualException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context)
        : base(info, context) { }

    #endregion

    #region Methods

    private static string CreateMessage(string obj1Name, string obj2Name, object obj1Value, object obj2Value)
    {
        return $"{obj1Name} and {obj2Name} haven't the same size : {obj1Name}={obj1Value}, {obj2Name}={obj2Value}.";
    }

    #endregion
}