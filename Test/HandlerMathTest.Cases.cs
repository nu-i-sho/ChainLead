namespace ChainLead.Test
{
    using ChainLead.Test.Help;
    using static ChainLead.Test.Help.Constants;
    using static ChainLead.Test.Help.Constants.Appends;

    public partial class HandlerMathTest
    {
        public record Case1(HandlerIndex[] ChainIndices);

        public static Case1[] Cases1 =>
        [
            new([ A, A ]), new([ A, B ]), new([ A, B, C ]), new([ A, B, C, D, E, F ]),
            new([ A, B, C, D, A, B, B, B, A, B, C, D, E, F, H, I, A, B, C, D, C, C, D]),
            new([ A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A, A])
        ];

        public record Case2(
            HandlerIndex?[] ChainIndicesWithNullsAsZeros,
            HandlerIndex[] ExpectedExecution);

        public static Case2[] Cases2 =>
        [
            new([null, A], [A]),
            new([A, null], [A]),
            new([null, A, null], [A]),
            new([null, null, null, null, A, null, null, null], [A]),

            new([A, B, C, D, null, I, J, null, I, null, null, E, I, H, null, F, F, I, null, null],
                [A, B, C, D, I, J, I, E, I, H, F, F, I]),

            new([A, null, A, null, A, null, A, null, A, null, A, null, A, A, A, A, null, null, null, A, null, A, A, A],
                [A, A, A, A, A, A, A, A, A, A, A, A, A, A])
        ];

        public record Case3(
            bool AIsConditional,
            bool BIsConditional,
            ConditionIndex ExpectedCondition,
            bool FinalConditionCheckResult,
            bool HandlersExecutionExpected);

        public static IEnumerable<Case3> Cases3
        {
            get
            {
                yield return new(false, true, Y, false, false);
                yield return new(false, true, Y, true, false);
                yield return new(true, false, X, false, false);
                yield return new(true, false, X, true, true);
                yield return new(true, true, Z, false, false);
                yield return new(true, true, Z, true, true);
            }
        }

        public record Case4(
            bool[] CheckSetup,
            bool[] CheckExpected,
            bool[] ExecutionExpected);

        public static IEnumerable<Case4> Cases4
        {
            get
            {
                yield return new(
                    CheckSetup:    [false, false, false],
                    CheckExpected: [false, false, true],
                    ExecutionExpected: [false, false]);

                yield return new(
                    CheckSetup:    [false, true, false],
                    CheckExpected: [false, false, true],
                    ExecutionExpected: [false, false]);

                yield return new(
                    CheckSetup:    [true,  false, false],
                    CheckExpected: [false, false, true],
                    ExecutionExpected: [false, false]);

                yield return new(
                    CheckSetup:    [true,  true,  false],
                    CheckExpected: [false, false, true],
                    ExecutionExpected: [false, false]);

                yield return new(
                    CheckSetup:    [false, false, true],
                    CheckExpected: [true,  true,  true],
                    ExecutionExpected: [false, false]);

                yield return new(
                    CheckSetup:    [false, true, true],
                    CheckExpected: [true,  true, true],
                    ExecutionExpected: [false, true]);

                yield return new(
                    CheckSetup:    [true, false, true],
                    CheckExpected: [true, true,  true],
                    ExecutionExpected: [true, false]);

                yield return new(
                    CheckSetup:    [true, true, true],
                    CheckExpected: [true, true, true],
                    ExecutionExpected: [true, true]);
            }
        }

        public record Case5(
            ConditionIndex[] AConditions,
            ConditionIndex[] BConditions,
            Dictionary<ConditionIndex, bool> ChecksSetup,
            ConditionIndex[] CheckExpected,
            HandlerIndex[] ExecuteExpected);

        public static IEnumerable<Case5> Cases5
        {
            get
            {
                yield return new(
                    AConditions:     [],
                    BConditions:     [],
                    ChecksSetup:  new(),
                    CheckExpected:   [],
                    ExecuteExpected: [A, B]);

                yield return new(
                    AConditions:     [X],
                    BConditions:     [],
                    ChecksSetup:  new() { { X, false } },
                    CheckExpected:   [X],
                    ExecuteExpected: []);

                yield return new(
                    AConditions:     [X],
                    BConditions:     [],
                    ChecksSetup:  new() { { X, true } },
                    CheckExpected:   [X],
                    ExecuteExpected: [A, B]);

                yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [],
                    ChecksSetup:  new() { { Y, false } },
                    CheckExpected:   [Y],
                    ExecuteExpected: []);

                yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [],
                    ChecksSetup:  new() { { Y, true }, { X, false } },
                    CheckExpected:   [Y, X],
                    ExecuteExpected: [B]);

                yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [],
                    ChecksSetup:  new() { { Y, true }, { X, true } },
                    CheckExpected:   [Y, X],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [],
                    ChecksSetup:  new() { { Z, false } },
                    CheckExpected:   [Z],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [],
                    ChecksSetup:  new() { { Z, true }, { Y, false } },
                    CheckExpected:   [Z, Y],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [],
                    ChecksSetup:  new() { { Z, true }, { Y, true }, { X, false } },
                    CheckExpected:   [Z, Y, X],
                    ExecuteExpected: [B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [],
                    ChecksSetup:  new() { { Z, true }, { Y, true }, { X, true } },
                    CheckExpected:   [Z, Y, X],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);
                
               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, false } },
                    CheckExpected:   [R],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, true } },
                    CheckExpected:   [R],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, true }, { X, false } },
                    CheckExpected:   [R, X],
                    ExecuteExpected: [B]);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, true }, { X, true } },
                    CheckExpected:   [R, X],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, true }, { Y, false } },
                    CheckExpected:   [R, Y],
                    ExecuteExpected: [B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false } },
                    CheckExpected:   [R, Y, X],
                    ExecuteExpected: [B]);               
                
               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, true } },
                    CheckExpected:   [R, Y, X],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { U, false } },
                    CheckExpected:   [R, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { U, true } },
                    CheckExpected:   [R, U],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { V, true }, { U, false } },
                    CheckExpected:   [R, V, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { V, true }, { U, true } },
                    CheckExpected:   [R, V, U],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { X, false }, { U, false } },
                    CheckExpected:   [R, X, U],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { X, true }, { U, false } },
                    CheckExpected:   [R, X, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { X, true }, { U, false } },
                    CheckExpected:   [R, X, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X, Y],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { X, true }, { U, true } },
                    CheckExpected:   [R, X, U],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { Y, false }, { U, false } },
                    CheckExpected:   [R, Y, U],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false }, { U, false } },
                    CheckExpected:   [R, Y, X, U],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, true }, { U, false } },
                    CheckExpected:   [R, Y, X, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { Y, false }, { U, true } },
                    CheckExpected:   [R, Y, U],
                    ExecuteExpected: [B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false }, { U, true } },
                    CheckExpected:   [R, Y, X, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, true }, { U, true } },
                    CheckExpected:   [R, Y, X, U],
                    ExecuteExpected: [A, B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, false }, { V, false } },
                    CheckExpected:   [R, Y, V],
                    ExecuteExpected: []);
                
               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false }, { V, false } },
                    CheckExpected:   [R, Y, X, V],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false }, { V, true }, { U, false } },
                    CheckExpected:   [R, Y, X, V, U],
                    ExecuteExpected: []);
                
               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false }, { V, false } },
                    CheckExpected:   [R, Y, X, V],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, true }, { V, true }, { U, false } },
                    CheckExpected:   [R, Y, X, V, U],
                    ExecuteExpected: [A]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, false }, { V, true }, { U, false } },
                    CheckExpected:   [R, Y, V, U],
                    ExecuteExpected: []);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, false }, { V, true }, { U, true } },
                    CheckExpected:   [R, Y, V, U],
                    ExecuteExpected: [B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, false }, { V, true }, { U, true } },
                    CheckExpected:   [R, Y, X, V, U],
                    ExecuteExpected: [B]);

               yield return new(
                    AConditions:     [X, Y, Z],
                    BConditions:     [U, V, W],
                    ChecksSetup:  new() { { R, true }, { Y, true }, { X, true }, { V, true }, { U, true } },
                    CheckExpected:   [R, Y, X, V, U],
                    ExecuteExpected: [B]);
            }
        }

        public record Case6(
            ConditionIndex[] AConditions,
            ConditionIndex[] BConditions);

        public static IEnumerable<Case6> Cases6
        {
            get
            {
                yield return new([Q, R], []);
                yield return new([Q, R, S], []);
                yield return new([Q, R, S, T, U], []);
                yield return new([Q, R, S, T, U, V, W, X], []);

                yield return new([], [Q, R]);
                yield return new([], [Q, R, S]);
                yield return new([], [Q, R, S, T, U]);
                yield return new([], [Q, R, S, T, U, V, W]);

                yield return new([Q], [R]);
                yield return new([Q, R], [S]);
                yield return new([Q, R, S], [T]);
                yield return new([Q, R, S], [T, U]);
                yield return new([Q, R, S], [T, U, V]);
                yield return new([Q, R, S, T, U], [V]);
                yield return new([Q, R, S, T, U], [V, W]);
                yield return new([Q, R, S, T, U], [V, W, X]);
                yield return new([Q, R, S, T, U], [V, W, X, Y]);
                yield return new([Q, R, S, T, U, V, W], [X]);
                yield return new([Q, R, S, T, U, V, W], [X, Y]);
                yield return new([Q, R, S, T, U, V, W, X], [Y]);

                yield return new([Q], [R, S]);
                yield return new([Q], [R, S, T]);
                yield return new([Q], [R, S, T, U]);
                yield return new([Q, R], [S, T, U, V]);
                yield return new([Q, R, S], [T, U, V, W]);
                yield return new([Q, R], [S, T, U, V, W, X]);
                yield return new([Q, R], [S, T, U, V, W, X, Y]);
                yield return new([Q, R, S, T], [U, V, W, X, Z]);
                yield return new([Q], [R, S, T, U, V, W, X, Y]);
                yield return new([Q, R, S],[ T, U, V, W, X, Y]);
            }
        }

        public record Case7(
            string AppendType,
            ConditionIndex[] AConditions,
            ConditionIndex[] BConditions,
            Dictionary<ConditionIndex, bool> ChecksSetup,
            MockIndex[] ExpectedCallsOrder);

        public static IEnumerable<Case7> Cases7
        {
            get
            {
                // C - U, D - V
                yield return new(
                    AppendType: InjectFirstIntoSecond, 
                    AConditions: [],
                    BConditions: [],
                    ChecksSetup: new(),
                    ExpectedCallsOrder: [A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, W, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, W, V, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [X, W, V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [X, W, V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [X, W, V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [X, W, V, U]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, W, V]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, W]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, true }, { V, false } },
                    ExpectedCallsOrder: [W, V, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [W, V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [W, V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { V, false } },
                    ExpectedCallsOrder: [W, V]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [W, V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [W, V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, W]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, W, V, B ]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [X, W, V, U, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [X, W, V, U, A, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, false } },
                    ExpectedCallsOrder: [Z]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { Y, false } },
                    ExpectedCallsOrder: [X, Y]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { Y, true }, { X, false} },
                    ExpectedCallsOrder: [X, Y, X]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { Y, true }, { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, Y, X, W, B]);

                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { Y, true }, { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, Y, X, W, V, B]);
                
                yield return new(
                    AppendType: InjectFirstIntoSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { Y, true }, { X, true }, { W, true }, { V, true }, { V, false } },
                    ExpectedCallsOrder: [X, Y, X, W, V, U, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [],
                    ChecksSetup: new(),
                    ExpectedCallsOrder: [A, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [U, A, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [A, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [A, U, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [A, V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [A, V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [A, V, U, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, U, A, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, W]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, W, V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false }, },
                    ExpectedCallsOrder: [X, W, V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true }, },
                    ExpectedCallsOrder: [X, W, V, U, A, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true }, },
                    ExpectedCallsOrder: [A, X, W, V, U, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false }, },
                    ExpectedCallsOrder: [A, X, W, V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [A, X, W, V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [A, X, W]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [A, X]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { U, true }, { V, false } },
                    ExpectedCallsOrder: [U, A, V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { U, true }, { V, true } },
                    ExpectedCallsOrder: [U, A, V, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, true }, { W, false } },
                    ExpectedCallsOrder: [V, U, A, W]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, true }, { W, true } },
                    ExpectedCallsOrder: [V, U, A, W, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, true }, { W, false } },
                    ExpectedCallsOrder: [U, A, W]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [U, A, W, V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, true }, { W, true }, { V, true } },
                    ExpectedCallsOrder: [U, A, W, V, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, true }, { X, false } },
                    ExpectedCallsOrder: [V, U, A, X]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, true }, { X, true }, { W, false } },
                    ExpectedCallsOrder: [V, U, A, X, W]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, true }, { X, true }, { W, true } },
                    ExpectedCallsOrder: [V, U, A, X, W, B]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, false } },
                    ExpectedCallsOrder: [W, V]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [W, V, U]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, false } },
                    ExpectedCallsOrder: [W, V, U, A, Z]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, false } },
                    ExpectedCallsOrder: [W, V, U, A, Z, Y]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [W, V, U, A, Z, Y, X]);

                yield return new(
                    AppendType: FirstWrapSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [W, V, U, A, Z, Y, X, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [],
                    ChecksSetup: new(),
                    ExpectedCallsOrder: [A, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [U, A, B]);
                
                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [U, A, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, U, A, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, A, U]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, A, U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, W, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, W, V, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [X, W, V, U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [X, W, V, U, A, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [X, A, W, V, U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [X, A, W, V, U]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, A, W, V]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, A, W]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, U, A, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, true }, { V, false } },
                    ExpectedCallsOrder: [W, V, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [W, V, U, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [W, V, U, A, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { U, true }, { V, true } },
                    ExpectedCallsOrder: [W, U, A, V, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { U, true }, { V, false } },
                    ExpectedCallsOrder: [W, U, V, A]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { U, false }, { V, false } },
                    ExpectedCallsOrder: [W, U, V]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, true }, { U, false }, { V, true } },
                    ExpectedCallsOrder: [W, U, V, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { V, false }, { W, false } },
                    ExpectedCallsOrder: [X, V, W]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { V, false }, { W, true } },
                    ExpectedCallsOrder: [X, V, W, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { V, true }, { U, false }, { W, false } },
                    ExpectedCallsOrder: [X, V, U, W]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { V, true }, { U, false }, { W, true } },
                    ExpectedCallsOrder: [X, V, U, W, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { V, true }, { U, true }, { W, false } },
                    ExpectedCallsOrder: [X, V, U, A, W]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { X, true }, { V, true }, { U, true }, { W, true } },
                    ExpectedCallsOrder: [X, V, U, A, W, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, false } },
                    ExpectedCallsOrder: [Z]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, false }, { Y, false } },
                    ExpectedCallsOrder: [Z, W, Y]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, false }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [Z, W, Y, X]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, false }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [Z, W, Y, X, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, false }, { Y, false } },
                    ExpectedCallsOrder: [Z, W, V, Y]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, false }, { Y, false } },
                    ExpectedCallsOrder: [Z, W, V, U, Y]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, true }, { Y, false } },
                    ExpectedCallsOrder: [Z, W, V, U, A, Y]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, false }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [Z, W, V, Y, X]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, false }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [Z, W, V, U, Y, X]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, true }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [Z, W, V, U, A, Y, X]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, false }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [Z, W, V, U, Y, X, B]);

                yield return new(
                    AppendType: PackFirstInSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { Z, true }, { W, true }, { V, true }, { U, true }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [Z, W, V, U, A, Y, X, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [],
                    ChecksSetup: [],
                    ExpectedCallsOrder: [A, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [U, A, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [A, U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U],
                    ChecksSetup: new() { { U, true } },
                    ExpectedCallsOrder: [A, U, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [V, A, U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [V, A, U, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [A, V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, false } },
                    ExpectedCallsOrder: [A, V, U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V],
                    ChecksSetup: new() { { V, true }, { U, true } },
                    ExpectedCallsOrder: [A, V, U, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [X]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [X, W, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [X, W, V, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [X, W, V, U, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W, X],
                    BConditions: [],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [X, W, V, U, A, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, true } },
                    ExpectedCallsOrder: [A, X, W, V, U, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, true }, { U, false } },
                    ExpectedCallsOrder: [A, X, W, V, U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [A, X, W, V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, true }, { W, false } },
                    ExpectedCallsOrder: [A, X, W]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [],
                    BConditions: [U, V, W, X],
                    ChecksSetup: new() { { X, false } },
                    ExpectedCallsOrder: [A, X]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { U, true }, { V, false } },
                    ExpectedCallsOrder: [U, A, V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V],
                    ChecksSetup: new() { { U, true }, { V, true } },
                    ExpectedCallsOrder: [U, A, V, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, false }, { W, false } },
                    ExpectedCallsOrder: [V, U, W]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, true }, { W, false } },
                    ExpectedCallsOrder: [V, U, A, W]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, false }, { W, true } },
                    ExpectedCallsOrder: [V, U, W, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W],
                    ChecksSetup: new() { { V, true }, { U, true }, { W, true } },
                    ExpectedCallsOrder: [V, U, A, W, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, false } },
                    ExpectedCallsOrder: [U]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, true }, { W, false } },
                    ExpectedCallsOrder: [U, A, W]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, true }, { W, true }, { V, false } },
                    ExpectedCallsOrder: [U, A, W, V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U],
                    BConditions: [V, W],
                    ChecksSetup: new() { { U, true }, { W, true }, { V, true } },
                    ExpectedCallsOrder: [U, A, W, V, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, false } },
                    ExpectedCallsOrder: [V]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, false}, { X, false } },
                    ExpectedCallsOrder: [V, U, X]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, true }, { X, false } },
                    ExpectedCallsOrder: [V, U, A, X]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, false }, { X, true }, { W, false } },
                    ExpectedCallsOrder: [V, U, X, W]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, false }, { X, true }, { W, true } },
                    ExpectedCallsOrder: [V, U, X, W, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V],
                    BConditions: [W, X],
                    ChecksSetup: new() { { V, true }, { U, true }, { X, true }, { W, true } },
                    ExpectedCallsOrder: [V, U, A, X, W, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, false } },
                    ExpectedCallsOrder: [W]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, false }, { Z, false }, },
                    ExpectedCallsOrder: [W, V, Z]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, false }, },
                    ExpectedCallsOrder: [W, V, U, Z]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, false }, },
                    ExpectedCallsOrder: [W, V, U, A, Z]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, false }, { Z, true }, { Y, false } },
                    ExpectedCallsOrder: [W, V, Z, Y]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, false }, { Z, true }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [W, V, Z, Y, X]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, false }, { Z, true }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [W, V, Z, Y, X, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, true }, { Y, false } },
                    ExpectedCallsOrder: [W, V, U, Z, Y]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, false } },
                    ExpectedCallsOrder: [W, V, U, A, Z, Y]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, true }, { Y, true }, { X, false } },
                    ExpectedCallsOrder: [W, V, U, Z, Y, X]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, false }, { Z, true }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [W, V, U, Z, Y, X, B]);

                yield return new(
                    AppendType: FirstCoverSecond,
                    AConditions: [U, V, W],
                    BConditions: [X, Y, Z],
                    ChecksSetup: new() { { W, true }, { V, true }, { U, true }, { Z, true }, { Y, true }, { X, true } },
                    ExpectedCallsOrder: [W, V, U, Z, A, Y, X, B]);
            }
        }
    }
}
