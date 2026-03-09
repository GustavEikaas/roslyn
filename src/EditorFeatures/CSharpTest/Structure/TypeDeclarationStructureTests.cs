// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class TypeDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<TypeDeclarationSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new TypeDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClass()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint:$$class C{|textspan:
{
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithLeadingComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span1:// Goo
// Bar|}
{|hint2:$$class C{|textspan2:
{
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestClassWithNestedComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint1:$$class C{|textspan1:
{
    {|span2:// Goo
    // Bar|}
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan1", "hint1", CSharpStructureHelpers.Ellipsis, autoCollapse: false),
                Region("span2", "// Goo ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint:$$interface I{|textspan:
{
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterfaceWithLeadingComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span1:// Goo
// Bar|}
{|hint2:$$interface I{|textspan2:
{
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterfaceWithNestedComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint1:$$interface I{|textspan1:
{
    {|span2:// Goo
    // Bar|}
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan1", "hint1", CSharpStructureHelpers.Ellipsis, autoCollapse: false),
                Region("span2", "// Goo ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStruct()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint:$$struct S{|textspan:
{
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStructWithLeadingComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span1:// Goo
// Bar|}
{|hint2:$$struct S{|textspan2:
{
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: false));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestStructWithNestedComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|hint1:$$struct S{|textspan1:
{
    {|span2:// Goo
    // Bar|}
}|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan1", "hint1", CSharpStructureHelpers.Ellipsis, autoCollapse: false),
                Region("span2", "// Goo ...", autoCollapse: true));
        }
    }
}
