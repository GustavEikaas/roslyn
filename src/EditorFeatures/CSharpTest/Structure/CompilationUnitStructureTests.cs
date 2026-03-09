// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class CompilationUnitStructureTests : AbstractCSharpSyntaxNodeStructureTests<CompilationUnitSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new CompilationUnitStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsings()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|hint:using {|textspan:System;
using System.Core;|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsingAliases()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|hint:using {|textspan:System;
using System.Core;
using text = System.Text;
using linq = System.Linq;|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExternAliases()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|hint:extern {|textspan:alias Goo;
extern alias Bar;|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExternAliasesAndUsings()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|hint:extern {|textspan:alias Goo;
extern alias Bar;
using System;
using System.Core;|}|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExternAliasesAndUsingsWithLeadingTrailingAndNestedComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|span1:// Goo
// Bar|}
{|hint2:extern {|textspan2:alias Goo;
extern alias Bar;
// Goo
// Bar
using System;
using System.Core;|}|}
{|span3:// Goo
// Bar|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: true),
                Region("span3", "// Goo ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsingsWithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|span1:// Goo
// Bar|}
{|hint2:using {|textspan2:System;
using System.Core;|}|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestExternAliasesWithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|span1:// Goo
// Bar|}
{|hint2:extern {|textspan2:alias Goo;
extern alias Bar;|}|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true),
                Region("textspan2", "hint2", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithComments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|span1:// Goo
// Bar|}";

            await VerifyBlockSpansAsync(code,
                Region("span1", "// Goo ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWithCommentsAtEnd()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|hint1:using {|textspan1:System;|}|}
{|span2:// Goo
// Bar|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan1", "hint1", CSharpStructureHelpers.Ellipsis, autoCollapse: true),
                Region("span2", "// Goo ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        [WorkItem(539359, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/539359")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUsingKeywordWithSpace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
$${|hint:using|} {|textspan:|}";

            await VerifyBlockSpansAsync(code,
                Region("textspan", "hint", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        [WorkItem(16186, "https://github.com/dotnet/roslyn/issues/16186")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInvalidComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"$${|span:/*/|}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/* / ...", autoCollapse: true));
        }
    }
}
