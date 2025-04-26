namespace Nuisho.ChainLead.Test;

using Types;

public static partial class Cases
{
    public static class Common
    {
        public sealed class _I_Attribute() : TestFixtureAttribute(typeof(int));
        public sealed class _II_Attribute() : TestFixtureAttribute(typeof(string));
        public sealed class _III_Attribute() : TestFixtureAttribute(typeof(Class));
        public sealed class _IV_Attribute() : TestFixtureAttribute(typeof(Struct));
        public sealed class _V_Attribute() : TestFixtureAttribute(typeof(ReadonlyStruct));
        public sealed class _VI_Attribute() : TestFixtureAttribute(typeof(Record));
        public sealed class _VII_Attribute() : TestFixtureAttribute(typeof(RecordStruct));
        public sealed class _VIII_Attribute() : TestFixtureAttribute(typeof(ReadonlyRecordStruct));
    }
}
