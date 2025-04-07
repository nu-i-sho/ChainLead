// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1106:Code should not contain empty statements",
    Justification = "Only the nominal of this interface is needed without any body.",
    Scope = "type",
    Target = "~T:Nuisho.ChainLead.Implementation.IZero`1")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1106:Code should not contain empty statements",
    Justification = "Only the nominal of this interface is needed without any body.",
    Scope = "type",
    Target = "~T:Nuisho.ChainLead.Implementation.IAtom`1")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1106:Code should not contain empty statements",
    Justification = "Only the nominal of this interface is needed without any body.",
    Scope = "type",
    Target = "~T:Nuisho.ChainLead.Implementation.ITrue`1")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.ReadabilityRules",
    "SA1106:Code should not contain empty statements",
    Justification = "Only the nominal of this interface is needed without any body.",
    Scope = "type",
    Target = "~T:Nuisho.ChainLead.Implementation.IFalse`1")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1313:Parameter names should begin with lower-case letter",
    Justification = "'_' name means that the parameter isn't used",
    Scope = "member",
    Target = "~M:Nuisho.ChainLead.Implementation.True`1.Check(`0)~System.Boolean")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.NamingRules",
    "SA1313:Parameter names should begin with lower-case letter",
    Justification = "'_' name means that the parameter isn't used",
    Scope = "member",
    Target = "~M:Nuisho.ChainLead.Implementation.False`1.Check(`0)~System.Boolean")]

[assembly: SuppressMessage(
    "Critical Code Smell",
    "S927:Parameter names should match base declaration and other partial definitions",
    Justification = "Name '_' is OK because the parameter isn't used",
    Scope = "member",
    Target = "~M:Nuisho.ChainLead.Implementation.False`1.Check(`0)~System.Boolean")]

[assembly: SuppressMessage(
    "Critical Code Smell",
    "S927:Parameter names should match base declaration and other partial definitions",
    Justification = "Name '_' is OK because the parameter isn't used",
    Scope = "member",
    Target = "~M:Nuisho.ChainLead.Implementation.True`1.Check(`0)~System.Boolean")]
