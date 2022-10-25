using Assets.Scripts.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MissionData : MonoBehaviour
    {
        public MissionType Type;

        public int SecondsChallenge;

        public int PointsChallenge;

        public int NumberOfCollection;
        public List<CollectionFruits> Names;
        public List<int> Amount;
    }
}