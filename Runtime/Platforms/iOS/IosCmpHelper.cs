#if UNITY_IOS || APPODEAL_DEV_IOS

// ReSharper disable CheckNamespace

using System;

namespace AppodealStack.Cmp
{
    internal static class IosCmpHelper
    {
        public static ConsentManagerError GetConsentManagerErrorFromInt(int cause)
        {
            if (Enum.IsDefined(typeof(ConsentManagerError), cause))
            {
                return (ConsentManagerError)cause;
            }

            return ConsentManagerError.Unspecified;
        }
    }
}
#endif
