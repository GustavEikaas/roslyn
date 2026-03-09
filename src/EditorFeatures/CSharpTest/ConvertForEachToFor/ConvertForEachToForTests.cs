// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.CodeStyle;
using Microsoft.CodeAnalysis.CSharp.ConvertForEachToFor;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.ConvertForEachToFor
{
    public partial class ConvertForEachToForTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(
            Workspace workspace, TestParameters parameters)
            => new CSharpConvertForEachToForCodeRefactoringProvider();

        private readonly CodeStyleOption<bool> onWithSilent = new CodeStyleOption<bool>(true, NotificationOption.Silent);

        private IDictionary<OptionKey, object> ImplicitTypeEverywhere => OptionsSet(
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWherePossible, onWithSilent),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeWhereApparent, onWithSilent),
            SingleOption(CSharpCodeStyleOptions.UseImplicitTypeForIntrinsicTypes, onWithSilent));

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmptyBlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array)
        {
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmptyBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array) ;
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++) ;
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Body()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array) Console.WriteLine(a);
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            int a = array[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task BlockBody()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array)
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            int a = array[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        /* comment */
        foreach[||](var a in array) /* comment */
        {
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        /* comment */
        for (int {|Rename:i|} = 0; i < array.Length; i++) /* comment */
        {
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array)
        /* comment */
        {
        }/* comment */
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        /* comment */
        {
        }/* comment */
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array) /* comment */;
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++) /* comment */;
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment4()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach[||](var a in array) Console.WriteLine(a); // test
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            int a = array[i];
            Console.WriteLine(a); // test
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment5()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var a in array) /* test */ Console.WriteLine(a); 
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++) /* test */
        {
            int a = array[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment6()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var a in array) 
            /* test */ Console.WriteLine(a); 
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            int a = array[i];
            /* test */
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Comment7()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        // test
        foreach[||](var a in new int[] { 1, 3, 4 })
        {
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        // test
        int[] {|Rename:array|} = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommentNotSupported()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var a /* test */ in array) ;
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommentNotSupported2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (/* test */ var a in array) ;
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CommentNotSupported3()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var a in array /* test */) ;
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CollectionStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        foreach[||](var a in new int[] { 1, 3, 4 })
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        int[] {|Rename:array|} = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            int a = array[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CollectionConflict()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = 1;

        foreach[||](var a in new int[] { 1, 3, 4 })
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = 1;

        int[] {|Rename:array1|} = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array1.Length; i++)
        {
            int a = array1[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IndexConflict()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var a in array)
        {
            int i = 0;
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i1|} = 0; i1 < array.Length; i1++)
        {
            int a = array[i1];
            int i = 0;
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task VariableWritten()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var a in array)
        {
            a = 1;
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WrongCaretPosition()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach (var a in array)
        {
            [||] 
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WrongCaretPosition1()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        [||] foreach (var a in array)
        {
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task WrongCaretPosition2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach (var a in array) [||] 
        {
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Field()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    int[] array = new int[] { 1, 3, 4 };

    void Method()
    {
        foreach [||] (var a in array)
        {
        }
    }
}
";
            var expected = @"
class Test
{
    int[] array = new int[] { 1, 3, 4 };

    void Method()
    {
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayElement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[][] { new int[] { 1, 3, 4 } };
        foreach [||] (var a in array[0])
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[][] { new int[] { 1, 3, 4 } };
        for (int {|Rename:i|} = 0; i < array[0].Length; i++)
        {
            int a = array[0][i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Parameter()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method(int[] array)
    {
        foreach [||] (var a in array)
        {
        }
    }
}
";
            var expected = @"
class Test
{
    void Method(int[] array)
    {
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Property()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    int [] Prop { get; } = new int[] { 1, 2, 3 };

    void Method()
    {
        foreach [||] (var a in Prop)
        {
        }
    }
}
";
            var expected = @"
class Test
{
    int [] Prop { get; } = new int[] { 1, 2, 3 };

    void Method()
    {
        for (int {|Rename:i|} = 0; i < Prop.Length; i++)
        {
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Interface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var array = (IList<int>)(new int[] { 1, 3, 4 });
        foreach[||] (var a in array)
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var array = (IList<int>)(new int[] { 1, 3, 4 });
        for (int {|Rename:i|} = 0; i < array.Count; i++)
        {
            int a = array[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IListOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new List<int>();
        foreach [||](var a in list)
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new List<int>();
        for (int {|Rename:i|} = 0; i < list.Count; i++)
        {
            int a = list[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IReadOnlyListOfT()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new ReadOnly<int>();
        foreach [||](var a in list)
        {
            Console.WriteLine(a);
        }

    }
}

class ReadOnly<T> : IReadOnlyList<T>
{
    public T this[int index] => throw new System.NotImplementedException();
    public int Count => throw new System.NotImplementedException();
    public IEnumerator<T> GetEnumerator() => throw new System.NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new System.NotImplementedException();
}
";
            var expected = @"
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new ReadOnly<int>();
        for (int {|Rename:i|} = 0; i < list.Count; i++)
        {
            int a = list[i];
            Console.WriteLine(a);
        }

    }
}

class ReadOnly<T> : IReadOnlyList<T>
{
    public T this[int index] => throw new System.NotImplementedException();
    public int Count => throw new System.NotImplementedException();
    public IEnumerator<T> GetEnumerator() => throw new System.NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new System.NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IList()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;

class Test
{
    void Method()
    {
        var list = new List();
        foreach [||](var a in list)
        {
            Console.WriteLine(a);
        }

    }
}

class List : IList
{
    public object this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsReadOnly => throw new NotImplementedException();
    public bool IsFixedSize => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public object SyncRoot => throw new NotImplementedException();
    public bool IsSynchronized => throw new NotImplementedException();
    public int Add(object value) => throw new NotImplementedException();
    public void Clear() => throw new NotImplementedException();
    public bool Contains(object value) => throw new NotImplementedException();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();
    public IEnumerator GetEnumerator() => throw new NotImplementedException();
    public int IndexOf(object value) => throw new NotImplementedException();
    public void Insert(int index, object value) => throw new NotImplementedException();
    public void Remove(object value) => throw new NotImplementedException();
    public void RemoveAt(int index) => throw new NotImplementedException();
}
";
            var expected = @"
using System;
using System.Collections;

class Test
{
    void Method()
    {
        var list = new List();
        for (int {|Rename:i|} = 0; i < list.Count; i++)
        {
            object a = list[i];
            Console.WriteLine(a);
        }

    }
}

class List : IList
{
    public object this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool IsReadOnly => throw new NotImplementedException();
    public bool IsFixedSize => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public object SyncRoot => throw new NotImplementedException();
    public bool IsSynchronized => throw new NotImplementedException();
    public int Add(object value) => throw new NotImplementedException();
    public void Clear() => throw new NotImplementedException();
    public bool Contains(object value) => throw new NotImplementedException();
    public void CopyTo(Array array, int index) => throw new NotImplementedException();
    public IEnumerator GetEnumerator() => throw new NotImplementedException();
    public int IndexOf(object value) => throw new NotImplementedException();
    public void Insert(int index, object value) => throw new NotImplementedException();
    public void Remove(object value) => throw new NotImplementedException();
    public void RemoveAt(int index) => throw new NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ImmutableArray()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
<Workspace>
    <Project Language=""C#"" AssemblyName=""Assembly1"" CommonReferences=""true"" CommonReferenceFacadeSystemRuntime = ""true"">
    <MetadataReference>" + typeof(ImmutableArray<>).Assembly.Location + @"</MetadataReference>
        <Document>
using System;
using System.Collections.Immutable;

class Test
{
    void Method()
    {
        var list = ImmutableArray.Create(1);
        foreach [||](var a in list)
        {
            Console.WriteLine(a);
        }
    }
}</Document>
    </Project>
</Workspace>";

            var expected = @"
using System;
using System.Collections.Immutable;

class Test
{
    void Method()
    {
        var list = ImmutableArray.Create(1);
        for (int {|Rename:i|} = 0; i < list.Length; i++)
        {
            int a = list[i];
            Console.WriteLine(a);
        }
    }
}";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ExplicitInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        foreach [||] (var a in list)
        {
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
";

            var expected = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        IReadOnlyList<int> {|Rename:list1|} = list;
        for (int {|Rename:i|} = 0; i < list1.Count; i++)
        {
            int a = list1[i];
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoubleExplicitInterface()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        foreach [||] (var a in list)
        {
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>, IReadOnlyList<string>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task DoubleExplicitInterfaceWithExplicitType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        foreach [||] (int a in list)
        {
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>, IReadOnlyList<string>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";

            var expected = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        IReadOnlyList<int> {|Rename:list1|} = list;
        for (int {|Rename:i|} = 0; i < list1.Count; i++)
        {
            int a = list1[i];
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>, IReadOnlyList<string>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MixedInterfaceImplementation()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Mixed();
        foreach [||] (var a in list)
        {
            Console.WriteLine(a);
        }
    }
}

class Mixed : IReadOnlyList<int>, IReadOnlyList<string>
{
    public int this[int index] => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";

            var expected = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Mixed();
        for (int {|Rename:i|} = 0; i < list.Count; i++)
        {
            int a = list[i];
            Console.WriteLine(a);
        }
    }
}

class Mixed : IReadOnlyList<int>, IReadOnlyList<string>
{
    public int this[int index] => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task MixedInterfaceImplementationWithExplicitType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Mixed();
        foreach [||] (string a in list)
        {
            Console.WriteLine(a);
        }
    }
}

class Mixed : IReadOnlyList<int>, IReadOnlyList<string>
{
    public int this[int index] => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";

            var expected = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Mixed();
        IReadOnlyList<string> {|Rename:list1|} = list;
        for (int {|Rename:i|} = 0; i < list1.Count; i++)
        {
            string a = list1[i];
            Console.WriteLine(a);
        }
    }
}

class Mixed : IReadOnlyList<int>, IReadOnlyList<string>
{
    public int this[int index] => throw new NotImplementedException();
    public int Count => throw new NotImplementedException();
    public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
    IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task PreserveUserExpression()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

namespace NS
{
    class Test
    {
        void Method()
        {
            foreach [||] (string a in new NS.Mixed())
            {
                Console.WriteLine(a);
            }
        }
    }

    class Mixed : IReadOnlyList<int>, IReadOnlyList<string>
    {
        public int this[int index] => throw new NotImplementedException();
        public int Count => throw new NotImplementedException();
        public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

        string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
        int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
        IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
    }
}
";

            var expected = @"
using System;
using System.Collections;
using System.Collections.Generic;

namespace NS
{
    class Test
    {
        void Method()
        {
            IReadOnlyList<string> {|Rename:list|} = new NS.Mixed();
            for (int {|Rename:i|} = 0; i < list.Count; i++)
            {
                string a = list[i];
                Console.WriteLine(a);
            }
        }
    }

    class Mixed : IReadOnlyList<int>, IReadOnlyList<string>
    {
        public int this[int index] => throw new NotImplementedException();
        public int Count => throw new NotImplementedException();
        public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

        string IReadOnlyList<string>.this[int index] => throw new NotImplementedException();
        int IReadOnlyCollection<string>.Count => throw new NotImplementedException();
        IEnumerator<string> IEnumerable<string>.GetEnumerator() => throw new NotImplementedException();
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbededStatement()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        if (true)
            foreach [||] (var a in new int[] {});
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task EmbededStatement2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        if (true)
            foreach [||] (var a in array) Console.WriteLine(a);
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        if (true)
            for (int {|Rename:i|} = 0; i < array.Length; i++)
            {
                int a = array[i];
                Console.WriteLine(a);
            }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task IndexConflict2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (var i in array)
        {
            Console.WriteLine(i);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i1|} = 0; i1 < array.Length; i1++)
        {
            int i = array[i1];
            Console.WriteLine(i);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task UseTypeAsUsedInForeach()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        foreach [||] (int a in array)
        {
            Console.WriteLine(i);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 3, 4 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            int a = array[i];
            Console.WriteLine(i);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task String()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        foreach [||] (var a in ""test"")
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        string {|Rename:str|} = ""test"";
        for (int {|Rename:i|} = 0; i < str.Length; i++)
        {
            char a = str[i];
            Console.WriteLine(a);
        }
    }
}
";

            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StringLocalConst()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        const string test = ""test"";
        foreach [||] (var a in test)
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        const string test = ""test"";
        for (int {|Rename:i|} = 0; i < test.Length; i++)
        {
            char a = test[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StringConst()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    const string test = ""test"";

    void Method()
    {
        foreach [||] (var a in test)
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    const string test = ""test"";

    void Method()
    {
        for (int {|Rename:i|} = 0; i < test.Length; i++)
        {
            char a = test[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ElementExplicitCast()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new object[] { 1, 2, 3 };
        foreach [||] (string a in array)
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        var array = new object[] { 1, 2, 3 };
        for (int {|Rename:i|} = 0; i < array.Length; i++)
        {
            string a = (string)array[i];
            Console.WriteLine(a);
        }
    }
}
";
            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task NotAssignable()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 2, 3 };
        foreach [||] (string a in array)
        {
            Console.WriteLine(a);
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ElementMissing()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        var array = new int[] { 1, 2, 3 };
        foreach [||] (in array)
        {
            Console.WriteLine(a);
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ElementMissing2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        foreach [||] (string a in )
        {
            Console.WriteLine(a);
        }
    }
}
";
            await TestMissingInRegularAndScriptAsync(text);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task StringExplicitType()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        foreach [||] (int a in ""test"")
        {
            Console.WriteLine(a);
        }
    }
}
";
            var expected = @"
class Test
{
    void Method()
    {
        string {|Rename:str|} = ""test"";
        for (int {|Rename:i|} = 0; i < str.Length; i++)
        {
            int a = str[i];
            Console.WriteLine(a);
        }
    }
}
";

            await TestInRegularAndScriptAsync(text, expected);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task Var()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        foreach [||] (var a in list)
        {
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
";

            var expected = @"
using System;
using System.Collections;
using System.Collections.Generic;

class Test
{
    void Method()
    {
        var list = new Explicit();
        var {|Rename:list1|} = (IReadOnlyList<int>)list;
        for (var {|Rename:i|} = 0; i < list1.Count; i++)
        {
            var a = list1[i];
            Console.WriteLine(a);
        }
    }
}

class Explicit : IReadOnlyList<int>
{
    int IReadOnlyList<int>.this[int index] => throw new NotImplementedException();
    int IReadOnlyCollection<int>.Count => throw new NotImplementedException();
    IEnumerator<int> IEnumerable<int>.GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
";
            await TestInRegularAndScriptAsync(text, expected, options: ImplicitTypeEverywhere);
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsConvertForEachToFor)]
#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task ArrayRank2()
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
        {
            var text = @"
class Test
{
    void Method()
    {
        foreach [||] (int a in new int[,] { {1, 2} })
        {
            Console.WriteLine(a);
        }
    }
}
";
            await TestMissingAsync(text);
        }
    }
}
