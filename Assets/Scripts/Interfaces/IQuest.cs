
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Interfaces
{
    public interface IQuest<T>
    {
        public List<T> ListGeneric { get; set; }
        public void AfterCheck(int i, bool result, bool loop = true);
        public int ListCheckedCount();
        public void Render(List<string> labels, Action<int> action);
    }
}