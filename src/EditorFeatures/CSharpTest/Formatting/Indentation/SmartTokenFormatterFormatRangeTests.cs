// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp.Utilities;
using Microsoft.CodeAnalysis.Editor.CSharp.Formatting.Indentation;
using Microsoft.CodeAnalysis.Editor.Implementation.Formatting;
using Microsoft.CodeAnalysis.Editor.UnitTests.Utilities;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Formatting.Rules;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Text.Shared.Extensions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Text.Operations;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Formatting.Indentation
{
    [UseExportProvider]
    public class SmartTokenFormatterFormatRangeTests
    {
        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BeginningOfFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"        using System;$$";
            var expected = @"        using System;";

            Assert.NotNull(await Record.ExceptionAsync(() => AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.None)));
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS
{

    }$$";

            var expected = @"using System;
namespace NS
{

}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS
{
        class Class
                {
        }
    }$$";

            var expected = @"using System;
namespace NS
{
    class Class
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS { }$$";

            var expected = @"using System;
namespace NS { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS { 
}$$";

            var expected = @"using System;
namespace NS
{
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS
{
    class Class { } 
}$$";

            var expected = @"using System;
namespace NS
{
    class Class { }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS
{
    class Class { 
} 
}$$";

            var expected = @"using System;
namespace NS
{
    class Class
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS
{
    class Class { 
} 
            namespace NS2
{}
}$$";

            var expected = @"using System;
namespace NS
{
    class Class
    {
    }
    namespace NS2
    { }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Namespace8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS { class Class { } namespace NS2 { } }$$";

            var expected = @"using System;
namespace NS { class Class { } namespace NS2 { } }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    class Class { 
}$$";

            var expected = @"using System;
class Class
{
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    class Class
{
    void Method(int i) {
                }
}$$";

            var expected = @"using System;
class Class
{
    void Method(int i)
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    class Class
{
    void Method(int i) { }
}$$";

            var expected = @"using System;
class Class
{
    void Method(int i) { }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    class Class
{
            delegate void Test(int i);
}$$";

            var expected = @"using System;
class Class
{
    delegate void Test(int i);
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Class5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    class Class
{
            delegate void Test(int i);
    void Method()
        {
                }
}$$";

            var expected = @"using System;
class Class
{
    delegate void Test(int i);
    void Method()
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Interface1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    interface II
{
            delegate void Test(int i);
int Prop { get; set; }
}$$";

            var expected = @"using System;
interface II
{
    delegate void Test(int i);
    int Prop { get; set; }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Struct1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    struct Struct
{
            Struct(int i)
    {
                }
}$$";

            var expected = @"using System;
struct Struct
{
    Struct(int i)
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Enum1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
    enum Enum
{
                A = 1, B = 2,
    C = 3
            }$$";

            var expected = @"using System;
enum Enum
{
    A = 1, B = 2,
    C = 3
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop { get { return 1; }$$";

            var expected = @"using System;
class Class
{
    int Prop { get { return 1; }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop { get { return 1; } }$$";

            var expected = @"using System;
class Class
{
    int Prop { get { return 1; } }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.IntKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop { get { return 1; }  
}$$";

            var expected = @"using System;
class Class
{
    int Prop
    {
        get { return 1; }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.IntKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop { get { return 1;   
}$$";

            var expected = @"using System;
class Class
{
    int Prop { get
        {
            return 1;
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.GetKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop {
        get { return 1;   
}$$";

            var expected = @"using System;
class Class
{
    int Prop {
        get { return 1;
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(16984, "https://github.com/dotnet/roslyn/issues/16984")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList5b()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop {
        get { return 1;   
}$$
}
}";

            var expected = @"using System;
class Class
{
    int Prop {
        get
        {
            return 1;
        }
}
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop 
        { 
get { return 1;   
} }$$";

            var expected = @"using System;
class Class
{
    int Prop
    {
        get
        {
            return 1;
        }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.IntKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    int Prop
    {
        get
        {
return 1;$$
        }
    }";

            var expected = @"using System;
class Class
{
    int Prop
    {
        get
        {
            return 1;
        }
    }";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(16984, "https://github.com/dotnet/roslyn/issues/16984")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    int Prop
    {
get
        {
            return 0;
        }$$
    }
}";

            var expected = @"class C
{
    int Prop
    {
        get
        {
            return 0;
        }
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(16984, "https://github.com/dotnet/roslyn/issues/16984")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList9()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    int Prop
    {
set
        {
            ;
        }$$
    }
}";

            var expected = @"class C
{
    int Prop
    {
        set
        {
            ;
        }
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(16984, "https://github.com/dotnet/roslyn/issues/16984")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList10()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    event EventHandler E
    {
add
        {
        }$$
        remove
        {
        }
    }

}";

            var expected = @"class C
{
    event EventHandler E
    {
        add
        {
        }
        remove
        {
        }
    }

}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(16984, "https://github.com/dotnet/roslyn/issues/16984")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task AccessorList11()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    event EventHandler E
    {
        add
        {
        }
remove
        {
        }$$
    }

}";

            var expected = @"class C
{
    event EventHandler E
    {
        add
        {
        }
        remove
        {
        }
    }

}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.CloseBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    public int Method()
    { }$$";

            var expected = @"using System;
class Class
{
    public int Method()
    { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    public int Method() { }$$";

            var expected = @"using System;
class Class
{
    public int Method() { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    public int Method() { 
}$$
}";

            var expected = @"using System;
class Class
{
    public int Method()
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    public static Class operator +(Class c1, Class c2) {
            }$$
}";

            var expected = @"using System;
class Class
{
    public static Class operator +(Class c1, Class c2)
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        { 
}$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        { { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        { { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Block8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        { { 
}$$
        }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        {
            {
            }
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SwitchStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        switch (a) {
            case 1:
                break;
}$$
    }
}";

            var expected = @"using System;
class Class
{
    void Method()
    {
        switch (a)
        {
            case 1:
                break;
        }
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SwitchStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        switch (true) { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        switch (true) { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SwitchStatement3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        switch (true)
        {
            case 1: { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        switch (true)
        {
            case 1: { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.ColonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SwitchStatement4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        switch (true)
        {
            case 1: { 
}$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        switch (true)
        {
            case 1:
                {
                }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.ColonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Initializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        var arr = new int[] { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        var arr = new int[] { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Initializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        var arr = new int[] { 
}$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        var arr = new int[] {
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Initializer3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        var arr = new { A = 1, B = 2
}$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        var arr = new
        {
            A = 1,
            B = 2
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Initializer4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        var arr = new { A = 1, B = 2 }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        var arr = new { A = 1, B = 2 }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Initializer5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        var arr = new[] { 
            1, 2, 3, 4,
            5 }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        var arr = new[] {
            1, 2, 3, 4,
            5 }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Initializer6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        var arr = new int[] { 
            1, 2, 3, 4,
            5 }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        var arr = new int[] {
            1, 2, 3, 4,
            5 }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        if (true) { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        if (true) { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        if (true) { 
        }$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        if (true)
        {
        }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        if (true)
        { }$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        if (true)
        { }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        while (true) {
}$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        while (true)
        {
        }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(8413, "https://github.com/dotnet/roslyn/issues/8413")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatementDoBlockAlone()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        do {
}$$
    }
}";

            var expected = @"using System;
class Class
{
    void Method()
    {
        do
        {
        }
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        do {
} while(true);$$
    }
}";

            var expected = @"using System;
class Class
{
    void Method()
    {
        do
        {
        } while (true);
    }
}";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        for (int i = 0; i < 10; i++)             {
}$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        for (int i = 0; i < 10; i++)
        {
        }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        foreach (var i in collection)            {
}$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        foreach (var i in collection)
        {
        }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        using (var resource = GetResource())           {
}$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        using (var resource = GetResource())
        {
        }
    }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement9()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        if (true)
                int i = 10;$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        if (true)
            int i = 10;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FieldlInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
          string str =              Console.Title;$$
";

            var expected = @"using System;
class Class
{
    string str = Console.Title;
";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayFieldlInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
namespace NS
{
    class Class
    {
                    string[] strArr = {           ""1"",                       ""2"" };$$
";

            var expected = @"using System;
namespace NS
{
    class Class
    {
        string[] strArr = { ""1"", ""2"" };
";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExpressionValuedPropertyInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
          public int  Three =>   1+2;$$
";

            var expected = @"using System;
class Class
{
    public int Three => 1 + 2;
";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement10()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
        if (true)
                int i = 10;$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        if (true)
            int i = 10;
    }";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement11()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                using (var resource = GetResource()) resource.Do();$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        using (var resource = GetResource()) resource.Do();";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement12()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                using (var resource = GetResource()) 
    resource.Do();$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        using (var resource = GetResource())
            resource.Do();";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement13()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                using (var resource = GetResource()) 
    resource.Do();$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        using (var resource = GetResource())
            resource.Do();
    }";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement14()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                do i = 10;$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        do i = 10;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement15()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                do
    i = 10;$$";

            var expected = @"using System;
class Class
{
    void Method()
    {
        do
            i = 10;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement16()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                do
    i = 10;$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        do
            i = 10;
    }";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbeddedStatement17()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
    void Method()
    {
                do
    i = 10;
while (true);$$
    }";

            var expected = @"using System;
class Class
{
    void Method()
    {
        do
            i = 10;
        while (true);
    }";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
                    int i = 10;
                    int i2 = 10;$$";

            var expected = @"using System;
class Class
{
                    int i = 10;
    int i2 = 10;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
            void Method(int i)
            {
            }

            void Method2()
            {
            }$$
}";

            var expected = @"using System;
class Class
{
            void Method(int i)
            {
            }

    void Method2()
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.CloseBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
            void Method(int i)
            {
            }

            A a = new A 
            {
                Prop = 1,
                Prop2 = 2
            };$$
}";

            var expected = @"using System;
class Class
{
            void Method(int i)
            {
            }

    A a = new A
    {
        Prop = 1,
        Prop2 = 2
    };
}";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.CloseBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
            void Method(int i)
            {
                        int i = 10;
             int i2 = 10;$$";

            var expected = @"using System;
class Class
{
            void Method(int i)
            {
                        int i = 10;
        int i2 = 10;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Class
{
            void Method(int i)
            {
                        int i = 10;
                if (true)
i = 50;$$";

            var expected = @"using System;
class Class
{
            void Method(int i)
            {
                        int i = 10;
        if (true)
            i = 50;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"        using System;
        using System.Linq;$$";

            var expected = @"        using System;
using System.Linq;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"            using System;

            namespace NS
            {
            }

        namespace NS2
        {
        }$$";

            var expected = @"            using System;

            namespace NS
            {
            }

namespace NS2
{
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.CloseBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FollowPreviousElement8()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

namespace NS
{
            class Class
            {
            }

        class Class1
        {
        }$$
}";

            var expected = @"using System;

namespace NS
{
            class Class
            {
            }

    class Class1
    {
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.CloseBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IfStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
            if (true)
        {
    }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        if (true)
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IfStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
            if (true)
        {
    }
else
        {
                }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        if (true)
        {
        }
        else
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IfStatement3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
            if (true)
        {
    }
else    if (false)
        {
                }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        if (true)
        {
        }
        else if (false)
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IfStatement4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
            if (true)
        return          ;
else    if (false)
                    return          ;$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        if (true)
            return;
        else if (false)
            return;";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TryStatement1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
                try
    {
        }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        try
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TryStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
                try
    {
        }
catch    (  Exception       ex)
                {
    }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        try
        {
        }
        catch (Exception ex)
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TryStatement3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
                try
    {
        }
catch    (  Exception       ex)
                {
    }
            catch               (Exception          ex2)
                      {
   }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        try
        {
        }
        catch (Exception ex)
        {
        }
        catch (Exception ex2)
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TryStatement4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
                try
    {
        }
                                finally
                      {
   }$$";

            var expected = @"using System;

class Class
{
    void Method()
    {
        try
        {
        }
        finally
        {
        }";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(6645, "https://github.com/dotnet/roslyn/issues/6645")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task TryStatement5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;

class Class
{
    void Method()
    {
        try {
        }$$
    }
}";

            var expected = @"using System;

class Class
{
    void Method()
    {
        try
        {
        }
    }
}";

            await AutoFormatOnCloseBraceAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        [WorkItem(537555, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537555")]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SingleLine()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C { void M() { C.M(    );$$ } }";

            var expected = @"class C { void M() { C.M(); } }";

            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StringLiterals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C { void M() { C.M(""Test {0}$$";

            var expected = string.Empty;
            await AutoFormatOnMarkerAsync(code, expected, SyntaxKind.StringLiteralToken, SyntaxKind.None);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CharLiterals()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C { void M() { C.M('}$$";

            var expected = string.Empty;
            await AutoFormatOnMarkerAsync(code, expected, SyntaxKind.CharacterLiteralToken, SyntaxKind.None);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CharLiterals1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"''';$$";

            var expected = string.Empty;
            await AutoFormatOnMarkerAsync(code, expected, SyntaxKind.EndOfFileToken, SyntaxKind.None);
        }

        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comments()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C { void M() { // { }$$";

            var expected = string.Empty;
            await AutoFormatOnMarkerAsync(code, expected, SyntaxKind.OpenBraceToken, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task FirstLineInFile()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;$$";

            await AutoFormatOnSemicolonAsync(code, "using System;", SyntaxKind.UsingKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Label1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void Method()
    {
                L           :               int             i               =               20;$$
    }
}";
            var expected = @"class C
{
    void Method()
    {
    L: int i = 20;
    }
}";
            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Label2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void Method()
    {
                L           :               
int             i               =               20;$$
    }
}";
            var expected = @"class C
{
    void Method()
    {
    L:
        int i = 20;
    }
}";
            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Label3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void Method()
    {
        int base = 10;
                L           :               
int             i               =               20;$$
    }
}";
            var expected = @"class C
{
    void Method()
    {
        int base = 10;
    L:
        int i = 20;
    }
}";
            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Label4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void Method()
    {
        int base = 10;
    L:
        int i = 20;
int         nextLine            =           30          ;$$
    }
}";
            var expected = @"class C
{
    void Method()
    {
        int base = 10;
    L:
        int i = 20;
        int nextLine = 30;
    }
}";
            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Label6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void Method()
    {
    L:
        int i = 20;
int         nextLine            =           30          ;$$
    }
}";
            var expected = @"class C
{
    void Method()
    {
    L:
        int i = 20;
        int nextLine = 30;
    }
}";
            await AutoFormatOnSemicolonAsync(code, expected, SyntaxKind.OpenBraceToken);
        }

        [WorkItem(537776, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537776")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DisappearedTokens()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class Class1
{
    int goo()
        return 0;
        }$$
}";

            var expected = @"class Class1
{
    int goo()
        return 0;
        }
}";
            await AutoFormatOnCloseBraceAsync(
                code,
                expected,
                SyntaxKind.ClassKeyword);
        }

        [WorkItem(537779, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537779")]
        [Fact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DisappearedTokens2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class Class1
{
    void Goo()
    {
        Object o=new Object);$$
    }
}";

            var expected = @"class Class1
{
    void Goo()
    {
        Object o=new Object);
    }
}";
            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.SemicolonToken);
        }

        [WorkItem(537793, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537793")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Delegate1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"delegate void MyDelegate(int a,int b);$$";

            var expected = @"delegate void MyDelegate(int a, int b);";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.DelegateKeyword);
        }

        [WorkItem(537827, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537827")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoubleInitializer()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void Method()
    {
        int[,] a ={{ 1 , 1 }$$
    }
}";

            var expected = @"class C
{
    void Method()
    {
        int[,] a ={{ 1 , 1 }
    }
}";

            await AutoFormatOnCloseBraceAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WorkItem(537825, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537825")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MissingToken1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class Class1
{
    int a = 1}$$;
}";

            var expected = @"public class Class1
{
    int a = 1};
}";

            await AutoFormatOnCloseBraceAsync(
                code,
                expected,
                SyntaxKind.PublicKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayInitializer1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class Class1
{
    var a = new [] 
    {
        1, 2, 3, 4
        }$$
}";

            var expected = @"public class Class1
{
    var a = new[]
    {
        1, 2, 3, 4
        }
}";

            await AutoFormatOnCloseBraceAsync(
                code,
                expected,
                SyntaxKind.NewKeyword);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayInitializer2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class Class1
{
    var a = new [] 
    {
        1, 2, 3, 4
        }   ;$$
}";

            var expected = @"public class Class1
{
    var a = new[]
    {
        1, 2, 3, 4
        };
}";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(537825, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537825")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MalformedCode()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace ClassLibrary1
{
    public class Class1
    {
        int a}$$;
    }
}";

            var expected = @"namespace ClassLibrary1
{
    public class Class1
    {
        int a};
    }
}";

            await AutoFormatOnCloseBraceAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(537804, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537804")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Colon_SwitchLabel()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
            switch(E.Type)
            {
                    case 1 :$$
            }
        }
    }
}";

            var expected = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
            switch(E.Type)
            {
                case 1:
            }
        }
    }
}";

            await AutoFormatOnColonAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(584599, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/584599")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Colon_SwitchLabel_Comment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
            switch(E.Type)
            {
                        // test
                    case 1 :$$
            }
        }
    }
}";

            var expected = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
            switch(E.Type)
            {
                // test
                case 1:
            }
        }
    }
}";

            await AutoFormatOnColonAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(584599, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/584599")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Colon_SwitchLabel_Comment2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
            switch(E.Type)
            {
                case 2:
                    // test
                    case 1 :$$
            }
        }
    }
}";

            var expected = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
            switch(E.Type)
            {
                case 2:
                // test
                case 1:
            }
        }
    }
}";

            await AutoFormatOnColonAsync(
                code,
                expected,
                SyntaxKind.ColonToken);
        }

        [Fact]
        [WorkItem(537804, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/537804")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Colon_Label()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
                    label   :$$
        }
    }
}";

            var expected = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
                    label   :
        }
    }
}";

            await AutoFormatOnColonAsync(
                code,
                expected,
                SyntaxKind.None);
        }

        [WpfFact]
        [WorkItem(538793, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538793")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Colon_Label2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
                    label   :   Console.WriteLine(10) ;$$
        }
    }
}";

            var expected = @"namespace ClassLibrary1
{
    public class Class1
    {
        void Test()
        {
        label: Console.WriteLine(10);
        }
    }
}";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(3186, "DevDiv_Projects/Roslyn")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SemicolonInElseIfStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        int a = 0;
        if (a == 0)
            a = 1;
        else if (a == 1)
            a=2;$$
        else
            a = 3;

    }
}";

            var expected = @"using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        int a = 0;
        if (a == 0)
            a = 1;
        else if (a == 1)
            a = 2;
        else
            a = 3;

    }
}";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [WorkItem(538391, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/538391")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SemicolonInElseIfStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"public class Class1
{
    void Method()
    {
        int a = 1;
        if (a == 0)
            a = 8;$$
                    else
                        a = 10;
    }
}";

            var expected = @"public class Class1
{
    void Method()
    {
        int a = 1;
        if (a == 0)
            a = 8;
        else
            a = 10;
    }
}";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [WorkItem(8385, "DevDiv_Projects/Roslyn")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NullCoalescingOperator()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C
{
    void M()
    {
        object o2 = null??null;$$
    }
}";

            var expected = @"class C
{
    void M()
    {
        object o2 = null ?? null;
    }
}";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(541517, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/541517")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task SwitchDefault()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using System;
class Program
{
    static void Main()
    {
        switch (DayOfWeek.Monday)
        {
            case DayOfWeek.Monday:
            case DayOfWeek.Tuesday:
                break;
            case DayOfWeek.Wednesday:
                break;
                default:$$
        }
    }
}";

            var expected = @"using System;
class Program
{
    static void Main()
    {
        switch (DayOfWeek.Monday)
        {
            case DayOfWeek.Monday:
            case DayOfWeek.Tuesday:
                break;
            case DayOfWeek.Wednesday:
                break;
            default:
        }
    }
}";

            await AutoFormatOnColonAsync(
                code,
                expected,
                SyntaxKind.SemicolonToken);
        }

        [WpfFact]
        [WorkItem(542538, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542538")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MissingTokens1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class Program
{
    static void Main(string[] args)
    {
        gl::$$
    }
}";

            var expected = @"class Program
{
    static void Main(string[] args)
    {
        gl::
    }
}";

            await AutoFormatOnMarkerAsync(
                code,
                expected,
                SyntaxKind.ColonColonToken,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(542538, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542538")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MissingTokens2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"class C { void M() { M(() => { }$$ } }";

            var expected = @"class C { void M() { M(() => { } } }";

            await AutoFormatOnCloseBraceAsync(
                code,
                expected,
                SyntaxKind.EqualsGreaterThanToken);
        }

        [WpfFact]
        [WorkItem(542953, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542953")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UsingAlias()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"using Alias=System;$$";

            var expected = @"using Alias = System;";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.UsingKeyword);
        }

        [WpfFact]
        [WorkItem(542953, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/542953")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NoLineChangeWithSyntaxError()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var code = @"struct Goo { public int member; }
class Program{
    void Main()
    {
        var f = new Goo { member;$$ }
    }
}";

            var expected = @"struct Goo { public int member; }
class Program{
    void Main()
    {
        var f = new Goo { member; }
    }
}";

            await AutoFormatOnSemicolonAsync(
                code,
                expected,
                SyntaxKind.OpenBraceToken);
        }

        [WpfFact]
        [WorkItem(620568, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/620568")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void SkippedTokens1()
        {
            var code = @";$$*";

            var expected = @";*";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(530830, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530830")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor()
        {
            var code = @"class C
{
    int Prop {          get             ;$$
}";

            var expected = @"class C
{
    int Prop {          get;
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(530830, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530830")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor2()
        {
            var code = @"class C
{
    int Prop {          get;                set             ;$$
}";

            var expected = @"class C
{
    int Prop {          get;                set;
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(530830, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/530830")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor3()
        {
            var code = @"class C
{
    int Prop {          get;                set             ;           }$$
}";

            var expected = @"class C
{
    int Prop { get; set; }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(784674, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/784674")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor4()
        {
            var code = @"class C
{
    int Prop {          get;$$             }
}";

            var expected = @"class C
{
    int Prop { get; }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(924469, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/924469")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor5()
        {
            var code = @"class C
{
    int Prop {          get;                set             ;$$           }
}";

            var expected = @"class C
{
    int Prop { get; set; }
}";
            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(924469, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/924469")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor6()
        {
            var code = @"class C
{
    int Prop { get;set;$$}
}";

            var expected = @"class C
{
    int Prop { get; set; }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(924469, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/924469")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void AutoPropertyAccessor7()
        {
            var code = @"class C
{
    int Prop     { get;set;$$}    
}";

            var expected = @"class C
{
    int Prop     { get; set; }    
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(912965, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/912965")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void NestedUsingStatement()
        {
            var code = @"class C
{
    public void M()
    {
        using (null)
            using(null)$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        using (null)
        using (null)
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(912965, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/912965")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void NestedNotUsingStatement()
        {
            var code = @"class C
{
    public void M()
    {
        using (null)
            for(;;)$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        using (null)
            for(;;)
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void UsingStatementWithNestedFixedStatement()
        {
            var code = @"class C
{
    public void M()
    {
        using (null)
        fixed (void* ptr = &i)
        {
        }$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        using (null)
            fixed (void* ptr = &i)
            {
            }
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FixedStatementWithNestedUsingStatement()
        {
            var code = @"class C
{
    public void M()
    {
        fixed (void* ptr = &i)
        using (null)$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        fixed (void* ptr = &i)
            using (null)
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FixedStatementWithNestedFixedStatement()
        {
            var code = @"class C
{
    public void M()
    {
        fixed (void* ptr1 = &i)
            fixed (void* ptr2 = &i)
            {
            }$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        fixed (void* ptr1 = &i)
        fixed (void* ptr2 = &i)
        {
        }
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FixedStatementWithNestedNotFixedStatement()
        {
            var code = @"class C
{
    public void M()
    {
        fixed (void* ptr = &i)
        if (false)
        {
        }$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        fixed (void* ptr = &i)
            if (false)
            {
            }
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void NotFixedStatementWithNestedFixedStatement()
        {
            var code = @"class C
{
    public void M()
    {
        if (false)
        fixed (void* ptr = &i)
        {
        }$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        if (false)
            fixed (void* ptr = &i)
            {
            }
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(954386, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/954386")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForFirstStatementOfBlock()
        {
            var code = @"class C
{
    public void M()
    {int s;$$
    }
}";

            var expected = @"class C
{
    public void M()
    {
        int s;
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(954386, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/954386")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForFirstMemberofType()
        {
            var code = @"class C
{int s;$$
    public void M()
    {
    }
}";

            var expected = @"class C
{
    int s;
    public void M()
    {
    }
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(954386, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/954386")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForFirstMethodMemberofType()
        {
            var code = @"interface C
{void s();$$
}";

            var expected = @"interface C
{
    void s();
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(17257, "https://github.com/dotnet/roslyn/issues/17257")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForConstructor()
        {
            var code = @"class C
{public C()=>f=1;$$
}";

            var expected = @"class C
{
    public C() => f = 1;
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(17257, "https://github.com/dotnet/roslyn/issues/17257")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForDestructor()
        {
            var code = @"class C
{~C()=>f=1;$$
}";

            var expected = @"class C
{
    ~C() => f = 1;
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(17257, "https://github.com/dotnet/roslyn/issues/17257")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForOperator()
        {
            var code = @"class C
{public static C operator +(C left, C right)=>field=1;$$
    static int field;
}";

            var expected = @"class C
{
    public static C operator +(C left, C right) => field = 1;
    static int field;
}";

            AutoFormatToken(code, expected);
        }

        [WpfFact]
        [WorkItem(954386, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/954386")]
        [Trait(Traits.Feature, Traits.Features.SmartTokenFormatting)]
        public void FormattingRangeForFirstMemberOfNamespace()
        {
            var code = @"namespace C
{delegate void s();$$
}";

            var expected = @"namespace C
{
    delegate void s();
}";

            AutoFormatToken(code, expected);
        }

        [WorkItem(981821, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/981821")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.Formatting)]
        public void FormatDirectiveTriviaAlwaysToColumnZero()
        {
            var code = @"class Program
{
    static void Main(string[] args)
    {
#if
        #$$
    }
}
";

            var expected = @"class Program
{
    static void Main(string[] args)
    {
#if
#
    }
}
";

            AutoFormatToken(code, expected);
        }

        [WorkItem(981821, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/981821")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.Formatting)]
        public void FormatDirectiveTriviaAlwaysToColumnZeroWithCode()
        {
            var code = @"class Program
{
    static void Main(string[] args)
    {
#if
        int s = 10;
        #$$
    }
}
";

            var expected = @"class Program
{
    static void Main(string[] args)
    {
#if
        int s = 10;
#
    }
}
";

            AutoFormatToken(code, expected);
        }

        [WorkItem(981821, "http://vstfdevdiv:8080/DevDiv2/DevDiv/_workitems/edit/981821")]
        [WpfFact]
        [Trait(Traits.Feature, Traits.Features.Formatting)]
        public void FormatDirectiveTriviaAlwaysToColumnZeroWithBrokenElseDirective()
        {
            var code = @"class Program
{
    static void Main(string[] args)
    {
#else
        #$$
    }
}
";

            var expected = @"class Program
{
    static void Main(string[] args)
    {
#else
#
    }
}
";

            AutoFormatToken(code, expected);
        }

        internal static void AutoFormatToken(string markup, string expected)
        {
            using (var workspace = TestWorkspace.CreateCSharp(markup))
            {
                var subjectDocument = workspace.Documents.Single();

                var commandHandler = workspace.GetService<FormatCommandHandler>();
                var typedChar = subjectDocument.GetTextBuffer().CurrentSnapshot.GetText(subjectDocument.CursorPosition.Value - 1, 1);
                commandHandler.ExecuteCommand(new TypeCharCommandArgs(subjectDocument.GetTextView(), subjectDocument.TextBuffer, typedChar[0]), () => { }, TestCommandExecutionContext.Create());

                var newSnapshot = subjectDocument.TextBuffer.CurrentSnapshot;

                Assert.Equal(expected, newSnapshot.GetText());
            }
        }

        private static Tuple<OptionSet, IEnumerable<IFormattingRule>> GetService(
            TestWorkspace workspace)
        {
            var options = workspace.Options;
            return Tuple.Create(options, Formatter.GetDefaultFormattingRules(workspace, LanguageNames.CSharp));
        }

        private Task AutoFormatOnColonAsync(string codeWithMarker, string expected, SyntaxKind startTokenKind)
        {
            return AutoFormatOnMarkerAsync(codeWithMarker, expected, SyntaxKind.ColonToken, startTokenKind);
        }

        private Task AutoFormatOnSemicolonAsync(string codeWithMarker, string expected, SyntaxKind startTokenKind)
        {
            return AutoFormatOnMarkerAsync(codeWithMarker, expected, SyntaxKind.SemicolonToken, startTokenKind);
        }

        private Task AutoFormatOnCloseBraceAsync(string codeWithMarker, string expected, SyntaxKind startTokenKind)
        {
            return AutoFormatOnMarkerAsync(codeWithMarker, expected, SyntaxKind.CloseBraceToken, startTokenKind);
        }

        private async Task AutoFormatOnMarkerAsync(string initialMarkup, string expected, SyntaxKind tokenKind, SyntaxKind startTokenKind)
        {
            using (var workspace = TestWorkspace.CreateCSharp(initialMarkup))
            {
                var tuple = GetService(workspace);
                var testDocument = workspace.Documents.Single();
                var buffer = testDocument.GetTextBuffer();
                var position = testDocument.CursorPosition.Value;

                var document = workspace.CurrentSolution.GetDocument(testDocument.Id);

                var root = (CompilationUnitSyntax)await document.GetSyntaxRootAsync();
                var endToken = root.FindToken(position);
                if (position == endToken.SpanStart && !endToken.GetPreviousToken().IsKind(SyntaxKind.None))
                {
                    endToken = endToken.GetPreviousToken();
                }

                Assert.Equal(tokenKind, endToken.Kind());
                var formatter = new SmartTokenFormatter(tuple.Item1, tuple.Item2, root);

                var tokenRange = FormattingRangeHelper.FindAppropriateRange(endToken);
                if (tokenRange == null)
                {
                    Assert.Equal(startTokenKind, SyntaxKind.None);
                    return;
                }

                Assert.Equal(startTokenKind, tokenRange.Value.Item1.Kind());
                if (tokenRange.Value.Item1.Equals(tokenRange.Value.Item2))
                {
                    return;
                }

                var changes = formatter.FormatRange(workspace, tokenRange.Value.Item1, tokenRange.Value.Item2, CancellationToken.None);
                var actual = GetFormattedText(buffer, changes);
                Assert.Equal(expected, actual);
            }
        }

        private static string GetFormattedText(ITextBuffer buffer, IList<TextChange> changes)
        {
            using (var edit = buffer.CreateEdit())
            {
                foreach (var change in changes)
                {
                    edit.Replace(change.Span.ToSpan(), change.NewText);
                }

                edit.Apply();
            }

            return buffer.CurrentSnapshot.GetText();
        }
    }
}
