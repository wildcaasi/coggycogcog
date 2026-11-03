namespace GearPuzzle
{
    public readonly struct Puzzle
    {
        public Cell DriverCell { get; }
        public Cell ReceiverCell { get; }
        public GearState DriverDirection { get; }
        public GearState TargetDirection { get; }

        public Gear Driver => new Gear(DriverCell, GearSize.Small);
        public Gear Receiver => new Gear(ReceiverCell, GearSize.Small);

        public Puzzle(Cell driverCell, Cell receiverCell, GearState driverDirection, GearState targetDirection)
        {
            DriverCell = driverCell;
            ReceiverCell = receiverCell;
            DriverDirection = driverDirection;
            TargetDirection = targetDirection;
        }
    }
}
