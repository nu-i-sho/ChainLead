namespace ChainLead.Test
{
    public static partial class Dummy
    {
        public interface ICollection<out TDummy, TIndex> 
                : IEnumerable<TDummy>
            where TDummy : IChainElement<TIndex>
            where TIndex : ChainElementIndex
        {
            TDummy Get(TIndex i) => this.First(x => x.Index.Equals(i));

            IEnumerable<TIndex> Indices => this.Select(x => x.Index);

            ICollection<TDummy, TIndex> this[TIndex first, TIndex second, params TIndex[] tail] =>
                this[Enumerable.Concat([first, second], tail)];

            ICollection<TDummy, TIndex> this[IEnumerable<TIndex> indices] { get; }

            void LogInto(IList<ChainElementIndex> acc)
            {
                foreach (var x in this)
                    x.LogsInto(acc);
            }

            void LogInto(IList<TIndex> acc)
            {
                foreach (var x in this)
                    x.LogsInto(acc);
            }

            void AddCallback(Action f)
            {
                foreach (var x in this)
                    x.AddCallback(f);
            }

            public interface IMutable : ICollection<TDummy, TIndex>
            {
                void Generate(TIndex i);

                void Generate(TIndex first, TIndex second, params TIndex[] tail)
                {
                    Generate(first);
                    Generate(second);

                    foreach (var i in tail)
                        Generate(i);
                }

                void Generate(IEnumerable<TIndex> indices)
                {
                    foreach (var i in indices)
                        Generate(i);
                }
            }
        }
    }
}
