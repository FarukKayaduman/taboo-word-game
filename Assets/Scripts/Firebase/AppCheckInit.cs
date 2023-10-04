using Firebase.AppCheck;
using UnityEngine;

namespace Firebase
{
    public class AppCheckInit : MonoBehaviour
    {
        #if !UNITY_EDITOR && UNITY_ANDROID
        private void Start ()
        {
            FirebaseAppCheck.SetAppCheckProviderFactory(
                PlayIntegrityProviderFactory.Instance);
        }
        #endif
    }
}