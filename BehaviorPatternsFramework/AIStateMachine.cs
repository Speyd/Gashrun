

using SFML.Window;

namespace BehaviorPatternsFramework;
public class AIStateMachine
{
    private IAIBehavior? _currentState = null;
    private readonly Dictionary<Type, IAIBehavior> _behavior = new();
    private readonly Dictionary<(Type, GameEventType), Type> _transitions = new();
    private AIContext _context;

    public AIStateMachine(AIContext context)
    {
        _context = context;
    }
    public AIStateMachine()
    {}


    public void SetAIContext(AIContext context)
    {
        _context = context;
    }

    public void AddBehavior(IAIBehavior state)
    {
        _behavior[state.GetType()] = state;
    }
    public void AddTransition<TFrom, TTo>(GameEventType trigger)
       where TFrom : IAIBehavior
       where TTo : IAIBehavior
    {
        _transitions[(typeof(TFrom), trigger)] = typeof(TTo);
    }

    public void SetBehavior<T>() where T : IAIBehavior
    {
        if (_currentState is not null)
            _currentState.Exit(_context);

        _currentState = _behavior[typeof(T)];
        _currentState.Enter(_context);
    }

    public void Update()
    {
        if (_currentState == null) return;

        _currentState.Update(_context);

        var evt = _currentState.GetNextEvent(_context);
        if (_transitions.TryGetValue((_currentState.GetType(), evt), out var nextType))
        {
            SetBehavior(nextType);
        }
    }

    private void SetBehavior(Type next)
    {
        if (_behavior.TryGetValue(next, out var state))
        {
            _currentState?.Exit(_context);
            _currentState = state;
            _currentState.Enter(_context);
        }
    }
}

