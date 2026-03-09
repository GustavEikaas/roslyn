// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class FieldDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<FieldDeclarationSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new FieldDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestFieldWithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|span:// Goo
    // Bar|}
    $$int F;
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "// Goo ...", autoCollapse: true));
        }
    }
}
