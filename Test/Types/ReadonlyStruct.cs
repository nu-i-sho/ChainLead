namespace Nuisho.ChainLead.Test.Types;

public readonly struct ReadonlyStruct(int id)
{
    public int Id => id;

    public Member.Class MemberClass { get; } = new (id + 100);
    public Member.Struct MemberStruct { get; } = new (id + 200);
    public Member.ReadonlyStruct MemberReadonlyStruct { get; } = new (id + 300);
    public Member.RefStruct MemberRefStruct => new (id + 400);
    public Member.ReadonlyRefStruct MemberReadonlyRefStruct => new (id + 500);
    public Member.Record MemberRecord { get; } = new (id + 600);
    public Member.RecordStruct MemberRecordStruct { get; } = new (id + 700);
    public Member.ReadonlyRecordStruct MemberReadonlyRecordStruct { get; } = new (id);

    public static string Name => "readonly struct";

    public override string ToString() => $"{Name} {id}";
}