// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.PopulateSwitch;
using Microsoft.CodeAnalysis.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics.PopulateSwitch
{
    public partial class PopulateSwitchTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new PopulateSwitchDiagnosticAnalyzer(), new PopulateSwitchCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task OnlyOnFirstToken()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch ([||]e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AllMembersAndDefaultExist()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                case MyEnum.FizzBuzz:
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AllMembersExist_NotDefault()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                case MyEnum.FizzBuzz:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_NotDefault()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_WithDefault()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                default:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_NotDefault_EnumHasExplicitType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum : long
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum : long
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_WithMembersAndDefaultInSection_NewValuesAboveDefaultSection()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                default:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.FizzBuzz:
                    break;
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_WithMembersAndDefaultInSection_AssumesDefaultIsInLastSection()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                default:
                    break;
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                default:
                    break;
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoMembersExist0()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                    break;
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
            }
        }
    }
}", index: 0);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoMembersExist1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                default:
                    break;
            }
        }
    }
}", index: 1);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoMembersExist2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                    break;
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingStaticEnum_AllMembersExist()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using static System.IO.FileMode;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = Append;
            [||]switch (e)
            {
                case CreateNew:
                    break;
                case Create:
                    break;
                case Open:
                    break;
                case OpenOrCreate:
                    break;
                case Truncate:
                    break;
                case Append:
                    break;
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingStaticEnum_AllMembersExist_OutOfDefaultOrder()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using static System.IO.FileMode;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = Append;
            [||]switch (e)
            {
                case CreateNew:
                    break;
                case OpenOrCreate:
                    break;
                case Truncate:
                    break;
                case Open:
                    break;
                case Append:
                    break;
                case Create:
                    break;
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingStaticEnum_MembersExist()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using static System.IO.FileMode;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = Append;
            [||]switch (e)
            {
                case CreateNew:
                    break;
                case Create:
                    break;
                case Open:
                    break;
                case OpenOrCreate:
                    break;
                default:
                    break;
            }
        }
    }
}",
@"using static System.IO.FileMode;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = Append;
            switch (e)
            {
                case CreateNew:
                    break;
                case Create:
                    break;
                case Open:
                    break;
                case OpenOrCreate:
                    break;
                case Truncate:
                    break;
                case Append:
                    break;
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingStaticEnum_NoMembersExist()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"using static System.IO.FileMode;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = Append;
            [||]switch (e)
            {
            }
        }
    }
}",
@"using static System.IO.FileMode;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = Append;
            switch (e)
            {
                case CreateNew:
                    break;
                case Create:
                    break;
                case Open:
                    break;
                case OpenOrCreate:
                    break;
                case Truncate:
                    break;
                case Append:
                    break;
                default:
                    break;
            }
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_NotDefault_EnumHasNonFlagsAttribute()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    [System.Obsolete]
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    [System.Obsolete]
    enum MyEnum
    {
        Fizz,
        Buzz,
        FizzBuzz
    }

    class MyClass
    {
        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_NotDefault_EnumIsNested()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"namespace ConsoleApplication1
{
    class MyClass
    {
        enum MyEnum
        {
            Fizz,
            Buzz,
            FizzBuzz
        }

        void Method()
        {
            var e = MyEnum.Fizz;
            [||]switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
            }
        }
    }
}",
@"namespace ConsoleApplication1
{
    class MyClass
    {
        enum MyEnum
        {
            Fizz,
            Buzz,
            FizzBuzz
        }

        void Method()
        {
            var e = MyEnum.Fizz;
            switch (e)
            {
                case MyEnum.Fizz:
                case MyEnum.Buzz:
                    break;
                case MyEnum.FizzBuzz:
                    break;
                default:
                    break;
            }
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_SwitchIsNotEnum()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestMissingInRegularAndScriptAsync(
@"using System;

namespace ConsoleApplication1
{
    class MyClass
    {
        void Method()
        {
            var e = ""test"";
            [||]switch (e)
            {
                case ""test1"":
                case ""test1"":
                default:
                    break;
            }
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAllMembersExist_NotDefault_UsingConstants()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
@"enum MyEnum
{
    Fizz,
    Buzz,
    FizzBuzz
}

class MyClass
{
    void Method()
    {
        var e = MyEnum.Fizz;
        [||]switch (e)
        {
            case (MyEnum)0:
            case (MyEnum)1:
                break;
        }
    }
}",
@"enum MyEnum
{
    Fizz,
    Buzz,
    FizzBuzz
}

class MyClass
{
    void Method()
    {
        var e = MyEnum.Fizz;
        switch (e)
        {
            case (MyEnum)0:
            case (MyEnum)1:
                break;
            case MyEnum.FizzBuzz:
                break;
            default:
                break;
        }
    }
}", index: 2);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsPopulateSwitch)]
        [WorkItem(13455, "https://github.com/dotnet/roslyn/issues/13455")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AllMissingTokens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            await TestInRegularAndScriptAsync(
            @"
enum MyEnum
{
    Fizz
}
class MyClass
{
    void Method()
    {
        var e = MyEnum.Fizz;
        [||]switch (e)
    }
}
",
            @"
enum MyEnum
{
    Fizz
}
class MyClass
{
    void Method()
    {
        var e = MyEnum.Fizz;
        switch (e)
        {
            case MyEnum.Fizz:
                break;
        }
    }
}");
        }
    }
}
