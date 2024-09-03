using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace General.UI
{
    public class UI_GameSpeedManager : MonoBehaviour
    {
        [SerializeField] private Button speedChanger;
        [SerializeField] private TextMeshProUGUI speedValueText;
        private readonly float[] speedValues = new float[4]
        {
            1.0f,
            2.0f,
            4.0f,
            0.5f
        };
        private int currentSelectedSpdValue = 0;
        private void OnEnable()
        {
            UpdateTimeScale();
            speedChanger.onClick.AddListener(OnPressedSpdChange);
            InputHandler.instance.onFastForwardKeyTap += OnFastForwardKeyTap;
        }
        private void OnDisable()
        {
            speedChanger.onClick.RemoveListener(OnPressedSpdChange);
            InputHandler.instance.onFastForwardKeyTap -= OnFastForwardKeyTap;
        }

        private void OnFastForwardKeyTap()
        {
            OnPressedSpdChange();
        }

        private void OnPressedSpdChange()
        {
            currentSelectedSpdValue++;
            if (currentSelectedSpdValue >= speedValues.Length)
                currentSelectedSpdValue = 0;

            UpdateTimeScale();
        }
        private void UpdateTimeScale()
        {
            Time.timeScale = speedValues[currentSelectedSpdValue];
            speedValueText.SetText($"Current speed:<br> {Time.timeScale}x");
        }
    }
}
