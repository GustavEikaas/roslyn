// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.UseInferredMemberName;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.InferredMemberName
{
    [Trait(Traits.Feature, Traits.Features.CodeActionsUseInferredMemberName)]
    public class UseInferredMemberNameTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseInferredMemberNameDiagnosticAnalyzer(), new CSharpUseInferredMemberNameCodeFixProvider());

        private static readonly CSharpParseOptions s_parseOptions =
            CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInferredTupleName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void M()
    {
        int a = 1;
        var t = ([||]a: a, 2);
    }
}",
@"
class C
{
    void M()
    {
        int a = 1;
        var t = (a, 2);
    }
}", parseOptions: s_parseOptions);
        }

        [Fact]
        [WorkItem(24480, "https://github.com/dotnet/roslyn/issues/24480")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInferredTupleName_WithAmbiguity()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void M()
    {
        int alice = 1;
        (int, int, string) t = ([||]alice: alice, alice, null);
    }
}", parameters: new TestParameters(parseOptions: s_parseOptions));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInferredTupleNameAfterCommaWithCSharp6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestActionCountAsync(
@"
class C
{
    void M()
    {
        int a = 2;
        var t = (1, [||]a: a);
    }
}", count: 0, parameters: new TestParameters(CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp6)));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInferredTupleNameAfterCommaWithCSharp7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestActionCountAsync(
@"
class C
{
    void M()
    {
        int a = 2;
        var t = (1, [||]a: a);
    }
}", count: 0, parameters: new TestParameters(CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp7)));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInferredTupleNameWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void M()
    {
        int a = 1;
        int b = 2;
        var t = ( /*before*/ {|FixAllInDocument:a:|} /*middle*/ a /*after*/, /*before*/ b: /*middle*/ b /*after*/);
    }
}",
@"
class C
{
    void M()
    {
        int a = 1;
        int b = 2;
        var t = ( /*before*/  /*middle*/ a /*after*/, /*before*/  /*middle*/ b /*after*/);
    }
}", parseOptions: s_parseOptions);
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInferredAnonymousTypeMemberName()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void M()
    {
        int a = 1;
        var t = new { [||]a= a, 2 };
    }
}",
@"
class C
{
    void M()
    {
        int a = 1;
        var t = new { a, 2 };
    }
}", parseOptions: s_parseOptions);
        }

        [Fact]
        [WorkItem(24480, "https://github.com/dotnet/roslyn/issues/24480")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInferredAnonymousTypeMemberName_WithAmbiguity()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingAsync(
@"
class C
{
    void M()
    {
        int alice = 1;
        var t = new { [||]alice=alice, alice };
    }
}", parameters: new TestParameters(parseOptions: s_parseOptions));
        }

        [Fact]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFixAllInferredAnonymousTypeMemberNameWithTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestAsync(
@"
class C
{
    void M()
    {
        int a = 1;
        int b = 2;
        var t = new { /*before*/ {|FixAllInDocument:a =|} /*middle*/ a /*after*/, /*before*/ b = /*middle*/ b /*after*/ };
    }
}",
@"
class C
{
    void M()
    {
        int a = 1;
        int b = 2;
        var t = new { /*before*/  /*middle*/ a /*after*/, /*before*/  /*middle*/ b /*after*/ };
    }
}", parseOptions: s_parseOptions);
        }
    }
}
