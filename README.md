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
