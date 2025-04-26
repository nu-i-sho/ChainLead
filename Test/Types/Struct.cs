namespace Nuisho.ChainLead.Test.Types;

public struct Struct(int id)
{
    public int Id => id;

    public Member.Class MemberClass { get; set; } = new (id + 10);
    public Member.Struct MemberStruct { get; set; } = new (id + 20);
    public Member.ReadonlyStruct MemberReadonlyStruct { get; set; } = new (id + 30);
    public Member.ReadonlyRefStruct MemberReadonlyRefStruct => new (id + 40);
    public Member.RefStruct MemberRefStruct => new (id + 50);
    public Member.Record MemberRecord { get; set; } = new (id + 60);
    public Member.RecordStruct MemberRecordStruct { get; set; } = new (id + 70);
    public Member.ReadonlyRecordStruct MemberReadonlyRecordStruct { get; set; } = new (id + 80);

    public static string Name => "struct";

    public override string ToString() => $"{Name} {id}";
}