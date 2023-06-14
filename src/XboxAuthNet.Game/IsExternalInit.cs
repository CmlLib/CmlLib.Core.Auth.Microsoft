// just dummy class for C# 9.0 `init` keyword

using System.ComponentModel;

namespace System.Runtime.CompilerServices
{   
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit {} // should be internal
}