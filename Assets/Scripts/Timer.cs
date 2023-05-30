public class Timer
{
    private float _currentTime;

    public bool IsFinished => _currentTime <= 0;

    public Timer(float _startTime)
    {
        Start(_startTime);
    }

    public void Start(float _startTime)
    {
        _currentTime = _startTime;
    }

    public void RemoveTime(float deltaTime)
    {
        if (_currentTime <= 0) return;

        _currentTime -= deltaTime;
    }
}
