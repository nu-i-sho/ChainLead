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
    .Select(Inject(Index).Into)
    .Select(WrapItUp(NewLine))
    .Aggregate(Join);
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
*(Yes, I know that bad practice to have mutable static data. I just chose this evil to provide some library language features (provided by static classes only) without losing the support of DIP (from SOLID). So, just configure it once, and you will have no problems with that.)*

In place of chain implementation use the following.   
```CSharp
using ChainLead.Contracts;
using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
```
## Handler creation
#### `MakeHandler`, `AsHandler`, `Zero`
One of two basic blocks of ChainLead is `IHandler<T>`. It is a simple interface with a single method `Execute(T state)`. We can implement it directly or create it in one of the following ways.
```CSharp
IHandler<StringBuilder> first = MakeHandler(acc => acc.Append(1));

Action<StringBuilder> action = acc => acc.Append(2); 
IHandler<StringBuilder> second = action.AsHandler();

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

Obviously, circles in the image are handlers, and bars are `Then` calls. 'Then' is directed (it has first and second parameters), and better to draw it as an arrow (but ugly). So, imagine that all vertical bars are arrows from up to down and horizontal - from left to right. Also, we know the order of `Then` calls (black numbers). Based on this, we can easily predict the order of handlers' executions (white numbers). That means `acb1` and `abc2` are logically the same chains (`abcd1` and `abcd2` too). Or, by more mathematical words - `Then` is associative.

### `Join`
`Join(a, b)` is the same as `a.Then(b)`, but I recommend not calling `Join` directly, and using it only as an object. For example
``` CSharp
var abcd = new[] { a, b, c, d }.Aggregate(Join);
```
Also, if you aren't sure that the collection of handlers you want to aggregate to chain contains at least one item - use `Handler<T>.Zero` as the starting state of the accumulator.  
``` CSharp
IHandler<int> ChainOf(IHandler<int>[] items) =>
    items.Aggregate(Handler<int>.Zero, Join);
```
### `Join(X).To`, `JoinItTo`
(In Progress)  

