namespace Nuisho.ChainLead.Test;

using System.Collections.Immutable;
using Contracts;
using Contracts.Syntax;
using Types;

using static Cases.SingleHandler;
using static Contracts.Syntax.ChainLeadSyntax;
using static Dummy.ConditionIndex;
using static Dummy.HandlerIndex;

public static partial class Cases
{
    public static class HandlerMath
    {
        public sealed class AllAppendsAttribute()
            : ValuesAttribute(
                nameof(IHandlerMath.FirstThenSecond),
                nameof(IHandlerMath.PackFirstInSecond),
                nameof(IHandlerMath.InjectFirstIntoSecond),
                nameof(IHandlerMath.FirstCoverSecond),
                nameof(IHandlerMath.FirstWrapSecond),
                nameof(IHandlerMath.JoinFirstWithSecond),
                nameof(IHandlerMath.MergeFirstWithSecond));

        public abstract class OriginalTestFixtureAttribute(Type t)
            : TestFixtureAttribute(t, new OriginalHandlerMathFactory());

        public sealed class _I_Attribute() : OriginalTestFixtureAttribute(typeof(int));
        public sealed class _II_Attribute() : OriginalTestFixtureAttribute(typeof(string));
        public sealed class _III_Attribute() : OriginalTestFixtureAttribute(typeof(Class));
        public sealed class _IV_Attribute() : OriginalTestFixtureAttribute(typeof(Struct));
        public sealed class _V_Attribute() : OriginalTestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _VI_Attribute() : OriginalTestFixtureAttribute(typeof(Record));
        public sealed class _VII_Attribute() : OriginalTestFixtureAttribute(typeof(RecordStruct));
        public sealed class _VIII_Attribute() : OriginalTestFixtureAttribute(typeof(ReadonlyRecordStruct));

        public abstract class SyntaxTestFixtureAttribute(Type t)
            : TestFixtureAttribute(t, new SyntaxHandlerMathFactory());

        public sealed class _IX_Attribute() : SyntaxTestFixtureAttribute(typeof(int));
        public sealed class _X_Attribute() : SyntaxTestFixtureAttribute(typeof(string));
        public sealed class _XI_Attribute() : SyntaxTestFixtureAttribute(typeof(Class));
        public sealed class _XII_Attribute() : SyntaxTestFixtureAttribute(typeof(Struct));
        public sealed class _XIII_Attribute() : SyntaxTestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _XIV_Attribute() : SyntaxTestFixtureAttribute(typeof(Record));
        public sealed class _XV_Attribute() : SyntaxTestFixtureAttribute(typeof(RecordStruct));
        public sealed class _XVI_Attribute() : SyntaxTestFixtureAttribute(typeof(ReadonlyRecordStruct));

        public abstract class SeparatedTestFixtureAttribute(Type t)
            : TestFixtureAttribute(t, new SeparatedSyntaxHandlerMathFactory());

        public sealed class _XVII_Attribute() : SeparatedTestFixtureAttribute(typeof(int));
        public sealed class _XVIII_Attribute() : SeparatedTestFixtureAttribute(typeof(string));
        public sealed class _XIX_Attribute() : SeparatedTestFixtureAttribute(typeof(Class));
        public sealed class _XX_Attribute() : SeparatedTestFixtureAttribute(typeof(Struct));
        public sealed class _XXI_Attribute() : SeparatedTestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _XXII_Attribute() : SeparatedTestFixtureAttribute(typeof(Record));
        public sealed class _XXIII_Attribute() : SeparatedTestFixtureAttribute(typeof(RecordStruct));
        public sealed class _XXIV_Attribute() : SeparatedTestFixtureAttribute(typeof(ReadonlyRecordStruct));

        public abstract class ReversedTestFixtureAttribute(Type t)
            : TestFixtureAttribute(t, new ReversedSyntaxHandlerMathFactory());

        public sealed class _XXV_Attribute() : ReversedTestFixtureAttribute(typeof(int));
        public sealed class _XXVI_Attribute() : ReversedTestFixtureAttribute(typeof(string));
        public sealed class _XXVII_Attribute() : ReversedTestFixtureAttribute(typeof(Class));
        public sealed class _XXVIII_Attribute() : ReversedTestFixtureAttribute(typeof(Struct));
        public sealed class _XXIX_Attribute() : ReversedTestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _XXX_Attribute() : ReversedTestFixtureAttribute(typeof(Record));
        public sealed class _XXXI_Attribute() : ReversedTestFixtureAttribute(typeof(RecordStruct));
        public sealed class _XXXII_Attribute() : ReversedTestFixtureAttribute(typeof(ReadonlyRecordStruct));

        public interface IHandlerMathFactory
        {
            ITestingHandlerMath Create(IConditionMath conditionMath);
        }

        public interface ITestingHandlerMath : IHandlerMath;

        public sealed class OriginalHandlerMathFactory
            : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(new Implementation.HandlerMath(conditionMath));

            public override string ToString() => "like _math.PackFirstInSecond(a, b)";

            sealed class Product(IHandlerMath math) :
                OriginalMathFactory.Product(math),
                ITestingHandlerMath
            {
                public IHandler<T> FirstCoverSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.FirstCoverSecond(prev, next);

                public IHandler<T> FirstThenSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.FirstThenSecond(prev, next);

                public IHandler<T> FirstWrapSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.FirstWrapSecond(prev, next);

                public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.InjectFirstIntoSecond(prev, next);

                public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.JoinFirstWithSecond(prev, next);

                public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.MergeFirstWithSecond(prev, next);

                public IHandler<T> PackFirstInSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Math.PackFirstInSecond(prev, next);
            }
        }

        public sealed class SyntaxHandlerMathFactory
            : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(conditionMath);

            public override string ToString() => "like PackFirstInSecond(a, b)";

            sealed class Product(IConditionMath conditionMath)
                    : SyntaxMathFactory.Product(conditionMath),
                ITestingHandlerMath
            {
                public IHandler<T> FirstThenSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.FirstThenSecond(prev, next);

                public IHandler<T> FirstCoverSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.FirstCoverSecond(prev, next);

                public IHandler<T> FirstWrapSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.FirstWrapSecond(prev, next);

                public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.InjectFirstIntoSecond(prev, next);

                public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.JoinFirstWithSecond(prev, next);

                public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.MergeFirstWithSecond(prev, next);

                public IHandler<T> PackFirstInSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    ChainLeadSyntax.PackFirstInSecond(prev, next);
            }
        }

        public sealed class SeparatedSyntaxHandlerMathFactory
            : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(conditionMath);

            public override string ToString() => "like Pack(a).In(b)";

            sealed class Product(IConditionMath conditionMath)
                    : SyntaxMathFactory.Product(conditionMath),
                ITestingHandlerMath
            {
                public IHandler<T> FirstThenSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    prev.Then(next);

                public IHandler<T> FirstCoverSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Use(prev).ToCover(next);

                public IHandler<T> FirstWrapSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Use(prev).ToWrap(next);

                public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Inject(prev).Into(next);

                public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Join(prev).With(next);

                public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Merge(prev).With(next);

                public IHandler<T> PackFirstInSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    Pack(prev).In(next);
            }
        }

        public sealed class ReversedSyntaxHandlerMathFactory
           : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(conditionMath);

            public override string ToString() => "like PackXIn(b).WhereXIs(a)";

            sealed class Product(IConditionMath conditionMath)
                    : SyntaxMathFactory.Product(conditionMath),
                ITestingHandlerMath
            {
                public IHandler<T> FirstThenSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    XThen(next).WhereXIs(prev);

                public IHandler<T> FirstCoverSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    XCover(next).WhereXIs(prev);

                public IHandler<T> FirstWrapSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    XWrap(next).WhereXIs(prev);

                public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    InjectXInto(next).WhereXIs(prev);

                public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    JoinXWith(next).WhereXIs(prev);

                public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    MergeXWith(next).WhereXIs(prev);

                public IHandler<T> PackFirstInSecond<T>(IHandler<T> prev, IHandler<T> next) =>
                    PackXIn(next).WhereXIs(prev);
            }
        }

        public sealed class BlueCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(BlueCases));

        public sealed class RedCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(RedCases));

        public sealed class GreenCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(GreenCases));

        public sealed class OrangeCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(OrangeCases));

        public sealed class YellowCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(YellowCases));

        public sealed class WhiteCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(WhiteCases));

        public sealed class BlackCasesAttribute()
            : ValueSourceAttribute(
                typeof(HandlerMath),
                nameof(BlackCases));

        public record BlueCase(
            ImmutableArray<Dummy.HandlerIndex> Chain,
            string NameForEasyFind = "")
        {
            public override string ToString()
            {
                var name = ViewOf(Chain);
                if (NameForEasyFind != string.Empty)
                    name = $"{NameForEasyFind}: {name}";

                return name;
            }
        }

        public static ImmutableArray<BlueCase> BlueCases =>
        [
            new ([A, A], "1"), new ([A, B], "2"), new ([A, B, C], "3"), new ([A, B, C, D, E, F], "4"),
            new ([A, B, C, D, A, B, B, B, A, B, C, D, E, F, H, I, A, B, C, D, C, C, D], "5"),
            new ([A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A], "6")
        ];

        public record RedCase(
            ImmutableArray<Dummy.HandlerIndex> Chain,
            ImmutableArray<Dummy.HandlerIndex> ExpectedExecution,
            string Name)
        {
            public override string ToString() => Name;

            public string Description { get; } =
                string.Join(
                    CaseBlocksSeparator,
                    ViewOf(Chain),
                    ViewOf(ExpectedExecution));

            public static readonly Dummy.HandlerIndex Zero = new ("ZERO");

            public static bool IsNotZero(Dummy.HandlerIndex x) => x != Zero;
        }

        public static IEnumerable<RedCase> RedCases
        {
            get
            {
                var zero = RedCase.Zero;

                yield return new ([zero, A], [A], "Sunday");
                yield return new ([A, zero], [A], "Monday");
                yield return new ([zero, A, zero], [A], "Tuesday");
                yield return new ([zero, zero, zero, zero, A, zero, zero, zero], [A], "Wednesday");

                yield return new (
                    [A, B, C, D, zero, I, J, zero, I, zero, zero, E, I, H, zero, F, F, I, zero, zero],
                    [A, B, C, D, I, J, I, E, I, H, F, F, I],
                    "Thursday");

                yield return new (
                    [A, zero, A, zero, A, zero, A, zero, A, zero, A, zero, A, A, A, A, zero, zero, zero, A, zero, A, A, A],
                    [A, A, A, A, A, A, A, A, A, A, A, A, A, A],
                    "Friday");
            }
        }

        public record GreenCase(
            bool AIsConditional,
            bool BIsConditional,
            Dummy.ConditionIndex ExpectedFinalCondition,
            bool FinalConditionCheckResult,
            string Name)
        {
            public override string ToString() => Name;

            public string Description { get; } =
                string.Join(
                    CaseBlocksSeparator,
                    ViewOf(AIsConditional),
                    ViewOf(BIsConditional),
                    ExpectedFinalCondition.Value,
                    ViewOf(FinalConditionCheckResult));
        }

        public static IEnumerable<GreenCase> GreenCases
        {
            get
            {
                yield return new (false, true, Y, false, "Sun");
                yield return new (false, true, Y, true, "Moon");
                yield return new (true, false, X, false, "Venus");
                yield return new (true, false, X, true, "Earth");
                yield return new (true, true, X & Y, false, "Mars");
                yield return new (true, true, X & Y, true, "Saturn");
            }
        }

        public record OrangeCase(
            ImmutableDictionary<Dummy.ConditionIndex, bool> CheckSetup,
            ImmutableDictionary<Dummy.ConditionIndex, bool> CheckExpectations,
            ImmutableDictionary<Dummy.HandlerIndex, bool> ExecutionExpectations,
            string Name)
        {
            public override string ToString() => Name;

            public string Description { get; } = string.Join(
                CaseBlocksSeparator,
                ViewOf(CheckSetup),
                ViewOf(CheckExpectations),
                ViewOf(ExecutionExpectations));

            public static readonly Dummy.HandlerIndex
                A = Dummy.HandlerIndex.A,
                B = Dummy.HandlerIndex.B;

            public static readonly Dummy.ConditionIndex
                A_Top = new ("A TOP"),
                B_Top = new ("B TOP"),
                A_Bottom = new ("A BOTTOM"),
                B_Bottom = new ("B BOTTOM");
        }

        public static IEnumerable<OrangeCase> OrangeCases
        {
            get
            {
                var a = OrangeCase.A;
                var b = OrangeCase.B;
                var a_bottom = OrangeCase.A_Bottom;
                var b_bottom = OrangeCase.B_Bottom;
                var a_top = OrangeCase.A_Top;
                var b_top = OrangeCase.B_Top;

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, false), (b_bottom, false), (a_top & b_top, false)),
                    CheckExpectations:     Dictionary((a_bottom, false), (b_bottom, false), (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, false), (b, false)),
                    Name: "West");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, false), (b_bottom, true),  (a_top & b_top, false)),
                    CheckExpectations:     Dictionary((a_bottom, false), (b_bottom, false), (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, false), (b, false)),
                    Name: "Northwest");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, true),  (b_bottom, false), (a_top & b_top, false)),
                    CheckExpectations:     Dictionary((a_bottom, false), (b_bottom, false), (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, false), (b, false)),
                    Name: "North");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, true),  (b_bottom, true),  (a_top & b_top, false)),
                    CheckExpectations:     Dictionary((a_bottom, false), (b_bottom, false), (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, false), (b, false)),
                    Name: "Northeast");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, false), (b_bottom, false), (a_top & b_top, true)),
                    CheckExpectations:     Dictionary((a_bottom, true),  (b_bottom, true),  (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, false), (b, false)),
                    Name: "East");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, false), (b_bottom, true), (a_top & b_top, true)),
                    CheckExpectations:     Dictionary((a_bottom, true),  (b_bottom, true), (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, false), (b, true)),
                    Name: "Southeast");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, true), (b_bottom, false), (a_top & b_top, true)),
                    CheckExpectations:     Dictionary((a_bottom, true), (b_bottom, true),  (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, true), (b, false)),
                    Name: "South");

                yield return new (
                    CheckSetup:            Dictionary((a_bottom, true), (b_bottom, true), (a_top & b_top, true)),
                    CheckExpectations:     Dictionary((a_bottom, true), (b_bottom, true), (a_top & b_top, true)),
                    ExecutionExpectations: Dictionary((a, true), (b, true)),
                    Name: "Southwest");
            }
        }

        public record YellowCase(
            ImmutableArray<Dummy.ConditionIndex> AConditions,
            ImmutableArray<Dummy.ConditionIndex> BConditions,
            ImmutableDictionary<Dummy.ConditionIndex, bool> ChecksSetup,
            ImmutableArray<Dummy.ConditionIndex> ExpectedToCheck,
            ImmutableArray<Dummy.HandlerIndex> ExpectedToExecute,
            string Name)
        {
            public static readonly Dummy.ConditionIndex TopAnd = new ("TOP AND");

            public override string ToString() => Name;

            public string Description { get; } =
                string.Join(
                    CaseBlocksSeparator,
                    ViewOf(AConditions),
                    ViewOf(BConditions),
                    ViewOf(ChecksSetup),
                    ViewOf(ExpectedToCheck),
                    ViewOf(ExpectedToExecute));
        }

        public static IEnumerable<YellowCase> YellowCases
        {
            get
            {
                var topAnd = YellowCase.TopAnd;

                yield return new (
                    AConditions:       [],
                    BConditions:       [],
                    ChecksSetup:       Dictionary<Dummy.ConditionIndex, bool>(),
                    ExpectedToCheck:   [],
                    ExpectedToExecute: [A, B],
                    Name: "James");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((X, false)),
                    ExpectedToCheck:   [X],
                    ExpectedToExecute: [],
                    Name: "Michael");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((X, true)),
                    ExpectedToCheck:   [X],
                    ExpectedToExecute: [A, B],
                    Name: "Robert");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Y, false)),
                    ExpectedToCheck:   [Y],
                    ExpectedToExecute: [],
                    Name: "John");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Y, true), (X, false)),
                    ExpectedToCheck:   [Y, X],
                    ExpectedToExecute: [B],
                    Name: "David");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Y, true), (X, true)),
                    ExpectedToCheck:   [Y, X],
                    ExpectedToExecute: [A, B],
                    Name: "William");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Z, false)),
                    ExpectedToCheck:   [Z],
                    ExpectedToExecute: [],
                    Name: "Richard");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Z, true), (Y, false)),
                    ExpectedToCheck:   [Z, Y],
                    ExpectedToExecute: [B],
                    Name: "Joseph");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Z, true), (Y, true), (X, false)),
                    ExpectedToCheck:   [Z, Y, X],
                    ExpectedToExecute: [B],
                    Name: "Thomas");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [],
                    ChecksSetup:       Dictionary((Z, true), (Y, true), (X, true)),
                    ExpectedToCheck:   [Z, Y, X],
                    ExpectedToExecute: [A, B],
                    Name: "Christopher");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Charles");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Daniel");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Matthew");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Anthony");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Mark");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Donald");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Steven");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Andrew");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, false)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [],
                    Name: "Paul");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, true)),
                    ExpectedToCheck:   [topAnd],
                    ExpectedToExecute: [A, B],
                    Name: "Joshua");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, true), (X, false)),
                    ExpectedToCheck:   [topAnd, X],
                    ExpectedToExecute: [B],
                    Name: "Kenneth");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, true), (X, true)),
                    ExpectedToCheck:   [topAnd, X],
                    ExpectedToExecute: [A, B],
                    Name: "Kevin");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, false)),
                    ExpectedToCheck:   [topAnd, Y],
                    ExpectedToExecute: [B],
                    Name: "Brian");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false)),
                    ExpectedToCheck:   [topAnd, Y, X],
                    ExpectedToExecute: [B],
                    Name: "Timothy");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, true)),
                    ExpectedToCheck:   [topAnd, Y, X],
                    ExpectedToExecute: [A, B],
                    Name: "Ronald");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (U, false)),
                    ExpectedToCheck:   [topAnd, U],
                    ExpectedToExecute: [A],
                    Name: "George");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (U, true)),
                    ExpectedToCheck:   [topAnd, U],
                    ExpectedToExecute: [A, B],
                    Name: "Jason");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (V, true), (U, false)),
                    ExpectedToCheck:   [topAnd, V, U],
                    ExpectedToExecute: [A],
                    Name: "Edward");

                yield return new (
                    AConditions:       [X],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (V, true), (U, true)),
                    ExpectedToCheck:   [topAnd, V, U],
                    ExpectedToExecute: [A, B],
                    Name: "Jeffrey");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (X, false), (U, false)),
                    ExpectedToCheck:   [topAnd, X, U],
                    ExpectedToExecute: [],
                    Name: "Ryan");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (X, true), (U, false)),
                    ExpectedToCheck:   [topAnd, X, U],
                    ExpectedToExecute: [A],
                    Name: "Jacob");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (X, true), (U, false)),
                    ExpectedToCheck:   [topAnd, X, U],
                    ExpectedToExecute: [A],
                    Name: "Nicholas");

                yield return new (
                    AConditions:       [X, Y],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (X, true), (U, true)),
                    ExpectedToCheck:   [topAnd, X, U],
                    ExpectedToExecute: [A, B],
                    Name: "Gary");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, false), (U, false)),
                    ExpectedToCheck:   [topAnd, Y, U],
                    ExpectedToExecute: [],
                    Name: "Eric");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false), (U, false)),
                    ExpectedToCheck:   [topAnd, Y, X, U],
                    ExpectedToExecute: [],
                    Name: "Jonathan");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, true), (U, false)),
                    ExpectedToCheck:   [topAnd, Y, X, U],
                    ExpectedToExecute: [A],
                    Name: "Stephen");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, false), (U, true)),
                    ExpectedToCheck:   [topAnd, Y, U],
                    ExpectedToExecute: [B],
                    Name: "Larry");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false), (U, true)),
                    ExpectedToCheck:   [topAnd, Y, X, U],
                    ExpectedToExecute: [B],
                    Name: "Justin");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, true), (U, true)),
                    ExpectedToCheck:   [topAnd, Y, X, U],
                    ExpectedToExecute: [A, B],
                    Name: "Scott");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, false), (V, false)),
                    ExpectedToCheck:   [topAnd, Y, V],
                    ExpectedToExecute: [],
                    Name: "Brandon");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false), (V, false)),
                    ExpectedToCheck:   [topAnd, Y, X, V],
                    ExpectedToExecute: [],
                    Name: "Benjamin");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false), (V, true), (U, false)),
                    ExpectedToCheck:   [topAnd, Y, X, V, U],
                    ExpectedToExecute: [],
                    Name: "Samuel");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false), (V, false)),
                    ExpectedToCheck:   [topAnd, Y, X, V],
                    ExpectedToExecute: [],
                    Name: "Gregory");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, true), (V, true), (U, false)),
                    ExpectedToCheck:   [topAnd, Y, X, V, U],
                    ExpectedToExecute: [A],
                    Name: "Alexander");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, false), (V, true), (U, false)),
                    ExpectedToCheck:   [topAnd, Y, V, U],
                    ExpectedToExecute: [],
                    Name: "Patrick");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, false), (V, true), (U, true)),
                    ExpectedToCheck:   [topAnd, Y, V, U],
                    ExpectedToExecute: [B],
                    Name: "Frank");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, false), (V, true), (U, true)),
                    ExpectedToCheck:   [topAnd, Y, X, V, U],
                    ExpectedToExecute: [B],
                    Name: "Raymond");

                yield return new (
                    AConditions:       [X, Y, Z],
                    BConditions:       [U, V, W],
                    ChecksSetup:       Dictionary((topAnd, true), (Y, true), (X, true), (V, true), (U, true)),
                    ExpectedToCheck:   [topAnd, Y, X, V, U],
                    ExpectedToExecute: [A, B],
                    Name: "Jack");
            }
        }

        public record WhiteCase(
            ImmutableArray<Dummy.ConditionIndex> AConditions,
            ImmutableArray<Dummy.ConditionIndex> BConditions,
            string Name)
        {
            public override string ToString() => Name;

            public string Description { get; } =
                string.Join(
                    CaseBlocksSeparator,
                    ViewOf(AConditions),
                    ViewOf(BConditions));
        }

        public static IEnumerable<WhiteCase> WhiteCases
        {
            get
            {
                yield return new ([Q, R], [], "Tyler");
                yield return new ([Q, R, S], [], "Aaron");
                yield return new ([Q, R, S, T, U], [], "Jose");
                yield return new ([Q, R, S, T, U, V, W, X], [], "Adam");

                yield return new ([], [Q, R], "Nathan");
                yield return new ([], [Q, R, S], "Henry");
                yield return new ([], [Q, R, S, T, U], "Zachary");
                yield return new ([], [Q, R, S, T, U, V, W], "Douglas");

                yield return new ([Q], [R], "Peter");
                yield return new ([Q, R], [S], "Kyle");
                yield return new ([Q, R, S], [T], "Noah");
                yield return new ([Q, R, S], [T, U], "Ethan");
                yield return new ([Q, R, S], [T, U, V], "Jeremy");
                yield return new ([Q, R, S, T, U], [V], "Christian");
                yield return new ([Q, R, S, T, U], [V, W], "Walter");
                yield return new ([Q, R, S, T, U], [V, W, X], "Keith");
                yield return new ([Q, R, S, T, U], [V, W, X, Y], "Austin");
                yield return new ([Q, R, S, T, U, V, W], [X], "Roger");
                yield return new ([Q, R, S, T, U, V, W], [X, Y], "Terry");
                yield return new ([Q, R, S, T, U, V, W, X], [Y], "Sean");

                yield return new ([Q], [R, S], "Gerald");
                yield return new ([Q], [R, S, T], "Carl");
                yield return new ([Q], [R, S, T, U], "Dylan");
                yield return new ([Q, R], [S, T, U, V], "Harold");
                yield return new ([Q, R, S], [T, U, V, W], "Jordan");
                yield return new ([Q, R], [S, T, U, V, W, X], "Jesse");
                yield return new ([Q, R], [S, T, U, V, W, X, Y], "Bryan");
                yield return new ([Q, R, S, T], [U, V, W, X, Z], "Lawrence");
                yield return new ([Q], [R, S, T, U, V, W, X, Y], "Arthur");
                yield return new ([Q, R, S], [ T, U, V, W, X, Y], "Gabriel");
            }
        }

        public record BlackCase(
            string Append,
            ImmutableArray<Dummy.ConditionIndex> AConditions,
            ImmutableArray<Dummy.ConditionIndex> BConditions,
            ImmutableDictionary<Dummy.ConditionIndex, bool> ChecksSetup,
            ImmutableArray<Dummy.ChainElementIndex> ExpectedCallsOrder,
            string Name)
        {
            public override string ToString() => Name;

            public string Description { get; } =
                string.Join(
                    CaseBlocksSeparator,
                    Append,
                    ViewOf(AConditions),
                    ViewOf(BConditions),
                    ViewOf(ChecksSetup),
                    ViewOf(ExpectedCallsOrder));
        }

        public static IEnumerable<BlackCase> BlackCases
        {
            get
            {
                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [],
                    ChecksSetup:        Dictionary<Dummy.ConditionIndex, bool>(),
                    ExpectedCallsOrder: [A, B],
                    Name: "Mary");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U, B],
                    Name: "Patricia");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [U, A, B],
                    Name: "Jennifer");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Linda");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [U, A, B],
                    Name: "Elizabeth");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V, B],
                    Name: "Barbara");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U, B],
                    Name: "Susan");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Jessica");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Karen");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U],
                    Name: "Sarah");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Lisa");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X, B],
                    Name: "Nancy");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, W, B],
                    Name: "Sandra");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true ), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, W, V, B],
                    Name: "Betty");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, W, V, U, B],
                    Name: "Ashley");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, W, V, U, A, B],
                    Name: "Emily");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, W, V, U, A, B],
                    Name: "Kimberly");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, W, V, U],
                    Name: "Margaret");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, W, V],
                    Name: "Donna");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, W],
                    Name: "Michelle");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X],
                    Name: "Carol");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Amanda");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U, B],
                    Name: "Melissa");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Deborah");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Stephanie");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Rebecca");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, true), (V, false)),
                    ExpectedCallsOrder: [W, V, B],
                    Name: "Sharon");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [W, V, U, B],
                    Name: "Laura");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [W, V, U, A, B],
                    Name: "Cynthia");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Dorothy");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (V, false)),
                    ExpectedCallsOrder: [W, V],
                    Name: "Amy");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [W, V, U, B],
                    Name: "Kathleen");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [W, V, U, A, B],
                    Name: "Angela");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X],
                    Name: "Shirley");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, W],
                    Name: "Emma");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, W, V, B],
                    Name: "Brenda");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true ), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, W, V, U, B],
                    Name: "Pamela");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, W, V, U, A, B],
                    Name: "Nicole");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, false)),
                    ExpectedCallsOrder: [Z],
                    Name: "Anna");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (Y, false)),
                    ExpectedCallsOrder: [Z, Y],
                    Name: "Samantha");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (Y, true), (X, false)),
                    ExpectedCallsOrder: [Z, Y, X],
                    Name: "Katherine");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (Y, true), (X, true), (W, false)),
                    ExpectedCallsOrder: [Z, Y, X, W, B],
                    Name: "Christine");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (Y, true), (X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [Z, Y, X, W, V, B],
                    Name: "Debra");

                yield return new (
                    Append:             nameof(IHandlerMath.InjectFirstIntoSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (Y, true), (X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [Z, Y, X, W, V, U, B],
                    Name: "Rachel");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [],
                    ChecksSetup:        Dictionary<Dummy.ConditionIndex, bool>(),
                    ExpectedCallsOrder: [A, B],
                    Name: "Carolyn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Janet");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [U, A, B],
                    Name: "Maria");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [A, U],
                    Name: "Olivia");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [A, U, B],
                    Name: "Heather");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [A, V],
                    Name: "Helen");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [A, V, U],
                    Name: "Catherine");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [A, V, U, B],
                    Name: "Diane");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Julie");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U],
                    Name: "Victoria");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Joyce");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X],
                    Name: "Lauren");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, W],
                    Name: "Kelly");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, W, V],
                    Name: "Christina");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, W, V, U],
                    Name: "Ruth");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, W, V, U, A, B],
                    Name: "Joan");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [A, X, W, V, U, B],
                    Name: "Virginia");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [A, X, W, V, U],
                    Name: "Judith");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [A, X, W, V],
                    Name: "Evelyn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [A, X, W],
                    Name: "Hannah");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [A, X],
                    Name: "Andrea");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Megan");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((U, true), (V, false)),
                    ExpectedCallsOrder: [U, A, V],
                    Name: "Cheryl");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((U, true), (V, true)),
                    ExpectedCallsOrder: [U, A, V, B],
                    Name: "Jacqueline");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Madison");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U],
                    Name: "Teresa");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, true), (W, false)),
                    ExpectedCallsOrder: [V, U, A, W],
                    Name: "Abigail");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, true), (W, true)),
                    ExpectedCallsOrder: [V, U, A, W, B],
                    Name: "Sophia");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Martha");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, true), (W, false)),
                    ExpectedCallsOrder: [U, A, W],
                    Name: "Sara");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [U, A, W, V],
                    Name: "Gloria");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, true), (W, true), (V, true)),
                    ExpectedCallsOrder: [U, A, W, V, B],
                    Name: "Janice");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Kathryn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U],
                    Name: "Ann");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, true), (X, false)),
                    ExpectedCallsOrder: [V, U, A, X],
                    Name: "Isabella");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, true), (X, true), (W, false)),
                    ExpectedCallsOrder: [V, U, A, X, W],
                    Name: "Judy");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, true), (X, true), (W, true)),
                    ExpectedCallsOrder: [V, U, A, X, W, B],
                    Name: "Charlotte");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Julia");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, false)),
                    ExpectedCallsOrder: [W, V],
                    Name: "Grace");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [W, V, U],
                    Name: "Amber");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, false)),
                    ExpectedCallsOrder: [W, V, U, A, Z],
                    Name: "Alice");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, true), (Y, false)),
                    ExpectedCallsOrder: [W, V, U, A, Z, Y],
                    Name: "Jean");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, true), (Y, true), (X, false)),
                    ExpectedCallsOrder: [W, V, U, A, Z, Y, X],
                    Name: "Denise");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstWrapSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, true), (Y, true), (X, true)),
                    ExpectedCallsOrder: [W, V, U, A, Z, Y, X, B],
                    Name: "Frances");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [],
                    ChecksSetup:        Dictionary<Dummy.ConditionIndex, bool>(),
                    ExpectedCallsOrder: [A, B],
                    Name: "Danielle");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U, B],
                    Name: "Daniella");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [U, A, B],
                    Name: "Natalie");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Beverly");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [U, A, B],
                    Name: "Diana");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V, B],
                    Name: "Brittany");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U, B],
                    Name: "Theresa");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Kayla");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Alexis");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, A, U],
                    Name: "Doris");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, A, U, B],
                    Name: "Lori");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X, B],
                    Name: "Tiffany");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, W, B],
                    Name: "Carl");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, W, V, B],
                    Name: "Dylan");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, W, V, U, B],
                    Name: "Harold");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, W, V, U, A, B],
                    Name: "Jesse");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, A, W, V, U, B],
                    Name: "Bryan");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, A, W, V, U],
                    Name: "Lawrence");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, A, W, V],
                    Name: "Arthur");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, A, W],
                    Name: "Gabriel");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X],
                    Name: "Bruce");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Logan");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U, B],
                    Name: "Billy");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Joe");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Alan");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, true), (V, false)),
                    ExpectedCallsOrder: [W, V, B],
                    Name: "Juan");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [W, V, U, B],
                    Name: "Elijah");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [W, V, U, A, B],
                    Name: "Willie");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (U, true), (V, true)),
                    ExpectedCallsOrder: [W, U, A, V, B],
                    Name: "Albert");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (U, true), (V, false)),
                    ExpectedCallsOrder: [W, U, A, V],
                    Name: "Wayne");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (U, false), (V, false)),
                    ExpectedCallsOrder: [W, U, V],
                    Name: "Randy");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, true), (U, false), (V, true)),
                    ExpectedCallsOrder: [W, U, V, B],
                    Name: "Mason");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Vincent");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X],
                    Name: "Liam");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (V, false), (W, false)),
                    ExpectedCallsOrder: [X, V, W],
                    Name: "Roy");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (V, false), (W, true)),
                    ExpectedCallsOrder: [X, V, W, B],
                    Name: "Bobby");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (V, true), (U, false), (W, false)),
                    ExpectedCallsOrder: [X, V, U, W],
                    Name: "Caleb");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (V, true), (U, false), (W, true)),
                    ExpectedCallsOrder: [X, V, U, W, B],
                    Name: "Bradley");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (V, true), (U, true), (W, false)),
                    ExpectedCallsOrder: [X, V, U, A, W],
                    Name: "Russell");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((X, true), (V, true), (U, true), (W, true)),
                    ExpectedCallsOrder: [X, V, U, A, W, B],
                    Name: "Lucas");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, false)),
                    ExpectedCallsOrder: [Z],
                    Name: "Zekiel");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, false), (Y, false)),
                    ExpectedCallsOrder: [Z, W, Y],
                    Name: "Yuri");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, false), (Y, true), (X, false)),
                    ExpectedCallsOrder: [Z, W, Y, X],
                    Name: "Tyde");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, false), (Y, true), (X, true)),
                    ExpectedCallsOrder: [Z, W, Y, X, B],
                    Name: "Turner");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, false), (Y, false)),
                    ExpectedCallsOrder: [Z, W, V, Y],
                    Name: "Trevor");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, true), (U, false), (Y, false)),
                    ExpectedCallsOrder: [Z, W, V, U, Y],
                    Name: "Stuart");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, true), (U, true), (Y, false)),
                    ExpectedCallsOrder: [Z, W, V, U, A, Y],
                    Name: "Stewart");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, false), (Y, true), (X, false)),
                    ExpectedCallsOrder: [Z, W, V, Y, X],
                    Name: "Royston");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, true), (U, false), (Y, true), (X, false)),
                    ExpectedCallsOrder: [Z, W, V, U, Y, X],
                    Name: "Rodney");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, true), (U, true), (Y, true), (X, false)),
                    ExpectedCallsOrder: [Z, W, V, U, A, Y, X],
                    Name: "Norman");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, true), (U, false), (Y, true), (X, true)),
                    ExpectedCallsOrder: [Z, W, V, U, Y, X, B],
                    Name: "Nigel");

                yield return new (
                    Append:             nameof(IHandlerMath.PackFirstInSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((Z, true), (W, true), (V, true), (U, true), (Y, true), (X, true)),
                    ExpectedCallsOrder: [Z, W, V, U, A, Y, X, B],
                    Name: "Neymar");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [],
                    ChecksSetup:        Dictionary<Dummy.ConditionIndex, bool>(),
                    ExpectedCallsOrder: [A, B],
                    Name: "Neville");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Melvyn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [U, A, B],
                    Name: "Leslie");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [A, U],
                    Name: "Kobe");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U],
                    ChecksSetup:        Dictionary((U, true)),
                    ExpectedCallsOrder: [A, U, B],
                    Name: "Iain");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Huxon");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [V, U, B],
                    Name: "Howard");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [V, U, A, B],
                    Name: "Horace");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [A, V],
                    Name: "Graham");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, false)),
                    ExpectedCallsOrder: [A, V, U],
                    Name: "Gordon");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V],
                    ChecksSetup:        Dictionary((V, true), (U, true)),
                    ExpectedCallsOrder: [A, V, U, B],
                    Name: "Glenn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [X],
                    Name: "Gary");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [X, W, B],
                    Name: "Finch");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [X, W, V, B],
                    Name: "Esteban");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [X, W, V, U, B],
                    Name: "Elison");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W, X],
                    BConditions:        [],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [X, W, V, U, A, B],
                    Name: "Duran");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, true)),
                    ExpectedCallsOrder: [A, X, W, V, U, B],
                    Name: "Drake");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, true), (U, false)),
                    ExpectedCallsOrder: [A, X, W, V, U],
                    Name: "Cyril");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [A, X, W, V],
                    Name: "Corby");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, true), (W, false)),
                    ExpectedCallsOrder: [A, X, W],
                    Name: "Clifford");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [],
                    BConditions:        [U, V, W, X],
                    ChecksSetup:        Dictionary((X, false)),
                    ExpectedCallsOrder: [A, X],
                    Name: "Claude");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Clarence");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((U, true), (V, false)),
                    ExpectedCallsOrder: [U, A, V],
                    Name: "Chad");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V],
                    ChecksSetup:        Dictionary((U, true), (V, true)),
                    ExpectedCallsOrder: [U, A, V, B],
                    Name: "Cecil");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Bill");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, false), (W, false)),
                    ExpectedCallsOrder: [V, U, W],
                    Name: "Arlyn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, true), (W, false)),
                    ExpectedCallsOrder: [V, U, A, W],
                    Name: "Ashton");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, false), (W, true)),
                    ExpectedCallsOrder: [V, U, W, B],
                    Name: "Barry");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W],
                    ChecksSetup:        Dictionary((V, true), (U, true), (W, true)),
                    ExpectedCallsOrder: [V, U, A, W, B],
                    Name: "Ajax");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, false)),
                    ExpectedCallsOrder: [U],
                    Name: "Sonnet");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, true), (W, false)),
                    ExpectedCallsOrder: [U, A, W],
                    Name: "Winslow");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, true), (W, true), (V, false)),
                    ExpectedCallsOrder: [U, A, W, V],
                    Name: "Quinton");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U],
                    BConditions:        [V, W],
                    ChecksSetup:        Dictionary((U, true), (W, true), (V, true)),
                    ExpectedCallsOrder: [U, A, W, V, B],
                    Name: "Polly");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, false)),
                    ExpectedCallsOrder: [V],
                    Name: "Prudence");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, false), (X, false)),
                    ExpectedCallsOrder: [V, U, X],
                    Name: "Ellison");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, true), (X, false)),
                    ExpectedCallsOrder: [V, U, A, X],
                    Name: "Flynn");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, false), (X, true), (W, false)),
                    ExpectedCallsOrder: [V, U, X, W],
                    Name: "Florence");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, false), (X, true), (W, true)),
                    ExpectedCallsOrder: [V, U, X, W, B],
                    Name: "Magnus");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V],
                    BConditions:        [W, X],
                    ChecksSetup:        Dictionary((V, true), (U, true), (X, true), (W, true)),
                    ExpectedCallsOrder: [V, U, A, X, W, B],
                    Name: "Clementine");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, false)),
                    ExpectedCallsOrder: [W],
                    Name: "Allegra");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, false), (Z, false)),
                    ExpectedCallsOrder: [W, V, Z],
                    Name: "Donte");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false), (Z, false)),
                    ExpectedCallsOrder: [W, V, U, Z],
                    Name: "Rogan");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, false)),
                    ExpectedCallsOrder: [W, V, U, A, Z],
                    Name: "Yousef");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, false), (Z, true), (Y, false)),
                    ExpectedCallsOrder: [W, V, Z, Y],
                    Name: "Marla");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, false), (Z, true), (Y, true), (X, false)),
                    ExpectedCallsOrder: [W, V, Z, Y, X],
                    Name: "Mikel");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, false), (Z, true), (Y, true), (X, true)),
                    ExpectedCallsOrder: [W, V, Z, Y, X, B],
                    Name: "Ares");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false), (Z, true), (Y, false)),
                    ExpectedCallsOrder: [W, V, U, Z, Y],
                    Name: "Stephano");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, true), (Y, false)),
                    ExpectedCallsOrder: [W, V, U, A, Z, Y],
                    Name: "Niccola");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false), (Z, true), (Y, true), (X, false)),
                    ExpectedCallsOrder: [W, V, U, Z, Y, X],
                    Name: "Apollo");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, false), (Z, true), (Y, true), (X, true)),
                    ExpectedCallsOrder: [W, V, U, Z, Y, X, B],
                    Name: "Booker");

                yield return new (
                    Append:             nameof(IHandlerMath.FirstCoverSecond),
                    AConditions:        [U, V, W],
                    BConditions:        [X, Y, Z],
                    ChecksSetup:        Dictionary((W, true), (V, true), (U, true), (Z, true), (Y, true), (X, true)),
                    ExpectedCallsOrder: [W, V, U, A, Z, Y, X, B],
                    Name: "Diamond");
            }
        }

        const char CaseBlocksSeparator = '•';

        static string ViewOf(Dummy.ChainElementIndex? x) =>
            x?.Value ?? ".";

        static string ViewOf(bool x) =>
            x ? "I" : "O";

        static string ViewOf(IEnumerable<Dummy.ChainElementIndex?> x) => WithHandleEmptyCollection(x, x =>
            $"[{x.Select(ViewOf).Aggregate(string.Concat)}]");

        static string ViewOf<TIndex>(ImmutableDictionary<TIndex, bool> x)
            where TIndex : Dummy.ChainElementIndex => WithHandleEmptyCollection(x, x =>
            $"[{x.Select(x => $"{{{ViewOf(x.Key)}-{ViewOf(x.Value)}}}")
                 .Aggregate(string.Concat)}]");

        static string WithHandleEmptyCollection<T>(IEnumerable<T> x,
            Func<IEnumerable<T>, string> mainConverter) =>
                x.Any() ? mainConverter(x) : "[]";

        static ImmutableDictionary<TKey, TValue> Dictionary<TKey, TValue>(
            params (TKey, TValue)[] records)
                where TKey : notnull =>
                    records.ToImmutableDictionary(
                        x => x.Item1,
                        x => x.Item2);
    }
}
