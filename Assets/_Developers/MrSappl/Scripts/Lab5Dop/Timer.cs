using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action OnTimeIsUp;
    [SerializeField] private float _gameTimeInSeconds;
    [SerializeField] private float _time;
    [SerializeField] private TMP_Text _timerCountText;
    public bool ActivateTimer = false;

    private void Start()
    {
        ResetTimer();
    }

    public void Activate()
    {
        ActivateTimer = true;
        ResetTimer();
    }

    public void Deactivate()
    {
        ActivateTimer = false;
    }

    private void Update()
    {
        if (!ActivateTimer)
        {
            return;
        }

        if (_time > 0)
        {
            _time -= Time.deltaTime;
            UpdateTextTimer();
            return;
        }

        _time = 0;
        UpdateTextTimer();
        OnTimeIsUp?.Invoke();
        ActivateTimer = false;
    }

    public void ResetTimer()
    {
        _time = _gameTimeInSeconds;
        UpdateTextTimer();
    }

    private void UpdateTextTimer()
    {
        _timerCountText.text = _time.ToString("0");
    }
}
