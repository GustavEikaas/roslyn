// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Structure.MetadataAsSource;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure.MetadataAsSource
{
    public class EnumDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<EnumDeclarationSyntax>
    {
        protected override string WorkspaceKind => CodeAnalysis.WorkspaceKind.MetadataAsSource;
        internal override AbstractSyntaxStructureProvider CreateProvider() => new MetadataEnumDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoCommentsOrAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
enum $$E
{
    A,
    B
}";

            await VerifyNoBlockSpansAsync(code);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint:{|textspan:[Bar]
|}enum $$E|}
{
    A,
    B
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithCommentsAndAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint:{|textspan:// Summary:
//     This is a summary.
[Bar]
|}enum $$E|}
{
    A,
    B
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithCommentsAttributesAndModifiers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint:{|textspan:// Summary:
//     This is a summary.
[Bar]
|}public enum $$E|}
{
    A,
    B
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }
    }
}
