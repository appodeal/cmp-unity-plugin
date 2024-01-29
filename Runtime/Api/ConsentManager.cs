// ReSharper disable CheckNamespace

using System;
using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ConsentManager
    {
        private const string Version = "2.0.0";

        private static readonly Lazy<IConsentManager> Lazy = new Lazy<IConsentManager>(() => new ConsentManagerClient());
        public static IConsentManager Instance { get => Lazy.Value; }

        public static string PluginVersion { get => Version; }
    }
}
