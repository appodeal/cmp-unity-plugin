// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [Serializable]
    [SuppressMessage("ReSharper", "NotAccessedField.Global")]
    public sealed class ConsentDebugSettings
    {
        public ConsentDebugGeography Geography;
        public List<string> TestDeviceIds;
    }
}
