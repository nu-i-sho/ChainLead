﻿namespace Nuisho.ChainLead.Contracts;

public interface ICondition<in T>
{
    bool Check(T state);
}
