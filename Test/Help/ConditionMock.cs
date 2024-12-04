namespace ChainLead.Test.Help
{
    using ChainLead.Contracts;
    using Moq;

    using static Constants;

    public class ConditionMock : 
        Mock<ICondition<int>>,
        IIndexedWith<ConditionIndex>
    {
        public ConditionMock(ConditionIndex index)
        {
            Name = index.View;
            Index = index;
        }

        public ConditionIndex Index { get; }

        public void Setup__Check(bool returns) =>
             Setup(o => o.Check(Arg))
            .Returns(returns);

        public void Setup__Check__LoggingInto(IList<ConditionIndex> acc) =>
             Setup(o => o.Check(Arg))
            .Callback(() => acc.Add(Index));

        public void Setup__Check__LoggingInto(IList<MockIndex> acc) =>
             Setup(o => o.Check(Arg))
            .Callback(() => acc.Add(Index));

        public void Verify__Check(Times times) =>
            Verify(o => o.Check(Arg), times);

        public void Verify__Check(Func<Times> times) =>
            Verify(o => o.Check(Arg), times);
    }
}
