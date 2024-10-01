using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAB2DOP
{
    public class ClearAllBTN : MonoBehaviour
    {
        private Button _btnClearAll;

        private void Awake()
        {
            _btnClearAll = GetComponent<Button>();
        }
        private void OnEnable()
        {
            _btnClearAll.onClick.AddListener(InvokeEvent);
        }

        private void OnDisable()
        {
            _btnClearAll.onClick.RemoveListener(InvokeEvent);
        }

        private void InvokeEvent()
        {
            InteractionManager.Instance.InvokeClearAll();
        }
    }
}

