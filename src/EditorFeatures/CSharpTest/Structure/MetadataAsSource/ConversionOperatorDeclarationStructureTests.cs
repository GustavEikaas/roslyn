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
    public class ConversionOperatorDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<ConversionOperatorDeclarationSyntax>
    {
        protected override string WorkspaceKind => CodeAnalysis.WorkspaceKind.MetadataAsSource;
        internal override AbstractSyntaxStructureProvider CreateProvider() => new MetadataConversionOperatorDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoCommentsOrAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    public static explicit operator $$Goo(byte b);
}";

            await VerifyNoBlockSpansAsync(code);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.MetadataAsSource)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WithAttributes()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:{|textspan:[Blah]
    |}public static explicit operator $$Goo(byte b);|}
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
class C
{
    {|hint:{|textspan:// Summary:
    //     This is a summary.
    [Blah]
    |}public static explicit operator $$Goo(byte b);|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }
    }
}
