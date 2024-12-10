namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Test.Types;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Cases.SingleHandler;
    using static ChainLead.Test.Dummy.ConditionIndex;
    using static ChainLead.Test.Dummy.HandlerIndex;

    public static partial class Cases
    {
        public static class HandlerMath
        {
            public class AllAppendsAttribute()
                : ValuesAttribute(
                    nameof(IHandlerMath.FirstThenSecond),
                    nameof(IHandlerMath.PackFirstInSecond),
                    nameof(IHandlerMath.InjectFirstIntoSecond),
                    nameof(IHandlerMath.FirstCoverSecond),
                    nameof(IHandlerMath.FirstWrapSecond),
                    nameof(IHandlerMath.JoinFirstWithSecond),
                    nameof(IHandlerMath.MergeFirstWithSecond));

            public class OriginalTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new OriginalHandlerMathFactory());

            public class _I_Attribute() : OriginalTestFixtureAttribute(typeof(int));
            public class _II_Attribute() : OriginalTestFixtureAttribute(typeof(string));
            public class _III_Attribute() : OriginalTestFixtureAttribute(typeof(Class));
            public class _IV_Attribute() : OriginalTestFixtureAttribute(typeof(Struct));
            public class _V_Attribute() : OriginalTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _VI_Attribute() : OriginalTestFixtureAttribute(typeof(Record));
            public class _VII_Attribute() : OriginalTestFixtureAttribute(typeof(RecordStruct));
            public class _VIII_Attribute() : OriginalTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public class SyntaxTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new SyntaxHandlerMathFactory());

            public class _IX_Attribute() : SyntaxTestFixtureAttribute(typeof(int));
            public class _X_Attribute() : SyntaxTestFixtureAttribute(typeof(string));
            public class _XI_Attribute() : SyntaxTestFixtureAttribute(typeof(Class));
            public class _XII_Attribute() : SyntaxTestFixtureAttribute(typeof(Struct));
            public class _XIII_Attribute() : SyntaxTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _XIV_Attribute() : SyntaxTestFixtureAttribute(typeof(Record));
            public class _XV_Attribute() : SyntaxTestFixtureAttribute(typeof(RecordStruct));
            public class _XVI_Attribute() : SyntaxTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public class SeparatedTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new SeparatedSyntaxHandlerMathFactory());

            public class _XVII_Attribute() : SeparatedTestFixtureAttribute(typeof(int));
            public class _XVIII_Attribute() : SeparatedTestFixtureAttribute(typeof(string));
            public class _XIX_Attribute() : SeparatedTestFixtureAttribute(typeof(Class));
            public class _XX_Attribute() : SeparatedTestFixtureAttribute(typeof(Struct));
            public class _XXI_Attribute() : SeparatedTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _XXII_Attribute() : SeparatedTestFixtureAttribute(typeof(Record));
            public class _XXIII_Attribute() : SeparatedTestFixtureAttribute(typeof(RecordStruct));
            public class _XXIV_Attribute() : SeparatedTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public class ReversedTestFixtureAttribute(Type t)
                : TestFixtureAttribute(t, new ReversedSyntaxHandlerMathFactory());

            public class _XXV_Attribute() : ReversedTestFixtureAttribute(typeof(int));
            public class _XXVI_Attribute() : ReversedTestFixtureAttribute(typeof(string));
            public class _XXVII_Attribute() : ReversedTestFixtureAttribute(typeof(Class));
            public class _XXVIII_Attribute() : ReversedTestFixtureAttribute(typeof(Struct));
            public class _XXIX_Attribute() : ReversedTestFixtureAttribute(typeof(ReadonlyStruct));
            public class _XXX_Attribute() : ReversedTestFixtureAttribute(typeof(Record));
            public class _XXXI_Attribute() : ReversedTestFixtureAttribute(typeof(RecordStruct));
            public class _XXXII_Attribute() : ReversedTestFixtureAttribute(typeof(ReadonlyRecordStruct));

            public interface IHandlerMathFactory
            {
                ITestingHandlerMath Create(IConditionMath conditionMath);
            }

            public interface ITestingHandlerMath : IHandlerMath;

            public class OriginalHandlerMathFactory
                : IHandlerMathFactory
            {
                public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(new Implementation.HandlerMath(conditionMath));

                public override string ToString() => "Original";

                class Product(IHandlerMath math) : 
                    OriginalMathFactory.Product(math),
                    ITestingHandlerMath
                {
                    public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.FirstCoverSecond(a, b);

                    public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.FirstThenSecond(a, b);

                    public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.FirstWrapSecond(a, b);

                    public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.InjectFirstIntoSecond(a, b);

                    public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.JoinFirstWithSecond(a, b);

                    public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.MergeFirstWithSecond(a, b);

                    public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Math.PackFirstInSecond(a, b);
                }
            }

            public class SyntaxHandlerMathFactory
                : IHandlerMathFactory
            {
                public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(conditionMath);

                public override string ToString() => "Syntax like FirstThenSecond[a, b]";

                class Product(IConditionMath conditionMath)
                        : SyntaxMathFactory.Product(conditionMath),
                    ITestingHandlerMath
                {
                    public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.FirstThenSecond(a, b);

                    public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.FirstCoverSecond(a, b);

                    public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.FirstWrapSecond(a, b);

                    public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.InjectFirstIntoSecond(a, b);

                    public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.JoinFirstWithSecond(a, b);

                    public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.MergeFirstWithSecond(a, b);

                    public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Contracts.Syntax.ChainLeadSyntax.PackFirstInSecond(a, b);
                }
            }

            public class SeparatedSyntaxHandlerMathFactory
                : IHandlerMathFactory
            {
                public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(conditionMath);

                public override string ToString() => "Syntax like Pack[a].In[b]";

                class Product(IConditionMath conditionMath)
                        : SyntaxMathFactory.Product(conditionMath),
                    ITestingHandlerMath
                {
                    public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        a.Then(b);

                    public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Use(a).ToCover(b);

                    public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Use(a).ToWrap(b);

                    public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Inject(a).Into(b);

                    public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Join(a).With(b);

                    public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Merge(a).With(b);

                    public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        Pack(a).In(b);
                }
            }

            public class ReversedSyntaxHandlerMathFactory
               : IHandlerMathFactory
            {
                public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                    new Product(conditionMath);

                public override string ToString() => "Syntax like PackXIn[a].WhereXIs[b]";

                class Product(IConditionMath conditionMath)
                        : SyntaxMathFactory.Product(conditionMath),
                    ITestingHandlerMath
                {
                    public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        XThen(b).WhereXIs(a);

                    public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        XCover(b).WhereXIs(a);

                    public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        XWrap(b).WhereXIs(a);

                    public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        InjectXInto(b).WhereXIs(a);

                    public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        JoinXWith(b).WhereXIs(a);

                    public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        MergeXWith(b).WhereXIs(a);

                    public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                        PackXIn(b).WhereXIs(a);
                }
            }

            public class BlueCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(BlueCases));

            public class RedCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(RedCases));

            public class GreenCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(GreenCases));

            public class OrangeCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(OrangeCases));

            public class YellowCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(YellowCases));

            public class WhiteCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(WhiteCases));

            public class BlackCasesAttribute()
                : ValueSourceAttribute(
                    typeof(HandlerMath),
                    nameof(BlackCases));

            public record BlueCase(
                Dummy.HandlerIndex[] ChainIndices,
                string NameForEasyFind = "")
            {
                public override string ToString()
                {
                    var name = ViewOf(ChainIndices);
                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static BlueCase[] BlueCases =>
            [
                new([ A, A ], "1"), new([ A, B ], "2"), new([ A, B, C ], "3"), new([ A, B, C, D, E, F ], "4"),
                new([ A, B, C, D, A, B, B, B, A, B, C, D, E, F, H, I, A, B, C, D, C, C, D], "5"),
                new([ A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A], "6")
            ];

            public record RedCase(
                Dummy.HandlerIndex?[] ChainIndicesWithNullsAsZeros,
                Dummy.HandlerIndex[] ExpectedExecution,
                string NameForEasyFind = "")
            {
                public override string ToString()
                {
                    var name = string.Join(
                        CaseBlocksSeparator,
                        ViewOf(ChainIndicesWithNullsAsZeros),
                        ViewOf(ExpectedExecution));
                    
                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static RedCase[] RedCases =>
            [
                new([null, A], [A], "Sunday"),
                new([A, null], [A], "Monday"),
                new([null, A, null], [A], "Tuesday"),
                new([null, null, null, null, A, null, null, null], [A], "Wednesday"),

                new([A, B, C, D, null, I, J, null, I, null, null, E, I, H, null, F, F, I, null, null],
                    [A, B, C, D, I, J, I, E, I, H, F, F, I],
                    "Thursday"),

                new([A, null, A, null, A, null, A, null, A, null, A, null, A, A, A, A, null, null, null, A, null, A, A, A],
                    [A, A, A, A, A, A, A, A, A, A, A, A, A, A],
                    "Friday")
            ];

            public record GreenCase(
                bool AIsConditional,
                bool BIsConditional,
                Dummy.ConditionIndex ExpectedFinalCondition,
                bool FinalConditionCheckResult,
                string NameForEasyFind = "")
            {
                public override string ToString()
                {
                    var name = string.Join(
                        CaseBlocksSeparator,
                        ViewOf(AIsConditional),
                        ViewOf(BIsConditional),
                        ExpectedFinalCondition.Value,
                        ViewOf(FinalConditionCheckResult));

                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static IEnumerable<GreenCase> GreenCases
            {
                get
                {
                    yield return new(false, true, Y, false, "Sun");
                    yield return new(false, true, Y, true, "Moon");
                    yield return new(true, false, X, false, "Venus");
                    yield return new(true, false, X, true, "Earth");
                    yield return new(true, true, X & Y, false, "Mars");
                    yield return new(true, true, X & Y, true, "Saturn");
                }
            }

            public record OrangeCase(
                bool[] CheckSetup,
                bool[] CheckExpected,
                bool[] ExecutionExpected,
                string NameForEasyFind = "")
            {
                public override string ToString()
                {
                    var name = string.Join(
                        CaseBlocksSeparator,
                        ViewOf(CheckSetup),
                        ViewOf(CheckExpected),
                        ViewOf(ExecutionExpected));
                
                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static IEnumerable<OrangeCase> OrangeCases
            {
                get
                {
                    yield return new(
                        CheckSetup:    [false, false, false],
                        CheckExpected: [false, false, true],
                        ExecutionExpected: [false, false],
                        NameForEasyFind: "West");

                    yield return new(
                        CheckSetup:    [false, true, false],
                        CheckExpected: [false, false, true],
                        ExecutionExpected: [false, false],
                        NameForEasyFind: "Northwest"); 

                    yield return new(
                        CheckSetup:    [true,  false, false],
                        CheckExpected: [false, false, true],
                        ExecutionExpected: [false, false],
                        NameForEasyFind: "North");

                    yield return new(
                        CheckSetup:    [true,  true,  false],
                        CheckExpected: [false, false, true],
                        ExecutionExpected: [false, false],
                        NameForEasyFind: "Northeast");

                    yield return new(
                        CheckSetup:    [false, false, true],
                        CheckExpected: [true,  true,  true],
                        ExecutionExpected: [false, false],
                        NameForEasyFind: "East");

                    yield return new(
                        CheckSetup:    [false, true, true],
                        CheckExpected: [true,  true, true],
                        ExecutionExpected: [false, true],
                        NameForEasyFind: "Southeast");

                    yield return new(
                        CheckSetup:    [true, false, true],
                        CheckExpected: [true, true,  true],
                        ExecutionExpected: [true, false],
                        NameForEasyFind: "South");

                    yield return new(
                        CheckSetup:    [true, true, true],
                        CheckExpected: [true, true, true],
                        ExecutionExpected: [true, true],
                        NameForEasyFind: "Southwest");
                }
            }

            public record YellowCase(
                Dummy.ConditionIndex[] AConditions,
                Dummy.ConditionIndex[] BConditions,
                Dictionary<Dummy.ConditionIndex, bool> ChecksSetup,
                Dummy.ConditionIndex[] CheckExpected,
                Dummy.HandlerIndex[] ExecuteExpected,
                string NameForEasyFind = "")
            {
                public static readonly Dummy.ConditionIndex TopAnd = new("TOP &");

                public override string ToString()
                {
                    var name = string.Join(
                        CaseBlocksSeparator,
                        ViewOf(AConditions),
                        ViewOf(BConditions),
                        ViewOf(ChecksSetup),
                        ViewOf(CheckExpected),
                        ViewOf(ExecuteExpected));

                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static IEnumerable<YellowCase> YellowCases
            {
                get
                {
                    yield return new(
                        AConditions:     [],
                        BConditions:     [],
                        ChecksSetup:     [],
                        CheckExpected:   [],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "James");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [],
                        ChecksSetup:  new() { { X, false } },
                        CheckExpected:   [X],
                        ExecuteExpected: [],
                        NameForEasyFind: "Michael");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [],
                        ChecksSetup:  new() { { X, true } },
                        CheckExpected:   [X],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Robert");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [],
                        ChecksSetup:  new() { { Y, false } },
                        CheckExpected:   [Y],
                        ExecuteExpected: [],
                        NameForEasyFind: "John");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [],
                        ChecksSetup:  new() { { Y, true }, { X, false } },
                        CheckExpected:   [Y, X],
                        ExecuteExpected: [B],
                        NameForEasyFind: "David");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [],
                        ChecksSetup:  new() { { Y, true }, { X, true } },
                        CheckExpected:   [Y, X],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "William");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [],
                        ChecksSetup:  new() { { Z, false } },
                        CheckExpected:   [Z],
                        ExecuteExpected: [],
                        NameForEasyFind: "Richard");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [],
                        ChecksSetup:  new() { { Z, true }, { Y, false } },
                        CheckExpected:   [Z, Y],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Joseph");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [],
                        ChecksSetup:  new() { { Z, true }, { Y, true }, { X, false } },
                        CheckExpected:   [Z, Y, X],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Thomas");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [],
                        ChecksSetup:  new() { { Z, true }, { Y, true }, { X, true } },
                        CheckExpected:   [Z, Y, X],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Christopher");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Charles");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Daniel");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Matthew");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Anthony");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Mark");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Donald");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Steven");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Andrew");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, false } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [],
                        NameForEasyFind: "Paul");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true } },
                        CheckExpected:   [YellowCase.TopAnd],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Joshua");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { X, false } },
                        CheckExpected:   [YellowCase.TopAnd, X],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Kenneth");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { X, true } },
                        CheckExpected:   [YellowCase.TopAnd, X],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Kevin");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Brian");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Timothy");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Ronald");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, U],
                        ExecuteExpected: [A],
                        NameForEasyFind: "George");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, U],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Jason");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { V, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, V, U],
                        ExecuteExpected: [A],
                        NameForEasyFind: "Edward");

                    yield return new(
                        AConditions:     [X],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { V, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, V, U],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Jeffrey");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { X, false }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, X, U],
                        ExecuteExpected: [],
                        NameForEasyFind: "Ryan");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { X, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, X, U],
                        ExecuteExpected: [A],
                        NameForEasyFind: "Jacob");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { X, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, X, U],
                        ExecuteExpected: [A],
                        NameForEasyFind: "Nicholas");

                    yield return new(
                        AConditions:     [X, Y],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { X, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, X, U],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Gary");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, false }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, U],
                        ExecuteExpected: [],
                        NameForEasyFind: "Eric");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, U],
                        ExecuteExpected: [],
                        NameForEasyFind: "Jonathan");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, U],
                        ExecuteExpected: [A],
                        NameForEasyFind: "Stephen");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, false }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, U],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Larry");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, U],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Justin");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, U],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Scott");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, false }, { V, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, V],
                        ExecuteExpected: [],
                        NameForEasyFind: "Brandon");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false }, { V, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, V],
                        ExecuteExpected: [],
                        NameForEasyFind: "Benjamin");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false }, { V, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, V, U],
                        ExecuteExpected: [],
                        NameForEasyFind: "Samuel");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false }, { V, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, V],
                        ExecuteExpected: [],
                        NameForEasyFind: "Gregory");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, true }, { V, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, V, U],
                        ExecuteExpected: [A],
                        NameForEasyFind: "Alexander");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, false }, { V, true }, { U, false } },
                        CheckExpected:   [YellowCase.TopAnd, Y, V, U],
                        ExecuteExpected: [],
                        NameForEasyFind: "Patrick");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, false }, { V, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, V, U],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Frank");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, false }, { V, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, V, U],
                        ExecuteExpected: [B],
                        NameForEasyFind: "Raymond");

                    yield return new(
                        AConditions:     [X, Y, Z],
                        BConditions:     [U, V, W],
                        ChecksSetup:  new() { { YellowCase.TopAnd, true }, { Y, true }, { X, true }, { V, true }, { U, true } },
                        CheckExpected:   [YellowCase.TopAnd, Y, X, V, U],
                        ExecuteExpected: [A, B],
                        NameForEasyFind: "Jack");
                }
            }

            public record WhiteCase(
                Dummy.ConditionIndex[] AConditions,
                Dummy.ConditionIndex[] BConditions,
                string NameForEasyFind = "")
            {
                public override string ToString()
                {
                    var name = string.Join(
                        CaseBlocksSeparator,
                        ViewOf(AConditions),
                        ViewOf(BConditions));

                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static IEnumerable<WhiteCase> WhiteCases
            {
                get
                {
                    yield return new([Q, R], [], "Tyler");
                    yield return new([Q, R, S], [], "Aaron");
                    yield return new([Q, R, S, T, U], [], "Jose");
                    yield return new([Q, R, S, T, U, V, W, X], [], "Adam");

                    yield return new([], [Q, R], "Nathan");
                    yield return new([], [Q, R, S], "Henry");
                    yield return new([], [Q, R, S, T, U], "Zachary");
                    yield return new([], [Q, R, S, T, U, V, W], "Douglas");

                    yield return new([Q], [R], "Peter");
                    yield return new([Q, R], [S], "Kyle");
                    yield return new([Q, R, S], [T], "Noah");
                    yield return new([Q, R, S], [T, U], "Ethan");
                    yield return new([Q, R, S], [T, U, V], "Jeremy");
                    yield return new([Q, R, S, T, U], [V], "Christian");
                    yield return new([Q, R, S, T, U], [V, W], "Walter");
                    yield return new([Q, R, S, T, U], [V, W, X], "Keith");
                    yield return new([Q, R, S, T, U], [V, W, X, Y], "Austin");
                    yield return new([Q, R, S, T, U, V, W], [X], "Roger");
                    yield return new([Q, R, S, T, U, V, W], [X, Y], "Terry");
                    yield return new([Q, R, S, T, U, V, W, X], [Y], "Sean");

                    yield return new([Q], [R, S], "Gerald");
                    yield return new([Q], [R, S, T], "Carl");
                    yield return new([Q], [R, S, T, U], "Dylan");
                    yield return new([Q, R], [S, T, U, V], "Harold");
                    yield return new([Q, R, S], [T, U, V, W], "Jordan");
                    yield return new([Q, R], [S, T, U, V, W, X], "Jesse");
                    yield return new([Q, R], [S, T, U, V, W, X, Y], "Bryan");
                    yield return new([Q, R, S, T], [U, V, W, X, Z], "Lawrence");
                    yield return new([Q], [R, S, T, U, V, W, X, Y], "Arthur");
                    yield return new([Q, R, S],[ T, U, V, W, X, Y], "Gabriel");
                }
            }

            public record BlackCase(
                string Append,
                Dummy.ConditionIndex[] AConditions,
                Dummy.ConditionIndex[] BConditions,
                Dictionary<Dummy.ConditionIndex, bool> ChecksSetup,
                Dummy.ChainElementIndex[] ExpectedCallsOrder,
                string NameForEasyFind = "")
            {
                public override string ToString()
                {
                    var name = string.Join(
                        CaseBlocksSeparator,
                        Append,
                        ViewOf(AConditions),
                        ViewOf(BConditions),
                        ViewOf(ChecksSetup),
                        ViewOf(ExpectedCallsOrder));

                    if (NameForEasyFind != string.Empty)
                        name = $"{NameForEasyFind}: {name}";

                    return name;
                }
            }

            public static IEnumerable<BlackCase> BlackCases
            {
                get
                {
                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [],
                        ChecksSetup: [],
                        ExpectedCallsOrder: [A, B],
                        NameForEasyFind: "Mary");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U, B],
                        NameForEasyFind: "Patricia");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [U, A, B],
                        NameForEasyFind: "Jennifer");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Linda");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [U, A, B],
                        NameForEasyFind: "Elizabeth");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V, B],
                        NameForEasyFind: "Barbara");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U, B],
                        NameForEasyFind: "Susan");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Jessica");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Karen");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U],
                        NameForEasyFind: "Sarah");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Lisa");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X, B],
                        NameForEasyFind: "Nancy");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, W, B],
                        NameForEasyFind: "Sandra");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, W, V, B],
                        NameForEasyFind: "Betty");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [X, W, V, U, B],
                        NameForEasyFind: "Ashley");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [X, W, V, U, A, B],
                        NameForEasyFind: "Emily");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [X, W, V, U, A, B],
                        NameForEasyFind: "Kimberly");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [X, W, V, U],
                        NameForEasyFind: "Margaret");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, W, V],
                        NameForEasyFind: "Donna");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, W],
                        NameForEasyFind: "Michelle");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X],
                        NameForEasyFind: "Carol");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Amanda");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U, B],
                        NameForEasyFind: "Melissa");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Deborah");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Stephanie");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Rebecca");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, true }, { V, false } },
                        ExpectedCallsOrder: [W, V, B],
                        NameForEasyFind: "Sharon");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [W, V, U, B],
                        NameForEasyFind: "Laura");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [W, V, U, A, B],
                        NameForEasyFind: "Cynthia");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Dorothy");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { V, false } },
                        ExpectedCallsOrder: [W, V],
                        NameForEasyFind: "Amy");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [W, V, U, B],
                        NameForEasyFind: "Kathleen");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [W, V, U, A, B],
                        NameForEasyFind: "Angela");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X],
                        NameForEasyFind: "Shirley");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, W],
                        NameForEasyFind: "Emma");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, W, V, B],
                        NameForEasyFind: "Brenda");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [X, W, V, U, B],
                        NameForEasyFind: "Pamela");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [X, W, V, U, A, B],
                        NameForEasyFind: "Nicole");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, false } },
                        ExpectedCallsOrder: [Z],
                        NameForEasyFind: "Anna");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { Y, false } },
                        ExpectedCallsOrder: [Z, Y],
                        NameForEasyFind: "Samantha");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [Z, Y, X],
                        NameForEasyFind: "Katherine");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { Y, true }, { X, true }, { W, false } },
                        ExpectedCallsOrder: [Z, Y, X, W, B],
                        NameForEasyFind: "Christine");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { Y, true }, { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [Z, Y, X, W, V, B],
                        NameForEasyFind: "Debra");

                    yield return new(
                        Append: nameof(IHandlerMath.InjectFirstIntoSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { Y, true }, { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [Z, Y, X, W, V, U, B],
                        NameForEasyFind: "Rachel");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [],
                        ChecksSetup: [],
                        ExpectedCallsOrder: [A, B],
                        NameForEasyFind: "Carolyn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Janet");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [U, A, B],
                        NameForEasyFind: "Maria");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [A, U],
                        NameForEasyFind: "Olivia");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [A, U, B],
                        NameForEasyFind: "Heather");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [A, V],
                        NameForEasyFind: "Helen");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [A, V, U],
                        NameForEasyFind: "Catherine");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [A, V, U, B],
                        NameForEasyFind: "Diane");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Julie");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U],
                        NameForEasyFind: "Victoria");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Joyce");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X],
                        NameForEasyFind: "Lauren");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, W],
                        NameForEasyFind: "Kelly");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, W, V],
                        NameForEasyFind: "Christina");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false }, },
                        ExpectedCallsOrder: [X, W, V, U],
                        NameForEasyFind: "Ruth");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true }, },
                        ExpectedCallsOrder: [X, W, V, U, A, B],
                        NameForEasyFind: "Joan");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true }, },
                        ExpectedCallsOrder: [A, X, W, V, U, B],
                        NameForEasyFind: "Virginia");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false }, },
                        ExpectedCallsOrder: [A, X, W, V, U],
                        NameForEasyFind: "Judith");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [A, X, W, V],
                        NameForEasyFind: "Evelyn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [A, X, W],
                        NameForEasyFind: "Hannah");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [A, X],
                        NameForEasyFind: "Andrea");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Megan");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { U, true }, { V, false } },
                        ExpectedCallsOrder: [U, A, V],
                        NameForEasyFind: "Cheryl");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { U, true }, { V, true } },
                        ExpectedCallsOrder: [U, A, V, B],
                        NameForEasyFind: "Jacqueline");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Madison");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U],
                        NameForEasyFind: "Teresa");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, true }, { W, false } },
                        ExpectedCallsOrder: [V, U, A, W],
                        NameForEasyFind: "Abigail");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, true }, { W, true } },
                        ExpectedCallsOrder: [V, U, A, W, B],
                        NameForEasyFind: "Sophia");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Martha");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, true }, { W, false } },
                        ExpectedCallsOrder: [U, A, W],
                        NameForEasyFind: "Sara");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [U, A, W, V],
                        NameForEasyFind: "Gloria");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, true }, { W, true }, { V, true } },
                        ExpectedCallsOrder: [U, A, W, V, B],
                        NameForEasyFind: "Janice");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Kathryn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U],
                        NameForEasyFind: "Ann");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, true }, { X, false } },
                        ExpectedCallsOrder: [V, U, A, X],
                        NameForEasyFind: "Isabella");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, true }, { X, true }, { W, false } },
                        ExpectedCallsOrder: [V, U, A, X, W],
                        NameForEasyFind: "Judy");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, true }, { X, true }, { W, true } },
                        ExpectedCallsOrder: [V, U, A, X, W, B],
                        NameForEasyFind: "Charlotte");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Julia");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, false } },
                        ExpectedCallsOrder: [W, V],
                        NameForEasyFind: "Grace");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [W, V, U],
                        NameForEasyFind: "Amber");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, false } },
                        ExpectedCallsOrder: [W, V, U, A, Z],
                        NameForEasyFind: "Alice");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, false } },
                        ExpectedCallsOrder: [W, V, U, A, Z, Y],
                        NameForEasyFind: "Jean");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [W, V, U, A, Z, Y, X],
                        NameForEasyFind: "Denise");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstWrapSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [W, V, U, A, Z, Y, X, B],
                        NameForEasyFind: "Frances");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [],
                        ChecksSetup: [],
                        ExpectedCallsOrder: [A, B],
                        NameForEasyFind: "Danielle");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U, B],
                        NameForEasyFind: "Danielle");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [U, A, B],
                        NameForEasyFind: "Natalie");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Beverly");

                    yield return new(
                    Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [U, A, B],
                        NameForEasyFind: "Diana");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V, B],
                        NameForEasyFind: "Brittany");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U, B],
                        NameForEasyFind: "Theresa");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Kayla");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Alexis");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, A, U],
                        NameForEasyFind: "Doris");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, A, U, B],
                        NameForEasyFind: "Lori");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X, B],
                        NameForEasyFind: "Tiffany");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, W, B],
                        NameForEasyFind: "Carl");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, W, V, B],
                        NameForEasyFind: "Dylan");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [X, W, V, U, B],
                        NameForEasyFind: "Harold");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [X, W, V, U, A, B],
                        NameForEasyFind: "Jesse");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [X, A, W, V, U, B],
                        NameForEasyFind: "Bryan");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [X, A, W, V, U],
                        NameForEasyFind: "Lawrence");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, A, W, V],
                        NameForEasyFind: "Arthur");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, A, W],
                        NameForEasyFind: "Gabriel");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X],
                        NameForEasyFind: "Bruce");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Logan");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U, B],
                        NameForEasyFind: "Billy");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Joe");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Alan");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, true }, { V, false } },
                        ExpectedCallsOrder: [W, V, B],
                        NameForEasyFind: "Juan");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [W, V, U, B],
                        NameForEasyFind: "Elijah");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [W, V, U, A, B],
                        NameForEasyFind: "Willie");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { U, true }, { V, true } },
                        ExpectedCallsOrder: [W, U, A, V, B],
                        NameForEasyFind: "Albert");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { U, true }, { V, false } },
                        ExpectedCallsOrder: [W, U, A, V],
                        NameForEasyFind: "Wayne");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { U, false }, { V, false } },
                        ExpectedCallsOrder: [W, U, V],
                        NameForEasyFind: "Randy");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, true }, { U, false }, { V, true } },
                        ExpectedCallsOrder: [W, U, V, B],
                        NameForEasyFind: "Mason");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Vincent");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X],
                        NameForEasyFind: "Liam");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { V, false }, { W, false } },
                        ExpectedCallsOrder: [X, V, W],
                        NameForEasyFind: "Roy");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { V, false }, { W, true } },
                        ExpectedCallsOrder: [X, V, W, B],
                        NameForEasyFind: "Bobby");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { V, true }, { U, false }, { W, false } },
                        ExpectedCallsOrder: [X, V, U, W],
                        NameForEasyFind: "Caleb");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { V, true }, { U, false }, { W, true } },
                        ExpectedCallsOrder: [X, V, U, W, B],
                        NameForEasyFind: "Bradley");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { V, true }, { U, true }, { W, false } },
                        ExpectedCallsOrder: [X, V, U, A, W],
                        NameForEasyFind: "Russell");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { X, true }, { V, true }, { U, true }, { W, true } },
                        ExpectedCallsOrder: [X, V, U, A, W, B],
                        NameForEasyFind: "Lucas");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, false } },
                        ExpectedCallsOrder: [Z],
                        NameForEasyFind: "Zekiel");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, false }, { Y, false } },
                        ExpectedCallsOrder: [Z, W, Y],
                        NameForEasyFind: "Yuri");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, false }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [Z, W, Y, X],
                        NameForEasyFind: "Tyde");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, false }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [Z, W, Y, X, B],
                        NameForEasyFind: "Turner");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, false }, { Y, false } },
                        ExpectedCallsOrder: [Z, W, V, Y],
                        NameForEasyFind: "Trevor");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, false }, { Y, false } },
                        ExpectedCallsOrder: [Z, W, V, U, Y],
                        NameForEasyFind: "Stuart");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, true }, { Y, false } },
                        ExpectedCallsOrder: [Z, W, V, U, A, Y],
                        NameForEasyFind: "Stewart");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, false }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [Z, W, V, Y, X],
                        NameForEasyFind: "Royston");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, false }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [Z, W, V, U, Y, X],
                        NameForEasyFind: "Rodney");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, true }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [Z, W, V, U, A, Y, X],
                        NameForEasyFind: "Norman");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, false }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [Z, W, V, U, Y, X, B],
                        NameForEasyFind: "Nigel");

                    yield return new(
                        Append: nameof(IHandlerMath.PackFirstInSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, true }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [Z, W, V, U, A, Y, X, B],
                        NameForEasyFind: "Neymar");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [],
                        ChecksSetup: [],
                        ExpectedCallsOrder: [A, B],
                        NameForEasyFind: "Neville");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Melvyn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [U, A, B],
                        NameForEasyFind: "Leslie");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [A, U],
                        NameForEasyFind: "Kobe");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U],
                        ChecksSetup: new() { { U, true } },
                        ExpectedCallsOrder: [A, U, B],
                        NameForEasyFind: "Iain");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Huxon");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [V, U, B],
                        NameForEasyFind: "Howard");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [V, U, A, B],
                        NameForEasyFind: "Horace");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [A, V],
                        NameForEasyFind: "Graham");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, false } },
                        ExpectedCallsOrder: [A, V, U],
                        NameForEasyFind: "Gordon");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V],
                        ChecksSetup: new() { { V, true }, { U, true } },
                        ExpectedCallsOrder: [A, V, U, B],
                        NameForEasyFind: "Glenn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [X],
                        NameForEasyFind: "Gary");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [X, W, B],
                        NameForEasyFind: "Finch");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [X, W, V, B],
                        NameForEasyFind: "Esteban");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [X, W, V, U, B],
                        NameForEasyFind: "Elison");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W, X],
                        BConditions: [],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [X, W, V, U, A, B],
                        NameForEasyFind: "Duran");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                        ExpectedCallsOrder: [A, X, W, V, U, B],
                        NameForEasyFind: "Drake");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                        ExpectedCallsOrder: [A, X, W, V, U],
                        NameForEasyFind: "Cyril");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [A, X, W, V],
                        NameForEasyFind: "Corby");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, true }, { W, false } },
                        ExpectedCallsOrder: [A, X, W],
                        NameForEasyFind: "Clifford");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [],
                        BConditions: [U, V, W, X],
                        ChecksSetup: new() { { X, false } },
                        ExpectedCallsOrder: [A, X],
                        NameForEasyFind: "Claude");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Clarence");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { U, true }, { V, false } },
                        ExpectedCallsOrder: [U, A, V],
                        NameForEasyFind: "Chad");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V],
                        ChecksSetup: new() { { U, true }, { V, true } },
                        ExpectedCallsOrder: [U, A, V, B],
                        NameForEasyFind: "Cecil");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Bill");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, false }, { W, false } },
                        ExpectedCallsOrder: [V, U, W],
                        NameForEasyFind: "Arlyn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, true }, { W, false } },
                        ExpectedCallsOrder: [V, U, A, W],
                        NameForEasyFind: "Ashton");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, false }, { W, true } },
                        ExpectedCallsOrder: [V, U, W, B],
                        NameForEasyFind: "Barry");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W],
                        ChecksSetup: new() { { V, true }, { U, true }, { W, true } },
                        ExpectedCallsOrder: [V, U, A, W, B],
                        NameForEasyFind: "Ajax");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, false } },
                        ExpectedCallsOrder: [U],
                        NameForEasyFind: "Sonnet");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, true }, { W, false } },
                        ExpectedCallsOrder: [U, A, W],
                        NameForEasyFind: "Winslow");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, true }, { W, true }, { V, false } },
                        ExpectedCallsOrder: [U, A, W, V],
                        NameForEasyFind: "Quinton");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U],
                        BConditions: [V, W],
                        ChecksSetup: new() { { U, true }, { W, true }, { V, true } },
                        ExpectedCallsOrder: [U, A, W, V, B],
                        NameForEasyFind: "Polly");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, false } },
                        ExpectedCallsOrder: [V],
                        NameForEasyFind: "Prudence");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, false }, { X, false } },
                        ExpectedCallsOrder: [V, U, X],
                        NameForEasyFind: "Ellison");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, true }, { X, false } },
                        ExpectedCallsOrder: [V, U, A, X],
                        NameForEasyFind: "Flynn");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, false }, { X, true }, { W, false } },
                        ExpectedCallsOrder: [V, U, X, W],
                        NameForEasyFind: "Florence");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, false }, { X, true }, { W, true } },
                        ExpectedCallsOrder: [V, U, X, W, B],
                        NameForEasyFind: "Magnus");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V],
                        BConditions: [W, X],
                        ChecksSetup: new() { { V, true }, { U, true }, { X, true }, { W, true } },
                        ExpectedCallsOrder: [V, U, A, X, W, B],
                        NameForEasyFind: "Clementine");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, false } },
                        ExpectedCallsOrder: [W],
                        NameForEasyFind: "Allegra");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, false }, { Z, false }, },
                        ExpectedCallsOrder: [W, V, Z],
                        NameForEasyFind: "Donte");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, false }, },
                        ExpectedCallsOrder: [W, V, U, Z],
                        NameForEasyFind: "Rogan");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, false }, },
                        ExpectedCallsOrder: [W, V, U, A, Z],
                        NameForEasyFind: "Yousef");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, false }, { Z, true }, { Y, false } },
                        ExpectedCallsOrder: [W, V, Z, Y],
                        NameForEasyFind: "Marla");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, false }, { Z, true }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [W, V, Z, Y, X],
                        NameForEasyFind: "Mikel");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, false }, { Z, true }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [W, V, Z, Y, X, B],
                        NameForEasyFind: "Ares");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, true }, { Y, false } },
                        ExpectedCallsOrder: [W, V, U, Z, Y],
                        NameForEasyFind: "Stephano");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, false } },
                        ExpectedCallsOrder: [W, V, U, A, Z, Y],
                        NameForEasyFind: "Niccola");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, true }, { Y, true }, { X, false } },
                        ExpectedCallsOrder: [W, V, U, Z, Y, X],
                        NameForEasyFind: "Apollo");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, true }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [W, V, U, Z, Y, X, B],
                        NameForEasyFind: "Booker");

                    yield return new(
                        Append: nameof(IHandlerMath.FirstCoverSecond),
                        AConditions: [U, V, W],
                        BConditions: [X, Y, Z],
                        ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, true }, { X, true } },
                        ExpectedCallsOrder: [W, V, U, A, Z, Y, X, B],
                        NameForEasyFind: "Diamond");
                }
            }

            const char CaseBlocksSeparator = '•';

            static string ViewOf(Dummy.ChainElementIndex? x) =>
                x?.Value ?? ".";

            static string ViewOf(bool x) =>
                x ? "I" : "O";

            static string ViewOf(Dummy.ChainElementIndex?[] x) => WithHandleEmptyCollection(x, x =>
                $"[{x.Select(ViewOf).Aggregate(string.Concat)}]");

            static string ViewOf(bool[] x) => WithHandleEmptyCollection(x, x =>
                $"[{x.Select(ViewOf).Aggregate(string.Concat)}]");

            static string ViewOf<TIndex>(Dictionary<TIndex, bool> x)
                where TIndex : Dummy.ChainElementIndex => WithHandleEmptyCollection(x, x =>
                $"[{x.Select(x => $"{{{ViewOf(x.Key)}-{ViewOf(x.Value)}}}")
                     .Aggregate(string.Concat)}]");

            static string WithHandleEmptyCollection<T>(IEnumerable<T> x,
                Func<IEnumerable<T>, string> mainConverter) =>
                    x.Any() ? mainConverter(x) : "[]";
        }
    }
}
