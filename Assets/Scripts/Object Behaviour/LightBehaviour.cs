using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.ObjectBehaviour
{
    public class LightBehaviour : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> lightObjects;

        private List<Light2dState> states;
        private List<Light2D> light2Ds;

        private void Awake()
        {
            states = new List<Light2dState>();
            light2Ds = new List<Light2D>();

            for (int i = 0; i < lightObjects.Count; i++)
            {
                //get Component
                states.Add(lightObjects[i].GetComponent<Light2dState>());
                light2Ds.Add(lightObjects[i].GetComponent<Light2D>());

                // set initiate
                var lData = states[i].Datas[0];
                light2Ds[i].intensity = lData.minIntensity;
                light2Ds[i].color = lData.lightColor;

                states[i].ActiveState(0, light2Ds[i]);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        public void ActiveState(int state) { for (int i = 0; i < lightObjects.Count; i++) states[i].ActiveState(state, light2Ds[i]); }
    }
}