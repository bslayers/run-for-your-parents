using UnityEngine;

public class AddInfoAttribute : PropertyAttribute
{
    public string Message { get; }

    public AddInfoAttribute(string message) { Message = message; }
}
