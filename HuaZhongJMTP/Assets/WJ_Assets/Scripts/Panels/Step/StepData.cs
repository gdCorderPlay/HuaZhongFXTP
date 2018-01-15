using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
    /// <summary>
    /// 第一层步骤 
    /// </summary>
    public class StepData
    {
        public string title;
        public List<StepItemData> items;

        public StepData(string _title,List<StepItemData> _items)
        {
            this.title = _title;
            this.items = _items;
        }
    }
}

