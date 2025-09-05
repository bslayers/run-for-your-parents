using UnityEngine;

public class RequiredReferenceAttribute : PropertyAttribute
{
    public string Message { get; }

    public RequiredReferenceAttribute(string message) { Message = message; }

    public RequiredReferenceAttribute() { }
}
