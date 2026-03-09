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
    public class DocumentationCommentStructureTests : AbstractCSharpSyntaxNodeStructureTests<DocumentationCommentTriviaSyntax>
    {
        internal override AbstractSyntaxStructureProvider CreateProvider() => new DocumentationCommentStructureProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocumentationCommentWithoutSummaryTag1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/// $$XML doc comment
/// some description
/// of
/// the comment|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// XML doc comment ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocumentationCommentWithoutSummaryTag2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/** $$Block comment
* some description
* of
* the comment
*/|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/** Block comment ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocumentationCommentWithoutSummaryTag3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/// $$<param name=""tree""></param>|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <param name=\"tree\"></param> ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocumentationComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/// <summary>
/// $$Hello C#!
/// </summary>|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocumentationCommentWithLongBannerText()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
{|span:/// $$<summary>
/// " + new string('x', 240) + @"
/// </summary>|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> " + new string('x', 106) + " ...", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultilineDocumentationComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/** <summary>
$$Hello C#!
</summary> */|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/** <summary> Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndentedDocumentationComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
    {|span:/// <summary>
    /// $$Hello C#!
    /// </summary>|}
    class Class3
    {
    }";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndentedMultilineDocumentationComment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
    {|span:/** <summary>
    $$Hello C#!
    </summary> */|}
    class Class3
    {
    }";

            await VerifyBlockSpansAsync(code,
                Region("span", "/** <summary> Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestDocumentationCommentOnASingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/// <summary>$$Hello C#!</summary>|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary>Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultilineDocumentationCommentOnASingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/** <summary>$$Hello C#!</summary> */|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/** <summary>Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndentedDocumentationCommentOnASingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
    {|span:/// <summary>$$Hello C#!</summary>|}
    class Class3
    {
    }";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary>Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestIndentedMultilineDocumentationCommentOnASingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
    {|span:/** <summary>$$Hello C#!</summary> */|}
    class Class3
    {
    }";

            await VerifyBlockSpansAsync(code,
                Region("span", "/** <summary>Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultilineSummaryInDocumentationComment1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/// <summary>
/// $$Hello
/// C#!
/// </summary>|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestMultilineSummaryInDocumentationComment2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
{|span:/// <summary>
/// $$Hello
/// 
/// C#!
/// </summary>|}
class Class3
{
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> Hello C#!", autoCollapse: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
        [WorkItem(2129, "https://github.com/dotnet/roslyn/issues/2129")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CrefInSummary()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|span:/// $$<summary>
    /// Summary with <see cref=""SeeClass"" />, <seealso cref=""SeeAlsoClass"" />, 
    /// <see langword=""null"" />, <typeparamref name=""T"" />, <paramref name=""t"" />, and <see unsupported-attribute=""not-supported"" />.
    /// </summary>|}
    public void M<T>(T t) { }
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> Summary with SeeClass, SeeAlsoClass, null, T, t, and not-supported.", autoCollapse: true));
        }

        [WorkItem(402822, "https://devdiv.visualstudio.com/DevDiv/_workitems?id=402822")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSummaryWithPunctuation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
class C
{
    {|span:/// $$<summary>
    /// The main entrypoint for <see cref=""Program""/>.
    /// </summary>
    /// <param name=""args""></param>|}
    void Main()
    {
    }
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> The main entrypoint for Program.", autoCollapse: true));
        }

        [WorkItem(20679, "https://github.com/dotnet/roslyn/issues/20679")]
        [Fact, Trait(Traits.Feature, Traits.Features.Outlining)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSummaryWithAdditionalTags()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            const string code = @"
public class Class1
{
    {|span:/// $$<summary>
    /// Initializes a <c>new</c> instance of the <see cref=""Class1"" /> class.
    /// </summary>|}
    public Class1()
    {

    }
}";

            await VerifyBlockSpansAsync(code,
                Region("span", "/// <summary> Initializes a new instance of the Class1 class.", autoCollapse: true));
        }
    }
}
