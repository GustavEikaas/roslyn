// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class SealedKeywordRecommenderTests : KeywordRecommenderTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAtRoot_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(SourceCodeKind.Script,
@"$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterClass_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(SourceCodeKind.Script,
@"class C { }
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterGlobalStatement_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(SourceCodeKind.Script,
@"System.Console.WriteLine();
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterGlobalVariableDeclaration_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(SourceCodeKind.Script,
@"int i = 0;
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInUsingAlias()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"using Goo = $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInEmptyStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(
@"$$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInCompilationUnit()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterExtern()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"extern alias Goo;
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterUsing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"using Goo;
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterNamespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"namespace N {}
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterTypeDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {}
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterDelegateDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"delegate void Goo();
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterMethodInClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
  void Goo() {}
  $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterFieldInClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
  int i;
  $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyInClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
  int i { get; }
  $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotBeforeUsing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(SourceCodeKind.Regular,
@"$$
using Goo;");
        }

        [WpfFact(Skip = "https://github.com/dotnet/roslyn/issues/9880"), Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotBeforeUsing_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(SourceCodeKind.Script,
@"$$
using Goo;");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterAssemblyAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"[assembly: goo]
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterRootAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"[goo]
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterNestedAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
  [goo]
  $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"struct S {
   $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInsideInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"interface I {
   $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterPartial()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"partial $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterAbstract()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"abstract $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"internal $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPublic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"public $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterStaticInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"static internal $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterInternalStatic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"internal static $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterInvalidInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"virtual internal $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"class $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterSealed()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"sealed $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterStatic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"static $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterNestedStatic()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"class C {
    static $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterNestedInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    internal $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterDelegate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"delegate $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterNestedAbstract()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"class C {
    abstract $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterNestedVirtual()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"class C {
    virtual $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterNestedOverride()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    override $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterNestedSealed()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(@"class C {
    sealed $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    int Goo { $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPropertyAfterAccessor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    int Goo { get; $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPropertyAfterAccessibility()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    int Goo { get; protected $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInPropertyAfterInternal()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    int Goo { get; internal $$");
        }
    }
}
