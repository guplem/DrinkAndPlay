using UnityEngine;
using System;

namespace Crystal
{
    public class SafeAreaDemo : MonoBehaviour
    {
        [SerializeField] private KeyCode keySafeArea = KeyCode.A;
        private SafeArea.SimDevice[] sims;
        private int simIdx;

        private void Awake ()
        {
            if (!Application.isEditor)
                Destroy (gameObject);

            sims = (SafeArea.SimDevice[])Enum.GetValues (typeof (SafeArea.SimDevice));
        }

        private void Update ()
        {
            if (Input.GetKeyDown (keySafeArea))
                ToggleSafeArea ();
        }

        /// <summary>
        /// Toggle the safe area simulation device.
        /// </summary>
        private void ToggleSafeArea ()
        {
            simIdx++;

            if (simIdx >= sims.Length)
                simIdx = 0;

            SafeArea.Sim = sims[simIdx];
            Debug.LogFormat ("Switched to sim device '{0}'\n with debug key '{1}'", sims[simIdx], keySafeArea);
        }
    }
}
