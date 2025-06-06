﻿namespace Nuisho.ChainLead.Test;

using System.Collections;

public static partial class Dummy
{
    public class ConditionCollection<T>(T token) :
        Dictionary<ConditionIndex, Condition<T>>, IConditionCollection<T>.IMutable
    {
        public T Token => token;

        public IEnumerable<ConditionIndex> Indices => Keys;

        public IConditionCollection<T> this[IEnumerable<ConditionIndex> indices]
        {
            get
            {
                var slice = new ConditionCollection<T>(token);
                foreach (var i in indices)
                    slice.Add(i, Get(i));

                return slice;
            }
        }

        public void AddRange(IEnumerable<Condition<T>> conditions)
        {
            foreach (var c in conditions)
                Add(c.Index, c);
        }

        public Condition<T> Get(ConditionIndex i) => this[i];

        public void Generate(ConditionIndex i) =>
            Add(i, new Condition<T>(i, token));

        IEnumerator<Condition<T>> IEnumerable<Condition<T>>.GetEnumerator() =>
            Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            Values.GetEnumerator();
    }
}
