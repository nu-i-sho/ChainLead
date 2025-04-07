namespace Nuisho.ChainLead.Test
{
    using System;
    using System.Text;
    using Contracts;
    using Contracts.Syntax;
    using Implementation;

    using static Contracts.Syntax.ChainLeadSyntax;

    [TestFixture]
    public class BurgerExampleAsTest
    {
        static string GetRecipeFor(Order order)
        {
            var state = new State(order);

            new[]
            {
                HamburgerRecipe.When(NameIs(Hamburger)),
                FishburgerRecipe.When(NameIs(FishBurger)),
                ChikenburgerRecipe.When(NameIs(ChikenBurger)
                                  .Or(NameIs(DoubleChikenBurger))),
                UncknownBurgerRecipe
            }
            .Select(WithConditionThat(RecipeIsMissing))
            .Aggregate(FirstThenSecond)
            .Execute(state);

            return state.Recipe.ToString();
        }

        static IHandler<State> HamburgerRecipe =>
            new[]
            {
                Cut(Bun),
                Slice(Pickle)
                    .Then(Again.When(OrderIncludeAdditional(Pickle))),
                Dice(Onion)
                    .When(Not(OrderExclude(Onion))),
                Salt(BeefPatty),
                Pepper(BeefPatty)
                    .When(Not(OrderExclude(Spicy))),
                Fry(BeefPatty),
                Fry(Bacon)
                    .When(OrderIncludeAdditional(Bacon)),
                Toast(Bun),
                Put(BottomBun),
                Put(BeefPatty),
                Add(Bacon)
                    .When(OrderIncludeAdditional(Bacon)),
                Add(TomatoKetchup),
                Add(PickleSlices)
                    .Then(Again.When(OrderIncludeAdditional(Pickle))),
                Add(Onion)
                    .When(Not(OrderExclude(Onion))),
                Add(Mustard)
                    .When(Not(OrderExclude(Mustard))),
                Put(TopBun)
            }
            .Select(Pack(Index).In(Dot).ThenIn(Space).ThenIn)
            .Select(XCover(NewLine).WhereXIs)
            .Aggregate(FirstThenSecond);

        static IHandler<State> FishburgerRecipe =>
            new[]
            {
                Cut(Bun),
                Cut(FishFillet),
                Salt(FishFilletPortion),
                Bread(FishFilletPortion),
                Slice(Cheese),
                Fry(FishFilletPortion),
                Toast(Bun),
                Put(BottomBun),
                Put(FishFilletPortion),
                Add(CheeseSlice),
                Add(TartareSauce),
                Put(TopBun)
            }
            .Select(Pack(Index).In(Dot).ThenIn(Space).ThenIn)
            .Select(XCover(NewLine).WhereXIs)
            .Aggregate(FirstThenSecond);

        static IHandler<State> ChikenburgerRecipe =>
            new[]
            {
                Cut(Bun),
                Cut(Lettuce)
            }
            .Concat(new[]
            {
                Salt(ChickenPatty),
                Pepper(ChickenPatty)
                    .When(Not(OrderExclude(Spicy))),
                Bread(ChickenPatty),
                Fry(ChickenPatty)
            }
            .Select(XCover(Again.When(NameStartWith(Double))).WhereXIs))
            .Concat(
            [
                Toast(Bun),
                Put(BottomBun),
                Put(ChickenPatty)
                    .Then(Again.When(NameStartWith(Double))),
                Add(Lettuce),
                Add(SandwichSauce),
                Put(TopBun)
            ])
            .Select(Pack(Index.Then(Dot).Then(Space)).In)
            .Select(XCover(NewLine).WhereXIs)
            .Aggregate(FirstThenSecond);

        static IHandler<State> UncknownBurgerRecipe =>
            MakeHandler<State>(x => x.Recipe.Append(
                $"There is no recipe for {x.Order.Name}"));

        const string Double = "Double";
        const string Hamburger = "Hamburger";
        const string FishBurger = "Fish-Burger";
        const string ChikenBurger = "Chiken-Burger";
        const string VeganBurger = "Vegan-Burger";
        const string DoubleChikenBurger = $"{Double} {ChikenBurger}";

        sealed class Order(string name, params string[] options)
        {
            public string Name => name;

            public string[] Options => options;
        }

        sealed class State(Order order)
        {
            public Order Order => order;

            public StringBuilder Recipe { get; } = new StringBuilder();

            public int StepCounter { get; set; } = 1;
        }

        const string Bun = "bun";
        const string TopBun = $"top {Bun}";
        const string BottomBun = $"bottom {Bun}";
        const string Pickle = "pickle";
        const string PickleSlices = $"{Pickle} slices";
        const string Onion = "onion";
        const string Spicy = "spicy";
        const string Mustard = "mustard";
        const string Bacon = "bacon";
        const string BeefPatty = "beef patty";
        const string TomatoKetchup = "tomato ketchup";
        const string FishFillet = "fish fillet";
        const string FishFilletPortion = $"{FishFillet} portion";
        const string Cheese = "cheese";
        const string CheeseSlice = $"{Cheese} slice";
        const string TartareSauce = "tartare sauce";
        const string ChickenPatty = "chicken patty";
        const string Lettuce = "lettuce";
        const string SandwichSauce = "sandwich sauce";

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
            var recipe = GetRecipeFor(new (Hamburger));

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
                    "14. Put top bun")));
        }

        [Test]
        public void HamburgerWithExtraPickleSlicesRecipeTest()
        {
            var recipe = GetRecipeFor(new (Hamburger,
                $"+ {Pickle}"));

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
                    "14. Put top bun")));
        }

        [Test]
        public void HamburgerWithAllToppingsRecipeTest()
        {
            var recipe = GetRecipeFor(new (Hamburger,
                $"+ {Pickle}",
                $"+ {Bacon}"));

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
                    "16. Put top bun")));
        }

        [Test]
        public void HamburgerWithoutEverythingThatCanBeExcludedRecipeTest()
        {
            var recipe = GetRecipeFor(new (Hamburger,
                $"- {Spicy}",
                $"- {Onion}",
                $"- {Mustard}"));

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
                    "10. Put top bun")));
        }

        [Test]
        public void HamburgerWithoutEverythingThatCanBeExcludedAndAllToppingsRecipeTest()
        {
            var recipe = GetRecipeFor(new (Hamburger,
                    $"- {Spicy}",
                    $"- {Onion}",
                    $"- {Mustard}",
                    $"+ {Pickle}",
                    $"+ {Bacon}"));

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
                    "12. Put top bun")));
        }

        [Test]
        public void FishBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new (FishBurger));

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
                    "12. Put top bun")));
        }

        [Test]
        public void ChickenBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new (ChikenBurger));

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
                    "12. Put top bun")));
        }

        [Test]
        public void DoubleChickenBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new (DoubleChikenBurger));

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
                    "12. Put top bun")));
        }

        [Test]
        public void VeganBurgerRecipeTest()
        {
            var recipe = GetRecipeFor(new (VeganBurger));

            Assert.That(recipe,
                Is.EqualTo($"There is no recipe for {VeganBurger}"));
        }

        static string Text(params string[] lines) =>
            lines.SelectMany(x => new[] { x, Environment.NewLine })
                 .Aggregate(string.Concat);
    }
}
