// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class SetKeywordRecommenderTests : KeywordRecommenderTests
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
        public async Task TestNotInEmptyStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(
@"$$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { [Bar] $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyAttributeAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { [Bar] private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyGet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get; $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyGetAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get; private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyGetAndAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get; [Bar] $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterPropertyGetAndAttributeAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get; [Bar] private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterGetAccessorBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get { } $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterGetAccessorBlockAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get { } private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterGetAccessorBlockAndAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get { } [Bar] $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterGetAccessorBlockAndAttributeAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int Goo { get { } [Bar] private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterPropertySetKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
   int Goo { set $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterPropertySetAccessor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
   int Goo { set; $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
   event Goo E { $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { [Bar] $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerAttributeAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { [Bar] private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGet()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get; $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get; private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetAndAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get; [Bar] $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetAndAttributeAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get; [Bar] private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetBlock()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get { } $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetBlockAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get { } private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetBlockAndAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get { } [Bar] $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAfterIndexerGetBlockAndAttributeAndPrivate()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C {
   int this[int i] { get { } [Bar] private $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterIndexerSetKeyword()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
   int this[int i] { set $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotAfterIndexerSetAccessor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"class C {
   int this[int i] { set; $$");
        }
    }
}
