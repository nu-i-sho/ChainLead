namespace ChainLead.Test.Types
{
    public class Class(int id)
    {
        public int Id => id;

        public Member.Class MemberClass { get; set; } = new(id + 1);
        public Member.Struct MemberStruct { get; set; } = new(id + 2);
        public Member.ReadonlyStruct MemberReadonlyStruct { get; set; } = new(id + 3);
        public Member.RefStruct MemberRefStruct => new(id + 4);
        public Member.ReadonlyRefStruct MemberReadonlyRefStruct => new(id + 5);
        public Member.Record MemberRecord { get; set; } = new(id + 6);
        public Member.RecordStruct MemberRecordStruct { get; set; } = new(id + 7);
        public Member.ReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new(id + 8);

        public static string Name => "class";

        public override string ToString() => $"{Name} {id}";
    }
}