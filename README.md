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


### Mocking Properties
1.Mocking properties to return a specific values
 ```c#
 mockValidator.Setup(x => x.LicenseKey)
              .Returns("EXPIRED");

```
2. Mocking properties to return a function value
3. Property Hierarchies - Auto-mocking property hierarchies
 ```c#
 mockValidator.Setup(x => x.Level1.Level2.Level3.Property)
              .Returns(GetPropertyValue);

```
4. Default value strategies for Properties.
 ```c#
mockValidator.DefaultValue = DefaultValue.Mock | DefaultValue.Empty (default)| DefaultValue.Custom 
```

5.**Tracking changes to mock properties values** - by default the mock fw doesn't keep states of mock properties. 
For example, we mock a object with a enum property, if this property is changed in the test process when the object returns to the test method, the new state of this property is lost. We can tell to the FW that keeps the state of specified properties or all properties to TRACKING CHANGES to mock properties values
 ```c#
mockValidator.SetupAllProperties();

or

mockValidator.SetupProperty(x => x.PropertyToTrack);
```
