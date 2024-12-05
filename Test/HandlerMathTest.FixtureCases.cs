namespace ChainLead.Test
{
    using ChainLead.Contracts;
    using ChainLead.Contracts.Syntax;
    using ChainLead.Implementation;
    using static ChainLead.Contracts.Syntax.ChainLeadSyntax;
    using static ChainLead.Test.Help.Constants;

    public partial class HandlerMathTest
    {
        public static IEnumerable<IHandlerMathFactory> FixtureCases
        {
            get
            {
                yield return new OriginalHandlerMathFactory();
                yield return new SyntaxHandlerMathFactory();
                yield return new SeparatedSyntaxHandlerMathFactory();
                yield return new ReversedSyntaxHandlerMathFactory();
            }
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
                : SingleHandlerMathTest.OriginalMathFactory.Product(math),
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
                : SingleHandlerMathTest.SyntaxMathFactory.Product(conditionMath),
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
                : SingleHandlerMathTest.SyntaxMathFactory.Product(conditionMath),
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
                : SingleHandlerMathTest.SyntaxMathFactory.Product(conditionMath),
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
