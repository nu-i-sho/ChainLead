namespace Nuisho.ChainLead.Test.Utils
{
    using Nuisho.ChainLead.Test.Types;

    public static class TokensProvider
    {
        static readonly Dictionary<Type, Func<int, object>> _tokens = new()
            {
                { typeof(int), id => id },
                { typeof(string), id => id.ToString() },
                { typeof(Class), id => new Class(id) },
                { typeof(Struct), id => new Struct(id) },
                { typeof(ReadonlyStruct), id => new ReadonlyStruct(id) },
                { typeof(Record), id => new Record(id) },
                { typeof(RecordStruct), id => new RecordStruct(id) },
                { typeof(ReadonlyRecordStruct), id => new ReadonlyRecordStruct(id) },
            };

        static readonly Random _randon = new();

        public static T Get<T>(int id) =>
            (T)_tokens[typeof(T)](id);

        public static T GetRandom<T>() =>
            Get<T>(Math.Abs(_randon.Next()));

    }
}