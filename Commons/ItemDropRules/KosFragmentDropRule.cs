using Clamity.Content.Bosses.KosFragmentBosses;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Clamity.Commons.ItemDropRules
{
    public class KosFragmentDropRule : CommonDrop
    {
        public KosFragmentDropRule()
            : base(ModContent.ItemType<KosFragment>(), 1)
        {

        }
        public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
        {
            ItemDropAttemptResult result;
            if (info.player.RollLuck(chanceDenominator) < chanceNumerator)
            {
                CommonCode.DropItem(info, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
                result = default(ItemDropAttemptResult);
                result.State = ItemDropAttemptResultState.Success;
                ClamitySystem.droppedKosFramgent = true;
                return result;
            }

            result = default(ItemDropAttemptResult);
            result.State = ItemDropAttemptResultState.FailedRandomRoll;
            return result;
        }
    }
}
