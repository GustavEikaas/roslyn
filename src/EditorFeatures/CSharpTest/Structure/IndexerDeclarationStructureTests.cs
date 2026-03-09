// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class IndexerDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<IndexerDeclarationSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new IndexerDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public string this[int index]{|textspan:
    {
        get { }
    }|}|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexerWithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|span1:// Goo
    // Bar|}
    {|hint2:$$public string this[int index]{|textspan2:
    {
        get { }
    }|}|}
}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndexerWithWithExpressionBodyAndComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|span:// Goo
    // Bar|}
    $$public string this[int index] => 0;
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "// Goo ...", autoCollapse: true));
        }
    }
}
