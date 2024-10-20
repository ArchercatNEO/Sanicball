using System.Collections.Generic;
using Godot;

namespace Sanicball.Utils;

public static class NodeExtensions
{
    public static IEnumerable<Node> ChildrenStream(this Node self)
    {
        for (int i = 0; i < self.GetChildCount(); i++)
        {
            yield return self.GetChild(i);
        }
    }
}