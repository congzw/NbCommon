using System;
using System.Threading;

namespace NbPilot.Common.Internal
{
    internal sealed class AutoResolveAsSingletonHelper<T>
    {
        public static readonly Lazy<T> Lazy = new Lazy<T>(LazyThreadSafetyMode.ExecutionAndPublication);
    }
}
