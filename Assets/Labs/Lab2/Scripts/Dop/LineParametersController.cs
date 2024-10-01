using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace LAB2DOP
{
    public class LineParametersController : MonoBehaviour
    {
        [SerializeField] private GameObject _linesControllers;
        [SerializeField] private ILineController _iLineControllers;

        private void Awake()
        {
            ILineController controller = _linesControllers.GetComponent<ILineController>();
            _iLineControllers = controller;
        }

        public void SetLineColor(ColorType ColorType)
        {
            UnityEngine.Color color;
            switch (ColorType)
            {
                case ColorType.Red:
                    color = UnityEngine.Color.red;
                    break;
                case ColorType.Green:
                    color = UnityEngine.Color.green;
                    break; 
                case ColorType.Blue:
                    color = UnityEngine.Color.blue;
                    break;
                default:
                    return;
            }
            Debug.LogError(ColorType);
            _iLineControllers.ChangeColorLine(color);
        }

        public void SetLineWidth(float width)
        {
            _iLineControllers.ChangeWidth(width);
        }
    }
}

