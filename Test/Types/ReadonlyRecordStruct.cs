namespace ChainLead.Test.Types
{
    public readonly record struct ReadonlyRecordStruct(int Id)
    {
        public Member.Class MemberClass { get; } = new(Id | 111);
        public Member.Struct MemberStruct { get; } = new(Id | 222);
        public Member.ReadonlyStruct MemberReadonlyStruct { get; } = new(Id | 333);
        public Member.RefStruct MemberRefStruct => new(Id | 444);
        public Member.ReadonlyRefStruct MemberReadonlyRefStruct => new(Id | 555);
        public Member.Record MemberRecord { get; } = new(Id | 666);
        public Member.RecordStruct MemberRecordStruct { get; } = new(Id | 777);
        public Member.ReadonlyRecordStruct MemberReadonlyRecordStruct { get; } = new(Id | 888);

        public static string Name => "readonly record struct";

        public override string ToString() => $"{Name} {Id}";
    }
}