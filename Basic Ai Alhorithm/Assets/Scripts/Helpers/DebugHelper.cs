using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using TMPro;
using DG.Tweening;
using General.Essencial;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;
namespace DebugHelper
{
    public static class DebugHelper
    {
        public static void DebugHowLongActionTake(Action action, out float elapsedTimeInMs)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            action?.Invoke();
            sw.Stop();
            elapsedTimeInMs = sw.ElapsedMilliseconds;
        }
        public static void TextPopup(Vector3 worldPosition, string text,Color color,float duration)
        {
            GameObject gameObject = new GameObject();
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            TextMeshProUGUI tmpro = gameObject.AddComponent<TextMeshProUGUI>();
            tmpro.fontSize = 1f;
            gameObject.transform.SetParent(GameManager.instance.worldSpaceCanv.transform, worldPositionStays:true);
            rectTransform.sizeDelta = Vector3.one * 0.5f;
            rectTransform.position = worldPosition + Random.Range(-0.5f, 0.5f) * Vector3.one;
            rectTransform.Rotate(new Vector3(76.7f, Random.Range(-5,5), Random.Range(-5, 5)));
            tmpro.SetText(text);
            tmpro.color = color;
            tmpro.enableWordWrapping = false;
            tmpro.fontStyle = FontStyles.Bold;
            rectTransform.DOAnchorPos3DZ(worldPosition.z + 1f, duration);
            tmpro.DOColor(endValue: new Color(tmpro.color.r, tmpro.color.g, tmpro.color.b, 0f), duration).SetEase(Ease.InQuart);
            DestroySelf destroySelf = gameObject.AddComponent<DestroySelf>();
            //safety reason for dotween to endeverything
            destroySelf.StartCounting(duration + 1f);
            Debug.Log(text);
        }
    }
}
