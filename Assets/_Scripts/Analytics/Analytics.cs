using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hookah.Analytics
{
    public class Analytics : MonoBehaviour
    {
        #region Variables
        public static Analytics Instance { get; private set; }
        #endregion

        #region UnityMethods
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);

                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}