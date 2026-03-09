// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Structure;
using Microsoft.CodeAnalysis.Structure;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Structure
{
    public class DisabledTextStructureTests : AbstractCSharpSyntaxTriviaStructureTests
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new DisabledTextTriviaStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDisabledIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
#if false
{|span:$$Blah
Blah
Blah|}
#endif
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDisabledElse()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
#if true
#else
{|span:$$Blah
Blah
Blah|}
#endif
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDisabledElIf()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
#if true
#elif false
{|span:$$Blah
Blah
Blah|}
#endif
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(531360, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531360")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DisabledCodeWithEmbeddedPreprocessorDirectivesShouldCollapseEntireDisabledRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if false
{|span:    void $$M()
    {
#region ""R""
       M();
#endregion
        }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(531360, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/531360")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DisabledCodeShouldNotCollapseUnlessItFollowsADirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if false
{|span:    void M()
    {
#region ""R""
       $$M();
#endregion
        }|}
#endif
    }
";

            await VerifyNoBlockSpansAsync(code);
        }

        [WorkItem(1070677, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1070677")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesShouldCollapseEntireDisabledRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {
#if Bar
       M();
#endif
        }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(459257, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=459257")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesWithElseShouldCollapseEntireDisabledRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {
#if Bar
       M();
#else

#endif
        }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(459257, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=459257")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesWithElifShouldCollapseEntireDisabledRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {
#if Bar
       M();
#elif Baz

#endif
        }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(459257, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=459257")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesWithElseAndElifShouldCollapseEntireDisabledRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {
#if Bar
       M();
#else

#elif Baz

#endif
        }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(1070677, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1070677")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesShouldCollapseEntireDisabledRegion2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
    void M()
    {
#if Bar
{|span:       $$M();
       M();|}
#endif
        }
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(1070677, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1070677")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesShouldCollapseEntireDisabledRegion3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {|}
#if Bar
       M();
       M();
        }
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(1070677, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1070677")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NestedDisabledCodePreProcessorDirectivesShouldCollapseEntireDisabledRegion4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {
#if Bar
       M();
#line 10
        //some more text...
        //text
#if Car
        //random text
        //text
#endif
        // more text
        // text
#endif
    }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }

        [WorkItem(1100600, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/1100600")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreprocessorDirectivesInTrailingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class P {
#if Goo
{|span:    void $$M()
    {
#if Bar
       M();
#line 10
        //some more text...
        //text
#if Car
        //random text
        //text
#endif
        // more text
        // text
#endif
    }|}
#endif
    }
";

            await VerifyBlockSpansAsync(code,
                Region("span", CSharpStructureHelpers.Ellipsis, autoCollapse: true));
        }
    }
}
