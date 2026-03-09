// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class ConstructorDeclarationStructureTests : AbstractCSharpSyntaxNodeStructureTests<ConstructorDeclarationSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new ConstructorDeclarationStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|textspan:
    {
    }|}|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|textspan:
    {
    }                 |}|}
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|textspan:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C(){|textspan:
    {
    }|}|} /* .ctor */
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C() // .ctor{|textspan:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C() /* .ctor */{|textspan:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C()
    // .ctor{|textspan:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructor8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|hint:$$public C()
    /* .ctor */{|textspan:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorWithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|span1:// Goo
    // Bar|}
    {|hint2:$$public C(){|textspan2:
    {
    }|}|} // .ctor
}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConstructorMissingCloseParenAndBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            // Expected behavior is that the class should be outlined, but the constructor should not.

            const string code = @"
class C
{
    $$C(
}";

            await VerifyNoBlockSpansAsync(code);
        }
    }
}
