public class Oven : Item, IElectrical,ISpeedBased,IOccupiable
{
    private bool _isTurnedOn; public bool IsTurnedOn { get => _isTurnedOn; set => _isTurnedOn = value; }
    private float _electricityUsage; public float ElectricityUsage { get => _electricityUsage; set => _electricityUsage = value; }
    private float _speed; public float Speed { get => _speed; set => _speed = value; }
    private bool _isOccupied; public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }

    public Oven_SO OvenData; 
    
    public override void InitializeFrom_Object(Item objectData = default)
    {
        base.InitializeFrom_Object(objectData);

        Oven oven = objectData as Oven;
        IsTurnedOn = oven.IsTurnedOn;
        ElectricityUsage = oven.ElectricityUsage;
        Speed = oven.Speed;
    }
}