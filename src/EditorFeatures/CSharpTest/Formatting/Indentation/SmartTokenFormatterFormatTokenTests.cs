// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Formatting.Indentation
{
    public class SmartTokenFormatterFormatTokenTests : FormatterTestsBase
    {
        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmptyFile1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"{";

            await ExpectException_SmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 0,
                expectedSpace: 0);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmptyFile2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"}";

            await ExpectException_SmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 0,
                expectedSpace: 0);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 1,
                expectedSpace: 0);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
}";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 1,
                expectedSpace: 0);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 2,
                expectedSpace: 0);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 3,
                expectedSpace: 4);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 3,
                expectedSpace: 4);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 4,
                expectedSpace: 4);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Method1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 5,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Method2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 5,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Method3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
            }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 6,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Property1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        int Goo
            {";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 5,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Property2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        int Goo
        {
            }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 6,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Event1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        event EventHandler Goo
            {";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 5,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Event2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        event EventHandler Goo
        {
            }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 6,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Indexer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        int this[int index]
            {";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 5,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Indexer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        int this[int index]
        {
            }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 6,
                expectedSpace: 8);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
        {";

            await AssertSmartTokenFormatterOpenBraceAsync(
                code,
                indentationLine: 6,
                expectedSpace: 12);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        }
        }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 6,
                expectedSpace: 0);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
            {
                }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 7,
                expectedSpace: 12);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
                {
        }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 7,
                expectedSpace: 12);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
            var a = new []          {
        }";

            var expected = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
            var a = new [] {
        }";

            await AssertSmartTokenFormatterOpenBraceAsync(
                expected,
                code,
                indentationLine: 6);
        }

        [Fact]
        [WorkItem(537827, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537827")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayInitializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace NS
{
    class Class
    {
        void Method(int i)
        {
            int[,] arr =
            {
                {1,1}, {2,2}
}
        }";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 9,
                expectedSpace: 12);
        }

        [Fact]
        [WorkItem(543142, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/543142")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EnterWithTrailingWhitespace()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class Class
{
    void Method(int i)
    {
        var a = new {
 };
";

            await AssertSmartTokenFormatterCloseBraceAsync(
                code,
                indentationLine: 5,
                expectedSpace: 8);
        }

        [WorkItem(9216, "DevDiv_Projects/Roslyn")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OpenBraceWithBaseIndentation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var markup = @"
class C
{
    void M()
    {
[|#line ""Default.aspx"", 273
        if (true)
$${
        }
#line default
#line hidden|]
    }
}";
            await AssertSmartTokenFormatterOpenBraceWithBaseIndentationAsync(markup, baseIndentation: 7, expectedIndentation: 11);
        }

        [WorkItem(9216, "DevDiv_Projects/Roslyn")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void CloseBraceWithBaseIndentation()
        {
            var markup = @"
class C
{
    void M()
    {
[|#line ""Default.aspx"", 273
        if (true)
        {
$$}
#line default
#line hidden|]
    }
}";
#pragma warning disable VSTHRD110 // Observe result of async calls
            AssertSmartTokenFormatterCloseBraceWithBaseIndentation(markup, baseIndentation: 7, expectedIndentation: 11);
#pragma warning restore VSTHRD110 // Observe result of async calls
        }

        [WorkItem(766159, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/766159")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestPreprocessor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M()
    {
        #
    }
}";
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine: 5, ch: '#');
            Assert.Equal(0, actualIndentation);
        }

        [WorkItem(766159, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/766159")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M()
    {
#region
    }
}";
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine: 5, ch: 'n');
            Assert.Equal(8, actualIndentation);
        }

        [WorkItem(766159, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/766159")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEndRegion()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M()
    {
        #region
#endregion
    }
}";
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine: 5, ch: 'n');

            Assert.Equal(8, actualIndentation);
        }

        [WorkItem(777467, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/777467")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSelect()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
using System;
using System.Linq;

class Program
{
    static IEnumerable<int> Goo()
    {
        return from a in new[] { 1, 2, 3 }
                    select
    }
}
";
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine: 9, ch: 't');

            Assert.Equal(15, actualIndentation);
        }

        [WorkItem(777467, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/777467")]
        [Fact, Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestWhere()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
using System;
using System.Linq;

class Program
{
    static IEnumerable<int> Goo()
    {
        return from a in new[] { 1, 2, 3 }
                    where
    }
}
";
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine: 9, ch: 'e');

            Assert.Equal(15, actualIndentation);
        }

        private Task AssertSmartTokenFormatterOpenBraceWithBaseIndentationAsync(string markup, int baseIndentation, int expectedIndentation)
        {
            MarkupTestFile.GetPositionAndSpan(markup,
                out var code, out var position, out TextSpan span);

            return AssertSmartTokenFormatterOpenBraceAsync(
                code,
                SourceText.From(code).Lines.IndexOf(position),
                expectedIndentation,
                baseIndentation,
                span);
        }

        private async Task AssertSmartTokenFormatterOpenBraceAsync(
            string code,
            int indentationLine,
            int expectedSpace,
            int? baseIndentation = null,
            TextSpan span = default(TextSpan))
        {
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine, '{', baseIndentation, span);
            Assert.Equal(expectedSpace, actualIndentation);
        }

        private async Task AssertSmartTokenFormatterOpenBraceAsync(
            string expected,
            string code,
            int indentationLine)
        {
            // create tree service
            using (var workspace = TestWorkspace.CreateCSharp(code))
            {
                var buffer = workspace.Documents.First().GetTextBuffer();

                var actual = await TokenFormatAsync(workspace, buffer, indentationLine, '{');
                Assert.Equal(expected, actual);
            }
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        private Task AssertSmartTokenFormatterCloseBraceWithBaseIndentation(string markup, int baseIndentation, int expectedIndentation)
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            MarkupTestFile.GetPositionAndSpan(markup,
                out var code, out var position, out TextSpan span);

            return AssertSmartTokenFormatterCloseBraceAsync(
                code,
                SourceText.From(code).Lines.IndexOf(position),
                expectedIndentation,
                baseIndentation,
                span);
        }

        private async Task AssertSmartTokenFormatterCloseBraceAsync(
            string code,
            int indentationLine,
            int expectedSpace,
            int? baseIndentation = null,
            TextSpan span = default(TextSpan))
        {
            var actualIndentation = await GetSmartTokenFormatterIndentationAsync(code, indentationLine, '}', baseIndentation, span);
            Assert.Equal(expectedSpace, actualIndentation);
        }

        private async Task ExpectException_SmartTokenFormatterOpenBraceAsync(
            string code,
            int indentationLine,
            int expectedSpace)
        {
            Assert.NotNull(await Record.ExceptionAsync(() => GetSmartTokenFormatterIndentationAsync(code, indentationLine, '{')));
        }

        private async Task ExpectException_SmartTokenFormatterCloseBraceAsync(
            string code,
            int indentationLine,
            int expectedSpace)
        {
            Assert.NotNull(await Record.ExceptionAsync(() => GetSmartTokenFormatterIndentationAsync(code, indentationLine, '}')));
        }
    }
}
