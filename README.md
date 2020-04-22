# xUnit-Moq-Demo-2
Implementing unit testing using Moq

### Inject mock dependencies by constructor
1. Test classes that use injecting services by constructor using Moq - Mock Framework - to mock these services.

### Mocking Methods
1. Configure mock object method return values.
```c#
mockValidator.Setup(..).Returns(true);

```


2. specific value (hardcoding value way)
```c#
x => x.IsValid("x")

```

3. Argument Matching in mocked methods (avoid harcoding values in test methods)
```c#
It.IsAny
It.IsInRange
...

```

3. **Mock behaviours** *MockBehavior.Strict* | *MockBehavior.Loose **(default)***

4. Mocking methods wit **out parameters**


