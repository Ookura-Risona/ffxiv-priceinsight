using System;
using Dalamud.Bindings.ImGui;

namespace PriceInsight;

internal class ConfigUI(PriceInsightPlugin plugin) : IDisposable {
    private bool settingsVisible = false;

    public bool SettingsVisible {
        get => settingsVisible;
        set => settingsVisible = value;
    }

    public void Dispose() {
    }

    public void Draw() {
        if (!SettingsVisible) {
            return;
        }

        var conf = plugin.Configuration;
        if (ImGui.Begin("Price Insight设置", ref settingsVisible,
                ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.AlwaysAutoResize)) {
            var configValue = conf.RefreshWithAlt;
            if (ImGui.Checkbox("按 Alt 刷新价格", ref configValue)) {
                conf.RefreshWithAlt = configValue;
                conf.Save();
            }

            configValue = conf.PrefetchInventory;
            if (ImGui.Checkbox("预获取库存中物品的价格", ref configValue)) {
                conf.PrefetchInventory = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("在登录时预获取所有背包、陆行鸟鞍囊和雇员中的物品的价格");

            configValue = conf.UseCurrentWorld;
            if (ImGui.Checkbox("将当前服务器当做原始服务器", ref configValue)) {
                conf.UseCurrentWorld = configValue;
                conf.Save();
                plugin.ClearCache();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("你当前所在的服务器将被视为你的“原始服务器”。\n如果你正在跨大区并想查看那里的的价格，这会很有用。");

            ImGui.Separator();
            ImGui.PushID(0);

            ImGui.Text("显示以下范围的最低价格：");

            configValue = conf.ShowRegion;
            if (ImGui.Checkbox("国服", ref configValue)) {
                conf.ShowRegion = configValue;
                conf.Save();
            }
            TooltipRegion();

            configValue = conf.ShowDatacenter;
            if (ImGui.Checkbox("大区", ref configValue)) {
                conf.ShowDatacenter = configValue;
                conf.Save();
            }

            configValue = conf.ShowWorld;
            if (ImGui.Checkbox("原始服务器", ref configValue)) {
                conf.ShowWorld = configValue;
                conf.Save();
            }

            ImGui.PopID();
            ImGui.Separator();
            ImGui.PushID(1);

            ImGui.Text("显示以下范围的最近购买记录：");

            configValue = conf.ShowMostRecentPurchaseRegion;
            if (ImGui.Checkbox("国服", ref configValue)) {
                conf.ShowMostRecentPurchaseRegion = configValue;
                conf.Save();
            }
            TooltipRegion();

            configValue = conf.ShowMostRecentPurchase;
            if (ImGui.Checkbox("大区", ref configValue)) {
                conf.ShowMostRecentPurchase = configValue;
                conf.Save();
            }

            configValue = conf.ShowMostRecentPurchaseWorld;
            if (ImGui.Checkbox("原始服务器", ref configValue)) {
                conf.ShowMostRecentPurchaseWorld = configValue;
                conf.Save();
            }

            ImGui.PopID();
            ImGui.Separator();

            var selectValue = conf.ShowDailySaleVelocityIn;
            if (ImGui.Combo("显示每日购买情况", ref selectValue, ["不显示", "服务器", "大区", "国服"])) {
                conf.ShowDailySaleVelocityIn = selectValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("根据过去四天的销售情况，显示平均每日销量。");

            selectValue = conf.ShowAverageSalePriceIn;
            if (ImGui.Combo("显示平均销售价格", ref selectValue, ["不显示", "服务器", "大区", "国服"])) {
                conf.ShowAverageSalePriceIn = selectValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("显示过去四天的平均售价。");

            configValue = conf.ShowStackSalePrice;
            if (ImGui.Checkbox("显示物品出售总价", ref configValue)) {
                conf.ShowStackSalePrice = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("如果以给定的单价出售，显示鼠标悬停的物品的总价。");

            configValue = conf.ShowAge;
            if (ImGui.Checkbox("显示数据更新时间", ref configValue)) {
                conf.ShowAge = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("显示距离价格信息最后一次更新过去了多久。\n可以关闭以减少提示框的冗余。");

            configValue = conf.ShowDatacenterOnCrossWorlds;
            if (ImGui.Checkbox("Show datacenter for foreign worlds", ref configValue)) {
                conf.ShowDatacenterOnCrossWorlds = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("Show the datacenter for worlds from other datacenters when displaying prices for the entire region.\nCan be turned off to reduce tooltip bloat.");

            configValue = conf.ShowBothNqAndHq;
            if (ImGui.Checkbox("始终显示NQ和HQ价格", ref configValue)) {
                conf.ShowBothNqAndHq = configValue;
                conf.Save();
            }
            if (ImGui.IsItemHovered())
                ImGui.SetTooltip("显示物品的 NQ 和 HQ 价格。\n关闭时只会显示当前品质的价格（使用 Ctrl 切换 NQ 和 HQ）。");
        }

        ImGui.End();
    }

    private static void TooltipRegion() {
        if (ImGui.IsItemHovered())
            ImGui.SetTooltip("包含通过跨大区可以前往的所有大区");
    }
}