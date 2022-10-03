﻿using Assets.Scripts.Controller;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.SceneManagement;

namespace Assets.Scripts.Model
{
    public class MissionData : MonoBehaviour
    {
        public MissionType Type;

        public int SecondsChallenge;

        public int PointsChallenge;

        public int NumberOfCollection;
        public List<string> Names;
        public List<int> Amount;
    }

    [CustomEditor(typeof(MissionData)), CanEditMultipleObjects]
    public class MissionDataEditor : Editor
    {
        MissionData missionData;

        private void OnEnable()
        {
            missionData = (MissionData)target;
        }

        public override void OnInspectorGUI()
        {
            missionData.Type = (MissionType)EditorGUILayout.EnumPopup("Mission Type:", missionData.Type);

            switch (missionData.Type)
            {
                case MissionType.LevelCompleted:
                    break;
                case MissionType.CompletionTime:
                    {
                        missionData.SecondsChallenge = EditorGUILayout.IntField(new GUIContent("Seconds Challenge:"), missionData.SecondsChallenge);
                        break;
                    }
                case MissionType.Point:
                    {
                        missionData.PointsChallenge = EditorGUILayout.IntField(new GUIContent("Points Challenge:"), missionData.PointsChallenge);
                        break;
                    }
                case MissionType.FullCollection:
                    {
                        missionData.NumberOfCollection = EditorGUILayout.IntField(new GUIContent("Number Of Collection:"), missionData.NumberOfCollection);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("Names"), new GUIContent("List Collection:"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("Amount"), new GUIContent("List Amount:"));
                        break;
                    }
            }

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkAllScenesDirty();
        }
    }
}