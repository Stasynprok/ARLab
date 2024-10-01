using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LAB2DOP
{
    public class SliderLineWidth : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private LineParametersController _lineParametersController;

        public void SetLineWidth()
        {
            _lineParametersController.SetLineWidth(_slider.value);
        }
    }
}

