// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Editor.UnitTests.GoToAdjacentMember;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.GoToAdjacentMember
{
    public class CSharpGoToAdjacentMemberTests : AbstractGoToAdjacentMemberTests
    {
        protected override string LanguageName => LanguageNames.CSharp;
        protected override ParseOptions DefaultParseOptions => CSharpParseOptions.Default;

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmptyFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"$$";
            Assert.Null(await GetTargetPositionAsync(code, next: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ClassWithNoMembers()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
$$
}";
            Assert.Null(await GetTargetPositionAsync(code, next: true));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BeforeClassWithMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"$$
class C
{
    [||]void M() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AfterClassWithMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    [||]void M() { }
}

$$";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BetweenClasses()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C1
{
    void M() { }
}

$$

class C2
{
    [||]void M() { }
} ";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BetweenClassesPrevious()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C1
{
    [||]void M() { }
}

$$

class C2
{
    void M() { }
} ";

            await AssertNavigatedAsync(code, next: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FromFirstMemberToSecond()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FromSecondToFirst()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    [||]void M1() { }
    $$void M2() { }
}";

            await AssertNavigatedAsync(code, next: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NextWraps()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    [||]void M1() { }
    $$void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreviousWraps()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DescendsIntoNestedType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }

    class N
    {
        [||]void M2() { }
    }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtConstructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]public C() { }
}";
            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtDestructor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]~C() { }
}";
            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]static C operator+(C left, C right) { throw new System.NotImplementedException(); }
}";
            await AssertNavigatedAsync(code, next: true);
        }
        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtField()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]int F;
}";
            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtFieldlikeEvent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]event System.EventHandler E;
}";
            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtAutoProperty()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }
    [||]int P { get; set ; }
}";
            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtPropertyWithAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }

    [||]int P
    {
        get { return 42; }
        set { }
    }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SkipsPropertyAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1() { }

    $$int P
    {
        get { return 42; }
        set { }
    }

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FromInsideAccessor()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1() { }

    int P
    {
        get { return $$42; }
        set { }
    }

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtIndexerWithAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }

    [||]int this[int i]
    {
        get { return 42; }
        set { }
    }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SkipsIndexerAccessors()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1() { }

    $$int this[int i]
    {
        get { return 42; }
        set { }
    }

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtEventWithAddRemove()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    $$void M1() { }

    [||]event EventHandler E
    {
        add { }
        remove { }
    }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SkipsEventAddRemove()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1() { }

    $$event EventHandler E
    {
        add { }
        remove { }
    }

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FromInsideMethod()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1()
    {
        $$System.Console.WriteLine();
    }

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NextFromBetweenMethods()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1() { }

    $$

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreviousFromBetweenMethods()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    [||]void M1() { }

    $$

    void M2() { }
}";

            await AssertNavigatedAsync(code, next: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NextFromBetweenMethodsInTrailingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    void M1()
    {
    } $$

    [||]void M2() { }
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreviousFromBetweenMethodsInTrailingTrivia()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    [||]void M1()
    {
    } $$

    void M2() { }
}";

            await AssertNavigatedAsync(code, next: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StopsAtExpressionBodiedMember()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    int M1() => $$42;

    [||]int M2() => 42;
}";

            await AssertNavigatedAsync(code, next: true);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
        [WorkItem(10588, "https://github.com/dotnet/roslyn/issues/10588")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreviousFromInsideCurrent()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
class C
{
    [||]void M1()
    {
        Console.WriteLine($$);
    }

    void M2()
    {
    }
}";

            await AssertNavigatedAsync(code, next: false);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NextInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
$$void M1() { }

[||]void M2() { }";

            await AssertNavigatedAsync(code, next: true, sourceCodeKind: SourceCodeKind.Script);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.GoToAdjacentMember)]
        [WorkItem(4311, "https://github.com/dotnet/roslyn/issues/4311")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PrevInScript()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"
[||]void M1() { }

$$void M2() { }";

            await AssertNavigatedAsync(code, next: false, sourceCodeKind: SourceCodeKind.Script);
        }
    }
}
