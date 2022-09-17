using System;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [Serializable]
    public class Light2dStateData
    {
        public Color lightColor;
        public float minIntensity;
        public float maxIntensity;
        public float linearSpeed;
    }
}