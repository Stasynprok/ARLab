using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAB2DOP
{
    public class DefaultScreenModeDop : MonoBehaviour, IInteractionManagerMode
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
            InteractionManager.Instance.SelectMode(mode);
        }
    }

}
