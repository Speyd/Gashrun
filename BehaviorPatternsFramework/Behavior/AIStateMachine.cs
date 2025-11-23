using BehaviorPatternsFramework.Enum;
using NGenerics.Extensions;
using System.Linq;

namespace BehaviorPatternsFramework.Behavior;
public class AIStateMachine
{
    private IAIBehavior? _currentState = null;
    private readonly List<IAIBehavior> _behaviors = new();
    private readonly Dictionary<(Type, BehaviorStatus), List<Type>> _transitions = new();
    private int _currentIndex = 0;

    public AIContext? Context { get; private set; } = null;

    public bool IsPassive { get; private set; } = false;
    public bool IsCompleted { get; private set; } = false;
    public bool IsSignaled { get; private set; } = false;
    public bool IsRunning { get; private set; } = false;


    public AIStateMachine(AIContext context, bool isPassive = false)
    {
        Context = context;
        IsPassive = isPassive;
    }
    public AIStateMachine(bool isPassive = false)
    {
        IsPassive = isPassive;
    }


    public void SetAIContext(AIContext context) => Context = context;

    public void AddBehavior(IAIBehavior state) => _behaviors.Add(state);


    public void AddTransition<TFrom, TTo>(BehaviorStatus trigger)
     where TFrom : IAIBehavior
     where TTo : IAIBehavior
    {
        var key = (typeof(TFrom), trigger);
        if (!_transitions.TryGetValue(key, out var list))
            _transitions[key] = list = new List<Type>();

        list.Add(typeof(TTo));
    }

    public void AddTransition(Type from, Type to, BehaviorStatus trigger)
    {
        if (!typeof(IAIBehavior).IsAssignableFrom(from))
            throw new ArgumentException("Type must implement IAIBehavior", nameof(from));
        if (!typeof(IAIBehavior).IsAssignableFrom(to))
            throw new ArgumentException("Type must implement IAIBehavior", nameof(to));

        var key = (from, trigger);
        if (!_transitions.TryGetValue(key, out var list))
            _transitions[key] = list = new List<Type>();

        list.Add(to);
    }


    public void SetBehavior<T>() where T : IAIBehavior
    {
        if (_currentState is not null)
            _currentState.Exit(Context);

        _currentState = _behaviors.OfType<T>().FirstOrDefault();
        _currentState?.Enter(Context);
    }
    public void SetBehavior(Type behaviorType)
    {
        if (!typeof(IAIBehavior).IsAssignableFrom(behaviorType))
            throw new ArgumentException("Type must implement IAIBehavior", nameof(behaviorType));

        if (_currentState is not null)
            _currentState.Exit(Context);

        _currentState = _behaviors.FirstOrDefault(b => b.GetType() == behaviorType);
        _currentState?.Enter(Context);
    }



    public void Signal()
    {
        if (IsPassive)
            IsSignaled = true;
    }
    public void Unsignal()
    {
        if (IsPassive && IsSignaled)
        {
            _currentState = _behaviors.FirstOrDefault();
            _currentIndex = 0;
            IsSignaled = false;
        }
    }

    public void Update()
    {
        if (_currentState is null || Context is null)
            return;

        if (IsPassive && !IsSignaled)
            return;

        _currentState.Update(Context);
        var key = (_currentState.GetType(), _currentState.Status);
        IsRunning = _transitions.Count > 0? _transitions.ContainsKey(key): _currentState.Status != BehaviorStatus.Failure;

        if (_behaviors.Count > 1)
            ProcessTransitions(key);
        else if(IsRunning)
            SetBehavior(_currentState);

        HandleCompletionIfNeeded();
    }

    private bool ProcessTransitions((Type, BehaviorStatus) key)
    {
        if (!_transitions.TryGetValue(key, out var nextTypes))
            return false;

        int currentIndex = _behaviors.IndexOf(_currentState!);
        var candidatesWithDistance = new Dictionary<IAIBehavior, int>();

        foreach (var nextType in nextTypes)
        {
            var candidates = _behaviors
                .Select((b, i) => new { Behavior = b, Index = i })
                .Where(x => nextType.IsInstanceOfType(x.Behavior))
                .ToList();

            var closest = candidates
                .OrderBy(x => GetIndex(candidates.Count, currentIndex, x.Index))
                .FirstOrDefault();

            if (closest is not null && closest.Behavior != _currentState)
                candidatesWithDistance[closest.Behavior] = GetIndex(candidates.Count, currentIndex, closest.Index);
        }

        var best = candidatesWithDistance
            .OrderBy(c => c.Value)
            .FirstOrDefault();


        if (best.Key is not null)
        {
            SetBehavior(best.Key);
            return true;
        }

        return false;
    }

    private int GetIndex(int maxIndex, int currentIndex, int x)
    {
        if(currentIndex == maxIndex)
            return Math.Abs(currentIndex - (currentIndex - x));
        else
            return Math.Abs(currentIndex < maxIndex ? x - 1 - currentIndex : currentIndex - maxIndex);
    }

    private void HandleCompletionIfNeeded()
    {
        if (_currentIndex < _behaviors.Count)
            return;

        IsCompleted = true;

        if (IsPassive)
        {
            IsSignaled = false;
            _currentIndex = 0;
        }
        else
        {
            _currentIndex = 0;
            IsCompleted = false;
        }
    }

    private void SetBehavior(IAIBehavior? nextState)
    {
        if (Context is null || nextState is null)
        {
            return;
        }

        _currentIndex++;
        _currentState?.Exit(Context);
        _currentState = nextState;
        _currentState?.Enter(Context);
    }

    public AIStateMachine GetDeepCopy()
    {
        var newStateMachine = new AIStateMachine();
        newStateMachine.IsPassive = IsPassive;

        _behaviors.ForEach(b => newStateMachine.AddBehavior(b.GetDeepCopy()));

        foreach (var kvp in _transitions)
        {
            foreach (var toType in kvp.Value)
            {
                newStateMachine.AddTransition(kvp.Key.Item1, toType, kvp.Key.Item2);
            }
        }

        if(_currentState is not null)
            newStateMachine.SetBehavior(_currentState.GetType());

        return newStateMachine;
    }


}