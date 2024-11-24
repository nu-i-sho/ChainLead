# ChainLead

ChainLead is a library for creating flexible and maintainable process chains with high human (not engineer) readability, like the following:

```CSharp
public static IHandler<State> Hamburger =>
    new[]
    {
        Cut(bun),
        Slice(pickle)
            .Then(Again.When(OrderIncludeAdditional(pickle))),
        Dice(onion)
            .When(Not(OrderExclude(onion))),
        Salt(beefPatty),
        Pepper(beefPatty)
            .When(Not(OrderExclude(pepper))),
        Fry(beefPatty),
        Fry(bacon)
            .When(OrderIncludeAdditional(bacon)),
        Toast(bun),
        Put(bottomBun),
        Put(beefPatty),
        Add(bacon)
            .When(OrderIncludeAdditional(bacon)),
        Add(tomatoKetchup),
        Add(pickleSlices)
            .Then(Again.When(OrderIncludeAdditional(pickle))),
        Add(onion)
            .When(Not(OrderExclude(onion))),
        Add(mustard)
            .When(Not(OrderExclude(mustard))),
        Put(topBun)
    }
    .Select(Pack(Index).In(Dot).ThenIn(Space).ThenIn)
    .Select(XCover(NewLine).WhereXIs)
    .Aggregate(FirstThenSecond);
```
*(You can see this code in context and more [here](https://github.com/nu-i-sho/ChainLead/blob/main/Test/BurgerExampleAsTest.cs).)*

## To use ChainLead
it is necessary to configure the library (syntax) language by providing the implementations of chain mathematics. But do it once in your solution assembly point (where Dependency Injection is configured). 
```CSharp
using ChainLead.Contracts;
using ChainLead.Implementation;
using ChainLead.Contracts.Syntax;

...
    IConditionMath conditionMath = new ConditionMath();
    IHandlerMath handlerMath = new HandlerMath(conditionMath);

    ChainLeadSyntax.ConfigureChainLeadSyntax
        .WithHandlerMath(handlerMath)
        .AndWithConditionMath(conditionMath);
...

```
*(Yes, I know that it is bad practice to have mutable static data. I compromised with that to provide some library language features (provided by static classes only) without losing the support of DIP (from SOLID). So, just configure it once, and you will have no problems with that.)*

Use the following 'usings' in source files where you build the chains to make the library syntax available.   
```CSharp
using ChainLead.Contracts;
using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
```
## Handler creation
#### `MakeHandler`, `AsHandler`, `Zero`
One of two basic blocks of ChainLead is `IHandler<T>`. It is a simple interface with a single method `void Execute(T state)`. We can implement it directly or create it in one of the following ways.
```CSharp
var addOne = MakeHandler<StringBuilder>(acc => acc.Append(1));

Action<StringBuilder> action = acc => acc.Append(2);
IHandler<StringBuilder> addTwo = action.AsHandler();

IHandler<StringBuilder> zero = Handler<StringBuilder>.Zero;
```
The last one does nothing, but it is not the same as a custom-implemented handler that does nothing. (The difference is below.)
## Handlers chain building
#### `Then`
We can create a new handler from two handlers. The execution of the new handler is the execution of both source handlers one by one.
```CSharp
var addA = MakeHandler<StringBuilder>(acc => acc.Append("A"));
var addB = MakeHandler<StringBuilder>(acc => acc.Append("B"));

var addAB = printA.Then(printB);
```
So, we can create the chain with any number of handlers. By the fact, it is a tree. We imagine the chain as a collection, but it doesn't matter because all executions of items in our tree have a determined and predictable order. The following code and image show how it works.
```CSharp
var ab = a.Then(b);

var abc1 = a.Then(b).Then(c);
var abc2 = a.Then(b.Then(c));

var abcd1 = a.Then(b).Then(c).Then(d);
var abcd2 = (a.Then(b)).Than(c.Then(d));
```
![ab, abc1, abc2, abcd1, abcd2](https://raw.githubusercontent.com/nu-i-sho/ChainLead/refs/heads/main/readme_img/2.svg)

Obviously, circles in the image are handlers, and bars are `Then` calls. 'Then' is directed (it has the first and the second parameters). All vertical bars join the first up handler with the second down and horizontal, and the first left with the second right. Also, we know the order of `Then` calls (black numbers). Based on this, we can easily predict the order of handlers' executions (white numbers). That means `abc1` and `abc2` are logically the same chains (`abcd1` and `abcd2` too). Or, in mathematical terms - `Then` is associative.

## Conditions creation
#### `MakeCondition`, `AsCondition`, `True`,  `False`
The second atom (basic block) of the ChainLead library is a condition - `ICondition<T>`. It contains a single method `bool Check(T state)` and its object can be created in one of the following ways (in addition to the custom interface implementation).
```CSharp
var isEmpty = MakeCondition<StringBuilder>(acc => acc.Length == 0);

Func<StringBuilder, bool> checkIsToBig = acc => acc.Length > 10000;
ICondition<StringBuilder> isToBig = checkIsToBig.AsCondition();

ICondition<StringBuilder> @true = Condition<StringBuilder>.True;
ICondition<StringBuilder> @false = Condition<StringBuilder>.False;
```

`Condition<T>.True` and `Condition<T>.False` checks return `true` and `false` accordingly, regardless of the input arguments.  

## Conditional handlers
#### `When`
We can use the extension method `When` on the handler and make it conditional. The conditional handler has the same type/interface as the original. It will be executed only in the case when all its (attached) conditions return true.
```CSharp
IHandler<State> goHomeWhenWeatherIsRainy = goHome.When(weatherIsRainy);

doSomething = doSomething
    .When(weatherIsSunny)
    .When(haveGoodMood);
```

## Conditions combinations 
#### `And`, `Or`, `Not`
Also, we can combine conditions with each other. For example, a double `When` in the previous code snippet can be replaced with an `And` expression.
```CSharp
ICondition<State> itIsNiceTimeToWork = weatherIsSunny.And(haveGoodMood);

doSomething = doSomething.When(itIsNiceTimeToWork);
jackDoesSomething = jackDoesSomething.When(itIsNiceTimeToWork);
bobDoesSomething = bobDoesSomething.When(itIsNiceTimeToWork);
danielDoesSomething = danielDoesSomething.When(itIsNiceTimeToWork);
```
`Or` and `Not` don't need an explanation. Instead, let's look at them in the context of use.
```CSharp
var passIsProhibited = foundDrugs.Or(foundWeapon).And(Not(isFbiAgent));

var securePass = pass.When(Not(passIsProhibited));
```
#### `WithConditionThat`, `FirstThenSecond`
Also, we can make a collection of handlers conditional by `Select(WithConditionThat)` and aggregate it into a single handler by `Aggregate(FirstThenSecond)`.
```CSharp
IHandler<State> workers = new[]
    {
         jackDoesSomething,
         bobDoesSomething,
         danielDoesSomething
    }
    .Select(WithConditionThat(itIsNiceTimeToWork))
    .Aggregate(FirstThenSecond);
```
Of course, we can do the same aggregation with `Aggregate(Then)`, but it is preferable to use `FirstThenSecond` for consistency with the methods described below.

## Chain is lazy
Currently, we have enough functionality described to call our library useful. I was not directly pointed, but everybody understands that chains built with ChainLead are lazy, and no handler or condition will be called without 'Execute' or 'Check' calls. The following images help us to memorize what a chain is. The left image demonstrates the chain as an object, and the right shows it during execution. This image is bound with the following code snipped.
```CSharp
IHandler<state> handler =
    new[] { a, b.When(f), c, d.When(g), e }
    .Select(WithConditionThat(h))
    .Aggregate(FirstThenSecond);

State s = ...
handler.Execute(s);
```
![chain execution](https://raw.githubusercontent.com/nu-i-sho/ChainLead/refs/heads/main/readme_img/3.svg)

We can imagine chain execution as a trip of the state through the handlers that mutate it under the control of the conditions. 

## Correct responsibility
It is bad practice to change the state in conditions because it is an erosion of responsibility. You can protect your code from this erosion by separating access to the state by two inherited interfaces. And use the read-only interfaced state for the conditions and the full-access interfaced state for the handlers. ChainLead provides (co/contra)variance to make it possible. Like the following
```CSharp
interface IReadOnlyState
{
    int A { get; }
    int B { get; }
    int C { get; }
}

abstract class IState : IReadOnlyState
{
    public virtual int A { get; set; }
    public virtual int B { get; set; }
    public virtual int C { get; set; }
}

class State : IState { }

...
var setA1 = MakeHandler<IState>(s => s.A = 1);
var setB5 = MakeHandler<IState>(s => s.B = 5);
var setC3 = MakeHandler<IState>(s => s.C = 3);
var setA5 = MakeHandler<IState>(s => s.C = 5);

var aEquals1 = MakeCondition<IReadOnlyState>(s => s.A == 1);
var bEquals5 = MakeCondition<IReadOnlyState>(s => s.B == 5);
var cEquals3 = MakeCondition<IReadOnlyState>(s => s.C == 3);

var handler = new[]
{
    setA1,
    setB5.When(aEquals1),
    setC3.When(bEquals5),
    setA5.When(cEquals3)
}
.Aggregate(FirstThenSecond);

handler.Execute(new State());
...
```
The responsibility of the condition is to check the state to comply with the capsulated *condition* and nothing more. If you add some side effect to the condition, be aware that it is not guaranteed to be called. ChainLead relies on the correct implementation of the `ICondition<T>` and doesn't care about side effects. For example, the following conditions do not call `a` because it doesn't make sense as ChainLead predicts the result without calling.  
```CSharp
var falseAndA = Condition<State>.False.And(a); // anything and false = false
var aAndFalse = a.And(Condition<State>.False); // false and anything = false

var trueOrA = Condition<State>.True.Or(a) // true or anything = true
var aOrTrue = a.Or(Condition<State>.True) // anything or true = true
```
#### `IsPredictableTrue`, `IsPredictableFalse`
We can check whether the condition is a `True` or `False` object by the following extension methods.
```CSharp
Assert.IsTrue(falseAndA.IsPredictableFalse());
Assert.IsTrue(aAndFalse.IsPredictableFalse());

Assert.IsTrue(trueOrA.IsPredictableTrue());
Assert.IsTrue(aOrTrue.IsPredictableTrue());
```
#### `IsZero` 
Also, we can check whether the handler is a `Zero` object. There is one more case of an uncalled condition when it is attached to a zero handler because there is no reason to check something to do nothing.

```CSharp
var handler = Handler<State>.Zero.When(something);
Assert.IsTrue(handler.IsZero());
```
*(Possibly, ChainLead will be extended with additional optimizations. Boolean algebra has the potential to do that. It is one more why relying on side effects in conditions is a bad idea. )* 

## Advanced chain building
#### `Pack(o).In`
Imagine that we have a good readable chain where some items are conditional 
```CSharp
var handler = new[]
    {
        addNewEmployee,
        addEmployeeManager.When(employeeHasManager),
        addDepartment.When(Not(employeeIsFreeContractor)),
        informOnboardingSystem,
        notifyEmployeesAboutFirstWorkingDay.When(Not(employeeStartedWork)),
    }
    .Aggregete(FirstThenSecond);
```
and want to add state logging before and after each handler execution. At first look, it is not a problem and we can update the building expression like the following
```CSharp
var handler = new[] { /* steps list */ }
    .Select(logState.Then)
    .Aggregete(FirstThenSecond)
    .Then(logState);
```
Everything looks great to me. But it has one problem. When conditions attached to handlers during execution return false, relevant handlers will not be executed but we will have redundant calls of `logState`. So, we need to couple our logging handler with each handler in our list and mix domain logic with copy-pasted technical details.
```CSharp
var handler = new[]
    {
        logState.Then(addNewEmployee),
        logState.Then(addEmployeeManager).When(employeeHasManager),
        logState.Then(addDepartment).When(Not(employeeIsFreeContractor)),
        logState.Then(informOnboardingSystem),
        logState.Then(notifyEmployeesAboutFirstWorkingDay).When(Not(employeeStartedWork)),
    }
    .Aggregete(FirstThenSecond)
    .Then(logState);
```
To avoid it we need some function that can couple two handlers but use the condition attached to the second one as the condition attached to the coupling result instead. ChainLead provides this function `Pack(first).In(Second)`, which can be used as in the code snipped below.
```CSharp
var handler = new[] { /* steps list */ }
    .Select(Pack(logState).In)
    .Aggregete(FirstThenSecond)
    .Then(logState);
```    
And our chain will have no extra logging handler calls as we want.
#### `Use(o).ToCover` 
The same problem exists when we want to add the handler to each handler in the main list to the end but under conditions attached to them. So, ChainLead has a function for this situation too. The following example shows the situation when we want to open a transaction before each handler call and close after.
```CSharp
var handler = new[]
    {
        addNewEmployee,
        addEmployeeManager.When(employeeHasManager),
        addDepartment.When(Not(employeeIsFreeContractor))
    }
    .Select(Pack(openTransaction).In)
    .Select(x => Use(x).ToCover(closseTransactin))
    .Aggregete(FirstThenSecond);
```
#### `XCover(o).WhereXIs`
If you like lambda expressions free code like me, you can use `XCover(closseTransactin).WhereXIs` instead of `x => Use(x).ToCover(closseTransactin)`.
```CSharp
var handler = new[] { /* db operations handlers list*/ }
    .Select(Pack(openTransaction).In)
    .Select(XCover(closseTransactin).WhereXIs)
    .Aggregete(FirstThenSecond);
```
#### `PackXInto(o).WhereXIs`
Of course, analogical syntax construction for reversed arguments order calls of `Pack(o).In` exists too. It is `PackXIn(o).WhereXIs`. 

#### `PackFirstInSecond`, `FirstCoverSecond`
'Pack' and 'Cover' have useful forms for aggregation, they are not carried `PackFirstInSecond` and `FirstCoverSecond`. It is not easy to find a good example where `Aggaragete(PackFirstInSecond)` or `Aggaragete(FirstCoverSecond)` is apt. But as you remember we are talking about "Advanced chain building" and using it in each real-life situation is not expected. 


The following image summarizes how the 'Pack' and 'Cover' functions work.
![Pack and Cover](https://raw.githubusercontent.com/nu-i-sho/ChainLead/refs/heads/main/readme_img/4.svg)
(In Progress)  
