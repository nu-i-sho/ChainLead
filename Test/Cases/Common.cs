namespace Nuisho.ChainLead.Test
{
    using Types;

    public static partial class Cases
    {
        public class Common
        {
            public class _I_Attribute() : TestFixtureAttribute(typeof(int));
            public class _II_Attribute() : TestFixtureAttribute(typeof(string));
            public class _III_Attribute() : TestFixtureAttribute(typeof(Class));
            public class _IV_Attribute() : TestFixtureAttribute(typeof(Struct));
            public class _V_Attribute() : TestFixtureAttribute(typeof(ReadonlyStruct));
            public class _VI_Attribute() : TestFixtureAttribute(typeof(Record));
            public class _VII_Attribute() : TestFixtureAttribute(typeof(RecordStruct));
            public class _VIII_Attribute() : TestFixtureAttribute(typeof(ReadonlyRecordStruct));
        }
    }
}
