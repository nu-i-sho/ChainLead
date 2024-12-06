namespace ChainLead.Test.Cases
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Implementation;

    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Cases.Common.Appends;
    using static ChainLead.Test.Cases.Common.Types;

    public partial class HandlerMathTest
    {
        public const string Original = "Original";
        public const string Syntax = "Syntax";
        public const string Separated = "Separated";
        public const string Reversed = "Reversed";

        public class _I_Attribute()      : TestFixtureAttribute(typeof(int), Original);
        public class _II_Attribute()     : TestFixtureAttribute(typeof(string), Original);
        public class _III_Attribute()    : TestFixtureAttribute(typeof(Class), Original);
        public class _IV_Attribute()     : TestFixtureAttribute(typeof(Struct), Original);
        public class _V_Attribute()      : TestFixtureAttribute(typeof(ReadonlyStruct), Original);
        public class _VI_Attribute()     : TestFixtureAttribute(typeof(Record), Original);
        public class _VII_Attribute()    : TestFixtureAttribute(typeof(RecordStruct), Original);
        public class _VIII_Attribute()   : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Original);

        public class _IX_Attribute()     : TestFixtureAttribute(typeof(int), Syntax);
        public class _X_Attribute()      : TestFixtureAttribute(typeof(string), Syntax);
        public class _XI_Attribute()     : TestFixtureAttribute(typeof(Class), Syntax);
        public class _XII_Attribute()    : TestFixtureAttribute(typeof(Struct), Syntax);
        public class _XIII_Attribute()   : TestFixtureAttribute(typeof(ReadonlyStruct), Syntax);
        public class _XIV_Attribute()    : TestFixtureAttribute(typeof(Record), Syntax);
        public class _XV_Attribute()     : TestFixtureAttribute(typeof(RecordStruct), Syntax);
        public class _XVI_Attribute()    : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Syntax);

        public class _XVII_Attribute()   : TestFixtureAttribute(typeof(int), Separated);
        public class _XVIII_Attribute()  : TestFixtureAttribute(typeof(string), Separated);
        public class _XIX_Attribute()    : TestFixtureAttribute(typeof(Class), Separated);
        public class _XX_Attribute()     : TestFixtureAttribute(typeof(Struct), Separated);
        public class _XXI_Attribute()    : TestFixtureAttribute(typeof(ReadonlyStruct), Separated);
        public class _XXII_Attribute()   : TestFixtureAttribute(typeof(Record), Separated);
        public class _XXIII_Attribute()  : TestFixtureAttribute(typeof(RecordStruct), Separated);
        public class _XXIV_Attribute()   : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Separated);

        public class _XXV_Attribute()    : TestFixtureAttribute(typeof(int), Reversed);
        public class _XXVI_Attribute()   : TestFixtureAttribute(typeof(string), Reversed);
        public class _XXVII_Attribute()  : TestFixtureAttribute(typeof(Class), Reversed);
        public class _XXVIII_Attribute() : TestFixtureAttribute(typeof(Struct), Reversed);
        public class _XXIX_Attribute()   : TestFixtureAttribute(typeof(ReadonlyStruct), Reversed);
        public class _XXX_Attribute()    : TestFixtureAttribute(typeof(Record), Reversed);
        public class _XXXI_Attribute()   : TestFixtureAttribute(typeof(RecordStruct), Reversed);
        public class _XXXII_Attribute()  : TestFixtureAttribute(typeof(ReadonlyRecordStruct), Reversed);


        public static class HandlerMathFactoryProvider
        {
            public static IHandlerMathFactory Get(string name) =>
                name switch
                {
                    Original => new OriginalHandlerMathFactory(),
                    Syntax => new SyntaxHandlerMathFactory(),
                    Separated => new SeparatedSyntaxHandlerMathFactory(),
                    Reversed => new ReversedSyntaxHandlerMathFactory(),
                    _ => throw new ArgumentOutOfRangeException(nameof(name))
                };
        }

        public class AppendProvider<T>(IHandlerMath math)
            : IProvider<Func<IHandler<T>, IHandler<T>, IHandler<T>>>
        {
            public Func<IHandler<T>, IHandler<T>, IHandler<T>> this[string append] =>
                append switch
                {
                    Common.Appends.FirstThenSecond => math.FirstThenSecond,
                    Common.Appends.PackFirstInSecond => math.PackFirstInSecond,
                    Common.Appends.InjectFirstIntoSecond => math.InjectFirstIntoSecond,
                    Common.Appends.FirstCoverSecond => math.FirstCoverSecond,
                    Common.Appends.FirstWrapSecond => math.FirstWrapSecond,
                    Common.Appends.JoinFirstWithSecond => math.JoinFirstWithSecond,
                    Common.Appends.MergeFirstWithSecond => math.MergeFirstWithSecond,
                    _ => throw new ArgumentOutOfRangeException(nameof(append))
                };
        }

        public interface IHandlerMathFactory
        {
            ITestingHandlerMath Create(IConditionMath conditionMath);
        }

        public interface ITestingHandlerMath : IHandlerMath;

        public class OriginalHandlerMathFactory
            : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(new HandlerMath(conditionMath));

            public override string ToString() => "Original";

            class Product(IHandlerMath math)
                : SingleHandlerTest.OriginalMathFactory.Product(math),
                ITestingHandlerMath
            {
                public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.FirstCoverSecond(a, b);

                public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.FirstThenSecond(a, b);

                public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.FirstWrapSecond(a, b);

                public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.InjectFirstIntoSecond(a, b);

                public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.JoinFirstWithSecond(a, b);

                public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.MergeFirstWithSecond(a, b);

                public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    math.PackFirstInSecond(a, b);
            }
        }

        public class SyntaxHandlerMathFactory
            : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(conditionMath);
            
            public override string ToString() => "Syntax like FirstThenSecond[a, b]";

            class Product(IConditionMath conditionMath) 
                : SingleHandlerTest.SyntaxMathFactory.Product(conditionMath),
                ITestingHandlerMath
            {
                public IHandler<T> FirstThenSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.FirstThenSecond(a, b);

                public IHandler<T> FirstCoverSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.FirstCoverSecond(a, b);

                public IHandler<T> FirstWrapSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.FirstWrapSecond(a, b);

                public IHandler<T> InjectFirstIntoSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.InjectFirstIntoSecond(a, b);

                public IHandler<T> JoinFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.JoinFirstWithSecond(a, b);

                public IHandler<T> MergeFirstWithSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.MergeFirstWithSecond(a, b);

                public IHandler<T> PackFirstInSecond<T>(IHandler<T> a, IHandler<T> b) =>
                    ChainLeadSyntax.PackFirstInSecond(a, b);
            }
        }

        public class SeparatedSyntaxHandlerMathFactory
            : IHandlerMathFactory
        {
            public ITestingHandlerMath Create(IConditionMath conditionMath) =>
                new Product(conditionMath);

            public override string ToString() => "Syntax like Pack[a].In[b]";

            class Product(IConditionMath conditionMath)
                : SingleHandlerTest.SyntaxMathFactory.Product(conditionMath),
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
                : SingleHandlerTest.SyntaxMathFactory.Product(conditionMath),
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
    }
}
