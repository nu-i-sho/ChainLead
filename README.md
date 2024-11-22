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
*(You can see fully contexted code [here](https://github.com/nu-i-sho/ChainLead/blob/main/Test/BurgerExampleAsTest.cs).)*

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
*(Yes, I know that it is bad practice to have mutable static data. I just chose this evil to provide some library language features (provided by static classes only) without losing the support of DIP (from SOLID). So, just configure it once, and you will have no problems with that.)*

In place of chain implementation use the following.   
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
So, we can create the chain with any number of handlers. By the fact, it will be a tree. We imagine the chain as a collection, but it doesn't matter because all executions of items in our tree have a determined and predictable order. The following code and image show how it works.
```CSharp
var ab = a.Then(b);

var abc1 = a.Then(b).Then(c);
var abc2 = a.Then(b.Then(c));

var abcd1 = a.Then(b).Then(c).Then(d);
var abcd2 = (a.Then(b)).Than(c.Then(d));
```
![ab, abc1, abc2, abcd1, abcd2](/readme_img/2.svg)

Obviously, circles in the image are handlers, and bars are `Then` calls. 'Then' is directed (it has first and second parameters)/ All vertical bars join the first up handler with the second down and horizontal - the first left with the second right. Also, we know the order of `Then` calls (black numbers). Based on this, we can easily predict the order of handlers' executions (white numbers). That means `acb1` and `abc2` are logically the same chains (`abcd1` and `abcd2` too). Or, by more mathematical words - `Then` is associative.

## Conditions creation
#### `MakeHandler`, `AsHandler`, `Zero`
The second atom (basic block) of the ChainLead library is a condition object `ICondition<T>. It contains a single method `bool Check(T state)` and its object can be created in one of the following ways (in addition to custom interface implementation).
```CSharp
var isEmpty = MakeCondition<StringBuilder>(acc => acc.Length == 0);

Func<StringBuilder, bool> checkIsToBig = acc => acc.Length > 10000;
ICondition<StringBuilder> isToBig = checkIsToBig.AsCondition();

ICondition<StringBuilder> @true = Condition<StringBuilder>.True;
ICondition<StringBuilder> @false = Condition<StringBuilder>.False;
```

`Condition<T>.True` and `Condition<T>.False` checks return `true` and `false` accordingly regardless of the input arguments.  

## Conditional handlers
#### `When`
We can use the extension method `When` on the handler and make it conditional. The conditional handler has the same type/interface as the original. It will be executed only in the case when all its (attached) conditions return true.
```CSharp
IHandler<State> goHomeWhenWeatherIsRainy = goHome.When(weatherIsRainy);

doSomething = doSomething
    .When(weatherIsSunny)
    .When(haveGoodMood);
```

## Condition expressions
#### `And`, `Or`, `Not`
Also, we can combine predicates from predicates. For example, a double `When` in the previous code snippet can be replaced with an `And` expression.
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
Also, we can make a collection of handlers conditional by `Select(WithConditionThat)` and aggregate it to a single handler by `Aggregate(FirstThenSecond)`.
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
Of course, we can do the same aggregation with `Aggregate(Then)`, but it is better to prefer `FirstThenSecond` for consistency with the methods described below.

## Chain is lazy
Currently, we have a described minimum enough functionality to call our library useful. I was not directly pointed, but everybody understood that chains built with ChainLead are lazy, and no one handler or condition will be called without 'Execute' or 'Check' calls. The following images help us memorize what a chain is. The left image demonstrates the chain as an object, and the right shows it during execution. This image is bound with the following code snipped.
```CSharp
IHandler<state> handler =
    new[] { a, b.When(f), c, d.When(g), e }
    .Select(WithConditionThat(h))
    .Aggregate(FirstThenSecond);

State s = ...
handler.Execute(s);
```
![chain execution](/readme_img/3.svg)

We can imagine chain execution as a trip of the state through the handlers that mutate it under the control of the conditions. 

## Correct responsibility
It is bad practice to change the state in predicates because it is an erosion of responsibility. You can protect your code from this erosion by separating access to the state by two inherited interfaces. And use the read-only interfaced state for the conditions and the full-access interfaced state for the handlers. ChainLead provides (co/contra)variance to make it possible. Like the following
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
The responsibility of the condition is to check the state to comply with the capsulated *condition* and nothing more. If you added some side effect to the condition, be aware that it is not guaranteed to be called. ChainLead relies on the correct implementation of the `ICondition<T>` and doesn't care about side effects. For example, the following conditions do not call `a`.  
```CSharp
var falseAndA = Condition<State>.False.And(a); // anything and false = false
var aAndFalse = a.And(Condition<State>.False); // false and anything = false

var trueOrA = Condition<State>.True.Or(a) // true or anything = true
var aOrTrue = a.Or(Condition<State>.True) // true or anything = true
```
(In Progress)  
