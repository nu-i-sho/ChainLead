namespace Nuisho.ChainLead.Test
{
    public static partial class Dummy
    {
        public interface ICollection<out TDummy, TIndex> 
                : IEnumerable<TDummy>
            where TDummy : IChainElement<TIndex>
            where TIndex : ChainElementIndex
        {
            IEnumerable<TIndex> Indices => this.Select(x => x.Index);

            TDummy Get(TIndex i);

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
