using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LAB3;

namespace LAB2_ZIP
{
    public class DefaultScreenMode : MonoBehaviour, IInteractionManagerMode
    {
        [SerializeField] private GameObject _ui;

        public void Activate()
        {
            _ui.SetActive(true);
        }

        public void Deactivate()
        {
            _ui.SetActive(false);
        }

        public void TouchInteraction(Touch[] touches)
        {
            return;
        }

        public void SelectMode(int mode)
        {
            LAB3.InteractionManager.Instance.SelectMode(mode);
        }
    }

}
