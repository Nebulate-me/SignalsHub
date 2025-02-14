# SignalsHub
Events bus for Unity

### How to use

- Put `Dispatcher.cs` script on any gameObject in your scene.

- Create Signal struct with data you need:
```C#
public struct TestSignal
{
    public int Counter;
}
```

- Add or remove listener to signal where you need:
```C#
SignalsHub.AddListener<TestSignal>(OnTestSignal);

public void OnTestSignal(TestSignal signal)
{
    Debug.Log($"Counter: {signal.Counter}");
}

SignalsHub.AddListenerOnce<TestSignal>(OnTestSignalOnce);

public void OnTestSignalOnce(TestSignal signal)
{
    Debug.Log("One time listener fired!");
}

SignalsHub.RemoveListener<TestSignal>();
```

- Send Signal where you need:
```C#
SignalsHub.DispatchAsync<TestSignal>(new TestSignal
{
    Counter = 1337,
})
```

### How to install

In Package Manager click `+` -> `Add package from git URL...` and paste `https://github.com/Nebulate-me/SignalsHub.git`
