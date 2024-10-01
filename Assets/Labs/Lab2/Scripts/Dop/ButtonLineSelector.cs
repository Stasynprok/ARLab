using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAB2DOP
{
    public enum ColorType
    {
        Red, Green, Blue
    }
    public class ButtonLineSelector : MonoBehaviour
    {
        [SerializeField] private ColorType _colorType;
        [SerializeField] private LineParametersController _lineParametersController;
        public void SetColorType()
        {
            _lineParametersController.SetLineColor(_colorType);
        }
    }
}

