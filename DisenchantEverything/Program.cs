using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;
using Noggog;
using Mutagen.Bethesda.FormKeys.SkyrimSE;

namespace DisenchantEverything
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetTypicalOpen(GameRelease.SkyrimSE, "YourPatcher.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            state.LoadOrder.PriorityOrder.Weapon().WinningOverrides().ForEach(weaponGetter =>
            {
                if (weaponGetter.HasKeyword(Skyrim.Keyword.MagicDisallowEnchanting))
                {
                    Weapon weapon = state.PatchMod.Weapons.GetOrAddAsOverride(weaponGetter);
                    weapon.Keywords!.Remove(Skyrim.Keyword.MagicDisallowEnchanting.AsGetter());
                }
            });

            state.LoadOrder.PriorityOrder.Armor().WinningOverrides().ForEach(armorGetter =>
            {
                if (armorGetter.HasKeyword(Skyrim.Keyword.MagicDisallowEnchanting))
                {
                    Armor armor = state.PatchMod.Armors.GetOrAddAsOverride(armorGetter);
                    armor.Keywords!.Remove(Skyrim.Keyword.MagicDisallowEnchanting.AsGetter());
                }
            });
        }
    }
}
