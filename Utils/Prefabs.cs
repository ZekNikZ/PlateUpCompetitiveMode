using Kitchen;
using KitchenData;
using KitchenLib.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlateUpCompetitiveMode.Utils
{
    internal class Prefabs
    {
        private static readonly Dictionary<string, GameObject> PrefabCache = new();

        public static GameObject Find(int id)
        {
            return (GDOUtils.GetExistingGDO(id) as IHasPrefab)?.Prefab;
        }

        internal static GameObject Create(string name)
        {
            if (!PrefabCache.ContainsKey($"{name}Empty"))
            {
                var parent = GameObject.Find("Prefabs");
                if (parent == null)
                {
                    parent = new GameObject("Prefabs");
                    parent.transform.localPosition = Vector3.positiveInfinity;
                }

                var copy = new GameObject(name);
                copy.transform.parent = parent.transform;
                PrefabCache.Add($"{name}Empty", copy);
            }

            return PrefabCache[$"{name}Empty"];
        }

        public static GameObject Find(string name, string copyName = "")
        {
            if (!PrefabCache.ContainsKey(name + copyName))
            {
                var prefab = Mod.Bundle?.LoadAsset<GameObject>(name);
                if (prefab == null)
                {
                    Mod.LogWarning($"Mod prefab with name \"{name}\" not found in asset bundle.");
                    return null;
                }

                if (copyName != "")
                {
                    var parent = GameObject.Find("Prefabs");
                    if (parent == null)
                    {
                        parent = new GameObject("Prefabs");
                        parent.transform.localPosition = Vector3.positiveInfinity;
                    }

                    var copy = UnityEngine.Object.Instantiate(prefab, parent.transform);
                    copy.name = name + copyName;
                    PrefabCache.Add(name + copyName, copy);
                }
                else
                {
                    PrefabCache.Add(name, prefab);
                }
            }

            return PrefabCache[name + copyName];
        }

        public static GameObject Copy(ViewType viewType, string copyName, Action<GameObject> modifier = null)
        {
            if (!PrefabCache.ContainsKey($"View {(int) viewType} {copyName}"))
            {
                var prefab = Mod.VanillaAssetDirectory.ViewPrefabs[viewType];
                if (prefab == null)
                {
                    Mod.LogWarning($"Existing view prefab with view type {(int) viewType} not found.");
                    return null;
                }

                var parent = GameObject.Find("ViewPrefabs");
                if (parent == null)
                {
                    parent = new GameObject("ViewPrefabs");
                    parent.transform.localPosition = Vector3.positiveInfinity;
                    parent.SetActive(false);
                }

                var copy = UnityEngine.Object.Instantiate(prefab, parent.transform);
                copy.name = $"View {(int)viewType} {copyName}";
                if (modifier != null)
                {
                    modifier.Invoke(copy);
                }
                PrefabCache.Add($"View {(int)viewType} {copyName}", copy);
            }

            return PrefabCache[$"View {(int)viewType} {copyName}"];
        }
    }
}
