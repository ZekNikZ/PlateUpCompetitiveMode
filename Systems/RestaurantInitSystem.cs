using Kitchen;
using KitchenMods;
using PlateUpCompetitiveMode.Views;
using Unity.Entities;
using UnityEngine;

namespace PlateUpCompetitiveMode.Systems
{
    [UpdateInGroup(typeof(ChangeModeGroup))]
    public class RestaurantInitSystem : GenericSystemBase, IModSystem
    {
        protected override void Initialise()
        {
            base.Initialise();
            RequireSingletonForUpdate<SCreateScene>();
        }

        protected override void OnUpdate()
        {
            if (GetSingleton<SCreateScene>().Type != SceneType.Kitchen)
            {
                return;
            }

            AddNewView(CustomViewType.TeamMoneyDisplay, new Vector3(1, 1, 0), new CTeamMoneyDisplay {
                Team = 1
            });
        }

        private Entity AddNewView<T>(ViewType view, Vector3 pos) where T : IComponentData
        {
            EntityManager entityManager = base.EntityManager;
            Entity entity = entityManager.CreateEntity();
            entityManager.AddComponent<T>(entity);
            entityManager.AddComponentData(entity, (CPosition)pos);
            entityManager.AddComponentData(entity, new CRequiresView
            {
                Type = view,
                ViewMode = ViewMode.Screen
            });
            return entity;
        }

        private Entity AddNewView<T>(ViewType view, Vector3 pos, T data) where T : struct, IComponentData
        {
            EntityManager entityManager = base.EntityManager;
            Entity entity = entityManager.CreateEntity();
            entityManager.AddComponentData(entity, data);
            entityManager.AddComponentData(entity, (CPosition)pos);
            entityManager.AddComponentData(entity, new CRequiresView
            {
                Type = view,
                ViewMode = ViewMode.Screen
            });
            return entity;
        }
    }
}
