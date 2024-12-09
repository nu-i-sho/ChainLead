namespace ChainLead.Test.Types
{
    public record struct RecordStruct(int Id)
    {
        public Member.Class MemberClass { get; set; } = new(Id >> 2);
        public Member.Struct MemberStruct { get; set; } = new(Id >> 3);
        public Member.ReadonlyStruct MemberReadonlyStruct { get; set; } = new(Id >> 4);
        public Member.RefStruct MemberRefStruct => new(Id >> 5);
        public Member.ReadonlyRefStruct MemberReadonlyRefStruct => new(Id >> 6);
        public Member.Record MemberRecord { get; set; } = new(Id >> 7);
        public Member.RecordStruct MemberRecordStruct { get; set; } = new(Id >> 8);
        public Member.ReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new(Id >> 9);

        public static string Name => "record struct";

        public override string ToString() => $"{Name} {Id}";
    }
}