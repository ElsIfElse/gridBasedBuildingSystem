public class Fridge : Item, IElectrical,ISpeedBased
{
    private bool _isTurnedOn; public bool IsTurnedOn { get => _isTurnedOn; set => _isTurnedOn = value; }
    private float _electricityUsage; public float ElectricityUsage { get => _electricityUsage; set => _electricityUsage = value; }
    private float _speed; public float Speed { get => _speed; set => _speed = value; }

    public Fridge_SO FridgeData; 

    public override void InitializeFrom_Object(Item objectData = default)
    {
        base.InitializeFrom_Object(objectData);

        Fridge fridgeData = objectData as Fridge;
        IsTurnedOn = fridgeData.IsTurnedOn;
        ElectricityUsage = fridgeData.ElectricityUsage;
        Speed = fridgeData.Speed;
    }
}