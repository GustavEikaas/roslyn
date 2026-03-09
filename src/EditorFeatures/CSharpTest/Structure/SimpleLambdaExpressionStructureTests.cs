// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class SimpleLambdaExpressionStructureTests : AbstractCSharpSyntaxNodeStructureTests<SimpleLambdaExpressionSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new SimpleLambdaExpressionStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambda()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        {|hint:$$f => {|textspan:{
            x();
        };|}|}
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambdaInForLoop()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        for (Action a = x$$ => { }; true; a()) { }
    }
}";

            await VerifyNoBlockSpansAsync(code);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambdaInMethodCall1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        someMethod(42, ""test"", false, {|hint:$$x => {|textspan:{
            return x;
        }|}|}, ""other arguments}"");
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestLambdaInMethodCall2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    void M()
    {
        someMethod(42, ""test"", false, {|hint:$$x => {|textspan:{
            return x;
        }|}|});
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }
    }
}
