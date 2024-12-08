namespace ChainLead.Test.Types
{
    public static class Member
    {
        public class Class(int id)
        {
            public int Id { get; } = id;
        }

        public struct Struct(int id)
        {
            public int Id { get; set; } = id;
        }

        public readonly struct ReadonlyStruct(int id)
        {
            public int Id { get; } = id;
        }

        public ref struct RefStruct(int id)
        {
            public int Id { get; set; } = id;
        }

        public readonly ref struct ReadonlyRefStruct(int id)
        {
            public int Id { get; } = id;
        }

        public record Record(int Id);

        public record struct RecordStruct(int Id);

        public readonly record struct ReadonlyRecordStruct(int Id);
    }
}