// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Test.Utilities;
using Microsoft.CodeAnalysis.Editor.UnitTests.BraceMatching;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.BraceMatching
{
    public class CSharpBraceMatcherTests : AbstractBraceMatcherTests
    {
        protected override TestWorkspace CreateWorkspaceFromCode(string code, ParseOptions options)
            => TestWorkspace.CreateCSharp(code, options);

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestEmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"$$";
            var expected = @"";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAtFirstPositionInFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"$$public class C { }";
            var expected = @"public class C { }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAtLastPositionInFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { }$$";
            var expected = @"public class C [|{|] }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCurlyBrace1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C $${ }";
            var expected = @"public class C { [|}|]";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCurlyBrace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C {$$ }";
            var expected = @"public class C { [|}|]";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCurlyBrace3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { $$}";
            var expected = @"public class C [|{|] }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestCurlyBrace4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { }$$";
            var expected = @"public class C [|{|] }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParen1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void Goo$$() { } }";
            var expected = @"public class C { void Goo([|)|] { } }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParen2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void Goo($$) { } }";
            var expected = @"public class C { void Goo([|)|] { } }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParen3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void Goo($$ ) { } }";
            var expected = @"public class C { void Goo( [|)|] { } }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParen4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void Goo( $$) { } }";
            var expected = @"public class C { void Goo[|(|] ) { } }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParen5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void Goo( )$$ { } }";
            var expected = @"public class C { void Goo[|(|] ) { } }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestParen6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void Goo()$$ { } }";
            var expected = @"public class C { void Goo[|(|]) { } }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSquareBracket1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { int$$[] i; }";
            var expected = @"public class C { int[[|]|] i; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSquareBracket2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { int[$$] i; }";
            var expected = @"public class C { int[[|]|] i; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSquareBracket3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { int[$$ ] i; }";
            var expected = @"public class C { int[ [|]|] i; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSquareBracket4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { int[ $$] i; }";
            var expected = @"public class C { int[|[|] ] i; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSquareBracket5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { int[ ]$$ i; }";
            var expected = @"public class C { int[|[|] ] i; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestSquareBracket6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { int[]$$ i; }";
            var expected = @"public class C { int[|[|]] i; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAngleBracket1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Goo$$<int> f; }";
            var expected = @"public class C { Goo<int[|>|] f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAngleBracket2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Goo<$$int> f; }";
            var expected = @"public class C { Goo<int[|>|] f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAngleBracket3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Goo<int$$> f; }";
            var expected = @"public class C { Goo[|<|]int> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestAngleBracket4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Goo<int>$$ f; }";
            var expected = @"public class C { Goo[|<|]int> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func$$<Func<int,int>> f; }";
            var expected = @"public class C { Func<Func<int,int>[|>|] f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<$$Func<int,int>> f; }";
            var expected = @"public class C { Func<Func<int,int>[|>|] f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<Func$$<int,int>> f; }";
            var expected = @"public class C { Func<Func<int,int[|>|]> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<Func<$$int,int>> f; }";
            var expected = @"public class C { Func<Func<int,int[|>|]> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<Func<int,int$$>> f; }";
            var expected = @"public class C { Func<Func[|<|]int,int>> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<Func<int,int>$$> f; }";
            var expected = @"public class C { Func<Func[|<|]int,int>> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<Func<int,int> $$> f; }";
            var expected = @"public class C { Func[|<|]Func<int,int> > f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestNestedAngleBracket8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { Func<Func<int,int>>$$ f; }";
            var expected = @"public class C { Func[|<|]Func<int,int>> f; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestString1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = $$""Goo""; }";
            var expected = @"public class C { string s = ""Goo[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestString2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = ""$$Goo""; }";
            var expected = @"public class C { string s = ""Goo[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestString3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = ""Goo$$""; }";
            var expected = @"public class C { string s = [|""|]Goo""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestString4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = ""Goo""$$; }";
            var expected = @"public class C { string s = [|""|]Goo""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestString5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = ""Goo$$ ";
            var expected = @"public class C { string s = ""Goo ";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVerbatimString1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = $$@""Goo""; }";
            var expected = @"public class C { string s = @""Goo[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVerbatimString2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = @$$""Goo""; }";
            var expected = @"public class C { string s = @""Goo[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVerbatimString3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = @""$$Goo""; }";
            var expected = @"public class C { string s = @""Goo[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVerbatimString4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = @""Goo$$""; }";
            var expected = @"public class C { string s = [|@""|]Goo""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestVerbatimString5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { string s = @""Goo""$$; }";
            var expected = @"public class C { string s = [|@""|]Goo""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""$${x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x[|}|], {y}""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{$$x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x[|}|], {y}""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x$$}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""[|{|]x}, {y}""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}$$, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""[|{|]x}, {y}""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, $${y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {y[|}|]""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {$$y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {y[|}|]""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {y$$}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, [|{|]y}""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {y}$$""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, [|{|]y}""; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString9()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $$[||]$""{x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {y}[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString10()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $[||]$$""{x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $""{x}, {y}[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString11()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $$[||]$@""{x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $@""{x}, {y}[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString12()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $[||]$$@""{x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $@""{x}, {y}[|""|]; }";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterpolatedString13()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $@$$""{x}, {y}""; }";
            var expected = @"public class C { void M() { var x = ""Hello""; var y = ""World""; var s = $@""{x}, {y}[|""|]; }";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalDirectiveWithSingleMatchingDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
public class C 
{
#if$$ CHK 
#endif
}";
            var expected = @"
public class C 
{
#if$$ CHK 
[|#endif|]
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalDirectiveWithTwoMatchingDirectives()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
public class C 
{
#if$$ CHK 
#else
#endif
}";
            var expected = @"
public class C 
{
#if$$ CHK 
[|#else|]
#endif
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestConditionalDirectiveWithAllMatchingDirectives()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
public class C 
{
#if CHK 
#elif RET
#else
#endif$$
}";
            var expected = @"
public class C 
{
[|#if|] CHK 
#elif RET
#else
#endif
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestRegionDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
public class C 
{
$$#region test
#endregion
}";
            var expected = @"
public class C 
{
#region test
[|#endregion|]
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterleavedDirectivesInner()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
#define CHK
public class C 
{
    void Test()
    {
#if CHK
$$#region test
    var x = 5;
#endregion
#else
    var y = 6;
#endif
    }
}";
            var expected = @"
#define CHK
public class C 
{
    void Test()
    {
#if CHK
#region test
    var x = 5;
[|#endregion|]
#else
    var y = 6;
#endif
    }
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestInterleavedDirectivesOuter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
#define CHK
public class C 
{
    void Test()
    {
#if$$ CHK
#region test
    var x = 5;
#endregion
#else
    var y = 6;
#endif
    }
}";
            var expected = @"
#define CHK
public class C 
{
    void Test()
    {
#if CHK
#region test
    var x = 5;
#endregion
[|#else|]
    var y = 6;
#endif
    }
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmatchedDirective1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
public class C 
{
$$#region test
}";
            var expected = @"
public class C 
{
#region test
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7120, "https://github.com/dotnet/roslyn/issues/7120")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmatchedDirective2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
#d$$efine CHK
public class C 
{
}";
            var expected = @"
#define CHK
public class C 
{
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7534, "https://github.com/dotnet/roslyn/issues/7534")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmatchedConditionalDirective()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class Program
{
    static void Main(string[] args)
    {#if$$

    }
}";
            var expected = @"
class Program
{
    static void Main(string[] args)
    {#if

    }
}";

            await TestAsync(code, expected);
        }

        [WorkItem(7534, "https://github.com/dotnet/roslyn/issues/7534")]
        [WpfFact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TestUnmatchedConditionalDirective2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class Program
{
    static void Main(string[] args)
    {#else$$

    }
}";
            var expected = @"
class Program
{
    static void Main(string[] args)
    {#else

    }
}";

            await TestAsync(code, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StartTupleDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { $$(int, int, int, int, int, int, int, int) x; }";
            var expected = @"public class C { (int, int, int, int, int, int, int, int[|)|] x; }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EndTupleDeclaration()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { (int, int, int, int, int, int, int, int)$$ x; }";
            var expected = @"public class C { [|(|]int, int, int, int, int, int, int, int) x; }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StartTupleLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { var x = $$(1, 2, 3, 4, 5, 6, 7, 8); }";
            var expected = @"public class C { var x = (1, 2, 3, 4, 5, 6, 7, 8[|)|]; }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EndTupleLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { var x = (1, 2, 3, 4, 5, 6, 7, 8)$$; }";
            var expected = @"public class C { var x = [|(|]1, 2, 3, 4, 5, 6, 7, 8); }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StartNestedTupleLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { var x = $$((1, 1, 1), 2, 3, 4, 5, 6, 7, 8); }";
            var expected = @"public class C { var x = ((1, 1, 1), 2, 3, 4, 5, 6, 7, 8[|)|]; }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StartInnerNestedTupleLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { var x = ($$(1, 1, 1), 2, 3, 4, 5, 6, 7, 8); }";
            var expected = @"public class C { var x = ((1, 1, 1[|)|], 2, 3, 4, 5, 6, 7, 8); }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EndNestedTupleLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { var x = (1, 2, 3, 4, 5, 6, 7, (8, 8, 8))$$; }";
            var expected = @"public class C { var x = [|(|]1, 2, 3, 4, 5, 6, 7, (8, 8, 8)); }";

            await TestAsync(code, expected, TestOptions.Regular);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.BraceMatching)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EndInnerNestedTupleLiteral()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class C { var x = ((1, 1, 1)$$, 2, 3, 4, 5, 6, 7, 8); }";
            var expected = @"public class C { var x = ([|(|]1, 1, 1), 2, 3, 4, 5, 6, 7, 8); }";

            await TestAsync(code, expected, TestOptions.Regular);
        }
    }
}
