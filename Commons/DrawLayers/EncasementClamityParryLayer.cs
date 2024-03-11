namespace Clamity.Commons.DrawLayers
{
    public class EncasementClamityParryLayer : PlayerDrawLayer
    {
        public enum EncasementType
        {
            FrozenArmor
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.FrontAccFront); //me when the player layer is called front acc front :skull:

        public EncasementType GetEncasementTypeFor(ClamityPlayer modPlayer) => modPlayer.frozenParrying ? EncasementType.FrozenArmor : EncasementType.FrozenArmor;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            ClamityPlayer modPlayer = drawPlayer.Clamity();
            var encasement = GetEncasementTypeFor(modPlayer);
            bool visible = drawInfo.shadow == 0f && !drawPlayer.dead;
            visible = encasement switch
            {
                EncasementType.FrozenArmor => visible && modPlayer.frozenParryingTime > 0,
                _ => false
            };
            return visible;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            var calPlayer = drawPlayer.Clamity();
            var calPlayer2 = drawPlayer.Calamity();
            var encasement = GetEncasementTypeFor(calPlayer);
            string tex = "Clamity/Commons/DrawLayers/";
            string texPlus = "NoneParry";
            int currentParry;
            float defaultOpacity;
            float scale;
            switch (encasement)
            {
                case EncasementType.FrozenArmor:
                    texPlus = "CryoParryShield";
                    currentParry = calPlayer.frozenParryingTime;
                    defaultOpacity = 0.725f;
                    scale = 1.15f;
                    break;
                default: //should never be this option
                    texPlus = "NoneParry";
                    currentParry = 0;
                    defaultOpacity = 0f;
                    scale = 0f;
                    break;
            }
            if (calPlayer2.blazingCore || calPlayer2.flameLickedShell)
                texPlus = "NoneParry";
            tex += texPlus;


            Texture2D texture = ModContent.Request<Texture2D>(tex).Value;
            Vector2 drawPos = drawInfo.Center - Main.screenPosition + new Vector2(0f, drawPlayer.gfxOffY);
            drawPos.Y += 15f;
            drawPos.X += 15f;


            int maxParry = 30; //all parries use thirty seconds as a max at this point in time, if this changes, this too should change
            float colorIntensity = currentParry >= 18 ? defaultOpacity : 1f - Utils.GetLerpValue(maxParry, 0f, currentParry, true);
            SpriteEffects spriteEffects = drawPlayer.direction != -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            drawInfo.DrawDataCache.Add(new DrawData(texture, drawPos, null, Color.White * colorIntensity, 0f, texture.Size() * 0.75f, scale, spriteEffects, 0));
        }
    }
}
