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
