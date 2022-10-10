using Assets.Scripts.Controller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Assets.Scripts.Model
{
    public class CollectionInfo : MonoBehaviour
    {
        [SerializeField]
        private List<CollectionStat> stats;

        public int GetPoints(CollectionFruits name)
        {
            var state = stats.FirstOrDefault((s) => s.Name == name);

            if (state is null) return 0;

            return state.PointsEarn;
        }

        public Sprite GetSprite(CollectionFruits name)
        {
            var state = stats.FirstOrDefault((s) => s.Name == name);

            if (state is null) return null;

            return state.Image;
        }
    }

    [Serializable]
    public class CollectionStat
    {
        public CollectionFruits Name;
        public Sprite Image;
        public int PointsEarn;
    }
}