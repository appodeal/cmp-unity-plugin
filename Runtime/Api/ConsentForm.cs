// ReSharper disable CheckNamespace

using System.Diagnostics.CodeAnalysis;

namespace AppodealStack.Cmp
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class ConsentForm
    {
        private readonly IConsentForm _consentFormImpl;

        private ConsentForm(IConsentForm consentFormImpl) { _consentFormImpl = consentFormImpl; }

        internal static ConsentForm CreateInstance(IConsentForm consentFormImpl) => new ConsentForm(consentFormImpl);

        public void Show() => _consentFormImpl?.Show();
    }
}
