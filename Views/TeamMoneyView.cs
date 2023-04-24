using Kitchen;
using KitchenMods;
using MessagePack;
using PlateUpCompetitiveMode.Utils;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PlateUpCompetitiveMode.Views
{
    public class TeamMoneyDisplayView: UpdatableObjectView<TeamMoneyDisplayView.ViewData>
    {
        [MessagePackObject(false)]
        public struct ViewData: ISpecificViewData, IViewData.ICheckForChanges<ViewData>
        {
            [Key(1)]
            public int Money;

            [Key(2)]
            public int Team;

            public IUpdatableObject GetRelevantSubview(IObjectView view)
            {
                return view.GetSubView<TeamMoneyDisplayView>();
            }

            public bool IsChangedFrom(ViewData check)
            {
                return Money != check.Money || Team != check.Team;
            }
        }

        public TextMeshPro MoneyNumber;
        public MeshRenderer TeamColor;

        public ViewData Data;

        public override void Initialise()
        {
            Mod.LogInfo("Team Money Display init start");
            base.Initialise();
            MoneyNumber.text = $"X - 0";
            Mod.LogInfo("Team Money Display init end");
        }

        protected override void UpdateData(ViewData data)
        {
            Mod.LogInfo("Team Money Display update start");
            if (data.Money != Data.Money)
            {
                MoneyNumber.text = $"{data.Team} - {data.Money}";
            }
            if (data.Team != Data.Team)
            {
                TeamColor.material.color = TeamConstants.TeamColors[data.Team];
            }
            Data = data;
            Mod.LogInfo("Team Money Display update end");
        }
    }

    public class UpdateTeamMoneyView : IncrementalViewSystemBase<TeamMoneyDisplayView.ViewData>, IModSystem
    {
        private EntityQuery Views;

        protected override void Initialise()
        {
            base.Initialise();
            Views = GetEntityQuery(new QueryHelper()
                .All(typeof(CLinkedView), typeof(CTeamMoneyDisplay))
            );
            RequireSingletonForUpdate<SKitchenStatus>();
            RequireSingletonForUpdate<SMoney>();
        }

        protected override void OnUpdate()
        {
            using var views = Views.ToComponentDataArray<CLinkedView>(Allocator.Temp);
            using var components = Views.ToComponentDataArray<CTeamMoneyDisplay>(Allocator.Temp);

            var money = GetSingleton<SMoney>();

            for (int i = 0; i < views.Length; i++)
            {
                var view = views[i];
                var data = components[i];

                SendUpdate(view, new TeamMoneyDisplayView.ViewData
                {
                    Money = money,
                    Team = data.Team
                });
            }
        }
    }
}
