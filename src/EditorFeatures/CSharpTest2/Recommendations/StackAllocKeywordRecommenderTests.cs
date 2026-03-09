// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Recommendations
{
    public class StackAllocKeywordRecommenderTests : KeywordRecommenderTests
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
        public async Task TestInEmptySpaceAfterAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"var v = $$"));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInUnsafeEmptySpace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"unsafe class C {
    void Goo() {
      var v = $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInUnsafeEmptySpace_AfterNonPointer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // There can be an implicit conversion to int
            await VerifyKeywordAsync(
@"unsafe class C {
    void Goo() {
      int v = $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInUnsafeEmptySpace_AfterPointer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"unsafe class C {
    void Goo() {
      int* v = $$");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(
@"unsafe class C {
    int* v = $$");
        }

        [WorkItem(544504, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544504")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideForStatementVarDecl1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C
{
    unsafe static void Main(string[] args)
    {
        for (var i = $$");
        }

        [WorkItem(544504, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544504")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideForStatementVarDecl2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C
{
    unsafe static void Main(string[] args)
    {
        for (int* i = $$");
        }

        [WorkItem(544504, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/544504")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInsideForStatementVarDecl3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(
@"class C
{
    unsafe static void Main(string[] args)
    {
        for (string i = $$");
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSOfAssignment_Span()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
Span<int> s = $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSOfAssignment_Pointer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"int* v = $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSOfAssignment_ReAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(
@"v = $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = (Span<char>)$$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = (Span<char>)$$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_True()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value ? $$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value ? $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_True_WithCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value ? (Span<int>)$$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value ? (Span<int>)$$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_False()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value ? stackalloc int[10] : $$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value ? stackalloc int[10] : $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_False_WithCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value ? stackalloc int[10] : (Span<int>)$$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value ? stackalloc int[10] : (Span<int>)$$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_NestedConditional_True()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value1 ? value2 ? $$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value1 ? value2 ? $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_NestedConditional_WithCast_True()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value1 ? value2 ? (Span<int>)$$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value1 ? value2 ? (Span<int>)$$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_NestedConditional_False()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value1 ? value2 ? stackalloc int [10] : $$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value1 ? value2 ? stackalloc int [10] : $$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestOnRHSWithConditionalExpression_NestedConditional_WithCast_False()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyKeywordAsync(AddInsideMethod(@"
var s = value1 ? value2 ? stackalloc int [10] : (Span<int>)$$"));

            await VerifyKeywordAsync(AddInsideMethod(@"
s = value1 ? value2 ? stackalloc int [10] : (Span<int>)$$"));
        }

        [WorkItem(23584, "https://github.com/dotnet/roslyn/issues/23584")]
        [Fact, Trait(Traits.Feature, Traits.Features.KeywordRecommending)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNotInLHSOfAssignment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await VerifyAbsenceAsync(AddInsideMethod(@"
var x $$ ="));

            await VerifyAbsenceAsync(AddInsideMethod(@"
x $$ ="));
        }
    }
}
