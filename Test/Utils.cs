namespace ChainLead.Test
{
    using ChainLead.Contracts;

    public static class Utils
    {
        public const int MagicId = 7689;

        public class Appends
        {
            public const string
                FirstThenSecond = nameof(IHandlerMath.FirstThenSecond),
                PackFirstInSecond = nameof(IHandlerMath.PackFirstInSecond),
                InjectFirstIntoSecond = nameof(IHandlerMath.InjectFirstIntoSecond),
                FirstCoverSecond = nameof(IHandlerMath.FirstCoverSecond),
                FirstWrapSecond = nameof(IHandlerMath.FirstWrapSecond),
                JoinFirstWithSecond = nameof(IHandlerMath.JoinFirstWithSecond),
                MergeFirstWithSecond = nameof(IHandlerMath.MergeFirstWithSecond);

            public static readonly string[] All =
            [
                FirstThenSecond,
                PackFirstInSecond,
                InjectFirstIntoSecond,
                FirstCoverSecond,
                FirstWrapSecond,
                JoinFirstWithSecond,
                MergeFirstWithSecond
            ];

            public interface IProvider<TAppend>
            {
                TAppend this[string key] { get; }
            }
        }

        public static class TokensProvider
        {
            private static readonly Dictionary<Type, Func<int, object>> _tokens = new()
            {
                { typeof(int), id => id },
                { typeof(string), id => id.ToString() },
                { typeof(Types.Class), id => new Types.Class(id) },
                { typeof(Types.Struct), id => new Types.Struct(id) },
                { typeof(Types.ReadonlyStruct), id => new Types.ReadonlyStruct(id) },
                { typeof(Types.Record), id => new Types.Record(id) },
                { typeof(Types.RecordStruct), id => new Types.RecordStruct(id) },
                { typeof(Types.ReadonlyRecordStruct), id => new Types.ReadonlyRecordStruct(id) },
            };

            public static T Get<T>(int id) => (T)_tokens[typeof(T)](id);
        }

        public static class Types
        {
            public class Class(int id)
            {
                public int Id { get; set; } = id;

                public MemberClass MemberClass { get; set; } = new(id + 1);
                public MemberStruct MemberStruct { get; set; } = new(id + 2);
                public MemberReadonlyStruct MemberReadonlyStruct { get; set; } = new(id + 3);
                public MemberRefStruct MemberRefStruct => new(id + 4);
                public MemberReadonlyRefStruct MemberReadonlyRefStruct => new(id + 5);
                public MemberRecord MemberRecord { get; set; } = new(id + 6);
                public MemberRecordStruct MemberRecordStruct { get; set; } = new(id + 7);
                public MemberReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new(id + 8);

                public static string Name => "class";

                public override string ToString() => $"{Name} {id}";
            }

            public struct Struct(int id)
            {
                public int Id { get; set; } = id;

                public MemberClass MemberClass { get; set; } = new(id + 10);
                public MemberStruct MemberStruct { get; set; } = new(id + 20);
                public MemberReadonlyStruct MemberReadonlyStruct { get; set; } = new(id + 30);
                public MemberReadonlyRefStruct MemberReadonlyRefStruct => new(id + 40);
                public MemberRefStruct MemberRefStruct => new(id + 50);
                public MemberRecord MemberRecord { get; set; } = new(id + 60);
                public MemberRecordStruct MemberRecordStruct { get; set; } = new(id + 70);
                public MemberReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new(id + 80);

                public static string Name => "struct";

                public override string ToString() => $"{Name} {id}";
            }

            public readonly struct ReadonlyStruct(int id)
            {
                public int Id { get; } = id;

                public MemberClass MemberClass { get; } = new(id + 100);
                public MemberStruct MemberStruct { get; } = new(id + 200);
                public MemberReadonlyStruct MemberReadonlyStruct { get; } = new(id + 300);
                public MemberRefStruct MemberRefStruct => new(id + 400);
                public MemberReadonlyRefStruct MemberReadonlyRefStruct => new(id + 500);
                public MemberRecord MemberRecord { get; } = new(id + 600);
                public MemberRecordStruct MemberRecordStruct { get; } = new(id +700);
                public MemberReadonlyRecordStruct MemberReadonlyRecordStruct { get; } = new(id);

                public static string Name => "readonly struct";

                public override string ToString() => $"{Name} {id}";
            }

            public record Record(int Id)
            {
                public MemberClass MemberClass { get; set; } = new(Id * 2);
                public MemberStruct MemberStruct { get; set; } = new(Id * 3);
                public MemberReadonlyStruct MemberReadonlyStruct { get; set; } = new(Id * 4);
                public MemberRefStruct MemberRefStruct => new(Id * 5);
                public MemberReadonlyRefStruct MemberReadonlyRefStruct => new(Id * 6);
                public MemberRecord MemberRecord { get; set; } = new(Id * 7);
                public MemberRecordStruct MemberRecordStruct { get; set; } = new(Id * 8);
                public MemberReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new(Id * 9);

                public static string Name => "record";

                public override string ToString() => $"{Name} {Id}";
            }

            public record struct RecordStruct(int Id)
            {
                public MemberClass MemberClass { get; set; } = new(Id >> 2);
                public MemberStruct MemberStruct { get; set; } = new(Id >> 3);
                public MemberReadonlyStruct MemberReadonlyStruct { get; set; } = new(Id >> 4);
                public MemberRefStruct MemberRefStruct => new(Id >> 5);
                public MemberReadonlyRefStruct MemberReadonlyRefStruct => new(Id >> 6);
                public MemberRecord MemberRecord { get; set; } = new(Id >> 7);
                public MemberRecordStruct MemberRecordStruct { get; set; } = new(Id >> 8);
                public MemberReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new(Id >> 9);

                public static string Name => "record struct";

                public override string ToString() => $"{Name} {Id}";
            }

            public readonly record struct ReadonlyRecordStruct(int Id)
            {
                public MemberClass MemberClass { get; } = new(Id | 111);
                public MemberStruct MemberStruct { get; } = new(Id | 222);
                public MemberReadonlyStruct MemberReadonlyStruct { get; } = new(Id | 333);
                public MemberRefStruct MemberRefStruct => new(Id | 444);
                public MemberReadonlyRefStruct MemberReadonlyRefStruct => new(Id | 555);
                public MemberRecord MemberRecord { get; } = new(Id | 666);
                public MemberRecordStruct MemberRecordStruct { get; } = new(Id | 777);
                public MemberReadonlyRecordStruct MemberReadonlyRecordStruct { get; } = new(Id | 888);

                public static string Name => "readonly record struct";

                public override string ToString() => $"{Name} {Id}";
            }

            public class MemberClass(int id)
            {
                public int Id { get; } = id;
            }

            public struct MemberStruct(int id)
            {
                public int Id { get; set; } = id;
            }

            public readonly struct MemberReadonlyStruct(int id)
            {
                public int Id { get; } = id;
            }

            public  ref struct MemberRefStruct(int id)
            {
                public int Id { get; set; } = id;
            }

            public readonly ref struct MemberReadonlyRefStruct(int id)
            {
                public int Id { get; } = id;
            }

            public record MemberRecord(int Id);

            public record struct MemberRecordStruct(int Id);

            public readonly record struct MemberReadonlyRecordStruct(int Id);
        }
    }
}
