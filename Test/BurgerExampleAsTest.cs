namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Implementation;
    using System.Text;
    using System;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;

    [TestFixture]
    public class BurgerExampleAsTest
    {
        static string GetRecipeFor(Order order)
        {
            var state = new State(order);

            new[]
            {
                Hamburger.When(NameIs(hamburger)),
                Fishburger.When(NameIs(fishBurger)),
                Chikenburger.When(NameIs(chikenBurger)
                              .Or(NameIs(doubleChikenBurger))),
                UncknownBurger
            }
            .Select(WithConditionThat(RecipeIsMissing))
            .Aggregate(FirstThenSecond)
            .Execute(state);

            return state.Recipe.ToString();
        }  

        static IHandler<State> Hamburger =>
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

        static IHandler<State> Fishburger =>
            new[]
            {
                Cut(bun),
                Cut(fishFillet),
                Salt(fishFilletPortion),
                Bread(fishFilletPortion),
                Slice(cheese),
                Fry(fishFilletPortion),
                Toast(bun),
                Put(bottomBun),
                Put(fishFilletPortion),
                Add(cheeseSlice),
                Add(tartareSauce),
                Put(topBun)
            }
            .Select(Pack(Index).In(Dot).ThenIn(Space).ThenIn)
            .Select(XCover(NewLine).WhereXIs)
            .Aggregate(FirstThenSecond);

        static IHandler<State> Chikenburger =>
            new[]
            {
                Cut(bun),
                Cut(lettuce)
            }
            .Concat(new[]
            {
                Salt(chickenPatty),
                Pepper(chickenPatty)
                    .When(Not(OrderExclude(pepper))),
                Bread(chickenPatty),
                Fry(chickenPatty)
            }
            .Select(XCover(Again.When(NameStartWith(@double))).WhereXIs))
            .Concat(
            [
                Toast(bun),
                Put(bottomBun),
                Put(chickenPatty)
                    .Then(Again.When(NameStartWith(@double))),
                Add(lettuce),
                Add(sandwichSauce),
                Put(topBun)
            ])
            .Select(Pack(Index.Then(Dot).Then(Space)).In)
            .Select(XCover(NewLine).WhereXIs)
            .Aggregate(FirstThenSecond);

        static IHandler<State> UncknownBurger =>
            MakeHandler<State>(x => x.Recipe.Append(
                $"There is no recipe for {x.Order.Name}"));

        const string @double = "Double";
        const string hamburger = "Hamburger";
        const string fishBurger = "Fish-Burger";
        const string chikenBurger = "Chiken-Burger";
        const string veganBurger = "Vegan-Burger";
        const string doubleChikenBurger = $"{@double} {chikenBurger}";

        class Order(string name, params string[] options)
        {
            public string Name => name;

            public string[] Options => options;
        }

        class State(Order order)
        {
            public Order Order => order;

            public StringBuilder Recipe { get; } = new StringBuilder();

            public int StepCounter { get; set; } = 1;
        }

        const string bun = "bun";
        const string topBun = $"top {bun}";
        const string bottomBun = $"bottom {bun}";
        const string pickle = "pickle";
        const string pickleSlices = $"{pickle} slices";
        const string onion = "onion";
        const string pepper = "pepper";
        const string mustard = "mustard";
        const string bacon = "bacon";
        const string beefPatty = "beef patty";
        const string tomatoKetchup = "tomato ketchup";
        const string fishFillet = "fish fillet";
        const string fishFilletPortion = $"{fishFillet} portion";
        const string cheese = "cheese";
        const string cheeseSlice = $"{cheese} slice";
        const string tartareSauce = "tartare sauce";
        const string chickenPatty = "chicken patty";
        const string lettuce = "lettuce";
        const string sandwichSauce = "sandwich sauce";

        static IHandler<State> Cut(string x) => RecipePoint($"Cut {x}");

        static IHandler<State> Slice(string x) => RecipePoint($"Slice {x}");

        static IHandler<State> Dice(string x) => RecipePoint($"Dice {x}");

        static IHandler<State> Salt(string x) => RecipePoint($"Salt {x}");

        static IHandler<State> Pepper(string x) => RecipePoint($"Pepper {x}");

        static IHandler<State> Add(string x) => RecipePoint($"Add {x}");

        static IHandler<State> Put(string x) => RecipePoint($"Put {x}");

        static IHandler<State> Fry(string x) => RecipePoint($"Fry {x}");

        static IHandler<State> Toast(string x) => RecipePoint($"Toast {x}");

        static IHandler<State> Bread(string x) => RecipePoint($"Bread {x}");

        static IHandler<State> Again =>
            MakeHandler<State>(x => x.Recipe.Append(" x2"));

        static IHandler<State> NewLine =>
            MakeHandler<State>(x => x.Recipe.AppendLine());

        static IHandler<State> Index =>
            MakeHandler<State>(x => x.Recipe.Append($"{x.StepCounter++}"));

        static IHandler<State> Dot =>
            MakeHandler<State>(x => x.Recipe.Append('.'));

        static IHandler<State> Space =>
            MakeHandler<State>(x => x.Recipe.Append(' '));

        static IHandler<State> RecipePoint(string text) =>
            MakeHandler<State>(x => x.Recipe.Append(text));

        static ICondition<State> RecipeIsMissing =>
            MakeCondition<State>(x => x.Recipe.Length == 0);

        static ICondition<State> OrderExclude(string ingredient) =>
            MakeCondition<State>(x => x.Order.Options.Contains($"- {ingredient}"));

        static ICondition<State> OrderIncludeAdditional(string ingredient) =>
            MakeCondition<State>(x => x.Order.Options.Contains($"+ {ingredient}"));

        static ICondition<State> NameStartWith(string str) =>
            MakeCondition<State>(x => x.Order.Name.StartsWith(str));

        static ICondition<State> NameIs(string str) =>
            MakeCondition<State>(x => x.Order.Name == str);

        [SetUp]
        public void Setup()
        {
            var conditionMath = new ConditionMath();
            var handlerMath = new HandlerMath(conditionMath);
            
            ChainLeadSyntax.Configure(handlerMath, conditionMath);
        }

        [Test]
        public void HamburgerRecipeTest()
        {
            var recipe = GetRecipeFor(new Order(hamburger));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Slice pickle",
                    "3. Dice onion",
                    "4. Salt beef patty",
                    "5. Pepper beef patty",
                    "6. Fry beef patty",
                    "7. Toast bun",
                    "8. Put bottom bun",
                    "9. Put beef patty",
                    "10. Add tomato ketchup",
                    "11. Add pickle slices",
                    "12. Add onion",
                    "13. Add mustard",
                    "14. Put top bun"
                )));
        }

        [Test]
        public void HamburgerWithExtraPickleSlicesRecipeTest()
        {
            var recipe = GetRecipeFor(new(hamburger,
                $"+ {pickle}"));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Slice pickle x2",
                    "3. Dice onion",
                    "4. Salt beef patty",
                    "5. Pepper beef patty",
                    "6. Fry beef patty",
                    "7. Toast bun",
                    "8. Put bottom bun",
                    "9. Put beef patty",
                    "10. Add tomato ketchup",
                    "11. Add pickle slices x2",
                    "12. Add onion",
                    "13. Add mustard",
                    "14. Put top bun"
                )));
        }

        [Test]
        public void HamburgerWithAllToppingsRecipeTest()
        {
            var recipe = GetRecipeFor(new(hamburger,
                $"+ {pickle}",
                $"+ {bacon}"));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Slice pickle x2",
                    "3. Dice onion",
                    "4. Salt beef patty",
                    "5. Pepper beef patty",
                    "6. Fry beef patty",
                    "7. Fry bacon",
                    "8. Toast bun",
                    "9. Put bottom bun",
                    "10. Put beef patty",
                    "11. Add bacon",
                    "12. Add tomato ketchup",
                    "13. Add pickle slices x2",
                    "14. Add onion",
                    "15. Add mustard",
                    "16. Put top bun"
                )));
        }

        [Test]
        public void HamburgerWithoutEverythingThatCanBeExcludedRecipeTest()
        {
            var recipe = GetRecipeFor(new(hamburger,
                $"- {pepper}",
                $"- {onion}",
                $"- {mustard}"));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Slice pickle",
                    "3. Salt beef patty",
                    "4. Fry beef patty",
                    "5. Toast bun",
                    "6. Put bottom bun",
                    "7. Put beef patty",
                    "8. Add tomato ketchup",
                    "9. Add pickle slices",
                    "10. Put top bun"
                )));
        }

        [Test]
        public void HamburgerWithoutEverythingThatCanBeExcludedAndAllToppingsRecipeTest()
        {
            var recipe = GetRecipeFor(new(hamburger,
                    $"- {pepper}",
                    $"- {onion}",
                    $"- {mustard}",
                    $"+ {pickle}",
                    $"+ {bacon}"));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Slice pickle x2",
                    "3. Salt beef patty",
                    "4. Fry beef patty",
                    "5. Fry bacon",
                    "6. Toast bun",
                    "7. Put bottom bun",
                    "8. Put beef patty",
                    "9. Add bacon",
                    "10. Add tomato ketchup",
                    "11. Add pickle slices x2",
                    "12. Put top bun"
                )));
        }

        [Test]
        public void FishBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new(fishBurger));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Cut fish fillet",
                    "3. Salt fish fillet portion",
                    "4. Bread fish fillet portion",
                    "5. Slice cheese",
                    "6. Fry fish fillet portion",
                    "7. Toast bun",
                    "8. Put bottom bun",
                    "9. Put fish fillet portion",
                    "10. Add cheese slice",
                    "11. Add tartare sauce",
                    "12. Put top bun"
                )));
        }

        [Test]
        public void ChickenBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new(chikenBurger));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Cut lettuce",
                    "3. Salt chicken patty",
                    "4. Pepper chicken patty",
                    "5. Bread chicken patty",
                    "6. Fry chicken patty",
                    "7. Toast bun",
                    "8. Put bottom bun",
                    "9. Put chicken patty",
                    "10. Add lettuce",
                    "11. Add sandwich sauce",
                    "12. Put top bun"
                )));
        }

        [Test]
        public void DoubleChickenBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new(doubleChikenBurger));

            Assert.That(recipe,
                Is.EqualTo(Text(
                    "1. Cut bun",
                    "2. Cut lettuce",
                    "3. Salt chicken patty x2",
                    "4. Pepper chicken patty x2",
                    "5. Bread chicken patty x2",
                    "6. Fry chicken patty x2",
                    "7. Toast bun",
                    "8. Put bottom bun",
                    "9. Put chicken patty x2",
                    "10. Add lettuce",
                    "11. Add sandwich sauce",
                    "12. Put top bun"
                )));
        }

        [Test]
        public void VeganBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new(veganBurger));

            Assert.That(recipe,
                Is.EqualTo($"There is no recipe for {veganBurger}"));
        }

        static string Text(params string[] lines) =>
            lines.SelectMany(x => new[] { x, Environment.NewLine })
                 .Aggregate(string.Concat);
    }
}
