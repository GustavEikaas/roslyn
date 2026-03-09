// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class ParamsKeywordRecommenderTests : KeywordRecommenderTests
    {
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAtRoot_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(SourceCodeKind.Script,
@"$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterClass_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(SourceCodeKind.Script,
@"class C { }
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterGlobalStatement_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(SourceCodeKind.Script,
@"System.Console.WriteLine();
$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterGlobalVariableDeclaration_Interactive()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(SourceCodeKind.Script,
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
        public async Task TestNotAfterAngle()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"interface IGoo<$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterfaceTypeVarianceNotAfterIn()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"interface IGoo<in $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterfaceTypeVarianceNotAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"interface IGoo<Goo, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterfaceTypeVarianceNotAfterAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"interface IGoo<[Goo]$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDelegateTypeVarianceNotAfterAngle()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"delegate void D<$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDelegateTypeVarianceNotAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"delegate void D<Goo, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDelegateTypeVarianceNotAfterAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"delegate void D<[Goo]$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotParamsBaseListAfterAngle()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"interface IGoo : Bar<$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInGenericMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"interface IGoo {
    void Goo<$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterParams()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo(ref $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterOut()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo(out $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterThis()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"static class C {
    static void Goo(this $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterMethodOpenParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    void Goo($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterMethodComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    void Goo(int i, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterMethodAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    void Goo(int i, [Goo]$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterConstructorOpenParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    public C($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterConstructorComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    public C(int i, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterConstructorAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    public C(int i, [Goo]$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterDelegateOpenParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"delegate void D($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterDelegateComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"delegate void D(int i, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterDelegateAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"delegate void D(int i, [Goo]$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    static int operator +($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterDestructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    ~C($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
    int this[$$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInObjectCreationAfterOpenParen()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      new Bar($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterParamsParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      new Bar(ref $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterOutParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      new Bar(out $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInObjectCreationAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      new Bar(baz, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInObjectCreationAfterSecondComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      new Bar(baz, quux, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInObjectCreationAfterSecondNamedParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      new Bar(baz: 4, quux: $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInInvocationExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      Bar($$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInInvocationAfterComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      Bar(baz, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInInvocationAfterSecondComma()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      Bar(baz, quux, $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInInvocationAfterSecondNamedParam()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
    void Goo() {
      Bar(baz: 4, quux: $$");
        }
    }
}
