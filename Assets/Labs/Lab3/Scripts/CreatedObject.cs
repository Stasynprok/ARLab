using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LAB3
{
    public class CreatedObject : MonoBehaviour
    {
        [SerializeField] private string _displayName;
        [SerializeField] private string _description;

        private int _number = -1;

        public string Name
        {
            get
            {
                if (_number >= 0)
                {
                    return _displayName + " " + _number.ToString();
                }
                else
                {
                    return _displayName;
                }
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        //Added
        public int Number
        {
            get { return _number; }
        }

        private MeshRenderer _meshRenderer;
        private List<Color> _startColors = new List<Color>();
        private List<Color> _onNonSelectColor = new List<Color>();

        private ParticleSystem _particleSystem;
        private ParticleSystemForceField _forceField;

        public void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _particleSystem = GetComponent<ParticleSystem>();
            _forceField = GetComponent<ParticleSystemForceField>();

            DisableParticles();
            if (_meshRenderer)
            {
                foreach (Material mat in _meshRenderer.materials)
                {
                    _startColors.Add(mat.color);
                }
                SetNonselectMaterials();
            }
        }

        private void SetNonselectMaterials()
        {
            for (int i = 0; i < _startColors.Count; i++)
            {
                Color startColor = _startColors[i];
                startColor.a = 0.5f;
                _onNonSelectColor.Add(startColor);
            }
        }
        public void ChangeMaterealOnSelectAnother()
        {
            if (!_meshRenderer)
            {
                return;
            }
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
            {
                _meshRenderer.materials[i].color = _onNonSelectColor[i];
            }

            DisableParticles();
        }

        public void ChangeMaterialOnUnselectAnother()
        {
            if (!_meshRenderer)
            {
                return;
            }
            for (int i = 0; i < _meshRenderer.materials.Length; i++)
            {
                _meshRenderer.materials[i].color = _startColors[i];
            }
        }

        public void ActivateParticlesField()
        {
            if (_forceField)
            {
                _forceField.enabled = true;
            }

            if (_particleSystem)
            {
                _particleSystem.Stop();
            }
        }

        public void ActivateParticles(Component trigger)
        {
            if (_particleSystem)
            {
                _particleSystem.Play();
            }
            if (_particleSystem)
            {
                
                _particleSystem.trigger.SetCollider(0, trigger);
                //_particleSystem.Stop();
            }
        }
        public void DisableParticles()
        {
            if (_forceField)
            {
                _forceField.enabled = false;
            }
            if (_particleSystem)
            {
                //_particleSystem.trigger.AddCollider(trigger);
                _particleSystem.Stop();
            }
        }

        //Added

        public void GiveNumber(int number)
        {
            _number = number;
        }

        
    }
}

