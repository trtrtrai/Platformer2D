using UnityEngine;
using TMPro;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        TMP_Text tmpTimer;

        public event TimeOut TimeOutEvent;
        public event TimeOutAsync TimeOutAsyncEvent;

        public void UpdateTimer(float sec)
        {
            if (sec < 0)
            {
                TimeOutEvent?.Invoke();
                TimeOutAsyncEvent?.Invoke();
                //Debug.Log("Wrong argument");
            }
            else
            {
                tmpTimer.text = ConvertToTimeForm(sec) + "s";
            }
        }

        private string ConvertToTimeForm(float sec)
        {
            int h = Mathf.FloorToInt(sec / 3600);
            sec -= h * 3600;
            int m = Mathf.FloorToInt(sec / 60);
            sec -= m * 60;
            int s = Mathf.FloorToInt(sec);

            return h == 0 ? $"{IntToString(m)}:{IntToString(s)}" : $"{IntToString(h)}:{IntToString(m)}:{IntToString(s)}";
        }

        private string IntToString(int num) => num < 10 ? $"0{num}" : num.ToString();

        public delegate void TimeOut();
        public delegate Task TimeOutAsync();
    }
}