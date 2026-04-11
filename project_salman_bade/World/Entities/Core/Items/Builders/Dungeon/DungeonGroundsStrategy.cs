namespace ConOOD_RpgGame.World.Entities.Core.Items.Builders.Dungeon
{
    public class DungeonGroundsStrategy : IDungeonStrategy
    {
        public void Configure(DungeonBuilder builder)
        {
            builder
                .StartWithFilledDungeon()
                .AddStep(new AddCentralRoomStep(8, 6))
                .AddStep(new AddPathsStep(80))
                .AddStep(new AddChambersStep(5, 3, 6))
                .AddStep(new AddItemsStep(8))
                .AddStep(new AddWeaponsStep(5))
            .AddStep(new AddEnemiesStep(4));
        }
    }
}