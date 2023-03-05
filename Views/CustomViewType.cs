using Kitchen;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PlateUpCompetitiveMode.Views
{
    public sealed class CustomViewType
    {
        public static readonly SortedList<int, CustomViewType> Values = new();
        public static readonly Dictionary<CustomViewType, GameObject> Prefabs = new();

        public static readonly CustomViewType None = new(628000);
        public static readonly CustomViewType TeamMoneyDisplay = new(628001, Utils.Prefabs.Copy(ViewType.MoneyDisplay, "TeamMoneyDisplay", (prefab) =>
        {
            UnityEngine.Object.Destroy(prefab.GetComponent<MoneyDisplayView>());

            var view = prefab.AddComponent<TeamMoneyDisplayView>();
            view.MoneyNumber = prefab.transform.GetChild(0).Find("Value").GetComponent<TextMeshPro>();

            var pos = prefab.transform.GetChild(0).localPosition;
            pos.x *= -1;
            prefab.transform.GetChild(0).localPosition = pos;
        }));

        private readonly int Value;

        private CustomViewType(int value, GameObject prefabGetter = null)
        {
            Value = value;
            Debug.Log(Values);
            Values.Add(value, this);
            if (prefabGetter != null)
            {
                Prefabs.Add(this, prefabGetter);
            }
        }

        public static implicit operator CustomViewType(int value)
        {
            return Values[value];
        }

        public static implicit operator int(CustomViewType value)
        {
            return value.Value;
        }

        public static implicit operator CustomViewType(ViewType value)
        {
            if (Values.TryGetValue((int) value, out var res))
            {
                return res;
            }
            else
            {
                return None;
            }
        }

        public static implicit operator ViewType(CustomViewType value)
        {
            return (ViewType)value.Value;
        }
    }
}
