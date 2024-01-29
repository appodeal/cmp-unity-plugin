#if UNITY_EDITOR

// ReSharper disable CheckNamespace

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace AppodealStack.Cmp
{
    internal class ApdEditorConsentFormVisual : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Button consentButton;
        [SerializeField] private Button doNotConsentButton;

        public event EventHandler OnConsentButtonClicked;
        public event EventHandler OnDoNotConsentButtonClicked;

        private void Awake()
        {
            Assert.IsNotNull(titleText);
            Assert.IsNotNull(consentButton);
            Assert.IsNotNull(doNotConsentButton);

            titleText.text = $"{PlayerSettings.productName} asks for your consent to use your personal data to:";
            consentButton.onClick.AddListener(ConsentButtonClicked);
            doNotConsentButton.onClick.AddListener(DoNotConsentButtonClicked);
        }

        public void ConsentButtonClicked() => OnConsentButtonClicked?.Invoke(this, EventArgs.Empty);

        public void DoNotConsentButtonClicked() => OnDoNotConsentButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
#endif
