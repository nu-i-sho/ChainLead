// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1106:Code should not contain empty statements",
    Justification = "Only the nominal of this class is needed without any implementation.",
    Scope = "type",
    Target = "~T:Nuisho.ChainLead.Contracts.Syntax.DI.Extension.CallToken")]

[assembly: SuppressMessage(
    "Design",
    "CA1033:Interface methods should be callable by child types",
    Justification = "It is accessible by interface, and that is enough.",
    Scope = "member",
    Target = "~M:Nuisho.ChainLead.Contracts.IExtendedHandler`1.Nuisho#ChainLead#Contracts#IHandler<T>#Execute(`0)")]
