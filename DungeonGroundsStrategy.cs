namespace ConOOD_RpgGame
{
    public class DungeonGroundsStrategy : IDungeonStrategy
    {
        public void Configure(DungeonBuilder builder)
        {
            builder
                .StartWithFilledDungeon()
                .AddStep(new AddCentralRoomStep(8, 6))
                .AddStep(new AddPathsStep(5, 18, 35))
                .AddStep(new AddChambersStep(5, 3, 6))
                .AddStep(new AddItemsStep(8))
                .AddStep(new AddWeaponsStep(4));
        }
    }
}