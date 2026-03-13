public class Sink : Item,IOccupiable
{
    public Sink_SO SinkData;

    private bool _isOccupied; public bool IsOccupied { get => _isOccupied; set => _isOccupied = value; }

    public override void InitializeFrom_Object(Item objectData = default)
    {
        base.InitializeFrom_Object(objectData);
    }
}