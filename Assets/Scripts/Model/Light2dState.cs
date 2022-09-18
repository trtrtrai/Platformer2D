using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.Model
{
    public class Light2dState : MonoBehaviour
    {
        public List<Light2dStateData> Datas;

        private int current;
        private bool transfer = false;
        private float ratio;
        private new Light2D light;

        public void ActiveState(int index, Light2D light)
        {
            current = index;
            this.light = light;
            var data = Datas[current];
            light.color = data.lightColor;
            light.intensity = data.minIntensity;
            var intenPerSec = (data.maxIntensity - data.minIntensity) / data.linearSpeed;
            ratio = intenPerSec / (1 / Time.unscaledDeltaTime);
            //Debug.Log(ratio);
        }

        private void Update()
        {
            float inten;
            if (!transfer) inten = light.intensity + ratio;
            else inten = light.intensity - ratio;
            light.intensity = Mathf.Clamp(inten, Datas[current].minIntensity, Datas[current].maxIntensity);

            if (light.intensity == Datas[current].maxIntensity || light.intensity == Datas[current].minIntensity) transfer = !transfer;
        }
    }
}