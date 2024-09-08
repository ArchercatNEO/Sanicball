using System;

namespace Sanicball.Plugins;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
#pragma warning disable CS9113 // Parameter is unread.
public class EnsureChildAttribute(string path) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
{

}
