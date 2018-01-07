# Table of Contents

<!-- TOC -->

- [Table of Contents](#table-of-contents)
- [The Spiral Language](#the-spiral-language)
    - [Overview](#overview)
        - [Intro](#intro)
        - [Design Philosophy](#design-philosophy)
    - [Dependencies](#dependencies)
                - [For the compiler:](#for-the-compiler)
                - [For the Cuda using Spiral libraries:](#for-the-cuda-using-spiral-libraries)
    - [Tutorials: Introduction to Spiral](#tutorials-introduction-to-spiral)
        - [0: The way to use the language](#0-the-way-to-use-the-language)
        - [1: Inlineables, Methods and Join Points](#1-inlineables-methods-and-join-points)
            - [Recursion, Destructuring and Pattern Matching](#recursion-destructuring-and-pattern-matching)
                - [Intensional Recursion](#intensional-recursion)
                - [Term Casting of Functions](#term-casting-of-functions)
                - [`<function>` error message](#function-error-message)
        - [2: Modules, Macros and Interop](#2-modules-macros-and-interop)
            - [Modules](#modules)
            - [Macros](#macros)
                - [Solve Me](#solve-me)
                - [Simple Array Sum (macro version)](#simple-array-sum-macro-version)
            - [Spiral libraries](#spiral-libraries)
        - [3: Loops and Arrays](#3-loops-and-arrays)
            - [Loops](#loops)
            - [Arrays](#arrays)
        - [3: Union Types and Lists](#3-union-types-and-lists)
            - [Type Splitting and Generic Parameters](#type-splitting-and-generic-parameters)
            - [Warning on combining union types, partial active patterns and join points](#warning-on-combining-union-types-partial-active-patterns-and-join-points)
        - [4: Continuation Passing Style, Monadic Computation and Parsing](#4-continuation-passing-style-monadic-computation-and-parsing)
            - [Parsing Benchmark](#parsing-benchmark)
        - [5: Tensors and Structural Reflection](#5-tensors-and-structural-reflection)
            - [Under the Hood](#under-the-hood)
                - [Layout Polymorphism](#layout-polymorphism)
                - [Dimensionality Polymorphism](#dimensionality-polymorphism)
                    - [Design of the Tensor](#design-of-the-tensor)
                    - [The Tensor Facade](#the-tensor-facade)
            - [Closing Comments](#closing-comments)
        - [6: The Cuda Backend (Sneak Peek)](#6-the-cuda-backend-sneak-peek)
    - [User Guide: The Spiral Power](#user-guide-the-spiral-power)
        - [1: Data Structures, Abstraction and Destructuring](#1-data-structures-abstraction-and-destructuring)
        - [2: Let Insertion and Common Subexpression Elimination](#2-let-insertion-and-common-subexpression-elimination)
        - [3: The If Statement](#3-the-if-statement)
            - [Raising Type Errors](#raising-type-errors)
        - [4: Boxing of Union Types](#4-boxing-of-union-types)
        - [5: Unboxing of Union Types](#5-unboxing-of-union-types)
        - [6: Join Points](#6-join-points)
        - [7: The Prepass and Unused Variable Filtering](#7-the-prepass-and-unused-variable-filtering)
        - [8: Pattern Compilation](#8-pattern-compilation)
            - [Type Equality Pattern](#type-equality-pattern)
            - [Tuples](#tuples)
            - [Active Patterns](#active-patterns)
            - [Boolean Patterns](#boolean-patterns)
            - [Literal Patterns](#literal-patterns)
            - [Module Patterns](#module-patterns)
            - [When Pattern](#when-pattern)
            - [Term Function Type Pattern](#term-function-type-pattern)
            - [The Pos Pattern](#the-pos-pattern)
            - [A Note On Case](#a-note-on-case)
        - [9: Layout Types](#9-layout-types)
        - [10: Macros](#10-macros)
            - [Parser Macros](#parser-macros)
        - [11: Operatives and the Core Library](#11-operatives-and-the-core-library)
            - [Virtualization](#virtualization)

<!-- /TOC -->

# The Spiral Language

## Overview

### Intro

As the world inexorably hurls towards the black maw of tomorrow, the power to face it is needed.

Throughout the history of programming languages, the choice was between fast or expressive; the two traditions are crystallized by the C and the Lisp family of languages. There has been a lot of effort into this, but always as languages developed and moved forward they stepped away from the bare metal and in turn lost some of that core vitality that is needed for performance.

The culprit for this is the heap allocation by default dogma introduced by Lisp decades ago. It is a clutch for languages with weak type systems.

Abstraction by heap allocation is a dead end. It works moderately well on the current generation of computers where CPU is still the dominant driver of computation.

It cannot work for devices like GPUs and the rest coming down the line. Many of the most important computational devices of the future won't support heap allocation so an alternative is needed to draw out their full power. It is of absolute importance that a language for that task have excellent control over inlining. Inlining therefore must comes as guarantee in the language and be a part of the type system.

Inlining is a trade-off that expresses the exchange of memory for computation. It should be the default instead of heap allocating.

A language good enough at propagating information so as to be capable of expressing inlining guarantees is also powerful enough for expressing a lot of other things well - without any abstraction overhead.

1) First class types.
2) Structural reflection through pattern matching.
3) Interoperability between different languages (such as F# and Cuda.)
4) First class functions.
5) Tuples as heterogeneous lists.
6) First class modules.
7) First class layouts of data structures.

Spiral is such a language.

Statically typed and with a lightweight, very powerful type system giving it expressiveness of dynamic languages and the speed of C, Spiral is the crystallization of staged functional programming. It boasts of having intensional polymorphism and first class staging. It was made for the sake of making a deep learning library which was too difficult to do in F# itself for its author.

### Design Philosophy

Automatically doing type inference, inlining and other optimizations requires restrictions and heuristics in order to ensure termination. Languages that make the choice of automating the important parts of their internals invariably hamstring their expressiveness. Even if they end up doing well on some low level benchmarks, they perform poorly when high level abstractions are required.

Spiral is different. The power of Spiral lies in its novel kind of language design, not compiler smartness. Spiral is a static language without any restrictions on either type inference or optimizations.

It introduces novel constructs such as inlineables and join points, exposes optimizations as polymorphism in its type system, and then ties them together and with the rest of the features in such a manner so as to allow programs to be written so they terminate.

The halting problem is the primary obstacle preventing the bridging of expressiveness and performance. Ultimately, C is a competing style more than it is a competing language. It is something other higher level languages regress to once they start worrying about performance.

In Spiral, inlining by hand will never be necessary.

In Spiral, the most abstract way of writing a program is also the optimal one from a performance standpoint.

Spiral exists to abstract away optimization.

## Dependencies

##### For the compiler:

FParsec

Visual Studio 2017 F# template (.NET desktop development)

##### For the Cuda using Spiral libraries:

ManagedCuda 8.0 + (CUBLAS,CURAND)

Cuda SDK 8.0 + 9.0 (8.0 for the libraries and 9.0 for the NVCC compiler)

The Cuda Unbound library

Visual Studio 2017 C++ tools individual component (VC++ 2017 v141 toolset (x86,x64))

## Tutorials: Introduction to Spiral

### 0: The way to use the language

The easiest way to do it right now would be to clone this repo. In the Testing project, look at `run.fs`. It has the latest example used for the tutorial. Select the `Testing` project as the starter one and point the output to the `output.fs` file in the `Temporary` project. No need to worry about getting it wrong - at worst an exception will be raised.

Modifying the Cuda configuration options in the `run.fs` file unless usage of libraries related to that is required.

### 1: Inlineables, Methods and Join Points

Spiral has great many similarities to other languages of the ML family, most notably F# with whom it shares the most similarity and a great deal of syntax, but in terms of semantics, it is different at its core.

```
inl x = 2 // Define a 64-bit integer in Spiral.
```
```
let x = 2 // Define a 32-bit integer in F#.
```

Unlike in F#, statements and function definitions in Spiral are preceded by `inl` instead of `let` which is short for `inl`ineable.

```
let x = 2
()
```

If a program like the above was disassembled, `x` would compile down to a public method in F#. In Spiral in contrast, there would be nothing.

```
module SpiralExample.Main
let cuda_kernels = """

extern "C" {
    
}
"""
```

It is not absolutely nothing though. If the program used Cuda kernels in it, they would gathered at the top of the file inside the `cuda_kernels` variable. For interests of brevity, the unremarkable top part will be cut out during the tutorials.

```
inl x = dyn 2
```
```
let (var_0: int64) = 2L // Generated F# code.
```

The reason Spiral is generating nothing is because `2` as defined is a literal and gets tracked as such by the partial evaluator inside the environment. In order to have it appear in the generated code, it is necessary to cast it from the type to the term level using `dyn`amize function. From here on out, the literal will be bound to a variable and the binding `x` will track `var_0` instead.

Being able to do this is useful for various reasons. For example, without it constructs such as runtime loops would be impossible to write in Spiral because the partial evaluator would diverge. Despite its static typing features, the language would essentially be constrained to being an interpreter for a pure dynamic functional language.

```
inl x = dyn 2
inl y = 3
(x + y) * (x + y)
```

```
let (var_0: int64) = 2L
let (var_1: int64) = (var_0 + 3L)
(var_1 * var_1)
```

To get a sense of how `dyn` works, here is a slightly more complex example. The evaluator does common subexpression elimination in the local scope and so it is smart enough to optimize the `x+y` into a single addition and a multiplication.

```
inl x = 2
inl y = 3
(x + y) * (x + y)
```

```
25L
```

Without `dyn`, all the arithmetic operations get evaluated at compile time. This is due to the simple fact that a variable added to a literal is a variable. In general if an operation has a variable as one of its inputs, then its output will also be a variable. The evaluator term casts the literals when necessary. For an operation to be evaluated at compile time, the partial evaluator must have support for it internally.

`inl` can also be used to define functions.

```
inl add a b = a + b
add 1 2, add 3 4
```

```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
Tuple0(3L, 7L)
```

Tuples are used in Spiral much in the same way as in other functional languages. As per their name, `inl`ineables are always inlined. There are no heuristics at play here in the evaluator. Spiral's staged functions are tracked at the type level and both their exact body and environments are known at every point of compilation. This is an absolute guarantee and there is no point at which they can be automatically converted into heap allocated closures.

```
inl add a b = a + b
inl f = add 1
f 2, f 3
```

```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
Tuple0(3L, 4L)
```

Of course, they can be partially applied.

Besides being staged, Spiral's functions allow more powerful forms of polymorphism than F#'s.

```
inl mult a b = a * b
inl f g = g 1 2, g 3.0 4.0 // Would give a type error in F#.
f mult
```

```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
Tuple0(2L, 12.000000)
```

Haskell could manage something like that using typeclasses and higher ranked types, but it would be nowhere as simple. Having to declare new types and then putting in the necessary annotations into the function would be needed.

On the other hand, being able to do this makes type inference for Spiral undecidable.

```
inl f _ = 1 + "qwe" // Does not give a type error
()
```

Undecidability manifests in Spiral like so - the body of the function is not evaluated until it is applied. That means that type errors can lurk in functions that are unused.

That having said, Spiral is a statically typed language and any type errors for code on the execution path will get reported at compile time along with the trace for it.

```
inl f _ = 1 + "qwe" // Does not give a type error
inl _ = 1
inl _ = 2
inl _ = 3
f ()
```

```
Error trace on line: 3, column: 5 in file "example1".
inl _ = 1
    ^
Error trace on line: 4, column: 5 in file "example1".
inl _ = 2
    ^
Error trace on line: 5, column: 5 in file "example1".
inl _ = 3
    ^
Error trace on line: 6, column: 1 in file "example1".
f ()
^
Error trace on line: 2, column: 7 in file "example1".
inl f _ = 1 + "qwe" // Does not give a type error
      ^
Error trace on line: 51, column: 11 in file "Core".
inl (+) a b = !Add(a,b)
          ^
`is_numeric a && get_type a = get_type b` is false.
a=lit 1i64, b=lit qwe
```

Since in order to achieve code reuse methods are necessary, Spiral makes it possible to make use of them with the `met` keyword. It works much like `inl`.

```
inl mult a b = a * b
met f g = g 1 2, g 3.0 4.0
f mult
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0(): Tuple0 =
    Tuple0(2L, 12.000000)
method_0()
```

`mult` can also be defined using `met` and passed into the other function.

```
met mult a b = a * b
met f g = g 1 2, g 3.0 4.0
f mult
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0(): Tuple0 =
    let (var_0: int64) = method_1()
    let (var_1: float) = method_2()
    Tuple0(var_0, var_1)
and method_1(): int64 =
    2L
and method_2(): float =
    12.000000
method_0()
```

The result would not be as one might expect since the methods would get specialized to the literal arguments passed to them. Instead it would be better to use `dyn` here. But rather than letting the caller `f` do the term casting operation, it would be better if the callee `mult` did it.

```
met mult (!dyn a) (!dyn b) = a * b
met f g = g 1 2, g 3.0 4.0
f mult
```

```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0(): Tuple0 =
    let (var_0: int64) = 1L
    let (var_1: int64) = 2L
    let (var_2: int64) = method_1((var_0: int64), (var_1: int64))
    let (var_3: float) = 3.000000
    let (var_4: float) = 4.000000
    let (var_5: float) = method_2((var_3: float), (var_4: float))
    Tuple0(var_2, var_5)
and method_1((var_0: int64), (var_1: int64)): int64 =
    (var_0 * var_1)
and method_2((var_0: float), (var_1: float)): float =
    (var_0 * var_1)
method_0()
```

`!` on the left (the pattern) side of the expression is the active pattern unary operator. It takes a function as its first argument, applies the input to it and rebinds the result to second argument of the pattern (in this case `a` and `b` respectively) before the body is evaluated.

#### Recursion, Destructuring and Pattern Matching

Much like in F#, recursive functions can be defined using `rec`.

```
inl rec foldl f s = function
    | x :: xs -> foldl f (f s x) xs
    | () -> s

inl sum = foldl (+) 0

sum (1,2,3)
```
```
6L
```

In the ML family of languages, `::` is the list cons pattern. In Spiral it is the the tuple cons pattern. Tuple are fully fledged heterogeneous lists in Spiral and can be treated as such.

```
inl a = 2,3
1 :: a
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
Tuple0(1L, 2L, 3L)
```

There are some interesting applications of this. 

```
inl rec foldl f s = function
    | x :: xs -> foldl f (f s x) xs
    | () -> s

met sum (!dyn l) = foldl (+) 0 l

sum (1,2,3), sum (1,2,3,4)
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0((var_0: int64), (var_1: int64), (var_2: int64)): int64 =
    let (var_3: int64) = (var_0 + var_1)
    (var_3 + var_2)
and method_1((var_0: int64), (var_1: int64), (var_2: int64), (var_3: int64)): int64 =
    let (var_4: int64) = (var_0 + var_1)
    let (var_5: int64) = (var_4 + var_2)
    (var_5 + var_3)
let (var_0: int64) = 1L
let (var_1: int64) = 2L
let (var_2: int64) = 3L
let (var_3: int64) = method_0((var_0: int64), (var_1: int64), (var_2: int64))
let (var_4: int64) = 1L
let (var_5: int64) = 2L
let (var_6: int64) = 3L
let (var_7: int64) = 4L
let (var_8: int64) = method_1((var_4: int64), (var_5: int64), (var_6: int64), (var_7: int64))
Tuple0(var_3, var_8)
```

While the language allows variant arguments, Spiral has the ability to specialize methods to their exact arguments and in combination with destructuring to implement variant arguments in a typesafe manner. 

```
inl rec foldl f s = function
    | x :: xs -> foldl f (f s x) xs
    | () -> s

met sum l = foldl (+) 0 l

sum (1,2,3,dyn 4), sum (2,2,3,dyn 4)
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0((var_0: int64)): int64 =
    (6L + var_0)
and method_1((var_0: int64)): int64 =
    (7L + var_0)
let (var_0: int64) = 4L
let (var_1: int64) = method_0((var_0: int64))
let (var_2: int64) = 4L
let (var_3: int64) = method_1((var_2: int64))
Tuple0(var_1, var_3)
```

By default, in Spiral tuples and modules are flattened and have their variables tracked individually. As can be seen in the generated code, when a tuple is passed into a function it is not the actual tuple that is being passed into it, but its arguments instead.

The specialization is exact to the structure, not just the types. If literals are being passed through the method call, the method will get specialized to them.

```
met f _ = 1,2,3
inl x = f ()
0
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
let rec method_0(): Tuple0 =
    Tuple0(1L, 2L, 3L)
let (var_0: Tuple0) = method_0()
let (var_1: int64) = var_0.mem_0
let (var_2: int64) = var_0.mem_1
let (var_3: int64) = var_0.mem_2
0L
```

Tuples themselves are only manifested in the generated code on join point and branch returns. They get destructured right away and tracked by their individual variables on binding and function application. Had the method return not been bound to `x`, it would not have been destructured. This is the desired behavior because otherwise destructuring might block tail call optimizations.

```
met f _ = 1
```
```
inl f _ = join 1
```

The above two code fragments are identical in Spiral. `met` is just syntax sugar for a function with a join point around its body.

Being able to do this is quite powerful as it allows more fine grained control over inlining.

```
inl rec foldl f s = function
    | x :: xs -> foldl f (f s x) xs
    | () -> s

inl rec forall f = function
    | x :: xs -> f x && forall f xs
    | () -> true

inl sum l = 
    if forall lit_is l then foldl (+) 0 l
    else join foldl (+) 0 (dyn l)

sum (1,2,3,4), sum (1,2,3,dyn 4)
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0((var_0: int64)): int64 =
    let (var_1: int64) = 1L
    let (var_2: int64) = 2L
    let (var_3: int64) = 3L
    let (var_4: int64) = (var_1 + var_2)
    let (var_5: int64) = (var_4 + var_3)
    (var_5 + var_0)
let (var_0: int64) = 4L
let (var_1: int64) = method_0((var_0: int64))
Tuple0(10L, var_1)
```
The `lit_is` always resolves at compile time to either `true` or `false` just like other structure testing functions. In combination with `forall` that allows for testing of whether all the arguments of `l` are known at compile time. Then using a static if, the two branches amount to either summing them all at compile time, or term casting them and pushing the work to runtime.

This ensures that the sum function does not get specialized to every arbitrary literal passed into it.

```
if true then 1 else "qwe" // Not a type error.
```
```
if dyn true then 1 else "qwe" // A type error.
```

If statements in Spiral default to evaluating only one branch if their conditional is known at compile time meaning they are static by default. This is the bedrock of its intensional (structural) polymorphism. Under the hood, the patterns get compiled to static if statements which allow it to branch on structures and types.

`if` goes hand in hand with join point specialization.

```
inl default_of = function
    | _: int64 -> 0
    | _: float64 -> 0.0
    | _: string -> ""

default_of 1, default_of 1.0, default_of "qwe"
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    val mem_2: string
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
Tuple0(0L, 0.000000, "")
```
Unlike ML languages which use Hindley-Milner global type inference, Spiral does not infer as much as propagate. A consequence of that besides undecidability is that it knows the exact structure and type of everything at all times. When done on non-union types as done up to now, this sort of branching has no runtime overhead whatsoever and can be readily seen by looking into the generated code.

[`function`](https://stackoverflow.com/questions/1839016/f-explicit-match-vs-function-syntax) is just shorthand for matching on the immediate argument like in F#.

One other thing that is different from F# is that `int64`,`float64` and `string` on the right side of the `:` operators are not type annotations, but standard variables. The types in Spiral are first class much like everything else.

```
inl int64_type = type 1
inl float64_type = type 1.0
inl string_type = type "qwe"

inl default_of = function
    | _: int64_type -> 0
    | _: float64_type -> 0.0
    | _: string_type -> ""

default_of 1, default_of 1.0, default_of "qwe"
```

```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    val mem_2: string
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
Tuple0(0L, 0.000000, "")
```

As can be seen, the two generated code fragments are identical. `:` on the pattern side is the type equality operator. It can be invoked outside the pattern using the `eq_type` function.

`type` is a keyword and like `join` it enters a new scope.

The types themselves can do more than be passed around or be matched on.

```
inl int64_type = type 1
inl float64_type = type 1.0
inl string_type = type "qwe"

inl default_of = function
    | _: int64_type -> 0
    | _: float64_type -> 0.0
    | _: string_type -> ""

inl a = type int64_type + 1
inl b = type float64_type + 1.0
inl c = type string_format "{0}, {1}" (string_type, "rty")

default_of a, default_of b, default_of c
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    val mem_2: string
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
Tuple0(0L, 0.000000, "")
```

The above is just to illustrate that inside the evaluator naked types are treated just like variables of the same type. If they should happen to slip on the term level that would cause an error.

```
int64 + 3
```
```
(naked_type (*int64*) + 3L)
```

These kinds of errors are easier to locate when they are shown in generated code. When they happen it is usually because of a missed argument to a curried function which causes its environment to spill into the generated code. This makes the usual error messages unhelpful, but looking at the error code gives a good indication of what is happening.

##### Intensional Recursion

Spiral in general does not need type annotations. The only exceptions are recursive functions when used in tandem with join points.

```
met rec fact (!dyn x) = if x > 1 then x * fact (x-1) else 1
fact 3
```
```
Process is terminated due to StackOverflowException.
```

The correct way to write the above would be.

```
met rec fact (!dyn x) = 
    if x > 1 then x * fact (x-1) else 1
    : int64 // or alternatively `: x`
fact 3
```
```
let rec method_0((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 > 1L)
    if var_1 then
        let (var_2: int64) = (var_0 - 1L)
        let (var_3: int64) = method_0((var_2: int64))
        (var_0 * var_3)
    else
        1L
let (var_0: int64) = 3L
method_0((var_0: int64))
```

`:` has the lowest precedence of all Spiral's constructs so it will get applied before any of the statements. It does not necessarily have to be put directly into the function. As reminder, on the pattern side `:` is not a type annotation, but a type equality test.

```
inl rec fact x =
    inl body x = if x > 1 then x * fact (x-1) else 1
    if lit_is x then body x
    else join (body x : int64)
fact 3, fact (dyn 3)
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 > 1L)
    if var_1 then
        let (var_2: int64) = (var_0 - 1L)
        let (var_3: int64) = method_0((var_2: int64))
        (var_0 * var_3)
    else
        1L
let (var_0: int64) = 3L
let (var_1: int64) = method_0((var_0: int64))
Tuple0(6L, var_1)
```

It takes some work, but it is not difficult to make functions stage polymorphic in Spiral.

Mutual recursion can also be done using join points.

```
// https://en.wikipedia.org/wiki/Hofstadter_sequence#Hofstadter_Female_and_Male_sequences
inl rec hof x = 
    inl male n = if n > 0 then n - hof.female (hof.male (n-1)) else 0
    inl female n = if n > 0 then n - hof.male (hof.female (n-1)) else 1
    match x with
    | .male (!dyn n) -> join male n : int64
    | .female (!dyn n) -> join female n : int64
hof.male 3
```
```
let rec method_0((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 > 0L)
    if var_1 then
        let (var_2: int64) = (var_0 - 1L)
        let (var_3: int64) = method_0((var_2: int64))
        let (var_4: int64) = method_1((var_3: int64))
        (var_0 - var_4)
    else
        0L
and method_1((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 > 0L)
    if var_1 then
        let (var_2: int64) = (var_0 - 1L)
        let (var_3: int64) = method_1((var_2: int64))
        let (var_4: int64) = method_0((var_3: int64))
        (var_0 - var_4)
    else
        1L
let (var_0: int64) = 3L
method_0((var_0: int64))
```

`.` here is the type literal lift operator. It has special syntax for strings and when used directly next to an expression, it binds more tightly than application similar to how F#'s method access works. It also has its own dedicated pattern as shown above.

```
inl f x = .(x)
inl a = f "asd"
inl b = .asd
eq_type a b
```
```
true
```

It works on any kind of literal, not just strings. Type literals can be converted to ordinary literals as well.

```
inl a = .1
inl b = .2
match a,b with
| .(a), .(b) -> a + b
```
```
3L
```

The difference between type literals and ordinary literals is that type literals will always be erased in generated code and it is impossible to push them at runtime by `dyn`ing them.

```
dyn (.a,"b",.false,true)
```
```
type Tuple0 =
    struct
    val mem_0: string
    val mem_1: bool
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: string) = "b"
let (var_1: bool) = true
Tuple0(var_0, var_1)
```

Using `print_static` can be used to inspect what the evaluator sees at compile time.

```
print_static (dyn (.a,"b",.false,true))
```
```
[type (type_lit (a)), var (string), type (type_lit (false)), var (bool)]
```

All the information in type literals is preserved at all times.

##### Term Casting of Functions

Spiral's functions as flexible as they are have the notable weakness of not being able to emulate recursive datatypes. For that they need to be cast to the term level.

Consider a silly example like the following where the function is used as a counter.

```
met rec loop f (!dyn i) =
    if i < 10 then loop (inl _ -> f() + 1) (i + 1)
    else f()
    : int64
loop (inl _ -> 0) 0
```

This will never compile for the reason that `f` continually expands its environment.

At first it tries to specialize the function for just `inl _ -> 0` -> `int64` -> `int64`. The second specialization it tries is `[inl _ -> 0; inl _ -> f() + 1]` -> `int64` -> `int64`. During the third it is trying to specialize it for `[inl _ -> 0; inl _ -> f() + 1; inl _ -> f() + 1]` -> `int64` -> `int64`. The syntax used here is just for the sake of description. The problem is the it is impossible for the compiler to ever terminate on the above program. The only way to do it would be to cast the function to the term level and track it as a variable.

```
inl rec loop f i =
    inl f, i = term_cast f (), dyn i
    join 
        if i < 10 then loop (inl _ -> f() + 1) (i + 1) else f()
        : int64

loop (inl _ -> 0) 0
```
```
let rec method_0 (): int64 =
    0L
and method_1((var_0: (unit -> int64)), (var_1: int64)): int64 =
    let (var_2: bool) = (var_1 < 10L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_4: (unit -> int64)) = method_2((var_0: (unit -> int64)))
        method_1((var_4: (unit -> int64)), (var_3: int64))
    else
        var_0()
and method_2 ((var_0: (unit -> int64))) (): int64 =
    let (var_1: int64) = var_0()
    (var_1 + 1L)
let (var_0: (unit -> int64)) = method_0
let (var_1: int64) = 0L
method_1((var_0: (unit -> int64)), (var_1: int64))
```

`term_cast` works by taking a function as its first argument and a type as its second. It emulates a function call, gets the return type of the term function from the result of that, and set the input type to the first argument. In the generated code, it flattens the arguments a single tuple level.

Term level functions have their environments hidden and the only information available to the evaluator is its type.

On the Cuda side, term functions are also allowed with the restriction that their environments be empty. Meaning, they cannot capture variables in their lexical scope and can only be used as function pointers. Despite that restriction, they are useful for interop with Cuda libraries.

All the features of Spiral with the exception of heap allocated modules and closures can be used on the Cuda side.

##### `<function>` error message

Don't be fooled by the `<function>` during type errors. As was repeatedly stated, functions are not at all opaque - they are fully transparent to the evaluator. The reason why they get printed like that is simply because they have a tendency to suck everything into the environment. And except for very small examples, trying to print out the raw AST of its body is worthless even for debugging as it is so convoluted.

```
if dyn true then
    inl a,b = dyn (1,2)
    inl _ -> a + b
else
    inl a,b = dyn (3.0,4.0)
    inl _ -> a + b
```
```
if dyn true then
^
Types in branches of If do not match.
Got: <function> and <function>
```

If functions have the same bodies, they can be returned from branches of a dynamic if statement if they also have the same environments.

```
if dyn true then
    inl a,b = dyn (1,2)
    inl _ -> a + b
else
    inl a,b = dyn (3,4)
    inl _ -> a + b
```
```
type Env0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: bool) = true
if var_0 then
    let (var_1: int64) = 1L
    let (var_2: int64) = 2L
    (Env0(var_1, var_2))
else
    let (var_3: int64) = 3L
    let (var_4: int64) = 4L
    (Env0(var_3, var_4))
```

### 2: Modules, Macros and Interop

#### Modules

Owing to Spiral's relatively dynamic nature, modules work much like [records](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/records) do in F# albeit with greatly expanded functionality. Unlike in F#, they do not need to be predefined.

```
inl f m =
    open m
    q + w + e
inl m1 = {q=1; w=2; e=3}
inl m2 = {q=1.0; w=2.0; e=3.0}
f m1, f m2
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: float
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
Tuple0(6L, 6.000000)
```

As per their namesake, they can be opened and passed as arguments. They have their own dedicated patterns.

```
inl f {q w e} = q + w + e
inl q = 1
inl w = 2
inl e = 3
inl m = {q w e}
f m
```
```
6L
```

They allow functional lens updates. Note that in the generated code their fields are ordered by their names.

```
inl f d = {d.data with a = self + 10}
inl a = 1
inl b = 2
inl c = 3
inl m = {data={a b c}}
f m
```
```
type Env0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
and Env1 =
    struct
    val mem_0: Env0
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
(Env1((Env0(11L, 2L, 3L))))
```

Fields can be added to them and removed arbitrarily in an immutable fashion. Using `without` on a non-existing field will not do anything.

```
inl a = 1
inl b = 2
inl c = 3
inl m = {a b c}
{m with d = 4; without a}
```
```
type Env0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
(Env0(2L, 3L, 4L))
```

Like with tuples which are represented by immutable lists in Spiral, the modules in Spiral allow anything immutable maps might do. For example, they can be mapped over(`module_map`), folded(`module_foldl`,``module_fold`), and filtered(`module_filter`). Here is the fold example.

```
inl m = {a=1; b=2; c=3}
module_foldl (inl key state value -> state + value) 0 m
```
```
6L
```

They support more powerful patterns than F# allows on records like not(`!`) and xor(`^`).

```
inl f {!nope (a ^ b)=s} = s
// f {nope=()} // Would trigger a type error
inl m = {a=1; b=2}
// f m // Without trigger a type error
f {m without a}, f {m without b}
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
Tuple0(2L, 1L)
```

Last, but not least, Spiral's modules and functions support several kinds of layouts. By default, like tuples they have a transparent structure whose variables are tracked on an individual basis. Here is the heap layout.

```
{a=1; b=2; c=3} |> dyn |> heap
```
```
type EnvHeap0 =
    {
    mem_0: int64
    mem_1: int64
    mem_2: int64
    }
let (var_0: int64) = 1L
let (var_1: int64) = 2L
let (var_2: int64) = 3L
({mem_0 = (var_0: int64); mem_1 = (var_1: int64); mem_2 = (var_2: int64)} : EnvHeap0)
```

Here are the 5 layouts in order: `indiv`,`heap`,`heapm`,`stack`,`packed_stack`.

```
{a=1; b=2; c=3} |> dyn |> heap |> heapm |> stack |> packed_stack
```
```
type EnvHeap0 =
    {
    mem_0: int64
    mem_1: int64
    mem_2: int64
    }
and EnvHeapMutable1 =
    {
    mutable mem_0: int64
    mutable mem_1: int64
    mutable mem_2: int64
    }
and EnvStack2 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
[<System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential,Pack=1)>]
and EnvPackedStack3 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
let (var_0: int64) = 1L
let (var_1: int64) = 2L
let (var_2: int64) = 3L
let (var_3: EnvHeap0) = ({mem_0 = (var_0: int64); mem_1 = (var_1: int64); mem_2 = (var_2: int64)} : EnvHeap0)
let (var_4: int64) = var_3.mem_0
let (var_5: int64) = var_3.mem_1
let (var_6: int64) = var_3.mem_2
let (var_7: EnvHeapMutable1) = ({mem_0 = (var_4: int64); mem_1 = (var_5: int64); mem_2 = (var_6: int64)} : EnvHeapMutable1)
let (var_8: int64) = var_7.mem_0
let (var_9: int64) = var_7.mem_1
let (var_10: int64) = var_7.mem_2
let (var_11: EnvStack2) = EnvStack2((var_8: int64), (var_9: int64), (var_10: int64))
let (var_12: int64) = var_11.mem_0
let (var_13: int64) = var_11.mem_1
let (var_14: int64) = var_11.mem_2
EnvPackedStack3((var_12: int64), (var_13: int64), (var_14: int64))
```

That is the tour of them, but it does not yet demonstrate their true power. The essence of modules converted to layout types is that they capture scope.

```
inl npc = 
    {
    health = dyn 0
    mana = dyn 0
    max_health = 40
    max_mana = 30
    } |> stack

inl ar = array_create npc 3
ar 0 <- {npc with health = dyn 10; mana = dyn 20}
ar 1 <- {npc with health = dyn 20; mana = dyn 10}
//ar 2 <- {npc with health = dyn 10; mana = dyn 20; max_health = 50} // Gives a type error
()
```
```
type EnvStack0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: int64) = 0L
let (var_1: int64) = 0L
let (var_2: EnvStack0) = EnvStack0((var_0: int64), (var_1: int64))
let (var_3: (EnvStack0 [])) = Array.zeroCreate<EnvStack0> (System.Convert.ToInt32(3L))
let (var_4: int64) = var_2.mem_0
let (var_5: int64) = var_2.mem_1
let (var_6: int64) = 10L
let (var_7: int64) = 20L
let (var_8: EnvStack0) = EnvStack0((var_6: int64), (var_7: int64))
var_3.[int32 0L] <- var_8
let (var_9: int64) = 20L
let (var_10: int64) = 10L
let (var_11: EnvStack0) = EnvStack0((var_9: int64), (var_10: int64))
var_3.[int32 1L] <- var_11
```

In layout types, literals and naked types become a part of the bigger type and are tracked at the type level. The individual variables are flattened and the intermediate structures are erased in the generated code, very similarly to how the arguments are handled at join points.

The `packed_stack` layout is just there in case it might be necessary to pass a tuple over to the Cuda side. In most cases though, it makes more sense to use the default (non)layout and pass them as individual arguments.

`heapm` layout is useful for mutably updating individual fields of a heap allocated module.

Layout types are there in order to allow finer control of the boxed representations of modules and functions. Without `heap` it would be impossible to heap allocate modules directly for example.

#### Macros
##### Solve Me

Modules are beautiful and elegant part of Spiral. Macros are definitely ugly, but they are the only way for Spiral to interop with other languages' libraries and are as such indispensable.

In Spiral they have the interesting property of also acting as types.

So far all the examples given in the tutorial were relatively unmotivated. Macros make it is possible to do IO among other thing which allow the language to be applied to real world problems.

As a very basic demonstration of them, let us start with this HackerRank problem.

```
// https://www.hackerrank.com/challenges/solve-me-first/problem
// The entire code is given, you can just review and submit!
open System

[<EntryPoint>]
let main argv = 
    let a = Console.ReadLine() |> int
    let b = Console.ReadLine() |> int
    printfn "%d" (a+b)
    0 // return an integer exit code
```

The above is the F# solution given directly. It just reads two ints from input, sums them and returns the sum. Doing it in Spiral without the IDE support or even direct language support for .NET constructs make it more complicated.

First the `System.Console` type needs to be defined.

```
//inl console = fs ((.text, "System.Console") :: ())
inl console = fs [text: "System.Console"]
print_static console
```
```
type (dotnet_type (System.Console))
```

What this has done is create the `[text: "System.Console"]` naked type. The type shown in the output is just how it gets printed - the actual type is determined by its body, namely `[text: "System.Console"]`. This is equivalent to `(.text, "System.Console") :: ()`

```
inl a = (.text, "System.Console") :: () |> fs
inl b = [text: "System.Console"] |> fs
eq_type a b
```
```
true
```

`[]` is just syntax sugar for named tuples. It has no extra functionality apart from what is provided by the standard constructs.

Since types are just macros it is possible to make nonsensical types.

```
print_static (fs [text: "1 + 2 + 3"])
```
```
type (dotnet_type (1 + 2 + 3))
```

Hence mistakes with macros will have to be responsibility of the downwards languages. But in the worst they will just lead to a type error.

Unlike in other languages where they are used for abstraction, macros in Spiral are only to be used for interop. They would not be good at all for that anyway given that at most they can print text.

```
inl console = fs [text: "System.Console"]
inl static_method static_type method_name args return_type = 
    macro.fs return_type [
        type: static_type
        text: "."
        text: method_name
        args: args
        ]
inl readline() = static_method console .ReadLine() string
inl a, b = readline(), readline()
()
```
```
let (var_0: string) = System.Console.ReadLine()
let (var_1: string) = System.Console.ReadLine()
```

The above program succinctly captures Spiral's approach to language interop. The facilities used for defining macro-based types and printing them are interwoven with one another. One extra ingredient macros evaluation require over macro type definitions is the return type.

What the `macro.fs` function is doing is printing the macro based on the second argument and returning a variable of the type in the first argument.

Now all the pieces are in place to finish the exercise.

```
inl console = fs [text: "System.Console"]
inl static_method static_type method_name args return_type = 
    macro.fs return_type [
        type: static_type
        text: "."
        text: method_name
        args: args
        ]
inl unop name arg return_type = 
    macro.fs return_type [
        text: name
        text: " "
        arg: arg
        ]
inl readline() = static_method console .ReadLine() string
inl writeline x = static_method console .WriteLine x string
inl int x = unop "int" x int64
inl a, b = readline(), readline()
writeline (int a + int b)
```
```
let (var_0: string) = System.Console.ReadLine()
let (var_1: string) = System.Console.ReadLine()
let (var_2: int32) = int var_0
let (var_3: int32) = int var_1
let (var_4: int32) = (var_2 + var_3)
System.Console.WriteLine(var_4)
```

##### Simple Array Sum (macro version)

This example is to demonstrate how macros can be used to interop with F# libraries which often take in functions as arguments.

The code fragments will be split into two. The first part loads the numbers into a Spiral array, splits them based on the whitespace char and convert them to ints.

```
//https://www.hackerrank.com/challenges/simple-array-sum/problem
inl console = fs [text: "System.Console"]
inl static_method static_type method_name args return_type = 
    macro.fs return_type [
        type: static_type
        text: "."
        text: method_name
        args: args
        ]

inl readline() = static_method console .ReadLine() string
inl writeline x = static_method console .WriteLine x string

inl array t = type (array_create t 0)
inl _, ar = readline(), macro.fs (array int32) [arg: readline(); text: ".Split [|' '|] |> Array.map int"]
```
```
let (var_0: string) = System.Console.ReadLine()
let (var_2: string) = System.Console.ReadLine()
let (var_3: (int32 [])) = var_2.Split [|' '|] |> Array.map int
```

The next part could also be done using macros, but is here to demonstrate an aspect of Spiral's intensional polymorphism.

```
// Converts a type level function to a term level function based on a type.
inl rec closure_of f tys = 
    match tys with
    | x => xs -> term_cast (inl x -> closure_of (f x) xs) x
    | x: f -> f
    | _ -> error_type "The tail of the closure does not correspond to the one being casted to."

inl add a b = a + b
inl add_closure = closure_of add (int32 => int32 => int32)

macro.fs int32 [text: "Array.fold "; arg: add_closure; text: " 0 "; arg: ar]
|> writeline
```
```
let (var_5: (int32 -> (int32 -> int32))) = method_0
let (var_6: int32) = Array.fold var_5 0 var_3
System.Console.WriteLine(var_6)
```

`closure_of` is an expanded version of `term_cast` that instead of converting by applying using the input argument converts a (staged) type level function to a term level using a target type thereby unstaging it. Term level functions have their own dedicated pattern for destructuring their types.

Naked types for them can be constructed with the `=>` operator. The `error_type` raises a type error with the specified message whenever it is evaluated.

What the `closure_of` function does can be better understood by rewriting it to a specific instance with two arguments.

```
inl closure_of_2 f (a' => b' => c') = 
    term_cast (inl a -> term_cast (inl b -> f a b : c') b') a'
closure_of_2 (+) (int32 => int32 => int32)
```
```
let rec method_0 ((var_0: int32)): (int32 -> int32) =
    method_1((var_0: int32))
and method_1 ((var_1: int32)) ((var_0: int32)): int32 =
    (var_1 + var_0)
method_0
```
The original version is just a more generic version of `closure_of_2` that loops over the arguments while both accumulating the results of the application of the closure and term casting it.

That is roughly it with regards to interop. Spiral of course does have its own libraries.

`closure_of` and other macro related functions can be found in the `Extern` module.

#### Spiral libraries

```
let example1 = 
    "example1",[array;console],"Module description.",
    """
open Console
inl _, b = readline(), macro.fs (array int32) [arg: readline(); text: ".Split [|' '|] |> Array.map int"]
Array.foldl (+) (dyn 0i32) b |> writeline
    """
```
The way Spiral is currently meant to be used is as a scripting language inside F#. The module argument is the list in the middle and the `array` and `console` are the modules of the same name respectively.
```
inl Array = ...
inl Console = ...
open Console
inl _, b = readline(), macro.fs (array int32) [arg: readline(); text: ".Split [|' '|] |> Array.map int"]
Array.foldl (+) (dyn 0i32) b |> writeline
```
The above is roughly how the program would be unfolded after parsing, but before typing and partial evaluation. Modules are unfolded in a flattened manner in the sequence they are input. Duplicate modules are ignored.

Much like F#, Spiral imposes a top down ordering of the program and modules cannot refer to each other recursively. If that functionality is required, it can be achieved using join points, but in general it should not be necessary.

This kind of constrained architecture cuts down on circular referencing and encourages purposeful laying out of programs.

Spiral libraries are (to be) covered in depth in the user guide and the reference.

### 3: Loops and Arrays

#### Loops

Most languages make it trivial to write loops and the user does not have to worry about them diverging except at runtime.

Spiral's staging abilities introduce new complexities into the mix. In Spiral, for every loop one writes, it is necessary to keep in mind whether it is intended to run at compile or at runtime.

Making functions stage polymorphic takes even more effort. Furthermore for recursive runtime functions it is easy to forget to put in the type annotation and to dynamize the counter.

For that reason, the bog standard `for` and `while` loops exist as a part of the `Loops` module in Spiral.

This chapter will be on building up the basic loop and then using it to implement the array library functions from first principles.

At this point, apart from union types and the Cuda backend, all the main language features have been introduced albeit shallowly.

This makes it possible to demonstrate how the architecture of a Spiral program differs from those in other languages.

```
let example = 
    "example",[console],"Module description.",
    """
open Console
met rec for {d with from=(!dyn from) to by body} =
    if from <= to then body from; for {d with from=from+by}
    else ()
    : ()

for {from=0; to=5; by=1; body=inl i ->
    string_format "The loop is on iteration {0}" i |> writeline
    }
    """
```
```
let rec method_0((var_0: int64)): unit =
    let (var_1: bool) = (var_0 <= 5L)
    if var_1 then
        let (var_2: string) = System.String.Format("The loop is on iteration {0}",var_0)
        let (var_3: string) = System.String.Format("{0}",var_2)
        System.Console.WriteLine(var_3)
        let (var_4: int64) = (var_0 + 1L)
        method_1((var_4: int64))
    else
        ()
and method_1((var_0: int64)): unit =
    let (var_1: bool) = (var_0 <= 5L)
    if var_1 then
        let (var_2: string) = System.String.Format("The loop is on iteration {0}",var_0)
        let (var_3: string) = System.String.Format("{0}",var_2)
        System.Console.WriteLine(var_3)
        let (var_4: int64) = (var_0 + 1L)
        method_1((var_4: int64))
    else
        ()
let (var_0: int64) = 0L
method_0((var_0: int64))
```
Somewhat inadvertently, the first example become a good lesson in why loops would be desirable as a part of the library. The first example was careful to `dyn` the counter and did not forget the annotation, but for some reason the loop got specialized to two functions one which only got called once.

It is not a compiler bug.

```
met rec for {d with from=(!dyn from) to by body} =
    if from <= to then body from; for {d with from=from+by}
```

The way join points work is that they specialize the call by their arguments. By rewriting the fragment highlighted above to an equivalent form it will be easy to demonstrate what is happening.

```
inl rec for d =
    inl from = dyn d.from
    inl {to by body} = d
    join
        if from <= to then body from; for {d with from=from+by}
        else ()
        : ()
```

What is going on is that `d` - the old one with the `from` field still as literal is getting passed through the join point and causes the redundant specialization to happen.

Here is the way to write the `for` function correctly.

Out of all the mistakes to make in Spiral, accidentally passing old state through the join point is the easiest one to make. With missed return type annotations and such the compiler will diverge and warn the user that way, but but this one has a way of preying on laziness.

In fact, this kind of error can happen in any language that supports records with mutable updates, not just Spiral. Spiral in particular just makes it obvious by looking at the argument count in the generated code.

```
met rec for {from=(!dyn from) to by body} =
    if from <= to then body from; for {from=from+by; to by body}
    else ()
    : ()
```
```
let rec method_0((var_0: int64)): unit =
    let (var_1: bool) = (var_0 <= 5L)
    if var_1 then
        let (var_2: string) = System.String.Format("The loop is on iteration {0}",var_0)
        let (var_3: string) = System.String.Format("{0}",var_2)
        System.Console.WriteLine(var_3)
        let (var_4: int64) = (var_0 + 1L)
        method_0((var_4: int64))
    else
        ()
let (var_0: int64) = 0L
method_0((var_0: int64))
```

The above output is the ideal for this kind of loop. Only the `var_0` varies, the other literals all get passed through the boundary and specialized along with the body.

This is kind of specialization important to do with Cuda kernels as using too many variables in place of literals can cause register spillage into global memory and cause drastic degradations of performance. Spiral makes it easy to keep such data static and propagate it through the program.

In addition, Spiral makes it trivial to this kind of specialization even across language boundaries. Partial evaluation is commonly refereed to as specialization. Staging makes it user directed. And being able to use staging constructs through the the type system as the basis of abstraction rather than being restricted to a second class macro inspired system is what makes Spiral's staging first class. That is a desirable trait as it increases uniformity of the language and with it, its power. It also simplifies its implementation greatly, so it is a good design principle to follow at all times.

More concretely, one of the main motivations for writing Spiral for its author is avoiding having to write unending litanies of wrappers for simple Cuda kernels.

Moving on, here is the static version of the loop.

```
inl rec for {from to by body} =
    if from <= to then body from; for {from=from+by; to by body}
    else ()

for {from=0; to=5; by=1; body=inl i ->
    string_format "The loop is on iteration {0}" i |> writeline
    }
```
```
System.Console.WriteLine("The loop is on iteration 0")
System.Console.WriteLine("The loop is on iteration 1")
System.Console.WriteLine("The loop is on iteration 2")
System.Console.WriteLine("The loop is on iteration 3")
System.Console.WriteLine("The loop is on iteration 4")
System.Console.WriteLine("The loop is on iteration 5")
```

Now what remains is to make the function stage polymorphic.

```
let example = 
    "example",[tuple;console],"Module description.",
    """
open Console
inl rec for {from to by body} =
    inl body from = 
        if from <= to then body from; for {from=from+by; to by body}
        else ()
    if Tuple.forall lit_is (from,to,by) then body from
    else 
        inl from = dyn from
        join body from : ()

for {from=0; to=5; by=1; body=inl i ->
    string_format "The loop is on iteration {0}" i |> writeline
    }

for {from=dyn 0; to=5; by=1; body=inl i ->
    string_format "The loop is on iteration {0}" i |> writeline
    }
    """
```
```
let rec method_0((var_0: int64)): unit =
    let (var_1: bool) = (var_0 <= 5L)
    if var_1 then
        let (var_2: string) = System.String.Format("The loop is on iteration {0}",var_0)
        let (var_3: string) = System.String.Format("{0}",var_2)
        System.Console.WriteLine(var_3)
        let (var_4: int64) = (var_0 + 1L)
        method_0((var_4: int64))
    else
        ()
System.Console.WriteLine("The loop is on iteration 0")
System.Console.WriteLine("The loop is on iteration 1")
System.Console.WriteLine("The loop is on iteration 2")
System.Console.WriteLine("The loop is on iteration 3")
System.Console.WriteLine("The loop is on iteration 4")
System.Console.WriteLine("The loop is on iteration 5")
let (var_0: int64) = 0L
method_0((var_0: int64))
```

The above loop can further improved in terms of functionality. Notice that its body has has a type `unit` which is represented by an empty tuple in Spiral. That is a throwback to C that has no place in modern language such as Spiral.

```
open Console
inl rec for {from to by state body} =
    inl body from = 
        if from <= to then for {to by body from=from+by; state=body {state i=from}}
        else state
    if Tuple.forall lit_is (from,to,by) then body from
    else 
        inl from = dyn from
        join body from : state

inl power a to = for {from=2; to by=1; state=a; body=inl {state} -> state * a}

power 2 3
```
```
8L
```

The above works, but various criticisms of the program could be made. For one, is it really necessary to give `by` every time? Vast majority of loops will in fact have it as `1` so if it is not given it makes sense to use that default instead of giving a type error.

Speaking of defaults, a decent guess would be that most loops are not intended to be unrolled and that a user is more likely to just forget to `dyn` the `from` field by accident.

```
open Console
inl rec for {d with to state body} =
    inl body {from by} = 
        if from <= to then for {to by body from=from+by; state=body {state i=from}}
        else state

    inl from =
        match d with
        | {from} -> dyn from
        | {static_from} -> static_from

    inl by =
        match d with
        | {by} -> by
        | _ -> 1

    if Tuple.forall lit_is (from,to,by) then body {from}
    else 
        inl from = dyn from
        join body {from by} : state

inl power a to = for {from=2; to state=a; body=inl {state} -> state * a}

power 2 3
```
```
let rec method_0((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 <= 3L)
    if var_1 then
        let (var_2: int64) = (var_0 + 1L)
        method_1((var_2: int64))
    else
        2L
and method_1((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 <= 3L)
    if var_1 then
        let (var_2: int64) = (var_0 + 1L)
        method_2((var_2: int64))
    else
        4L
and method_2((var_0: int64)): int64 =
    let (var_1: bool) = (var_0 <= 3L)
    if var_1 then
        let (var_2: int64) = (var_0 + 1L)
        method_3((var_2: int64))
    else
        8L
// ...and so on up to method_63
```

Not quite as planned. An error made now is that the state gets specialized for every different power of 2.

With a single added `dyn` that can be fixed.

```
inl power a to = for {from=2; to state=dyn a; body=inl {state} -> state * a}
```
```
let rec method_0((var_0: int64), (var_1: int64)): int64 =
    let (var_2: bool) = (var_1 <= 3L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_4: int64) = (var_0 * 2L)
        method_0((var_4: int64), (var_3: int64))
    else
        var_0
let (var_0: int64) = 2L
let (var_1: int64) = 2L
method_0((var_0: int64), (var_1: int64))
```

As a matter of convention, Spiral library functions that take in `state` never `dyn` it directly. That responsibility should fall onto the user. 

As an example of the reason for that, the state might be an option type so it might be better to specialize it for both of its states without instantiating it directly. Or it might be a tuple with some fields which would be desirable to remain as literals.

`for` is intended to be used as a primitive and so requires some flexibility; it would not do to block functionality.

Looking over the function now it seems fine, but it is a bit uncomfortable how from has to start from `2`. It is not like the loop has to use the `<=` operator for comparison in the conditional. In a lot of cases `<` make a lot more sense.

Furthermore, an user might want to iterate downwards. That can be accommodated.

```
open Console
inl rec for {d with state body} =
    inl check =
        match d with
        | {near_to} from -> from < near_to 
        | {to} from -> from <= to
        | {down_to} from -> from >= down_to
        | {near_down_to} from -> from > near_down_to

    inl from =
        match d with
        | {from} -> dyn from
        | {static_from} -> static_from

    inl {(to ^ near_to ^ down_to ^ near_down_to)=to} = d

    inl by =
        match d with
        | {by} -> by
        | _ -> 1

    inl rec loop {from state} =
        inl body {from} = 
            if check from then loop {from=from+by; state=body {state i=from}}
            else state

        if Tuple.forall lit_is (from,to,by) then body {from}
        else 
            inl from = dyn from
            join body {from} : state

    loop {from state}

inl power a near_to = for {from=1; near_to state=dyn a; body=inl {state} -> state * a}

power 2 3
```
```
let rec method_0((var_0: int64), (var_1: int64)): int64 =
    let (var_2: bool) = (var_1 < 3L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_4: int64) = (var_0 * 2L)
        method_0((var_4: int64), (var_3: int64))
    else
        var_0
let (var_0: int64) = 2L
let (var_1: int64) = 1L
method_0((var_0: int64), (var_1: int64))
```

The module member queries are all done statically and so maximum polymorphism is attained. The above program also demonstrates why lexical scope is so great.

The above is starting to near the functionality of the `for` function in the actual library. To make it more professional, rather than returning a pattern miss error on when a field is missed, it would be better to tell the user what the problem is.

```
open Console
inl for {d with state body} =
    inl check =
        match d with
        | {near_to} from -> from < near_to 
        | {to} from -> from <= to
        | {down_to} from -> from >= down_to
        | {near_down_to} from -> from > near_down_to
        | _ -> error_type "Only one of `to`,`near_to`,`down_to`,`near_down_to` needs be present."


    inl from =
        match d with
        | {from=(!dyn from) ^ static_from=from} -> from
        | _ -> error_type "Only one of `from` and `static_from` field to loop needs to be present."

    inl to =
        match d with
        | {(to ^ near_to ^ down_to ^ near_down_to)=to} -> to
        | _ -> error_type "Only one of `to`,`near_to`,`down_to`,`near_down_to` is allowed."

    inl by =
        match d with
        | {by} -> by
        | _ -> 1

    inl rec loop {from state} =
        inl body {from} = 
            if check from then loop {from=from+by; state=body {state i=from}}
            else state

        if Tuple.forall lit_is (from,to,by) then body {from}
        else 
            inl from = dyn from
            join body {from} : state

    loop {from state}

inl power a near_to = for {static_from=1; near_to state=a; body=inl {state} -> state * a}

power 2 3
```
```
8L
```

The above design is in fact superior to what is currently in the standard library. A lot of features of the language were developed along with the library and some parts of it did not keep up. The author also got to fancy with the design of it. At the time that was useful for pushing the language, but not so much from a design perspective.

Another issue with the standard library as it stands is that in fact its author did not know how to program in the language he was making and had to learn it as he going along.

```
    inl from =
        match d with
        | {from=(!dyn from) ^ static_from=from} -> from
        | _ -> error_type "Only one of `from` and `static_from` field to loop needs to be present."
```

This part here is highlighted in order to show the xor (`^`) pattern might be used in tandem with active patterns.

All the features in the making of the loop so far have been covered in the previous chapters and now it can be seen how they come together.

It is not done yet.

In order to attain the full functionality of C style loops, Spiral's loops also need the ability to break out. Strictly speaking, this cannot be done in a functional language and having `return` would make even less sense in Spiral than it does in ML variants, but the same functionality can be achieved instead by writing the loop body and calling the continuation for the next iteration in tail position.

As motivating example, imagine trying to iterate over nested arrays trying to find a specific item before breaking out. With the loop as was written above, there is no way to stop before reaching the end.

To start things off, first the nested arrays need to be created.

```
open Console
inl for {d with body} =
    inl state = 
        match d with
        | {state} -> state
        | _ -> ()
/// ...
inl array_init near_to f =
    assert (near_to >= 0) "The input to init needs to be greater or equal to 0."
    // Somewhat of an ugly practice in order to infer the type in a language that doesn't support inference. 
    // For large functions, it is recommended to put them in a join point otherwise compile times could 
    // become exponential if the function contains branches.
    // For a simple map for an array like here, it does not matter.
    inl typ = type (f 0) 
    inl ar = array_create typ near_to
    for {from=0; near_to; body=inl {i} -> ar i <- f i}
    ar

inl rec zeroes = function
    | x :: x' -> array_init x (inl _ -> zeroes x')
    | () -> ""

inl ar = zeroes (4,4,4,4)
ar 0 0 0 2 <- "princess"
```
```
let rec method_3((var_0: ((((string []) []) []) [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_8: (((string []) []) [])) = Array.zeroCreate<((string []) [])> (System.Convert.ToInt32(4L))
        let (var_9: int64) = 0L
        method_2((var_8: (((string []) []) [])), (var_9: int64))
        var_0.[int32 var_1] <- var_8
        method_3((var_0: ((((string []) []) []) [])), (var_3: int64))
    else
        ()
and method_2((var_0: (((string []) []) [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_6: ((string []) [])) = Array.zeroCreate<(string [])> (System.Convert.ToInt32(4L))
        let (var_7: int64) = 0L
        method_1((var_6: ((string []) [])), (var_7: int64))
        var_0.[int32 var_1] <- var_6
        method_2((var_0: (((string []) []) [])), (var_3: int64))
    else
        ()
and method_1((var_0: ((string []) [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_4: (string [])) = Array.zeroCreate<string> (System.Convert.ToInt32(4L))
        let (var_5: int64) = 0L
        method_0((var_4: (string [])), (var_5: int64))
        var_0.[int32 var_1] <- var_4
        method_1((var_0: ((string []) [])), (var_3: int64))
    else
        ()
and method_0((var_0: (string [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        var_0.[int32 var_1] <- ""
        method_0((var_0: (string [])), (var_3: int64))
    else
        ()
let (var_6: ((((string []) []) []) [])) = Array.zeroCreate<(((string []) []) [])> (System.Convert.ToInt32(4L))
let (var_7: int64) = 0L
method_3((var_6: ((((string []) []) []) [])), (var_7: int64))
let (var_8: (((string []) []) [])) = var_6.[int32 0L]
let (var_9: ((string []) [])) = var_8.[int32 0L]
let (var_10: (string [])) = var_9.[int32 0L]
var_10.[int32 2L] <- "princess"
```

First off, the case for `state` that was forgotten is added at the top of the `for` function. As the big comment states inferring the type returned by `f` involves evaluating it twice which might not be a good idea depending on what it is.

The `Lazy` module's only function `lazy` for example puts a join point before evaluating the function because it might otherwise repeat long evaluations and those long evaluation in combination with branching (such as when nesting lazy values) might make compilation time take exponential time.

The `assert` function if its conditional can be statically determined and is true gives a type error at compile time instead of triggering at runtime. Spiral has support for throwing exceptions, but not catching or cleaning up after them, so they are intended to be used only for unrecoverable errors.

As the example shows, nesting loops is straightforward in Spiral. It is a decent bit more elegant than doing it with macros which are the only choice in languages with weaker type systems. In Spiral, type inference and partial evaluation are one.

Its type system is extremely powerful, and yet it does not have parametric polymorphism. Adding parametric polymorphism would significantly increase the complexity of both the language and its implementation, would not make the language any more expressive and would make it a lot harder to integrate the partial evaluator with the type system. This would make the language quite a bit slower.

It is interesting to consider the implication of this - in Lisp languages, its raw AST flavored syntax is there for the reason of supporting its macro meta-programming feature. Maybe a really powerful type system does require the absence of parametricity?

In light of what Spiral can do, it might be worth considering whether the programming language community at large collectively missed a whole evolutionary branch of languages with static typing, but without parametric polymorphism.

If that is not convincing yet, maybe it will be after the tutorials are through.

Here is how to write a breakable version of the `for` function in to take advantage of the continuation passing style.

```
inl for' {d with body} =
    inl finally =
        match d with
        | {finally} -> finally
        | _ -> id

    inl state = 
        match d with
        | {state} -> state
        | _ -> ()

    inl check =
        match d with
        | {near_to} from -> from < near_to 
        | {to} from -> from <= to
        | {down_to} from -> from >= down_to
        | {near_down_to} from -> from > near_down_to
        | _ -> error_type "Only one of `to`,`near_to`,`down_to`,`near_down_to` needs be present."

    inl from =
        match d with
        | {from=(!dyn from) ^ static_from=from} -> from
        | _ -> error_type "Only one of `from` and `static_from` field to loop needs to be present."

    inl to =
        match d with
        | {(to ^ near_to ^ down_to ^ near_down_to)=to} -> to
        | _ -> error_type "Only one of `to`,`near_to`,`down_to`,`near_down_to` is allowed."

    inl by =
        match d with
        | {by} -> by
        | _ -> 1

    inl rec loop {from state} =
        inl body {from} = 
            if check from then 
                inl next state = loop {state from=from+by}
                body {next state i=from}
            else finally state

        if Tuple.forall lit_is (from,to,by) then body {from}
        else 
            inl from = dyn from
            join body {from} : finally state

    loop {from state}
```

There is a significant amount of duplication now that will need to be eliminated. The highlights are the addition of the `finally` field and the parts inside `loop`.

```
            if check from then 
                inl next state = loop {state from=from+by}
                body {next state i=from}
            else finally state
```
Instead of the loop calling itself, it instead passes a function to the body and lets it do it instead.

The `finally` field is useful for resuming the outer loop. It can also be used to set the state to `unit`, which would allow the loop to change states without having to resort to union types.

First a utility function for reversing a tuple is needed. This is standard fare for functional programmers and closely mirrors how one would reverse a list in ML styled languages.

```
// Reverses a tuple
inl tuple_rev = 
    inl rec loop state = function
        | x :: xs -> loop (x :: state) xs
        | () -> state
    loop ()
```

Here is how to apply the breakable for loop function. The goal is to find the coordinates of "princess". The method is generalized to an arbitrary of number of nested arrays.

```
// Correct version
inl rec find_index {next state} = function
    | ar & @array_is _ -> 
        inl body {next i} = find_index {next state=i::state} (ar i)
        for' {from=0; near_to=array_length ar; finally=next; body}
    | "princess" -> tuple_rev state
    | _ -> next ()

find_index {state=(); next = inl _ -> failwith (type (dim)) "The princess is in another castle."} ar
```
`failwith` unlike in F#, requires the return type in Spiral but otherwise functions the same.

The `@` operator on the pattern side is a partial active pattern. Unlike F#'s which expect an option type, what `@` takes in is a function with three arguments in the `inl arg on_fail on_succ -> ...` form. `on_fail` and `on_succ` are to be called in tail position and possibly with join points around them when done so multiple times. They represent pattern failure and pattern success respectively.

Here is a small example.

```
inl f pat = function
    | @pat x -> x
    | _ -> error_type "The pattern failed to trigger."

inl pat x on_fail on_succ =
    match x with
    | x: string | x: int64 -> join on_succ (string_format "{0} joined" x)
    | _ -> on_fail()

f pat "qwe", f pat 123
```
```
type Tuple0 =
    struct
    val mem_0: string
    val mem_1: string
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0(): string =
    "qwe joined"
and method_1(): string =
    "123 joined"
let (var_0: string) = method_0()
let (var_1: string) = method_1()
Tuple0(var_0, var_1)
```

Anything in Spiral can be passed as an argument, and since that includes functions it also applies to partial active patterns.

The output of the compiled program is rather large, but it will be reproduced in bulk as an example this time to show that all the loops are being unfolded correctly into tail recursive functions.

```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    val mem_3: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2, arg_mem_3) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2; mem_3 = arg_mem_3}
    end
let rec method_3((var_0: ((((string []) []) []) [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_8: (((string []) []) [])) = Array.zeroCreate<((string []) [])> (System.Convert.ToInt32(4L))
        let (var_9: int64) = 0L
        method_2((var_8: (((string []) []) [])), (var_9: int64))
        var_0.[int32 var_1] <- var_8
        method_3((var_0: ((((string []) []) []) [])), (var_3: int64))
    else
        ()
and method_4((var_0: ((((string []) []) []) [])), (var_1: int64), (var_2: int64)): Tuple0 =
    let (var_3: bool) = (var_2 < var_1)
    if var_3 then
        let (var_4: (((string []) []) [])) = var_0.[int32 var_2]
        let (var_5: int64) = var_4.LongLength
        let (var_6: int64) = 0L
        method_5((var_4: (((string []) []) [])), (var_2: int64), (var_5: int64), (var_0: ((((string []) []) []) [])), (var_1: int64), (var_6: int64))
    else
        (failwith "The princess is in another castle.")
and method_2((var_0: (((string []) []) [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_6: ((string []) [])) = Array.zeroCreate<(string [])> (System.Convert.ToInt32(4L))
        let (var_7: int64) = 0L
        method_1((var_6: ((string []) [])), (var_7: int64))
        var_0.[int32 var_1] <- var_6
        method_2((var_0: (((string []) []) [])), (var_3: int64))
    else
        ()
and method_5((var_0: (((string []) []) [])), (var_1: int64), (var_2: int64), (var_3: ((((string []) []) []) [])), (var_4: int64), (var_5: int64)): Tuple0 =
    let (var_6: bool) = (var_5 < var_2)
    if var_6 then
        let (var_7: ((string []) [])) = var_0.[int32 var_5]
        let (var_8: int64) = var_7.LongLength
        let (var_9: int64) = 0L
        method_6((var_7: ((string []) [])), (var_5: int64), (var_1: int64), (var_8: int64), (var_0: (((string []) []) [])), (var_2: int64), (var_3: ((((string []) []) []) [])), (var_4: int64), (var_9: int64))
    else
        let (var_11: int64) = (var_1 + 1L)
        method_4((var_3: ((((string []) []) []) [])), (var_4: int64), (var_11: int64))
and method_1((var_0: ((string []) [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        let (var_4: (string [])) = Array.zeroCreate<string> (System.Convert.ToInt32(4L))
        let (var_5: int64) = 0L
        method_0((var_4: (string [])), (var_5: int64))
        var_0.[int32 var_1] <- var_4
        method_1((var_0: ((string []) [])), (var_3: int64))
    else
        ()
and method_6((var_0: ((string []) [])), (var_1: int64), (var_2: int64), (var_3: int64), (var_4: (((string []) []) [])), (var_5: int64), (var_6: ((((string []) []) []) [])), (var_7: int64), (var_8: int64)): Tuple0 =
    let (var_9: bool) = (var_8 < var_3)
    if var_9 then
        let (var_10: (string [])) = var_0.[int32 var_8]
        let (var_11: int64) = var_10.LongLength
        let (var_12: int64) = 0L
        method_7((var_10: (string [])), (var_8: int64), (var_1: int64), (var_2: int64), (var_11: int64), (var_0: ((string []) [])), (var_3: int64), (var_4: (((string []) []) [])), (var_5: int64), (var_6: ((((string []) []) []) [])), (var_7: int64), (var_12: int64))
    else
        let (var_14: int64) = (var_1 + 1L)
        method_5((var_4: (((string []) []) [])), (var_2: int64), (var_5: int64), (var_6: ((((string []) []) []) [])), (var_7: int64), (var_14: int64))
and method_0((var_0: (string [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 4L)
    if var_2 then
        let (var_3: int64) = (var_1 + 1L)
        var_0.[int32 var_1] <- ""
        method_0((var_0: (string [])), (var_3: int64))
    else
        ()
and method_7((var_0: (string [])), (var_1: int64), (var_2: int64), (var_3: int64), (var_4: int64), (var_5: ((string []) [])), (var_6: int64), (var_7: (((string []) []) [])), (var_8: int64), (var_9: ((((string []) []) []) [])), (var_10: int64), (var_11: int64)): Tuple0 =
    let (var_12: bool) = (var_11 < var_4)
    if var_12 then
        let (var_13: string) = var_0.[int32 var_11]
        let (var_14: bool) = (var_13 = "princess")
        if var_14 then
            Tuple0(var_3, var_2, var_1, var_11)
        else
            let (var_15: int64) = (var_11 + 1L)
            method_7((var_0: (string [])), (var_1: int64), (var_2: int64), (var_3: int64), (var_4: int64), (var_5: ((string []) [])), (var_6: int64), (var_7: (((string []) []) [])), (var_8: int64), (var_9: ((((string []) []) []) [])), (var_10: int64), (var_15: int64))
    else
        let (var_18: int64) = (var_1 + 1L)
        method_6((var_5: ((string []) [])), (var_2: int64), (var_3: int64), (var_6: int64), (var_7: (((string []) []) [])), (var_8: int64), (var_9: ((((string []) []) []) [])), (var_10: int64), (var_18: int64))
let (var_6: ((((string []) []) []) [])) = Array.zeroCreate<(((string []) []) [])> (System.Convert.ToInt32(4L))
let (var_7: int64) = 0L
method_3((var_6: ((((string []) []) []) [])), (var_7: int64))
let (var_8: (((string []) []) [])) = var_6.[int32 0L]
let (var_9: ((string []) [])) = var_8.[int32 0L]
let (var_10: (string [])) = var_9.[int32 0L]
var_10.[int32 2L] <- "princess"
let (var_11: int64) = var_6.LongLength
let (var_12: int64) = 0L
method_4((var_6: ((((string []) []) []) [])), (var_11: int64), (var_12: int64))
```

The continuation passing style is the key to a significant amount of abstractive power. It is difficult to understand in terms of what the program does, instead what is needed is to focus on what the program is.

There are numerous ways of writing `find_index` incorrectly that would not get immediately caught by the type system.

1) Forgetting to pass in the array.

```
...
find_index {state=(); next = inl _ -> failwith (type (dim)) "The princess is in another castle."}
```
```
...
method_3((var_6: ((((string []) []) []) [])), (var_7: int64))
let (var_8: (((string []) []) [])) = var_6.[int32 0L]
let (var_9: ((string []) [])) = var_8.[int32 0L]
let (var_10: (string [])) = var_9.[int32 0L]
var_10.[int32 2L] <- "princess"
(Env7((Env4((Env3((Env2((Env1((Env0(naked_type (*bool*))))))))))), (Env3((Env2((Env1((Env0(naked_type (*bool*))))))))), (Env6(Tuple5(4L, 4L, 4L, 4L)))))
```
Seeing a dozen nested `Env`s along with a naked type in the generated code is almost always a sign of forgetting to apply an argument somewhere.

2) Passing the state in incorrectly in the else branch.

```
...
| _ -> next state
```

This would still have it compile and run correctly, but the code would have 40 lines more of useless specializations. It won't be shown here.

3) Passing the state even more incorrectly.

```
inl rec find_index {next state} = function
    | ar & @array_is _ -> 
        inl body {next state i} = find_index {next state=i::state} (ar i)
        for' {from=0; near_to=array_length ar; state finally=next; body}
    | "princess" -> tuple_rev state
    | _ -> next state
```

This would cause it to diverge as it would continually append to `state` inside the loop.

In general though, programs written in a higher order style tend to work well after they typecheck much like in F# despite the language feeling more dynamic. And it is not necessarily the case that Spiral is less typesafe than F#. 

In fact it is the opposite for tasks that require union types due to F# having insufficient polymorphism in its type system. Many tasks that would otherwise require writing an interpreter in other languages can be done at compile time in Spiral.

Loop unrolling is just one of those examples.

Before the section on Loops can be finished there is just one bit of cleaning up left to do. That would be to merge `for` and `for'` into one function. Here is the full example in its completed form with a last minute change to `by`. The new loops are going to go into the standard library.

```
inl for_template kind {d with body} =
    inl finally =
        match d with
        | {finally} -> finally
        | _ -> id

    inl state = 
        match d with
        | {state} -> state
        | _ -> ()

    inl check =
        match d with
        | {near_to} from -> from < near_to 
        | {to} from -> from <= to
        | {down_to} from -> from >= down_to
        | {near_down_to} from -> from > near_down_to
        | _ -> error_type "Only one of `to`,`near_to`,`down_to`,`near_down_to` needs be present."

    inl from =
        match d with
        | {from=(!dyn from) ^ static_from=from} -> from
        | _ -> error_type "Only one of `from` and `static_from` field to loop needs to be present."

    inl to =
        match d with
        | {(to ^ near_to ^ down_to ^ near_down_to)=to} -> to
        | _ -> error_type "Only one of `to`,`near_to`,`down_to`,`near_down_to` is allowed."

    inl by =
        match d with
        | {by} -> by
        | {to | near_to} -> 1
        | {down_to | near_down_to} -> -1

    inl rec loop {from state} =
        inl body {from} = 
            if check from then 
                match kind with
                | .Standard ->
                    loop {from=from+by; state=body {state i=from}}
                | .CPSd ->
                    inl next state = loop {state from=from+by}
                    body {next state i=from}
            else finally state

        if Tuple.forall lit_is (from,to,by) then body {from}
        else 
            inl from = dyn from
            join (body {from} : finally state)

    loop {from state}

inl for' = for_template .CPSd
inl for = for_template .Standard

inl array_init near_to f =
    assert (near_to >= 0) "The input to init needs to be greater or equal to 0."
    // Somewhat of an ugly practice in order to infer the type in a language that doesn't support inference. 
    // For large functions, it is recomended to put them in a join point otherwise compile times could 
    // become exponential if the function contains branches.
    // For a simple map for an array like here, it does not matter.
    inl typ = type (f 0) 
    inl ar = array_create typ near_to
    for {from=0; near_to; body=inl {i} -> ar i <- f i}
    ar

inl rec zeroes = function
    | x :: x' -> array_init x (inl _ -> zeroes x')
    | () -> ""

inl dim = (4,4,4,4)
inl ar = zeroes dim
ar 0 0 0 2 <- "princess"

// Reverses a tuple
inl tuple_rev = 
    inl rec loop state = function
        | x :: xs -> loop (x :: state) xs
        | () -> state
    loop ()

// Correct version
inl rec find_index {next state} = function
    | ar & @array_is _ -> 
        inl body {next i} = find_index {next state=i::state} (ar i)
        for' {from=0; near_to=array_length ar; finally=next; body}
    | "princess" -> tuple_rev state
    | _ -> next ()

find_index {state=(); next = inl _ -> failwith (type (dim)) "The princess is in another castle."} ar
```

There was a lot of material covered here. The logic of `find_index` as well as the other loop unrolling functions might seem confusing to the uninitiated, and would no doubt be to the author had he encountered this over a year ago. But ultimately the function is just 5 lines long and there is nothing particular magical about it; the function is fully explicit. Thinking about it for a long time will help and so will mentally rehearsing the motions until the pieces fall into place.

One useful tool in gaining understanding is trying to manually expand the loop. Here is what happens if `find_index` is expanded a single step of recursion.

```
inl rec find_index {next state} = function
    | ar & @array_is _ -> 
        inl body {next i} = 
            inl state = i :: state
            match ar i with
            | ar & @array_is _ -> 
                inl body {next i} = find_index {next state=i::state} (ar i)
                for' {from=0; near_to=array_length ar; finally=next; body}
            | "princess" -> tuple_rev state
            | _ -> next ()
        for' {from=0; near_to=array_length ar; finally=next; body}
    | "princess" -> tuple_rev state
    | _ -> next ()
```

Supposing the input is one dimensional, that is if the type of the array was `string []` it become possible to do more partial evaluation by hand.

```
inl rec find_index {next state} = function
    | ar & @array_is _ -> 
        inl body {next i} = 
            inl state = i :: state
            match ar i with
            | "princess" -> tuple_rev state
            | _ -> next ()
        for' {from=0; near_to=array_length ar; finally=next; body}
```

This program corresponds to a single loop and is in fact what the program would get specialized to had it been given only a string array as input.

Seeing similar examples of this pattern will no doubt help and there will be a significant number of them throughout these tutorials.

#### Arrays

Compared to the intensity of the previous section, this one should be a breeze in comparison. The most important of the array functions `init` was already introduced. 

```
let array =
    (
    "Array",[tuple;loops],"The array module",
    """
open Loops

/// Creates an empty array with the given type.
/// t -> t array
inl empty t = array_create t 0

/// Creates a singleton array with the given element.
/// x -> t array
inl singleton x =
    inl ar = array_create x 1
    ar 0 <- x
    ar

/// Applies a function to each elements of the collection, threading an accumulator argument through the computation.
/// If the input function is f and the elements are i0..iN then computes f..(f i0 s)..iN.
/// (s -> a -> s) -> s -> a array -> s
inl foldl f state ar = for {from=0; near_to=array_length ar; state; body=inl {state i} -> f state (ar i)}

/// Applies a function to each element of the array, threading an accumulator argument through the computation. 
/// If the input function is f and the elements are i0...iN then computes f i0 (...(f iN s)).
/// (a -> s -> a) -> a array -> s -> s
inl foldr f ar state = for {from=array_length ar-1; down_to=0; state; body=inl {state i} -> f (ar i) state}
...
```

Here are some of the basic ones. Having a loop as a part of the standard library makes it really easy to implement the two `fold` functions. Unlike in F# where `foldl` and `foldr` are `fold` and `foldBack`, here the Haskell naming convention has been followed for no special reason apart from `foldr` being more elegant to write than `foldBack`.

```
// Creates an array given a dimension and a generator function to compute the elements.
// ?(.is_static) -> int -> (int -> a) -> a array
inl init = 
    inl body is_static n f =
        assert (n >= 0) "The input to init needs to be greater or equal to 0."
        inl typ = type (f 0)
        inl ar = array_create typ n
        inl d = 
            inl d = {near_to=n; body=inl {i} -> ar i <- f i}
            if is_static then {d with from = 0} else {d with static_from = 0}
        for d
        ar
    function
    | .static -> body true
    | n -> body false n
```

`init` here is a example how the architecture of a Spiral function differs from that in F#. The interesting part is that `.static` can be passed into it as an optional argument that will allow it to be run statically. Otherwise this is the same as the `init` from the previous section so no demonstration of it should be necessary.

```
/// Builds a new array that contains elements of a given array.
/// a array -> a array
met copy ar = init (array_length ar) ar

/// Builds a new array whose elements are the result of applying a given function to each of the elements of the array.
/// (a -> b) -> a array -> a array
inl map f ar = init (array_length ar) (ar >> f)
```

`init` is useful as it is easy to derive other functions from it. These two function exactly as in F# and other functional languages.

```
/// Returns a new array containing only elements of the array for which the predicate function returns `true`.
/// (a -> bool) -> a array -> a array
inl filter f ar =
    inl ar' = array_create ar.elem_type (array_length ar)
    inl count = foldl (inl s x -> if f x then ar' s <- x; s+1 else s) (dyn 0) ar
    init count ar'
```

Being able to apply arrays directly instead of having to index them allows them to be used more like functions. Also worthy of note is the `ar.elem_type`. In Spiral there is no inference, only propagation so the type of an array must be extracted directly. In Spiral, types are first class and can be used as values. This can be exploited to get around the lack of inference in most cases.

Arrays are a simple examples of how types might be held in structures.

Unlike other .NET types, arrays are built into the language directly.

```
/// Merges all the arrays in a tuple into a single one.
/// a array tuple -> a array
inl append l =
    inl ar' = array_create ((fst l).elem_type) (Tuple.foldl (inl s l -> s + array_length l) 0 l)
    inl ap s ar = foldl (inl i x -> ar' i <- x; i+1) s ar
    Tuple.foldl ap (dyn 0) l |> ignore
    ar'
```

Like how `init` can match on its first arguments before deciding whether to run statically or not, being able to iterate over tuples in order to merge the arrays is a standard use case for intensional polymorphism.

```
/// Flattens an array of arrays into a single one.
/// a array array -> a array
inl concat ar =
    inl count = foldl (inl s ar -> s + array_length ar) (dyn 0) ar
    inl ar' = array_create ar.elem_type.elem_type count
    (foldl << foldl) (inl i x -> ar' i <- x; i+1) (dyn 0) ar |> ignore
    ar'
```

`foldl << foldl` is a good way to compose folds for nested arrays.

Writing functions in this higher order style is the optimal way to program in Spiral. For contrast, here is how `concat` is implemented in F#'s source.

```
let concatArrays (arrs : 'T[][]) : 'T[] =
    let mutable acc = 0    
    for h in arrs do
        acc <- acc + h.Length        
        
    let res = Microsoft.FSharp.Primitives.Basics.Array.zeroCreateUnchecked acc  
        
    let mutable j = 0
    for i = 0 to arrs.Length-1 do     
        let h = arrs.[i]
        let len = h.Length
        Array.Copy(h,0,res,j,len)        
        j <- j + len
    res               
```

It is not quite C, but it is the same style inherited from it. All fast languages tend to regress to that particular kind of programming when performance or just the guarantee of it becomes a necessity.

Even in a pure and lazy language like Haskell, looking under the hood of some of its fast libraries will reveal this and other kinds of regressions.

On the strength of its inlining guarantees, the goal of Spiral is to liberate programmers from that gravitic impulse towards C.

During the last 45 years there have been numerous attempts at bridging the expressiveness of dynamic languages with the performance of C, none of which have borne fruit.

Assuming Spiral can be scaled, it or some other language of similar design with powerful first class types and staging features will finally break beyond the atmosphere to bring light of civilization into the cold, dead space that lies beyond.

The above example is not that bad actually. It is only 12 lines in F# vs 5 in Spiral. It is hardly a reason to create a new language and propose the jettison of parametric polymoprhism.

In the following chapters there will be examples of programs, most notably of Spiral's tensors, whose functionalities have such requirements that would pretty much break any existing language.

The next two functions are all that remains of the module.

```
/// Tests if all the elements of the array satisfy the given predicate.
/// (a -> bool) -> a array -> bool
inl forall f ar = for' {from=0; near_to=array_length ar; state=true; body = inl {next state i} -> f (ar i) && next state}

/// Tests if any the element of the array satisfies the given predicate.
/// (a -> bool) -> a array -> bool
inl exists f ar = for' {from=0; near_to=array_length ar; state=false; body = inl {next state i} -> f (ar i) || next state}

{empty singleton foldl foldr init copy map filter append concat forall exists} 
|> stack
    """) |> module_
```

On the F# side it is necessary to wrap the module in a type using the `module_` function. That `|> stack` at the end is not necessary and only has something to do with the way the language is currently implemented. Omitting the conversion of the module to a layout type would not break anything, at most there might be a minor compilation slowdown. More details are (to be) provided in the user guide.

Modules with no free variables such as the `Array` module whose fields are entirely made of combinators always get converted into naked types rather than variables and hence have no overhead.

### 3: Union Types and Lists

Discriminated union types in Spiral take direct inspiration from F#'s own. That having said, the lack of type inference and the aggressive unboxing of them by the Spiral evaluator makes them less convenient to work with. Nonetheless, union types capture the essence of dynamism and are absolutely essential in a modern language.

Since Spiral has first class types, type string literals take the place of case names. Furthermore, types can be defined anywhere in the program rather than only at the top level like in F#.

A non-recursive union type like the Option can be defined like the following. `\/` is the type union keyword operator. It has a lower precedence than tuples.

```
inl Option x = .Some, x \/ .None

// constructors
inl some x = box (Option x) (.Some, x)
inl none x = box (Option x) (.None)

none int64
```
```
type Union0 =
    | Union0Case0 of Tuple1
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: int64
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
Union0Case1
```

Commentary on the quality of the generated code will be left for the user guide. Pattern matching on the boxed union values can be done the same way as in F#.

```
match none int64 with
| .Some, x -> x
| .None -> -11
```
```
11L
```

The word 'staging' means 'defering for later'. Just like literals, the creation of union types is deferred for as long as possible in Spiral.

In order to actually instantiate the type, it is necessary to `dyn` it or return it from a join point or an if branch. The end of the entire program also qualifies for instantiation.

```
match none int64 |> dyn with
| .Some, x -> x
| .None -> -11
```
```
type Union0 =
    | Union0Case0 of Tuple1
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: int64
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
let (var_0: Union0) = Union0Case1
match var_0 with
| Union0Case0(var_1) ->
    var_1.mem_0
| Union0Case1 ->
    -11L
```

The above is roughly what would be expect to in F# or the MLs. Spiral's pattern matching is more flexible though.

```
inl TypeA = .A \/ .B
inl TypeB = .B \/ .C

inl f = function
    | .A -> 1
    | .B -> 2
    | .C -> 3

box TypeA .A |> dyn |> f |> ignore
box TypeB .C |> dyn |> f |> ignore
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
and Union1 =
    | Union1Case0
    | Union1Case1
let (var_0: Union0) = Union0Case0
let (var_1: int64) =
    match var_0 with
    | Union0Case0 ->
        1L
    | Union0Case1 ->
        2L
let (var_2: Union1) = Union1Case1
let (var_3: int64) =
    match var_2 with
    | Union1Case0 ->
        2L
    | Union1Case1 ->
        3L
```

Despite this added flexibility, it is in fact exhaustive. Unlike in F#, this is not a warning, but an error as Spiral's union types are intended to be used on devices which have no capabilities for raising exceptions.

```
inl TypeA = .A \/ .B

inl f = function
    | .A -> 1
    | .C -> 3

box TypeA .A |> dyn |> f |> ignore
```
```
...
Error trace on line: 35, column: 7 in file "example".
    | .A -> 1
      ^
Error trace on line: 36, column: 7 in file "example".
    | .C -> 3
      ^
Pattern miss error. The argument is type (type_lit (B))
```

As it never matches `.B` it goes over the edge and returns a type error.

Here is how recursive datatypes like lists might be defined.

```
let example = 
    "example",[option;tuple;loops],"Module description.",
    """
open Loops
inl rec List x = join_type () \/ x, List x

/// Creates an empty list with the given type.
/// t -> List t
inl empty x = box (List x) ()

/// Creates a single element list with the given type.
/// x -> List x
inl singleton x = box (List x) (x, empty x)

/// Immutable appends an element to the head of the list.
/// x -> List x -> List x
inl cons a b = 
    inl t = List a
    box t (a, box t b)

singleton 3 |> cons 2 |> cons 1
    """
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
(Rec0Case1(Tuple1(1L, (Rec0Case1(Tuple1(2L, (Rec0Case1(Tuple1(3L, Rec0Case0)))))))))
```

`join_type` is similar to the standard `join` except it is used to define types. It always returns a naked type and on entry converts everything in the environment to their types and that includes literals. That means that passing literals through the type join point requires doing it on the type level or inside a layout type.

```
/// Creates a list by calling the given generator on each index.
/// ?(.static) -> int -> (int -> a) -> List a
inl init =
    inl body is_static n f =
        inl t = type (f 0)
        inl d = {near_to=n; state=empty t; body=inl {next i state} -> cons (f i) (next state)}
        if is_static then for' {d with static_from=0}
        else for' {d with from=0}

    function
    | .static -> body true
    | x -> body false x
```

The above function resembles the `init` in the `Array` module in structure. There is an interesting usage of the breakable `for'` here. Usually the `next` is intended to be called in tail position, but here it is not. Instead the `state` is used merely to ship the empty list to the end of it.

```
inl x = init.static 3 id |> dyn
()
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: Rec0) = (Rec0Case1(Tuple1(0L, (Rec0Case1(Tuple1(1L, (Rec0Case1(Tuple1(2L, Rec0Case0)))))))))
```

The above is nearly identical to the `singleton 3 |> cons 2 |> cons 1` example.

The next function on the list would be the `map`. This is where things start to get tricky. Here is an example of it that does not work.

```
inl rec map f l = 
    inl loop l =
        match l with
        | x,xs -> cons (f x) (map f xs)
        | () -> l // Error #1
        : ??? // Error #2
    if box_is l then loop l
    else join loop l
```

Error #2 should be obvious - there is no return type. Error #1 is more subtle - and is related to the way pattern matching is compiled. 

Backtracking to the earlier example.

```
inl TypeA = .A \/ .B

inl f = function
    | x -> x

box TypeA .A |> dyn |> f |> ignore
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
let (var_0: Union0) = Union0Case0
```

The above is as one would expect.

```
inl TypeA = .A \/ .B

inl f = function
    | "qwe" -> ()
    | x -> x

box TypeA .A |> dyn |> f |> ignore
```
```
...
Error trace on line: 35, column: 7 in file "example".
    | "qwe" -> ()
      ^
All the cases in pattern matching clause with dynamic data must have the same type.
Got: [type_lit (A), type_lit (B)]
```

The error message gives an indication of what is wrong. In Spiral, the match case is not what triggers unboxing - the operations that actually need to unbox the union type are what do it. That means literal, type literal, type equality, tuple and module patterns. This applies even to those patterns that have nothing to do with the variable's type and would have been expected to be skipped.

It gets worse. Spiral is really aggressive at rewriting the terms it is unboxing even if they are outside its intended scope.

```
inl x = box TypeA .A |> dyn
print_static x // var (union {type_lit (A) | type_lit (B)})

match x with
| "qwe" -> ()
| _ -> 
    // prints twice
    // type (type_lit (A))
    // type (type_lit (B))
    print_static x 
    x
```
```
...
All the cases in pattern matching clause with dynamic data must have the same type.
Got: [type_lit (A), type_lit (B)]
```

The way things are currently is the fault of whoever wrote the pattern matching compiler. Since patterns would be difficult to compile otherwise, internally Spiral uses the same mechanism used to do common subexpression elimination to pass information over multiple branches. There is no issue at all with this when not dealing with union types, but here there is some friction there.

There is something good about the current arrangement that MLs do not have.

```
inl TypeA = .A \/ .B \/ .C \/ .D

inl f g = function
    | .A -> 1
    | .B -> 2
    | x -> 
        dyn "Just passing through." |> ignore
        g x

f (function
    | .C -> 3
    | .D -> 4)
    (box TypeA .A |> dyn )
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
    | Union0Case2
    | Union0Case3
let (var_0: Union0) = Union0Case0
match var_0 with
| Union0Case0 ->
    1L
| Union0Case1 ->
    2L
| Union0Case2 ->
    let (var_1: string) = "Just passing through."
    3L
| Union0Case3 ->
    let (var_2: string) = "Just passing through."
    4L
```

That would be that the exhaustiveness check is not local to the pattern. As long as all the branches of it are properly handled, the pattern does not have to be squeezed all into one place and can be composed. This is one of the safety aspects at compile time that F# does not have.

Regardless of the merits and demerits of this approach, in order to complete the map function some kind of method for getting what would be the generic parameter of the list in a parametric language is needed.

#### Type Splitting and Generic Parameters

```
/// Returns the element type of the list.
/// a List -> a type
inl elem_type l =
    match split l with
    | (), (a,b) when eq_type (List a) l -> a
    | _ -> error_type "Expected a List in elem_type."
```

The way the `split` function works is that it splits an union type into its individual components and returns them as a tuple. After it has been split, this makes it possible to match on the types of it directly.

```
inl Option x = .Some, x \/ .None
print_static (Option int64) // type (union {[type_lit (Some), int64] | type_lit (None)})
print_static (Option int64 |> split) // [type ([type_lit (Some), int64]), type (type_lit (None))]
```

This is a bit of a hack. Spiral has union and not sum types, meaning they are not ordered. Or better put, they are ordered, just not based on how the were input.

The above example works for lists and is how they are implemented in the standard library, but there are alternative ways of implementing the basic list.

```
inl rec List x = join_type 
    inl el = stack {elem_type=x}
    el, () \/ el, (x, List x)
```

They all involve sticking the type in directly by using layout types. Since layout types capture the scope by the expression instead of type and since `x` can only ever be a naked type once it passes the `join_type` point, that assures that it will always be instantiated.

If adding `el` to all the branches of a larger type by hand is tedious, it is possible to automate that. It needs to be done inside the type join point. Here is how it would be done on a tuple.

```
inl rec List x = join_type 
    inl el = stack {elem_type=x}
    inl typ = () \/ x, List x
    split typ
    |> Tuple.map (inl x -> el, x)
    |> Tuple.reducel (inl a b -> a \/ b)

// [type ([layout_stack {elem_type=type (int64)}, []]), type ([layout_stack {elem_type=type (int64)}, [int64, rec_type 0]])]
print_static (split (List int64))
```

Using first class types Spiral can emulate what would be generic parameters of a container in a language with parametric polymorphism.

With `elem_type` in hand, it becomes possible to implement map.

```
/// Builds a new list whose elements are the results of applying the given function to each of the elements of the list.
/// (a List -> b List) -> a List -> List b
inl rec map f l = 
    inl t = elem_type l
    inl loop = function
        | x,xs -> cons (f x) (map f xs)
        | () -> empty t
    if box_is l then loop l
    else join loop l : List t

inl l = init.static 3 id |> map ((*) 2) |> dyn
()
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: Rec0) = (Rec0Case1(Tuple1(0L, (Rec0Case1(Tuple1(2L, (Rec0Case1(Tuple1(4L, Rec0Case0)))))))))
```

The static version of map works fine now.

Here is how the non-static version looks like.

```
init 3 id |> map ((*) 2) |> dyn
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_1((var_0: int64)): Rec0 =
    let (var_1: bool) = (var_0 < 3L)
    if var_1 then
        let (var_2: int64) = (var_0 + 1L)
        let (var_3: Rec0) = method_1((var_2: int64))
        (Rec0Case1(Tuple1(var_0, var_3)))
    else
        Rec0Case0
and method_2((var_0: Rec0)): Rec0 =
    match var_0 with
    | Rec0Case0 ->
        Rec0Case0
    | Rec0Case1(var_1) ->
        let (var_2: int64) = var_1.mem_0
        let (var_3: Rec0) = var_1.mem_1
        let (var_4: int64) = (2L * var_2)
        let (var_5: Rec0) = method_2((var_3: Rec0))
        (Rec0Case1(Tuple1(var_4, var_5)))
let (var_0: int64) = 0L
let (var_1: Rec0) = method_1((var_0: int64))
method_2((var_1: Rec0))
```

This does not demonstrate Spiral's true power. The function can map over lists that are partially static.

```
inl l = dyn (singleton 3) |> cons 2 |> cons 1 |> cons 0
map ((*) 2) l |> dyn
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_1((var_0: Rec0)): Rec0 =
    match var_0 with
    | Rec0Case0 ->
        Rec0Case0
    | Rec0Case1(var_1) ->
        let (var_2: int64) = var_1.mem_0
        let (var_3: Rec0) = var_1.mem_1
        let (var_4: int64) = (2L * var_2)
        let (var_5: Rec0) = method_1((var_3: Rec0))
        (Rec0Case1(Tuple1(var_4, var_5)))
let (var_0: Rec0) = (Rec0Case1(Tuple1(3L, Rec0Case0)))
let (var_1: Rec0) = method_1((var_0: Rec0))
(Rec0Case1(Tuple1(0L, (Rec0Case1(Tuple1(2L, (Rec0Case1(Tuple1(4L, var_1)))))))))
```

The first 3 elements are done at compile time, and the rest is done at runtime.

With the map done, `foldl` and `foldr` are straightforward enough.

```
/// Applies a function f to each element of the collection, threading an accumulator argument through the computation. 
/// The fold function takes the second argument, and applies the function f to it and the first element of the list. 
/// Then, it feeds this result into the function f along with the second element, and so on. It returns the final result. 
/// If the input function is f and the elements are i0...iN, then this function computes f (... (f s i0) i1 ...) iN.
/// (s -> a -> s) -> s -> a List -> s
inl rec foldl f s l = 
    inl loop = function
        | x, xs -> foldl f (f s x) xs
        | () -> s
    if box_is l then loop l
    else join loop l : s

inl l = dyn (singleton 3) |> cons 2 |> cons 1 |> cons 0
foldl (+) (dyn 0) l
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_1((var_0: Rec0), (var_1: int64)): int64 =
    match var_0 with
    | Rec0Case0 ->
        var_1
    | Rec0Case1(var_2) ->
        let (var_3: int64) = var_2.mem_0
        let (var_4: Rec0) = var_2.mem_1
        let (var_5: int64) = (var_1 + var_3)
        method_1((var_4: Rec0), (var_5: int64))
let (var_0: Rec0) = (Rec0Case1(Tuple1(3L, Rec0Case0)))
let (var_1: int64) = 0L
let (var_2: int64) = (var_1 + 1L)
let (var_3: int64) = (var_2 + 2L)
method_1((var_0: Rec0), (var_3: int64))
```

One thing the example above demonstrates is that Spiral does require the user to know whether compile time or runtime execution is being targeted. The above fragment is not ideal since it would be better to sum the static part of the list and then `dyn` the state rather than do so at the beginning.

```
/// Applies a function to each element of the collection, threading an accumulator argument through the computation. 
/// If the input function is f and the elements are i0...iN, then this function computes f i0 (...(f iN s)).
/// (a -> s -> s) -> a List -> s -> s
inl rec foldr f l s = 
    inl loop = function
        | x, xs -> f x (foldr f xs s)
        | () -> s
    if box_is l then loop l
    else join loop l : s

inl l = dyn (singleton 3) |> cons 2 |> cons 1 |> cons 0
foldr (+) l (dyn 0)
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_1((var_0: Rec0), (var_1: int64)): int64 =
    match var_0 with
    | Rec0Case0 ->
        var_1
    | Rec0Case1(var_2) ->
        let (var_3: int64) = var_2.mem_0
        let (var_4: Rec0) = var_2.mem_1
        let (var_5: int64) = method_1((var_4: Rec0), (var_1: int64))
        (var_3 + var_5)
let (var_0: Rec0) = (Rec0Case1(Tuple1(3L, Rec0Case0)))
let (var_1: int64) = 0L
let (var_2: int64) = method_1((var_0: Rec0), (var_1: int64))
let (var_3: int64) = (2L + var_2)
(1L + var_3)
```

The next are `head` and `tail`.

```
open Option

/// Returns the first element of the list.
/// a List -> a Option
inl head l =
    inl t = elem_type l
    match l with
    | x, xs -> some x
    | () -> none t

/// Returns the list without the first element.
/// a List -> a List Option
inl tail l =
    inl t = elem_type l
    match l with
    | x, xs -> some xs
    | () -> none (List t)
```

As the above are straightforward so there is no need to run them. That having said, it would be interesting to know how it might be possible to implement them in continuation passing style for greater efficiency.

```
/// Returns the first element of the list.
/// a List -> {some=(a -> a) none=(a type -> a)} -> a
inl head' l {some none} =
    inl t = elem_type l
    match l with
    | x, xs -> some x
    | () -> none t

/// Returns the list without the first element.
/// a List -> {some=(a List -> a List) none=(a List type -> a List)} -> a List
inl tail' l {some none} =
    inl t = elem_type l
    match l with
    | x, xs -> some xs
    | () -> none (List t)

inl l = dyn (singleton 3)
tail' l {
    some = id
    none = inl x -> failwith x "The list is empty."
    }
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: Rec0) = (Rec0Case1(Tuple1(3L, Rec0Case0)))
match var_0 with
| Rec0Case0 ->
    (failwith "The list is empty.")
| Rec0Case1(var_1) ->
    let (var_3: int64) = var_1.mem_0
    var_1.mem_1
```

The best is left for `last`.

```
/// Returns the last element of the list.
/// a List -> a Option
inl last l =
    inl t = elem_type l
    foldl (inl _ x -> some x) (none t) l

inl l = dyn (singleton 3) |> cons 2 |> cons 1 |> cons 0
last l 
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
and Union2 =
    | Union2Case0 of Tuple3
    | Union2Case1
and Tuple3 =
    struct
    val mem_0: int64
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
let rec method_1((var_0: Rec0)): Union2 =
    match var_0 with
    | Rec0Case0 ->
        (Union2Case0(Tuple3(2L)))
    | Rec0Case1(var_1) ->
        let (var_2: int64) = var_1.mem_0
        let (var_3: Rec0) = var_1.mem_1
        method_2((var_3: Rec0), (var_2: int64))
and method_2((var_0: Rec0), (var_1: int64)): Union2 =
    match var_0 with
    | Rec0Case0 ->
        (Union2Case0(Tuple3(var_1)))
    | Rec0Case1(var_2) ->
        let (var_3: int64) = var_2.mem_0
        let (var_4: Rec0) = var_2.mem_1
        method_2((var_4: Rec0), (var_3: int64))
let (var_0: Rec0) = (Rec0Case1(Tuple1(3L, Rec0Case0)))
method_1((var_0: Rec0))
```

The above way of specializing it is close to ideal. It would be better had `method_1` been inlined, but this is a decent showing. As can be seen, the option type is staged and only the int inside is passed through until it is time to return from the function at which point the instantiation happens. In F#, this way of doing `last` would be grossly inefficient as a new option would be instantiated at each step. Very few languages allow passing of literals across call boundaries due to the uncertainty whether the optimizer will diverge. Spiral achieves its efficiency by making dealing with the [halting problem](https://en.wikipedia.org/wiki/Halting_problem) the user's responsibility. This is not a bad strategy - as the halting problem is NP Hard, other compilers' optimizers have no choice but to rely on fallible heuristics whereas the user has to determine whether the program will terminate anyway and has no say in what the black box is deciding. It is not so in Spiral.

The essence of Spiral is to convert the termination proofs implicitly and informally present in the program into polymorphism.

Here is how it would be done in CPS for that last bit of efficiency.

```
/// Returns the last element of the list.
/// a List -> {some=(a -> a) none=(a type -> a)} -> a
inl rec last' l {some none} =
    inl t = elem_type l
    inl loop = function
        | x, xs -> last' xs {some none=some x}
        | () -> none t
    if box_is l then loop l
    else join loop l : none t

inl l = dyn (singleton 3)
last' l {
    some = inl x _ -> x
    none = inl t -> failwith t "The list is empty."
    }
```
```
type Rec0 =
    | Rec0Case0
    | Rec0Case1 of Tuple1
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Rec0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_1((var_0: Rec0)): int64 =
    match var_0 with
    | Rec0Case0 ->
        (failwith "The list is empty.")
    | Rec0Case1(var_1) ->
        let (var_3: int64) = var_1.mem_0
        let (var_4: Rec0) = var_1.mem_1
        method_2((var_4: Rec0), (var_3: int64))
and method_2((var_0: Rec0), (var_1: int64)): int64 =
    match var_0 with
    | Rec0Case0 ->
        var_1
    | Rec0Case1(var_2) ->
        let (var_3: int64) = var_2.mem_0
        let (var_4: Rec0) = var_2.mem_1
        method_2((var_4: Rec0), (var_3: int64))
let (var_0: Rec0) = (Rec0Case1(Tuple1(3L, Rec0Case0)))
method_1((var_0: Rec0))
```

It can no longer be implemented in terms of fold, but otherwise is rather simple.

Note that `head'`, `tail'` and `last'` are just more generic versions of the non-CPS versions. Assuming the 3 original functions were missing, here is how they might be implemented in terms of CPS'd ones.

```
/// Returns the first element of the list.
/// a List -> a Option
inl head l = head' l {some none}

/// Returns the list without the first element.
/// a List -> a List Option
inl tail l = tail' l {some none}

/// Returns the last element of the list.
/// a List -> a Option
inl last l = last' l {some=const << some; none}
```

Here `const` is simply `inl x _ -> x` as was used inside `some` of the previous example. The continuation passing style is great for writing generic code in Spiral as it meshes well with its typing scheme. The monadic computations that will be shown in the following chapters are just syntax sugar over CPS.

The capacity to make specialized functions from generic one like the above is an important factor in ensuring code correctness. Eliminating code duplication and ensuring single responsibility is possible without performance impact in Spiral.

```
/// Returns a new list that contains the elements of the first list followed by elements of the second.
/// a List -> a List -> a List
inl append = foldr cons

/// Returns a new list that contains the elements of each list in order.
/// a List List -> a List
inl concat l & !elem_type !elem_type t = foldr append l (empty t)

{List empty cons init map foldl foldr singleton head' tail' last' head tail last append concat} |> stack
```

With this, the new List module is done.

#### Warning on combining union types, partial active patterns and join points

Union types in Spiral are an example of a well designed feature with some implementation issues. Union types work, partial active patterns work and join points work, but right now they are a pick two out of three kind of deal. The reason for this is related to how Spiral will aggressively rewrite even variables outside of its intended scope.

```
inl ab = box (.A \/ .B)
inl x = dyn (ab .A, ab .A, ab .A)
match x with
| .A, .A, _ -> 1
| .A, .B, .B -> 2
| _, _, .A -> 3
| _ -> 4   
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
let (var_0: Union0) = Union0Case0
let (var_1: Union0) = Union0Case0
let (var_2: Union0) = Union0Case0
match var_0 with
| Union0Case0 ->
    match var_1 with
    | Union0Case0 ->
        1L
    | Union0Case1 ->
        match var_2 with
        | Union0Case0 ->
            3L
        | Union0Case1 ->
            2L
| Union0Case1 ->
    match var_2 with
    | Union0Case0 ->
        3L
    | Union0Case1 ->
        4L
```

The above compiles nicely, but suppose a partial active pattern with a join point is inserted in the middle.

```
inl ab = box (.A \/ .B)
inl x = dyn (ab .A, ab .A, ab .A)
inl pat arg on_fail on_succ = join on_fail ()
match x with
| .A, .A, _ -> 1
| @pat _ -> -1
| .A, .B, .B -> 2
| _, _, .A -> 3
| _ -> 4    
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
let rec method_0((var_0: Union0), (var_1: Union0), (var_2: Union0)): int64 =
    match var_0 with
    | Union0Case0 ->
        match var_1 with
        | Union0Case0 ->
            match var_2 with
            | Union0Case0 ->
                3L
            | Union0Case1 ->
                4L
        | Union0Case1 ->
            match var_2 with
            | Union0Case0 ->
                3L
            | Union0Case1 ->
                2L
    | Union0Case1 ->
        match var_2 with
        | Union0Case0 ->
            3L
        | Union0Case1 ->
            4L
let (var_0: Union0) = Union0Case0
let (var_1: Union0) = Union0Case0
let (var_2: Union0) = Union0Case0
match var_0 with
| Union0Case0 ->
    match var_1 with
    | Union0Case0 ->
        1L
    | Union0Case1 ->
        method_0((var_0: Union0), (var_1: Union0), (var_2: Union0))
| Union0Case1 ->
    method_0((var_0: Union0), (var_1: Union0), (var_2: Union0))
```

What is going on here is that the evaluator is forgetting that it already tested the variables and starts unboxing them again inside the join point. This is because join points throw away local left to right rewrite information.

A workaround would be to put join points in the clause bodies.

```
inl ab = box (.A \/ .B)
inl x = dyn (ab .A, ab .A, ab .A)
inl pat arg on_fail on_succ = on_fail ()
match x with
| .A, .A, _ -> join 1
| @pat _ -> join -1
| .A, .B, .B -> join 2
| _, _, .A -> join 3
| _ -> join 4
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
let rec method_0(): int64 =
    1L
and method_1(): int64 =
    3L
and method_2(): int64 =
    2L
and method_3(): int64 =
    4L
let (var_0: Union0) = Union0Case0
let (var_1: Union0) = Union0Case0
let (var_2: Union0) = Union0Case0
match var_0 with
| Union0Case0 ->
    match var_1 with
    | Union0Case0 ->
        method_0()
    | Union0Case1 ->
        match var_2 with
        | Union0Case0 ->
            method_1()
        | Union0Case1 ->
            method_2()
| Union0Case1 ->
    match var_2 with
    | Union0Case0 ->
        method_1()
    | Union0Case1 ->
        method_3()
```
Now the result is what one might want.

In the future this might not be an issue and in fact, it might just get fixed as a natural process of making the compiler run faster. The way patterns work now is inefficient from a compilation speed perspective and there is room for improvement there. In fact, since pattern matching is so ubiquitous in Spiral, that would be the first thing one would want to optimize in order to speed up compilation.

The way they work now though has the great combination of them being both elegant, highly effective and simple to implement.

It is difficult to imagine what could be added to Spiral for it generate better code for runtime at this point. Spiral's one pass is THE optimization pass to optimize these patterns at runtime, but it does not have any capacity to optimize its own compilation.

Right now Spiral's worst problem is its poor library support. The libraries are always in text and have to be parsed and prepassed from the scratch on every compilation. The way most languages solve that is by inventing an intermediate bytecode format, but without a doubt there exists a language design that would allow both the language to be fused to libraries, and to optimize both itself and the programs it is applied to. Without a doubt, such a language would significantly exceed Spiral in quality.

One thing is for certain, such a language would be hard to write as a standard compiler in F#. A lot more infrastructure support would be necessary in order to support a fundamentally different approach towards compiler construction. Its own platform and a surgical compiler like [Lancet](https://github.com/TiarkRompf/lancet) would be a prerequisite. MLs are more suited towards writing interpreters than language towers. 

[Racket](https://racket-lang.org/) has a superior ecosystem for writing such a language compared to the .NET, but it is the author's opinion that the parser in particular is not the best place to do compile time evaluation of functions and that syntax should not be used for abstraction - it should be used for ergonomics and should be consistent. Parsing should be a step to get rid of syntax for the rest of the passes.

Macros do not make sense in dynamic languages as a tool for language creation. They are absolute insanity in static languages. Often when static languages reach the limit of their design they cram macros to do everything else - like performance optimizations for example, and tout them as a feature rather than an admission of failure in language design. 

Wanting macros in order to optimize performance will never happen in Spiral.

### 4: Continuation Passing Style, Monadic Computation and Parsing

Now that union types are out of the way, slowly the subject can move towards the more fun stuff that can be done with the language. CPS is a great way of writing highly abstract, generic and very fast code in Spiral and so the language has support for programming in such a style using monadic syntax. Modules are a significant aid as well for programming in CPS.

This chapter will be short and won't go into depth of how monads work. Neither will it explain how parsers work. As both of those subjects are highly complex, it would take a lot of time to cover them. For parsers combinators in particular, the place place to learn how they work would be to start with the [FParsec documentation](http://www.quanttec.com/fparsec/).

For monads in particular, it is best to study their specific instances. There is a large amount of tutorials online regarding them, most often in the context of the Haskell language. More closer to home, the author's understanding of them went through a dramatic improvement once he stopped trying to figure out what the higher kinded types are doing and simply focused on the functions themselves in terms of flow.

```
inl on_succ a = (a,())
inl on_log x = ((),Tuple.singleton x)
inl (>>=) (a,w) f = // The writer monad.
    inl a',w' = f a
    (a',Tuple.append w w')

inl add x y = x + y |> on_succ

inm x = add 1 1
inm _ = on_log x
inm y = add 3 4
inm _ = on_log y
inm z = add 5 6
inm _ = on_log z
on_succ (x+y+z) // Tuple2(20L, Tuple1(2L, 7L, 11L))
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
and Tuple1 =
    struct
    val mem_0: int64
    val mem_1: Tuple0
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
Tuple1(20L, Tuple0(2L, 7L, 11L))
```

What the `inm` keyword does is merely rewrite `inm x = f` to `f >>= inl x -> ...`. Meaning the above example could have been done manually as...

```
add 1 1 >>= inl x ->
on_log x >>= inl _ ->
add 3 4 >>= inl y ->
on_log y >>= inl _ ->
add 5 6 >>= inl z ->
on_log z >>= inl _ ->
on_succ (x+y+z)
```

The writer monad pattern is notable as it is used to accumulate the backwards trace in the ML library, so it is worth keeping in mind. Despite the power of monads, most of the time they are used to encapsulate state.

Apart from `inm` there is also `inb`.

```
inl f x ret =
    Console.writeline x
    ret()
    Console.writeline "done"
inb x = f "hello"
Console.writeline "doing work"
```
```
System.Console.WriteLine("hello")
System.Console.WriteLine("doing work")
System.Console.WriteLine("done")
```

A pattern similar to the above is used to emulate stack allocation of Cuda memory in the ML library.

Anyway, here is a simple Spiral parser library written in CPS style just for the sake of making a benchmark. A more advanced version in monadic style can be found in the standard library.

#### Parsing Benchmark

```
let example = 
    "example",[option;tuple;loops;extern_;console],"Module description.",
    """
/// Converts a string to a parser stream.
/// string -> parser_stream
inl string_stream str {idx on_succ on_fail} =
    inl f idx = idx >= 0 && idx < string_length str
    inl branch cond = if cond then on_succ (str idx) else on_fail "string index out of bounds" 
    match idx with
    | a, b -> branch (f a && f b)
    | _ -> branch (f idx)

/// Runs a parser given the string and the expected return type.
/// t type -> string -> t parser -> t
inl run ret_type str parser = 
    inl stream = dyn str |> string_stream

    inl d = {
        stream
        on_succ = inl x state -> id x
        on_fail = inl x state -> failwith ret_type x
        on_type = ret_type
        }

    parser d {pos=dyn 0}

/// Reads a char.
/// char parser
inl stream_char {stream on_succ on_fail} {state with pos} =
    stream {
        idx = pos
        on_succ = inl c -> on_succ c {state with pos=pos+1}
        on_fail = inl msg -> on_fail msg state
        }

inl is_digit x = x >= '0' && x <= '9'
inl is_whitespace x = x = ' '
inl is_newline x = x = '\n' || x = '\r'

/// Reads a digit.
/// char parser
inl digit {stream on_succ on_fail} state =
    stream_char {
        stream on_fail
        on_succ = inl x state' -> 
            if is_digit x then on_succ x state'
            else on_fail "digit" state
        } state

inl convert_type = fs [text: "System.Convert"]
inl to_uint64 x = Extern.FS.StaticMethod convert_type .ToUInt64 x uint64
/// Reads an 64-bit integer parser.
/// uint64 parser
inl puint64 {stream on_succ on_fail on_type} state =
    inl error state = on_fail "puint64" state
    inl rec loop i on_fail state =
        digit {
            stream
            on_fail=inl _ state -> on_fail i state
            on_succ=inl c state ->
                inl max = 1844674407370955161u64 // max of uint64 / 10u64
                if i <= max then
                    inl i' = i * 10u64 + to_uint64 c - to_uint64 '0'
                    if i < i' then join loop i' on_succ state
                    else error state
                else error state
            } state
        : on_type
    loop (dyn 0u64) (inl _ state -> error state) state

/// The skips an all the whitespaces (including newlines) before succeeding.
/// unit parser
met rec spaces {d with stream on_succ on_fail on_type} state =
    stream_char {
        stream
        on_fail = inl _ state -> on_succ () state
        on_succ = inl c state' -> 
            if is_whitespace c || is_newline c then spaces d state'
            else on_succ () state
        } state
    : on_type

/// Runs the first and then the second parser before returning the result of the second parser.
/// a parser -> b parser -> b parser
inl (>>.) a b {d with on_succ} state = a {d with on_succ = inl _ state -> b d state} state
/// Runs the first and then the second parser before returning the result of the first parser.
/// a parser -> b parser -> a parser
inl (.>>) a b {d with on_succ} state = 
    a {d with on_succ = inl a state -> 
        b {d with on_succ = inl _ state -> on_succ a state} state
        } state

/// Runs the tuple of parsers before returning their result.
/// tuple parser
inl rec tuple l {d with on_succ} state =
    match l with
    | x :: xs ->
        x {d with on_succ = inl x state ->
            tuple xs {d with on_succ = inl xs state ->
                on_succ (x :: xs) state
                } state
            } state
    | () -> on_succ () state

/// Parses an unsigned 64-bit integer and returns it after parsing whitespaces.
/// uint64 parser
inl num = puint64 .>> spaces

run (uint64,uint64,uint64) "123 456 789" (tuple (num, num, num))
    """
```
As the output is 376 lines long, it won't be pasted. Here is a straightforward translation of the above to F#.
```
let example2 = 
    /// Converts a string to a parser stream.
    /// string -> parser_stream
    /// Note: The functionality of this functions needs to be pared down in F# due to lack of intensional polymorphism.
    let string_stream (str: string) (idx, on_succ, on_fail) =
        if idx >= 0 && idx < str.Length then on_succ str.[idx] else on_fail "string index out of bounds" 

    /// Runs a parser given the string and the expected return type.
    /// string -> t parser -> t
    let run str parser = 
        let stream = string_stream str

        let d = 
            stream
            ,fun x state -> id x
            ,fun x state -> failwith x

        parser d 0

    /// Reads a char.
    /// char parser
    let stream_char (stream, on_succ, on_fail) pos =
        stream 
            (pos
            ,fun c -> on_succ c (pos+1)
            ,fun msg -> on_fail msg pos
            )

    let is_digit x = x >= '0' && x <= '9'
    let is_whitespace x = x = ' '
    let is_newline x = x = '\n' || x = '\r'

    /// Reads a digit.
    /// char parser
    let digit (stream, on_succ, on_fail) state =
        stream_char ( 
            stream 
            ,fun x state' -> 
                if is_digit x then on_succ x state'
                else on_fail "digit" state
            ,on_fail
            ) state
            

    /// Reads an 64-bit integer parser.
    /// uint64 parser
    let puint64 (stream, on_succ, on_fail) state =
        let error state = on_fail "puint64" state
        let rec loop i on_fail state =
            digit (
                stream
                ,fun c state ->
                    let max = 1844674407370955161UL // max of uint64 / 10u64
                    if i <= max then
                        let i' = i * 10UL + System.Convert.ToUInt64 c - System.Convert.ToUInt64 '0'
                        if i < i' then loop i' on_succ state
                        else error state
                    else error state
                ,fun _ state -> on_fail i state
                ) state
        loop 0UL (fun _ state -> error state) state

    /// The skips an all the whitespaces (including newlines) before succeeding.
    /// unit parser
    let rec spaces (stream, on_succ, on_fail as d) state =
        stream_char (
            stream
            ,fun c state' -> 
                if is_whitespace c || is_newline c then spaces d state'
                else on_succ () state
            ,fun _ state -> on_succ () state
            ) state

    /// Runs the first and then the second parser before returning the result of the second parser.
    /// a parser -> b parser -> b parser
    let (>>.) a b (stream,on_succ,on_fail as d) state = a (stream,(fun _ state -> b d state), on_fail) state
    /// Runs the first and then the second parser before returning the result of the first parser.
    /// a parser -> b parser -> a parser
    let (.>>) a b (stream,on_succ,on_fail) state = 
        a (stream, (fun a state -> b (stream, (fun _ state -> on_succ a state),on_fail) state), on_fail) state

    /// Runs the tuple of parsers before returning their result.
    /// tuple parser
    /// Note: This one is ugly. It is impossible to make a generic tuple without great type hackery in F#. 
    /// Check out FParsec.Pipes library to see how that might be done.
    let tuple3 (a,b,c) (stream,on_succ,on_fail) =
        a (
            stream 
            ,fun a ->
                b (
                    stream
                    ,fun b ->
                        c (
                            stream
                            ,fun c ->
                                on_succ (a,b,c)
                            ,on_fail)
                    ,on_fail)
            ,on_fail)

    /// Parses an unsigned 64-bit integer and returns it after parsing whitespaces.
    /// uint64 parser
    let num = puint64 .>> spaces

    run "123 456 789" (tuple3 (num, num, num))
```

Here are the performance figures from testing the above two programs using the [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library . What the above program is doing is merely parsing 3 unsigned 64-bit ints and returning them in a tuple.

```
           Method |       Mean |     Error |    StdDev |
----------------- |-----------:|----------:|----------:|
 FullySpecialized |   292.8 ns | 0.8448 ns | 0.7902 ns |
           FSharp | 3,616.2 ns | 8.9547 ns | 8.3762 ns |
```

The Spiral parser is about 12x times faster. That is quite a nice improvement and roughly what one could expect. The interesting thing not noted in the benchmark is that once the Spiral's output has been compiled to F# code, it runs instantaneously while in F#'s case, there is a noticeable delay while the .NET JIT tries to optimize it. Obviously, in Spiral's case the JIT does not have much work left for it. Apart from register allocation, Spiral already does everything else and does a better job if it finishes compiling.

If anything, the above understates the advantage that Spiral has over F#. Given how poor F# is at optimizing [monads](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/computation-expressions), for parsers written in a monadic style the author would not be surprised to see the gap widen by another 10x.

Nevertheless, one point in F#'s favor are compile times. The parser being tested here is somewhat trivial, but for a more complex parser such as the one for the Spiral compiler, the author would not be surprised to see it go up into 100s of thousands of lines of code. Given that the IDE gets crushed by the weight of a 20k LOC parser, it has him wondering how would he even compile such a monster? As a rule of the thumb, Spiral's evaluator generates around 3k LOC per second currently.

For that sake, rather than CPSing everything it would be important to do boxing. In the previous section on lists, it was shown how in Spiral, the CPS form of `head`,`tail` and `last` is just a more generic form of the one that uses the option type. Note that this is not the case in other functional languages as they lack Spiral's inlining guarantees.

There are two choices for doing boxing in Spiral and they are good to know in order to cut down on exceesive specialization and making sure the evaluator does not diverge.

1) Do term casting.

```
/// Term casting for parsers
/// a type -> a parser -> a parser
inl term_cast typ p {d with on_succ on_fail on_type} state =
    inl term_cast_uncurried g a b = // This is to make sure only one closure is allocated.
        inl g = term_cast (inl a, b -> g a b) (a,b)
        inl a b -> g (a,b)
    p {d with 
        on_succ = term_cast_uncurried on_succ typ state
        on_fail = term_cast_uncurried on_fail string state
        } state

inl puint64 d state = join puint64 d state // Make sure that the unrolled outer loop is rolled in.

/// Parses an unsigned 64-bit integer and returns it after parsing whitespaces.
/// uint64 parser
inl num = term_cast uint64 (puint64 .>> spaces)

run (uint64,uint64,uint64) "123 456 789" (tuple (num, num, num))
```

The amount of code generated drops down to 141 LOC with this. Unfortunately it does make the program slower by about 40%.

```
           Method |       Mean |     Error |    StdDev |
----------------- |-----------:|----------:|----------:|
       TermCasted |   406.6 ns | 1.2316 ns | 1.1520 ns |
 FullySpecialized |   292.8 ns | 0.8448 ns | 0.7902 ns |
           FSharp | 3,616.2 ns | 8.9547 ns | 8.3762 ns |
```

2) Box using union types.

```
inl ParserResult x state = .ParserSucc, x, state \/ .ParserFail, string, state

/// Union boxing for parsers
/// a type -> a parser -> a parser
inl box typ p {d with on_succ on_fail} state = 
    inl on_type = ParserResult typ state
    inl box = box on_type
    p {d with
        on_succ = inl x state -> box (.ParserSucc, x, state)
        on_fail = inl x state -> box (.ParserFail, x, state)
        on_type
        } state
    |> function
        | .ParserSucc, x, state -> on_succ x state
        | .ParserFail, x, state -> on_fail x state

inl puint64 d state = join puint64 d state // Make sure that the unrolled outer loop is rolled in.

/// Parses an unsigned 64-bit integer and returns it after parsing whitespaces.
/// uint64 parser
inl num = box uint64 (puint64 .>> spaces)

run (uint64,uint64,uint64) "123 456 789" (tuple (num, num, num))
```

This actually improves the running time significantly.

```
           Method |       Mean |     Error |    StdDev |
----------------- |-----------:|----------:|----------:|
       TermCasted |   406.6 ns | 1.2316 ns | 1.1520 ns |
             Boxy |   199.4 ns | 0.9976 ns | 0.9332 ns |
 FullySpecialized |   292.8 ns | 0.8448 ns | 0.7902 ns |
           FSharp | 3,616.2 ns | 8.9547 ns | 8.3762 ns |
```

The boxy parser is by far the best variant. It comes out to only 170 LOCs and is 45% faster than the fully specialized version and 18x times than the F# version. This also demonstrates that code duplication does have a noticeable performance impact. The lines of code reduced would be much more dramatic for a larger parser thereby making the application of this technique a necessity in a serious library. Currently the `Parsing` module in the standard library is lacking in that regard and the above benchmark is actually the first time the author used union type boxing on parsers or did benchmarking of a Spiral program. The `Parsing` module was not intented to be a serious parsing library, but to drive the development of the language in a challening area.

When the first version of it was created Spiral did not have modules nor monadic syntax nor good error messages, but it did have a lot of compiler bugs as if to make up for it. Spiral's modules were created in part because refactoring the parser was simply so painful during those days - it would take the author hours to fix the type errors that in F# would have taken him 10m. It is much better now thankfully.

As the author has no intention of doing so and wants to do machine learning instead, for those interested in parsing Spiral is a very good language to experiment with in the context of staged functional programming.

It will no doubt be a very productive language for such a purpose depending on how much weight is put on performance. If full weight is put on it then there is no doubt that Spiral would be orders of magnitute more productive at such a task than other languages.

The reason for that is that it is not enough to judge merely by how long would it take to write a parser, but how long would it take to get it on par with Spiral in performance. It took the author ~5h to make the parser for this benchmark in Spiral and then maybe 20m to transcribe it to F# by hand. In order to get to the Boxy level of performance, how long would it have taken to specialize all of that by hand and test it? Days?

Just how hard would such a fast handwritten C-style parser be to modify after that? It would be completely inflexible, so a decent guess is quite hard. It would also take a special kind of masochism to deliberately write code in such a style.

ML styled languages still have some advantages over Spiral due to having type inference which is a great aid to refactoring and explorability, but C offshots can be completely replaced with no regret.

The 4 parser benchmarked in this section can be found in [this folder](https://github.com/mrakgr/The-Spiral-Language/tree/master/Benchmarking) of the repo.

### 5: Tensors and Structural Reflection

The development of Spiral was driven by the need for a language with great capability for abstraction whose semantics would allow for it to be compiled to very fast code suitable for GPUs and the architectures coming down the line. During the early days of its development when it was intended as a Cuda backend for the ML library Spiral actually had built in arrays that would track variables at on the type level, but that tensors could be designed like the way they currently could be was beyond the imagination of its author and makes him glad that he decided to complete the language instead of leaving Spiral in a half finished state as a crappy ML library backend.

Tensors in Spiral represent the crystallization of its power; they are the point at which all of its features flow together to create something that cannot be done in any other language.

Unlike parsers of the previous chapter which were complicated - so complicated that they could not be explained, tensors are actually rather simple and intended for all ages.

The implementation of `HostTensor` in the standard library is somewhat convoluted due to needing to be generic in order to be reusable on the Cuda side in addition to having a lot of functionality, so this chapter will follow the format of giving a few examples of its most salient features and then show how a simpler tensor can be derived from first principles in order to attain understanding of it.

Tensors in Spiral can have an arbitrary number of dimensions, arbitrary types and arbitrary layouts in addition to supporting views. Indexing into them emulates the partial application of functions. `init` for them takes arguments in curried form which supports scope control.

```
let example = 
    "example",[option;tuple;loops;extern_;console;host_tensor],"Module description.",
    """
inl ar = array_create string 3
Tuple.foldl (inl i x -> ar i <- x; i+1) 0 ("zero","one","two") |> ignore
HostTensor.init (3,5,{from=2; to=5}) (inl a ->
    inl x = ar a
    string_format "x is {0}" x |> Console.writeline
    inl b c -> x, b, c
    )
|> HostTensor.show.all
|> Console.writeline
    """
```

The generated code won't be posted as the loops for printing the tensors and initializing it are long, but here is what comes out when the program has been ran.

```
x is zero
x is one
x is two
[|
    [|
        [|[zero, 0, 2]; [zero, 0, 3]; [zero, 0, 4]; [zero, 0, 5]|]
        [|[zero, 1, 2]; [zero, 1, 3]; [zero, 1, 4]; [zero, 1, 5]|]
        [|[zero, 2, 2]; [zero, 2, 3]; [zero, 2, 4]; [zero, 2, 5]|]
        [|[zero, 3, 2]; [zero, 3, 3]; [zero, 3, 4]; [zero, 3, 5]|]
        [|[zero, 4, 2]; [zero, 4, 3]; [zero, 4, 4]; [zero, 4, 5]|]
    |]
    [|
        [|[one, 0, 2]; [one, 0, 3]; [one, 0, 4]; [one, 0, 5]|]
        [|[one, 1, 2]; [one, 1, 3]; [one, 1, 4]; [one, 1, 5]|]
        [|[one, 2, 2]; [one, 2, 3]; [one, 2, 4]; [one, 2, 5]|]
        [|[one, 3, 2]; [one, 3, 3]; [one, 3, 4]; [one, 3, 5]|]
        [|[one, 4, 2]; [one, 4, 3]; [one, 4, 4]; [one, 4, 5]|]
    |]
    [|
        [|[two, 0, 2]; [two, 0, 3]; [two, 0, 4]; [two, 0, 5]|]
        [|[two, 1, 2]; [two, 1, 3]; [two, 1, 4]; [two, 1, 5]|]
        [|[two, 2, 2]; [two, 2, 3]; [two, 2, 4]; [two, 2, 5]|]
        [|[two, 3, 2]; [two, 3, 3]; [two, 3, 4]; [two, 3, 5]|]
        [|[two, 4, 2]; [two, 4, 3]; [two, 4, 4]; [two, 4, 5]|]
    |]
|]
```
Having the arguments to `init` be partially applied rather than given all at once is what allow the outside array to be indexed only in the outer loop. Had it been otherwise, the indexing would have needed to be done inside the innermost loop. `init` gives loops for free. Important operations such as map, rotation and reduction can be implemented in terms of init on the host (CPU) side.

Tensors themselves emulate partial application of functions. Here is how they can be indexed into.

```
inl ar = array_create string 3
Tuple.foldl (inl i x -> ar i <- x; i+1) 0 ("zero","one","two") |> ignore
inl tns = HostTensor.init (3,5,{from=2; to=5}) (inl a ->
    inl x = ar a
    inl b c -> x, b, c
    )
inl f x = x 0 2 .get
tns 0 |> f |> Console.writeline
tns 1 |> f |> Console.writeline
tns 2 |> f |> Console.writeline
```
```
[zero, 0, 2]
[one, 0, 2]
[two, 0, 2]
```

This is convenient for views. Views can take more than a single argument in which case they need to be passed as a tuple. Like application, views work on dimensions from left to right - from the outermost to the innermost.

```
inl ar = array_create string 3
Tuple.foldl (inl i x -> ar i <- x; i+1) 0 ("zero","one","two") |> ignore
inl tns = HostTensor.init (3,5,{from=2; to=5}) (inl a ->
    inl x = ar a
    inl b c -> x, b, c
    )

tns.view (1,{from=2;near_to=4})
|> HostTensor.show.all
|> Console.writeline
```
```
[|
    [|
        [|[zero, 2, 2]; [zero, 2, 3]; [zero, 2, 4]; [zero, 2, 5]|]
        [|[zero, 3, 2]; [zero, 3, 3]; [zero, 3, 4]; [zero, 3, 5]|]
    |]
|]
```
Both the tensor applications and views are done done immutably. Apart from join points which are memoized, Spiral's metalanguage is pure and there is no way of mutably updating tuples or modules.

For the sake of demonstration, here is how `.set` works. It is very similar to `.get`.

```
inl tns = HostTensor.init ({from=2; near_to=5}) id
inl modify f t = t .set (t .get |> f)
tns 2 |> modify ((*) 2)
tns 3 |> modify ((+) 22)
tns 4 |> modify (const -11)

HostTensor.show.all tns
|> Console.writeline
```
```
[|4; 25; -11|]
```
In addition to `.view`, `.view_span` is useful.
```
inl tns_a = HostTensor.init ({from=2; near_to=10}) id

HostTensor.show.all tns_a
|> string_format "tns_a = {0}"
|> Console.write

inl tns_b = tns_a.view_span {from=0;near_to=2}
HostTensor.show.all tns_b
|> string_format "tns_b = {0}"
|> Console.write

inl tns_c = tns_a.view_span {from=2;near_to=4}
HostTensor.show.all tns_c
|> string_format "tns_c = {0}"
|> Console.write

tns_c 0 .get |> Console.writeline
tns_c 1 .get |> Console.writeline
//tns_c 2 .get |> Console.writeline // Would trigger the range check assertion at compile time.
```
```
tns_a = [|2; 3; 4; 5; 6; 7; 8; 9|]
tns_b = [|2; 3|]
tns_c = [|4; 5|]
4
5
```
`.view_span` is similar to view except starts the indexing from the beginning of the dimensions of the target tensor and rebases the dimensions so they start from 0. That means that `.view_span -1` would always be an out of bounds error as would all negative values of the index. Given how Spiral specializes join points, it is useful for avoiding code bloat as well.

For the sake of machine learning, it is preferable to keep the tensor sizes as constants to get rid of as many assertion at compile time as possible. Note that in the last line, had it not been commented out the compiler would have raised a type error at compile time.

Tensors have even more to offer. By default, their layout is that of tuple of arrays. Meaning a tuple of type `float32 * int64 * int64` would be represented using 3 arrays internally each for the separate elements of the tuple. The main motivation behind this design is to make it easy to pass them through language boundaries onto the Cuda side as unless the arrays were of primitive types, it would be difficult to align them in memory, but there are performance benefits as well to allowing such a representation.

That varies from problem to problem, so it would be even better if it was easy to switch between representations at will.

```
inl tns_toa = HostTensor.init (5,5) (inl a b -> a,b)
inl tns_aot = HostTensor.init.aot (5,5) (inl a b -> tns_toa a b .get)
()
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
...
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(25L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(25L))
let (var_2: int64) = 0L
method_0((var_0: (int64 [])), (var_1: (int64 [])), (var_2: int64))
let (var_5: (Tuple0 [])) = Array.zeroCreate<Tuple0> (System.Convert.ToInt32(25L))
let (var_6: int64) = 0L
method_2((var_0: (int64 [])), (var_1: (int64 [])), (var_5: (Tuple0 [])), (var_6: int64))
```

The full thing won't be posted as it is 100 LOC, but it should be imaginable that `method_0` is `init` for `tns_toa` and `method_2` is `init` for `tns_aot`. Spiral's tensors are the perfect abstraction where layouts are concerned and it is possible to mix and match `.aot` and `.toa` layout using the `zip` function and it will still work fine. For most usecases, the default tuple of arrays is sensible.

Saying tuple of arrays does not cover it completely though. The tensors work fine with modules.

```
inl a = HostTensor.init (3,3) (inl a b -> {a b})
a 2 2 .set {a=99}
HostTensor.show a
|> Console.writeline
```
```
[|
    [|{a = 0; b = 0}; {a = 0; b = 1}; {a = 0; b = 2}|]
    [|{a = 1; b = 0}; {a = 1; b = 1}; {a = 1; b = 2}|]
    [|{a = 2; b = 0}; {a = 2; b = 1}; {a = 99; b = 2}|]
|]
```

When tensor are in `toa` form they have the added feature of allowing the module fields to be mutated individually. This is not possible in general in Spiral as modules and tuples are immutable; if they were represented as `aot` or if the modules were tuples this would not be possible. What is going on is that when they are represented as separate entities this changes their semantics to reflect that and this is the correct behavior even in a functional language.

This is the short tour of the tensors in Spiral. The next section will be on how they are implemented.

#### Under the Hood

As was demonstrated, there are two aspects of tensor polymorphism - one was that they have an arbitrary number of dimensions and the other was that are layout polymorphic. In a language with weaker type systems that would require creating specific tensor instances for both of those concerns, but Spiral can handle them naturally.

Even better, the dimensionality of the tensor is really a separate concern from its layout and so the two subjects can be taken in as separate pieces.

##### Layout Polymorphism

Creating an array of `int64 * int64 * int64` can be done like this in Spiral.

```
array_create (int64,int64,int64) 8
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    val mem_2: int64
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
Array.zeroCreate<Tuple0> (System.Convert.ToInt32(8L))
```

The above is also known as `aot` or `array of tuples` form. The opposite of it, the `toa` or `tuple of arrays` form would be this.

```
inl ar = array_create int64 8, array_create int64 8, array_create int64 8
()
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
```

Rather than copy pasting like, it would be better if the type were mapped directly.

```
inl ar = Tuple.map (inl x -> array_create x 8) (int64,int64,int64)
()
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
```

The same output as in the above example shows up. By now, the general principle of how Spiral's tensors achieve their layout polymorphism should be becoming clearer. Of course, the above is woefully incomplete. For example, if the tuple were nested then the `toa` layout would not be achieved.

```
inl ar = Tuple.map (inl x -> array_create x 8) (int64,(int64,int64))
()
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_1: (Tuple0 [])) = Array.zeroCreate<Tuple0> (System.Convert.ToInt32(8L))
```

What should be done is to write a function capable mapping over nested tuples.

```
inl rec toa_map f = function
    | x :: x' -> toa_map f x :: toa_map f x'
    | () -> ()
    | x -> f x

inl ar = toa_map (inl x -> array_create x 8) (int64,(int64,int64))
()
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
```

Note the subtle difference between `toa_map` and a regular `map`. In `| x :: x' -> toa_map f x :: toa_map f x'` the function recurses on `x` as well, not just on the tail.

For modules, a little extra is needed in `toa_map`.

```
inl toa_map f =
    inl rec loop = function
        | x :: x' -> loop x :: loop x'
        | () -> ()
        | {} as x -> module_map (const loop) x
        | x -> f x
    loop

inl ar = toa_map (inl x -> array_create x 8) {x=int64; y=int64,int64}
()
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
```

Before the function is complete there are two more things needed. That would be to stop the function from unboxing every union type and to stop it from recursing on every module. This last one is more of a convenience than necessity.

```
inl toa_map f =
    inl rec loop = function
        | x when caseable_box_is x -> f x // This needs to be in the first position to prevent the unboxing from triggering.
        | x :: x' -> loop x :: loop x'
        | () -> ()
        | {!toa_map_block} & x -> module_map (const loop) x
        | x -> f x
    loop

inl ar = toa_map (inl x -> array_create x 8) {x=int64; y=int64,int64; o=Option.Option float32}
()
```
```
type Union0 =
    | Union0Case0 of Tuple1
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: float32
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
let (var_0: (Union0 [])) = Array.zeroCreate<Union0> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_3: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
```

With this as the basis, it is easy to make a `toa` version of array create.

```
inl toa_array_create typ size = toa_map (inl x -> array_create x size) typ
```

Indexing into such an array is quite similar to creating it. It is just a straightforward application of `toa_map`.

```
inl toa_index ar idx = toa_map (inl ar -> ar idx) ar

inl ar = toa_array_create {x=int64; y=int64,int64; o=Option.Option float32} 8
inl el = toa_index ar 0
()
```
```
type Union0 =
    | Union0Case0 of Tuple1
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: float32
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
let (var_0: (Union0 [])) = Array.zeroCreate<Union0> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_3: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_4: Union0) = var_0.[int32 0L]
let (var_5: int64) = var_1.[int32 0L]
let (var_6: int64) = var_2.[int32 0L]
let (var_7: int64) = var_3.[int32 0L]
```

Setting such an array is a bit more difficult. In order to do that, `toa_map2` would be needed first.

```
inl toa_map2 f a b =
    inl rec loop = function
        | x, y when caseable_box_is x || caseable_box_is y -> f x y
        | x :: x', y :: y' -> loop (x,y) :: loop (x',y')
        | (),() -> ()
        | {!toa_map_block} & x, {!toa_map_block} & y -> module_map (inl k y -> loop (x k, y)) y
        | x, y -> f x y
    loop (a,b)
```

With this it is possible to implement `toa_set`.

```
inl toa_set ar idx v = toa_map2 (inl ar v -> ar idx <- v) ar v |> ignore

inl ar = toa_array_create {x=int64; y=int64,int64; o=Option.Option float32} 8
toa_set ar 0 {x=2; y=1,1; o=Option.some 2.2f32}
()
```
```
type Union0 =
    | Union0Case0 of Tuple1
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: float32
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
let (var_0: (Union0 [])) = Array.zeroCreate<Union0> (System.Convert.ToInt32(8L))
let (var_1: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_2: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
let (var_3: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(8L))
var_0.[int32 0L] <- (Union0Case0(Tuple1(2.200000f)))
var_1.[int32 0L] <- 2L
var_2.[int32 0L] <- 1L
var_3.[int32 0L] <- 1L
```

There is an interesting design decision of whether to allow partial setting of fields in modules in `toa_map2`.

```
| {!toa_map_block} & x, {!toa_map_block} & y -> module_map (inl k y -> loop (x k, y)) y
```

The above particular line could have also been written like so.

```
| {!toa_map_block} & x, {!toa_map_block} & y -> module_map (inl k x -> loop (x, y k)) x
```

Doing it like that would have disallowed the partial module sets as the map would have gone through every field of the `toa` array instead of every field of the setter.

In this section the way to make layout polymorphic arrays was described and is how Spiral's tensors attain such a capability. The ability to reflect on the structure of everything in the language at no runtime cost is a powerful one and solves a lot of the issues currently facing persons writing numerical libraries. It also covers an aspect of polymorphism that parametric languages do not touch.

The next section will be on how to take what has been done so far and make the 1d `toa` array, a tensor capable of support arbitrary dimensions.

##### Dimensionality Polymorphism

In one of the previous chapters, it was mentioned that Spiral is not necessarily less typesafe than F#, and that in some cases the opposite is in fact the case. Whenever dynamism and therefore union types are needed, a step is made into an entirely different paradigm for languages with weaker type systems due to have to push typechecking to runtime.

```
List.take 5 [1;2;3] // An error at runtime.
```
```
Tuple.take 5 (1,2,3) // An error at compile time.
```

This comparison might seem unfair since lists in F# are explicitly runtime constructs, but that is in fact the point. Spiral's parent language as it stands now does not have any substitute for Spiral's tuples at all and has absolutely no way of making tensors arbitrary in their dimensions at compile time. All it can do is support very specific instances of them.

So back when the author was making the ML library in F#, he had `d2M` for a 2D GPU tensor, and `d4M` for 4d GPU tensors. Afterwards he found the need for a 3d tensor so he made `d3M` too and so on. There was the `Df`, a lazy scalar host tensor as well.

Layout polymorphism? Forget that. The best what was possible was having them be generic in their type.

Now, there is no doubt that making a very specific instance of a tensor (such as `d2M`) is easier than making a fully blown generic tensor, but making such a tensor type is definitely easier than having to make a specific instance for all the endless varieties of tensors. Making specific instances of the more generic type by hand gets tiresome really quickly. It is humiliating to have to use personal effort because the tool one is using is not good enough.

###### Design of the Tensor

```
{
    bodies = { ar size offset toa_map_block } structure
    dim
}
```

The above is very similar to the type of the Spiral's `HostTensor` in pseudo-code. It is actually one of its previous designs that is easier to explain as it has a more uniform structure. 

In it, `dim` is just the dimension of the tensor and might be `(2,3,4)` for a 3d tensor, `(10,20)` for a 2d tensor or `()` for a scalar tensor for example.

`size` and `offset` inside the `bodies` are directly related to it. Elements of `offset` are always to be multiples of their related `size` element.

Before the coding can start, some simple examples need to be gone though by hand so that it becomes clear what is trying to be done in the first place.

1) On tensor creation.

```
/// Creating a 1d tensor of type int64 and dim 10
inl dim = 10
inl ar = array_create int64 dim
inl tns = {
    bodies = {
        ar
        size = Tuple.singleton 1
        offset = Tuple.singleton 0
        toa_map_block = ()
        }
    dim = Tuple.singleton dim
    }
```
```
/// Creating a 2d tensor of type int64 and dim 10, 10
inl dim = 10, 10
inl ar = array_create int64 (10 * 10)
inl tns = {
    bodies = {
        ar
        size = 10, 1
        offset = 0, 0
        toa_map_block = ()
        }
    dim
    }
```
```
/// Creating a 3d tensor of type int64 and dim 10, 5, 5
inl dim = 10, 5, 5
inl ar = array_create int64 (10 * 5 * 5)
inl tns = {
    bodies = {
        ar
        size = 5*5, 5, 1
        offset = 0, 0, 0
        toa_map_block = ()
        }
    dim
    }
```

By now some patterns should be coming out. `ar` is always inserted directly into the tensor body, `size` is just the rightwards [scan product](https://stackoverflow.com/questions/23491216/f-cumulative-product-of-an-array) of `dim`, `offset` is always `dim` mapped to 0, and `dim` is always itself. The only difference is 1d when `size`, `offset` and `dim` are wrapped in a tuple.

```
/// Creating a nd tensor in the array of tuples layout.
inl tensor_aot_create typ dim =
    inl dim = match dim with _ :: _ -> dim | x -> x :: ()
    inl array_size :: size = Tuple.scanr (*) dim 1
    inl offset = Tuple.map (const 0) dim
    {
    bodies = {
        ar = array_create typ array_size
        size offset
        toa_map_block=()
        }
    dim
    }
```

Here is the pattern abstracted and codified. The tuple of arrays version is similar to the above.

```
/// Creating a nd tensor in the tuple of arrays layout.
inl tensor_toa_create typ dim =
    inl dim = match dim with _ :: _ -> dim | x -> x :: ()
    inl array_size :: size = Tuple.scanr (*) dim 1
    inl offset = Tuple.map (const 0) dim
    inl make_body typ = {
        toa_map_block=()
        ar = array_create typ array_size
        size offset
        }
    {
    bodies = toa_map make_body typ            
    dim
    }
```

The two functions have a very similar internal structure. It can be factored out like so.

```
/// Creating a nd tensor
inl tensor_create {dsc with typ dim} =
    inl dim = match dim with _ :: _ -> dim | x -> x :: ()
    inl array_size :: size = Tuple.scanr (*) dim 1
    inl offset = Tuple.map (const 0) dim
    inl make_body typ = {
        toa_map_block=()
        ar = array_create typ array_size
        size offset
        }
    {
    bodies = 
        match dsc with
        | {layout=x} -> 
            match x with
            | .aot -> make_body typ
            | .toa -> toa_map make_body typ
        | _ -> toa_map make_body typ
    dim
    }
```

The beautiful thing about this is that since all the sizes are known, Spiral can track them at compile time.

```
inl tns = tensor_create {typ=int64,string,float32; dim=10,5,5}
()
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(250L))
let (var_1: (string [])) = Array.zeroCreate<string> (System.Convert.ToInt32(250L))
let (var_2: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(250L))
```

Unless the tensor is returned from a join point, or a dynamic if statement, or put into an array, those fields will never be instantiated due being tracked at the type level and the tensor will have the very minimal overhead at runtime.

Spiral's `HostTensor` actually tracks lower and upper bounds in `dim` as well, but that won't be done for this tutorial.

2) On application.

```
{
bodies = {
    ar
    size = 25, 5, 1
    offset = 0, 0, 0
    toa_map_block = ()
    }
dim = 10, 5, 5
}
```

For the purpose of explanation, the 3d tensor from the previous example will be the starting point.

How should applying 2 to the tensor transform it?

It should be into this 2d tensor.

```
{
bodies = {
    ar
    size = 5, 1
    offset = 25*2 + 0, 0 // 50
    toa_map_block = ()
    }
dim = 5, 5
}
```

Applying 3 to the above should turn it into this.

```
{
bodies = {
    ar
    size = 1 :: ()
    offset = 25*2 + 5*3 + 0 :: () // 65
    toa_map_block = ()
    }
dim = 5 :: ()
}
```

Now that it is has been applied twice, the resulting tensor has gone from 3d to 1d. Once more and it will be scalar. Here is simulating the application of 4.

```
{
bodies = {
    ar
    size = ()
    offset = 25*2 + 5*3 + 1*4 // 69
    toa_map_block = ()
    }
dim = ()
}
```

Note how now the offset it a scalar and can be used to index into an array. The rules of tensors are quite simple, only 0d (scalar) tensors can be indexed and set and cannot be applied, while the opposite holds for tensors with higher number of dimensions.

Also note that `dim` plays no role in calculating the top of the new offset.

What was omitted in the above example is the boundary checking. If the value being applied was out of range that would have triggered an error.

Here is the above intuition in code.

```
inl tensor_apply i {dim=d::dim bodies} =
    assert (i >= 0 && i < d) "Tensor application out of bounds"

    {
    dim
    bodies = 
        toa_map (inl {d with size=s::size offset=o::offset} ->
            inl o = o + i * s
            inl offset = 
                match offset with
                | o' :: offset -> o + o' :: offset
                | () -> o
            {d with size offset}
            ) bodies
    }
```

The `| o' :: offset -> o + o' :: offset` might be surprising, but that is needed because the offsets might not be zero and that should not be thrown away. That can happen in tensors whose inner dimensions were viewed.

```
inl tns = tensor_create {typ=int64,string,float32; dim=10,5,5}
tensor_apply 2 tns
|> tensor_apply 3
|> tensor_apply 4
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(250L))
let (var_1: (string [])) = Array.zeroCreate<string> (System.Convert.ToInt32(250L))
let (var_2: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(250L))
(Env4(Tuple3((Env0(var_0, 69L)), (Env1(var_1, 69L)), (Env2(var_2, 69L)))))
```

The types were cut out in the above generated code. As can be seen since scalar tensors have only their offset and an array that is what gets printed. 

```
inl tns = 
    tensor_create {typ=int64,string,float32; dim=10,5,5}
    |> tensor_apply (dyn 2)
    |> tensor_apply 3
    |> tensor_apply 4
join tns
```
```
let rec method_0((var_0: (int64 [])), (var_1: int64), (var_2: (string [])), (var_3: (float32 []))): Env0 =
    (Env0(Tuple4((Env1(var_0, var_1)), (Env2(var_2, var_1)), (Env3(var_3, var_1)))))
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(250L))
let (var_1: (string [])) = Array.zeroCreate<string> (System.Convert.ToInt32(250L))
let (var_2: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(250L))
let (var_3: int64) = 2L
let (var_4: bool) = (var_3 >= 0L)
let (var_6: bool) =
    if var_4 then
        (var_3 < 10L)
    else
        false
let (var_7: bool) = (var_6 = false)
if var_7 then
    (failwith "Tensor application out of bounds")
else
    ()
let (var_8: int64) = (var_3 * 25L) // 50
let (var_9: int64) = (var_8 + 15L) // 65
let (var_10: int64) = (var_9 + 4L) // 69
method_0((var_0: (int64 [])), (var_10: int64), (var_1: (string [])), (var_2: (float32 [])))
```

In the above example the first application was `dyn`ed. This makes the assertion check necessary at runtime, but the line of note is the last one. Due to common subexpression rewriting, in the end all the bodies end up with the same index variable.

This will be reflected when they are being passed through join points. 

```
met f (a,b,c) = a+b+c
inl x = dyn 3
f (x,x,x)
```
```
let rec method_0((var_0: int64)): int64 =
    let (var_1: int64) = (var_0 + var_0)
    (var_1 + var_0)
let (var_0: int64) = 3L
method_0((var_0: int64))
```

The moral of that is - for efficiency tensors should not be returned from anything apart from inlineables and should not be stored into arrays. Spiral's natural style is towards continuation passing and (mostly) functional purity. Tuples and modules should be used to store arguments whenever possible and opaque structures should be avoided.

3) On indexing and setting.

These two are easy since all the hard work has already been done by `tensor_apply`.

```
inl tensor_index {bodies dim=()} = toa_map (inl {ar offset} -> ar offset) bodies
inl tensor_set {bodies dim=()} v = toa_map2 (inl {ar offset} v -> ar offset <- v) bodies v |> ignore

inl tns = 
    tensor_create {typ=int64,string,float32; dim=10,5,5}
    |> tensor_apply 2
    |> tensor_apply 3
    |> tensor_apply 4

tensor_set tns (1,"asd",3.3f32)
```
```
let (var_0: (int64 [])) = Array.zeroCreate<int64> (System.Convert.ToInt32(250L))
let (var_1: (string [])) = Array.zeroCreate<string> (System.Convert.ToInt32(250L))
let (var_2: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(250L))
var_0.[int32 69L] <- 1L
var_1.[int32 69L] <- "asd"
var_2.[int32 69L] <- 3.300000f
```

Here is the `aot` form for good measure.

```
inl tns = 
    tensor_create {layout=.aot; typ=int64,string,float32; dim=10,5,5}
    |> tensor_apply 2
    |> tensor_apply 3
    |> tensor_apply 4

tensor_set tns (1,"asd",3.3f32)
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: string
    val mem_2: float32
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
let (var_0: (Tuple0 [])) = Array.zeroCreate<Tuple0> (System.Convert.ToInt32(250L))
var_0.[int32 69L] <- Tuple0(1L, "asd", 3.300000f)
```
###### The Tensor Facade

Now that is is possible to create, apply, index and set the tensors the thing that remains is to make it applicable directly. To that, what is needed is to make a facade. The only thing of note in the following is that on standard application the facade rewraps itself. The rest should be straightforward.

```
inl rec tensor_facade tns = function
    | .get -> tensor_get tns
    | .set v -> tensor_set tns v
    | .(_) & x -> tns x
    | x -> tensor_apply x tns |> tensor_facade

inl tensor_create = tensor_create >> tensor_facade

inl tns = tensor_create {layout=.aot; typ=int64,string,float32; dim=10,5,5} 
tns 2 3 4 .set (1,"asd",3.3f32)
```
```
type Tuple0 =
    struct
    val mem_0: int64
    val mem_1: string
    val mem_2: float32
    new(arg_mem_0, arg_mem_1, arg_mem_2) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1; mem_2 = arg_mem_2}
    end
let (var_0: (Tuple0 [])) = Array.zeroCreate<Tuple0> (System.Convert.ToInt32(250L))
var_0.[int32 69L] <- Tuple0(1L, "asd", 3.300000f)
```

#### Closing Comments

Views on tensors are similar to apply and just push the offsets without reducing the dimensionality of the tensor. It won't be covered in the tutorial and the interested should just look into the standard library implementation of them for specific details.

There is an interesting programming lesson that the author (re)experienced while making the tensor tutorial. Near the beginning there was a comment that the tensor design for the tutorial is more uniform than the one in the standard library. It is actually even more than that. The way it has been designed here is in fact how the author remembered the tensor. He knew it was not like this in the standard library, but when he looked he was actually surprised at how it was made indicating that he in fact forgot about it.

It is not the first time it happened that a piece poorly fit into memory to him and won't be the last. Memory mismatches are a sure sign that a particular piece of software should be redesigned. Having been left alone for a while, all but the most salient features of a program faded from memory indicating that in fact the rest are worthless and should be removed. Once he is done with the documentation that will surely be done.

With this all that has needed to be said in order to understand tensors has been said, but a digression needs to be made to highlight just how great they really are.

The tensor tutorial should have been rather clear and straightforward and unless something has badly gone wrong, those reading this chapter should have a clear picture of their essence.

The number of languages in which a tensor can be implemented in such a manner can at the time of writing, 2 days to 2018, be literally counted on one finger. Being able to implement tensors like this is what essentially convinced the author that now that he has Spiral, it might be a good idea challenge the other deep learning frameworks for supremacy even though they have big corporate sponsorship.

There is no need to consider how tensors are made in a language made for numeric computation like Julia. Take PyTorch for instance, and go to the [tour of its internals](http://pytorch.org/blog/). It is essentially a tour of poor programming practices: using C macros for everything including making tensors type generic, the absolutely fearsome `static PyTypeObject py_FloatType` which is badly in need of modules or at least SML records, the friction between different components that just jumps out.

From what has been heard of PyTorch in action by the author, there has been nothing but praise.

Nevertheless, if the best tools for the job in 2018 for making tensors generic are the 1972 C macros then probably something went wrong somewhere and not just with PyTorch specifically.

Whether it be composability or performance or lack of safety, those kinds of problems exist due to the weak type systems. But it is not like type systems have to be just about solving constraints, nor do they have to be segregated from the rest of the compiler passes. Lisps had the great idea of integrating parsing with the rest of compilation passes. There is nothing preventing similar to be done with a type system.

Once that fusion is done, a piece of the power that is released can be seen in this chapter - a properly done tensor type.

### 6: The Cuda Backend (Sneak Peek)

(work in progress for the time being)

At the time of writing `12/30/2017` though the Cuda backend works just fine, the allocator, the `CudaTensor` and `CudaKernels` are all bound in the deep `Learning` module and are not even properly organized which makes it hard to properly introduce them. They are very fresh, and most of the Cuda content in the language has been written in the last two weeks before the start of the tutorials, so they need more time to mature. Out of the Cuda kernels only two `map` and `map_redo` have been written so far which are enough for activation functions and cost functions, but not biases or accuracy. The author has not even gotten to doing `init` yet.

Hence, this chapter will be very brief.

```
inl fact to = Loops.for {from=2; to state=dyn 1; body=inl {state i} -> state * i}
fact 3
```
```
let rec method_0((var_0: int64), (var_1: int64)): int64 =
    let (var_2: bool) = (var_1 <= 3L)
    if var_2 then
        let (var_3: int64) = (var_0 * var_1)
        let (var_4: int64) = (var_1 + 1L)
        method_0((var_3: int64), (var_4: int64))
    else
        var_0
let (var_0: int64) = 1L
let (var_1: int64) = 2L
method_0((var_0: int64), (var_1: int64))
```

Here is the Cuda version. The `cuda` keyword is just syntax sugar for `inl threadIdx blockIdx blockDim gridDim -> ...`. `openb` is the CPS version of `open`.

```
inl fact to = Loops.for {from=2; to state=dyn 1; body=inl {state i} -> state * i}
openb Cuda
run {
    blockDim=1
    gridDim=1
    kernel=cuda fact 3 |> ignore
    }
```
```
let cuda_kernels = """

extern "C" {
    __global__ void method_1();
    __device__ long long int method_2(long long int var_0, long long int var_1);
    
    __global__ void method_1() {
        long long int var_0 = threadIdx.x;
        long long int var_1 = threadIdx.y;
        long long int var_2 = threadIdx.z;
        long long int var_3 = blockIdx.x;
        long long int var_4 = blockIdx.y;
        long long int var_5 = blockIdx.z;
        long long int var_6 = 1;
        long long int var_7 = 2;
        long long int var_8 = method_2(var_6, var_7);
    }
    __device__ long long int method_2(long long int var_0, long long int var_1) {
        char var_2 = (var_1 <= 3);
        if (var_2) {
            long long int var_3 = (var_0 * var_1);
            long long int var_4 = (var_1 + 1);
            return method_2(var_3, var_4);
        } else {
            return var_0;
        }
    }
}
"""
```
The rest of the output is initialization code and won't be shown.

More information will be provided in the coming months.

## User Guide: The Spiral Power

While the tutorial was meant to be a light introduction to the language, the user guide is intended to be an in depth guide not just into the language, but into the workings of the compiler. It is not intended to be exhaustive, but to cover the core language features and their implementation for the sake of making the user more confident in traversing the Spiral's source should that be needed. 

As it currently stands, the compiler in its entirety is at about 4.1k LOC. And while the parser and the codegens have their charm points, the part of interest which is the partial evaluator for Spiral is only 1.6k LOC. Out of that 1.6k probably around 1000 lines are vital to its functioning, meaning as the language continues to grow and more operatives get added, the essence of it should remain intact.

It took the author 14 months to get the language up to this point, but with it as a reference it would be closer to 1 month long student project in terms of difficulty if a sensible tool like F# is used instead of C++.

The first 9 chapters document the language's core. The rest can be read in any order.

### 1: Data Structures, Abstraction and Destructuring

```
and TypedExpr =
    // Data structures
    | TyT of Ty
    | TyV of TyTag // int * Ty
    | TyBox of TypedExpr * Ty
    | TyList of TypedExpr list
    | TyMap of EnvTerm * MapType
    | TyLit of Value

    // Operations
    | TyLet of TyTag * TypedExpr * TypedExpr * Ty * Trace
    | TyState of TypedExpr * TypedExpr * Ty * Trace
    | TyOp of Op * TypedExpr list * Ty
    | TyJoinPoint of JoinPointKey * JoinPointType * Arguments * Ty
```

Nearly everything that can be done is Spiral is by manipulating the above 6 data structures. Chop off the first 3 and Spiral would be a pure dynamic functional language. `TyBox` represents a staged union type. `TyMap` is reused for both functions and modules. Modules are in essence functions without a body, they can be thought of as first class environments. In `TyMap` `EnvTerm` is almost the immutable map and can be thought of as that for all intents and purposes for now.

The dogma of information flows in Spiral is to let as much of it through forward. Inlineables always preserve all information and unlike most other languages, Spiral allows the exact structures, that is `TypedExpr`s themselves, to be propagated forward through specialization boundaries. In Spiral those are join points, but in most other languages those are standard functions.

In order to for there to be a separation between compile time and runtime, abstraction is necessary.

Abstraction is the process of turning a non-variable (`TyBox`,`TyList`,`TyMap`,`TyLit`) into an abstract variable. There are 3 ways of doing that: join point returns, and dynamic if branch and dynamic case returns.

When that is done, apart from what is preserved in the common subexpression dictionary, all information about the structure apart from its type is thrown away and it is replaced with a `TyV`. `TyV` is just a tag and a type representing a variable.

Here are a few examples just to make sure the lesson sticks. It is rough typing the following out by hand. Keeping track of information is why compilers exist. The more a language allows the user to reason locally, the better it is at doing its job.

```
TyLit (TyInt64 1) -> TyV(100, PrimT Int64T)
TyBox (TyT(LitT (LitString "None")),UnionT {LitT (LitString "None"); ListT [LitT (LitString Some); PrimT Int64T]}) -> TyV(101,UnionT {LitT (LitString "None"); ListT [LitT (LitString Some); PrimT Int64T]})
TyList [TyV(2,PrimT Float32T); TyLit (LitString "Hello"); TyV(3,PrimT CharT)] -> TyV(102,ListT [PrimT Float32T; PrimT StringT; PrimT CharT])
```

Destructuring is what usually comes after abstraction as it is used so often in the language. It is not the opposite of abstraction - it can't be since abstraction is the process throwing away information in a principled manner. Once that information is lost it can't be recovered.

Destructuring is always done at bindings and at list and map creation and before join points entries, so fully abstracted variables will never appear in the environment when the printed via `print_static`.

Here is how the process roughly works.

```
TyV(100, PrimT Int64T) -> TyV(100, PrimT Int64T) // no change
TyV(101,UnionT {LitT (LitString "None"); ListT [LitT (LitString Some); PrimT Int64T]}) -> TyV(101,UnionT {LitT (LitString "None"); ListT [LitT (LitString Some); PrimT Int64T]}) // no change
TyV(102,ListT [PrimT Float32T; PrimT StringT; PrimT CharT]) -> TyList [TyV(103,PrimT Float32T); TyV(104,PrimT StringT); TyV(105,PrimT CharT)]
```

Spiral is crazy about turning variables inside out whenever it can. This has the effect of flattening them when they are passed through join points and makes it easy to support having partially static maps and lists in the language. Note that this does not change their type in any way. In the above examples the left and the right sides have completely equal types.

`destructure` is probably the single most important function in the language as without it everything else would be impossible. It is the first hurdle to overcome when making a language with first class staging.

Note the the way F# for example or other functional languages do destructuring is not the same. There is a notable difference between the philosophy of Spiral and the rest of the pack. Spiral follows the dogma of `inline first and optimize later` while generally high level languages inherit the Lisp philosophy of `heap allocate first and optimize later`.

That means in F# for example pattern matching is necessary to destructure a tuple since otherwise they would be packed. This is done at runtime. Here is a look into how F# does it based on its AST. The following examples uses its [code quotation](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/code-quotations) feature.

```
let x = 1,2,3
let t =
    <@
    let a,b,c = x
    ()
    @>
```
```
val x : int * int * int = (1, 2, 3)
val t : Quotations.Expr<unit> =
  Let (c, TupleGet (PropertyGet (None, x, []), 2),
     Let (b, TupleGet (PropertyGet (None, x, []), 1),
          Let (a, TupleGet (PropertyGet (None, x, []), 0), Value (<null>))))
```

Most compilers worth their salt would optimize this away, but Spiral's optimizations are actually a part of its semantics thereby making them guarantees. Replacing optimizations with guarantees and making it a part of the type system is important for getting predictable and predictably good performance.

In Spiral something like a simple binding would mean something completely different.

```
inl a,b,c = x
```

There is no way that in the above example that `x` would not have already been destructured, hence what that tuple pattern does is tries to unbox a union type, but otherwise does absolutely nothing at runtime. It adds a few names to the environment and that is it. `destructure` would be called and then it would do nothing once it saw `TyList` as input.

```
inl a,b,c = join 1,2,3
```

Now if it was something like the above then it would be doing destructuring. It would go roughly `1,2,3 -> var [int64; int64; int64]` in the join point and then `var [int64; int64; int64] -> [var int64; var int64; var int64]` on destructuring.

Keeping the environments fully destructured at all times is important for than just efficiency at runtime, making specialization coherent, supporting partially static structures and making other operations easier - it also solves one of the great language interop problems. "How are arguments to be passed through join points to other languages?"

It is impossible to pass tuples of primitives to the Cuda side in their default form. The reason for that is that C might decide to layout the struct differently than the .NET JIT does and there is no way to access the layout information directly.

By flattening structures out of the way completely at runtime, interop becomes a much easier task. That is one of the main motivations for supporting tuple of array tensors as the default as well.

The time it took for the author to write the `destructure` function can be measured in many months.

```
let rec destructure (d: LangEnv) (r: TypedExpr): TypedExpr = 
    let inline destructure r = destructure d r

    let inline cse_eval on_succ on_fail r = 
        match Map.tryFind r !d.cse_env with
        | Some x -> on_succ x
        | None -> on_fail r
    let inline cse_recurse r = cse_eval destructure id r

    let inline let_insert_cse r = 
        cse_eval 
            cse_recurse
            (fun r ->
                let x = make_tyv_and_push_typed_expr d r
                cse_add d r x
                x)
            r

    let index_tuple_args r tuple_types = 
        let unpack (s,i as state) typ = 
            if is_unit typ then tyt typ :: s, i
            else (destructure <| TyOp(ListIndex,[r;TyLit <| LitInt64 (int64 i)],typ)) :: s, i + 1
        List.fold unpack ([],0) tuple_types
        |> fst |> List.rev

    let env_unseal r x =
        let unseal (m,i as state) (k: string) typ = 
            if is_unit typ then Map.add k (tyt typ) m, i
            else
                let r = TyOp(MapGetField,[r; tyv(i,typ)], typ) |> destructure 
                Map.add k r m, i + 1
        Map.fold unseal (Map.empty,0) x |> fst

    let inline destructure_var r map_vvt map_funt =
        match get_type r with
        | ListT tuple_types -> tyvv(map_vvt tuple_types)
        | MapT (env,t) -> tymap(map_funt env, t)
        | _ -> cse_recurse r
    
    match r with
    | TyMap _ | TyList _ | TyLit _ -> r
    | TyBox _ -> cse_recurse r
    | TyT _ -> destructure_var r (List.map (tyt >> destructure)) (Map.map (fun _ -> (tyt >> destructure)) >> Env)
    | TyV _ -> destructure_var r (index_tuple_args r) (env_unseal r >> Env)
    | TyOp _ -> let_insert_cse r
    | TyJoinPoint _ | TyLet _ | TyState _ -> on_type_er (trace d) "Compiler error: This should never appear in destructure. It should go directly into d.seq."
```

What was covered so far is essentially the `TyV _` case. `TyT _` case works very similarly to that except there is no need to make any work for the code generator in the later stage. Understanding the specifics of the function is not necessary at this point. What is desired is to show that there is a direct mapping between what was talked about and code. 

If one can understand the above function and how join points work, that would be enough to understand 90% of Spiral so it a goal to strive for.

Based on what was discussed up to now, the first time reader of the user guide's model should roughly be as if the above function was written like this. The following is `destructure` without common subexpression elimination.

```
let rec destructure (d: LangEnv) (r: TypedExpr): TypedExpr = 
    let inline destructure r = destructure d r

    let index_tuple_args r tuple_types = 
        let unpack (s,i as state) typ = 
            if is_unit typ then tyt typ :: s, i
            else (destructure <| TyOp(ListIndex,[r;TyLit <| LitInt64 (int64 i)],typ)) :: s, i + 1
        List.fold unpack ([],0) tuple_types
        |> fst |> List.rev

    let env_unseal r x =
        let unseal (m,i as state) (k: string) typ = 
            if is_unit typ then Map.add k (tyt typ) m, i
            else
                let r = TyOp(MapGetField,[r; tyv(i,typ)], typ) |> destructure 
                Map.add k r m, i + 1
        Map.fold unseal (Map.empty,0) x |> fst

    let inline destructure_var r map_vvt map_funt =
        match get_type r with
        | ListT tuple_types -> tyvv(map_vvt tuple_types)
        | MapT (env,t) -> tymap(map_funt env, t)
        | _ -> cse_recurse r
    
    match r with
    | TyMap _ | TyList _ | TyLit _ -> r
    | TyBox _ -> make_tyv_and_push_typed_expr d r
    | TyT _ -> destructure_var r (List.map (tyt >> destructure)) (Map.map (fun _ -> (tyt >> destructure)) >> Env)
    | TyV _ -> destructure_var r (index_tuple_args r) (env_unseal r >> Env)
    | TyOp _ -> make_tyv_and_push_typed_expr d r
    | TyJoinPoint _ | TyLet _ | TyState _ -> on_type_er (trace d) "Compiler error: This should never appear in destructure. It should go directly into d.seq."
```
`destructure_var` can be completely thought of as a recursive map.

Suppose something like `var [int64; int64]` bound to `var_12` is destructured. 

Inside `destructure` the type of the variable is iterated over. 

The first element is converted to `list_index var_12 0`. Then a recursive call is made on that. What happens is that then the `TyOp _` case gets hit. `make_tyv_and_push_typed_expr` then converts that into a `TyV` which then gets returned.

The second element is converted to `list_index var_12 1`. Then a recursive call is made on that. What happens is that then the `TyOp _` case gets hit. `make_tyv_and_push_typed_expr` then converts that into a `TyV` which then gets returned.

The reason why this is done recursively is because lists might have sublists and such. Without that only a single level would get destructured which would be poor.

The reason why first converting to a `TyOp(ListIdex,[var_12,0])` and such is needed is for the code generation pass. As can be inferred from that, it stands to reason that `make_tyv_and_push_typed_expr` is doing something to pass that information along. What is that function is doing is commonly known as let insertion.

### 2: Let Insertion and Common Subexpression Elimination

When the code is generated, it tends to be a series of `let` statements punctuated by an expression.

```
let var_0 = ...
let var_1 = ...
let var_2 = ...
...
```

Based on that information it be guessed that during evaluation a list of let statements is maintained in the environment that gets added every time `make_tyv_and_push_typed_expr` gets called. That is in fact exactly what happens, but the way the function is implemented might seem convoluted at first.

```
let inline make_tyv_and_push_typed_expr_template (even_if_unit: bool) (d: LangEnv) (ty_exp: TypedExpr): TypedExpr =
    let ty = get_type ty_exp
    if is_unit ty then
        if even_if_unit then
            let seq = !d.seq
            let trace = trace d
            d.seq := fun rest -> TyState(ty_exp,rest,get_type rest,trace) |> seq
        tyt ty
    else
        let v = make_tyv_ty d ty
        let seq = !d.seq
        let trace = trace d
        d.seq := fun rest -> TyLet(v,ty_exp,rest,get_type rest,trace) |> seq
        tyv v
    
let make_tyv_and_push_typed_expr_even_if_unit d ty_exp = make_tyv_and_push_typed_expr_template true d ty_exp
let make_tyv_and_push_typed_expr d ty_exp = make_tyv_and_push_typed_expr_template false d ty_exp
```

There are actually two variants of the function packed into a single one based on whether the type of the expression is a unit. Using some partial evaluation by hand, it should be possible to clarify the function by splitting them into two.

```
let make_tyv_and_push_typed_expr_even_if_unit (d: LangEnv) (ty_exp: TypedExpr): TypedExpr = //make_tyv_and_push_typed_expr_template true d ty_exp
    let ty = get_type ty_exp
    let seq = !d.seq
    let trace = trace d
    
    if is_unit ty then
        d.seq := fun rest -> TyState(ty_exp,rest,get_type rest,trace) |> seq
        tyt ty
    else
        let v = make_tyv_ty d ty
        d.seq := fun rest -> TyLet(v,ty_exp,rest,get_type rest,trace) |> seq
        tyv v

let make_tyv_and_push_typed_expr (d: LangEnv) (ty_exp: TypedExpr): TypedExpr = //make_tyv_and_push_typed_expr_template false d ty_exp
    let ty = get_type ty_exp
    if is_unit ty then
        tyt ty
    else
        let v = make_tyv_ty d ty
        let seq = !d.seq
        let trace = trace d
        d.seq := fun rest -> TyLet(v,ty_exp,rest,get_type rest,trace) |> seq
        tyv v
```

With this rewrite hopefully the distinction between the two functions should be clearer.

`d.seq` is in fact a list, but it is implemented as a closure of type `(TypedExpr -> TypedExpr) ref`. To explain the reason for that, a look at the full `TypedExpr` type is needed once again.

```
and TypedExpr =
    // Data structures
    | TyT of Ty
    | TyV of TyTag
    | TyBox of TypedExpr * Ty
    | TyList of TypedExpr list
    | TyMap of EnvTerm * MapType
    | TyLit of Value

    // Operations
    | TyLet of TyTag * TypedExpr * TypedExpr * Ty * Trace
    | TyState of TypedExpr * TypedExpr * Ty * Trace
    | TyOp of Op * TypedExpr list * Ty
    | TyJoinPoint of JoinPointKey * JoinPointType * Arguments * Ty
```

The way it is implemented like that is because of `TyLet`. Using a list directly would be annoying because `TyLet` is a list itself and it would be troublesome to add to end if the arraignment was different.

So the solution is to just do some lambda magic to do that instead and emulate an immutable stack that way in a statically safe manner. The same goes for `TyState` which is like `TyLet` except for side effecting operations and not new bindings.

`make_tyv_and_push_typed_expr` and `make_tyv_and_push_typed_expr_even_if_unit` are used for non side effecting and side effecting operations respectively. Destructuring that was shown in the previous chapter is an example of the former, for example it should not happen that runtime unit types such as type level literals should ever be instantiated. As those get printed as "" during code generation trying to do so would give an error when the F# or Cuda compilers get the code.

As a part of general mechanism of supporting the first class types in Spiral, in both let insertion and destructuring any operations that would produce a unit type gets converted to a unwrapped (naked) type. Side effecting or potentially side effecting operations such as join points therefore must call `make_tyv_and_push_typed_expr_even_if_unit` before `destructure` gets to it otherwise it will cause errors. For example if `make_tyv_and_push_typed_expr_even_if_unit` was not called inside join points then those with unit type as return would not get printed at all.

Even worse, repeating join points in local scope would get rewritten to the same variable. This would not be a problem for pure functions, but for side effecting ones it would lead to erroneous results.

The way (CSE) common subexpression elimination works is simple in Spiral.

```
inl q = x + 3
...
inl w = x + 3
...
```

All CSE happens in `destructure`. What happens is that the call to `destructure` gets `TyOp(Add,[TyV(5,PrimT Int64T); TyLit (LitInt64 3L)])` or similar as the argument. It hits the `TyOp` branch and then performs a check against the CSE dictionary with is a standard map of type `Map<TypedExpr,TypedExpr> ref`.

The first time it happens in `inl q = x + 3`, the key is not present in the dictionary so what it does is calls `make_tyv_and_push_typed_expr` with the argument. It gets back something like `TyV(9,PrimT Int64T)`. That gets bound to the key and added to the dictionary like `Map.add (TyOp(Add,[TyV(5,PrimT Int64T); TyLit (LitInt64 3L)])) (TyV(9,PrimT Int64T))`.

The actual code is a tad more complicated.

```
let cse_add' d r x = let e = !d.cse_env in if r <> x then Map.add r x e else e
let cse_add d r x = d.cse_env := cse_add' d r x
```

There is a check to make sure that the variable being added to the map is not itself because that could lead to divergence, but otherwise the intent should be straightforward.

Once the new key and its value in the map, the program proceeds and `inl w = x + 3` is hit eventually. As the arguments flows through `destructure` the check for `TyOp(Add,[TyV(5,PrimT Int64T); TyLit (LitInt64 3L)])` and this time the key is present dictionary. Since it is present, instead calling `make_tyv_and_push_typed_expr` to perform let insertion, the expression just gets rewritten.

Note that this happens recursively. Here is the full `destructure` once again.

```
let rec destructure (d: LangEnv) (r: TypedExpr): TypedExpr = 
    let inline destructure r = destructure d r

    let inline cse_eval on_succ on_fail r = 
        match Map.tryFind r !d.cse_env with
        | Some x -> on_succ x
        | None -> on_fail r
    let inline cse_recurse r = cse_eval destructure id r

    let inline let_insert_cse r = 
        cse_eval 
            cse_recurse
            (fun r ->
                let x = make_tyv_and_push_typed_expr d r
                cse_add d r x
                x)
            r

    let index_tuple_args r tuple_types = 
        let unpack (s,i as state) typ = 
            if is_unit typ then tyt typ :: s, i
            else (destructure <| TyOp(ListIndex,[r;TyLit <| LitInt64 (int64 i)],typ)) :: s, i + 1
        List.fold unpack ([],0) tuple_types
        |> fst |> List.rev

    let env_unseal r x =
        let unseal (m,i as state) (k: string) typ = 
            if is_unit typ then Map.add k (tyt typ) m, i
            else
                let r = TyOp(MapGetField,[r; tyv(i,typ)], typ) |> destructure 
                Map.add k r m, i + 1
        Map.fold unseal (Map.empty,0) x |> fst

    let inline destructure_var r map_vvt map_funt =
        match get_type r with
        | ListT tuple_types -> tyvv(map_vvt tuple_types)
        | MapT (env,t) -> tymap(map_funt env, t)
        | _ -> cse_recurse r
    
    match r with
    | TyMap _ | TyList _ | TyLit _ -> r
    | TyBox _ -> cse_recurse r
    | TyT _ -> destructure_var r (List.map (tyt >> destructure)) (Map.map (fun _ -> (tyt >> destructure)) >> Env)
    | TyV _ -> destructure_var r (index_tuple_args r) (env_unseal r >> Env)
    | TyOp _ -> let_insert_cse r
    | TyJoinPoint _ | TyLet _ | TyState _ -> on_type_er (trace d) "Compiler error: This should never appear in destructure. It should go directly into d.seq."
```

And here it is pared down so that the CSE using parts are highlighted.

```
let rec destructure (d: LangEnv) (r: TypedExpr): TypedExpr = 
    let inline destructure r = destructure d r

    let inline cse_eval on_succ on_fail r = 
        match Map.tryFind r !d.cse_env with
        | Some x -> on_succ x
        | None -> on_fail r
    let inline cse_recurse r = 
        cse_eval 
            destructure // on_succ
            id // on_fail
            r

    let inline let_insert_cse r = 
        cse_eval 
            cse_recurse // on_succ
            (fun r -> // on_fail
                let x = make_tyv_and_push_typed_expr d r
                cse_add d r x
                x)
            r
    
    match r with
    | TyMap _ | TyList _ | TyLit _ -> r
    | TyBox _ -> cse_recurse r
    | TyOp _ -> let_insert_cse r
    | TyJoinPoint _ | TyLet _ | TyState _ -> on_type_er (trace d) "Compiler error: This should never appear in destructure. It should go directly into d.seq."
```

It is possible to simplify it even further.

```
let rec destructure (d: LangEnv) (r: TypedExpr): TypedExpr = 
    let inline destructure r = destructure d r
    let cse_recurse r = 
        match Map.tryFind r !d.cse_env with
        | Some x -> destructure x
        | None -> r

    let let_insert_cse r = 
        match Map.tryFind r !d.cse_env with
        | Some x -> cse_recurse x
        | None -> 
            let x = make_tyv_and_push_typed_expr d r
            cse_add d r x
            x
    
    match r with
    | TyMap _ | TyList _ | TyLit _ -> r
    | TyBox _ -> cse_recurse r
    | TyOp _ -> let_insert_cse r
    | TyJoinPoint _ | TyLet _ | TyState _ -> on_type_er (trace d) "Compiler error: This should never appear in destructure. It should go directly into d.seq."
```

This should hopefully be clear. `cse_recurse` does not call itself recursively. It does call `destructure` instead, but for all intents and purposes the function can thought of calling itself recursively.

Suppose that the dictionary is like the following (in pseudo-code):

```
TyOp(Add,[TyV(5,PrimT Int64T); TyLit (LitInt64 3L)]) => TyV(9,PrimT Int64T)
TyV(9,PrimT Int64T) => TyV(12,PrimT Int64T)
```

So `x+3` will first get rewritten to `TyV(9,PrimT Int64T)`. Then `destructure` will be called again.

The `| TyV _ -> destructure_var r (index_tuple_args r) (env_unseal r >> Env)` case will get hit. After that happens it will attempt to pattern match on this.

```
let inline destructure_var r map_vvt map_funt =
    match get_type r with
    | ListT tuple_types -> tyvv(map_vvt tuple_types)
    | MapT (env,t) -> tymap(map_funt env, t)
    | _ -> cse_recurse r
```

It is not `ListT` nor `MapT` so `cse_recurse` in the last branch gets called. Then `TyV(9,PrimT Int64T)` would get rewritten to `TyV(12,PrimT Int64T)`.

`let_insert_cse` is just the same as `cse_recurse` apart from the extra step of doing let insertion.

With this the let insertion and destructuring have been covered in full. The functions related to them serve as focal points of the entire language and with the understanding of them the paths to understanding of everything else become unblocked. In programming in general, programs tend to be easier to write than to read. 

Those two functions on the other hand are definitely on the exact opposite of the spectrum as they literally took months and months of refinement to make.

CSE rewriting has some additional uses which will be covered in the following chapter.

### 3: The If Statement

Spiral originally started out with the dynamic `if` as is found in most statically typed languages, but as an experiment the author decided to make the static the default one. After two month of use, he noted that not even once had he used the other version and when he ran into a bug with the dynamic `if` that cemented its removal for good from the language.

Here is the static `if` in full.

```
let if_static (d: LangEnv) (cond: Expr) (tr: Expr) (fl: Expr): TypedExpr =
    match tev d cond with
    | TyLit (LitBool true) -> tev d tr
    | TyLit (LitBool false) -> tev d fl
    | TyType (PrimT BoolT) as cond ->
        let b x = cse_add' d cond (TyLit <| LitBool x)
        let tr = tev_assume (b true) d tr
        let fl = tev_assume (b false) d fl
        let type_tr, type_fl = get_type tr, get_type fl
        if type_tr = type_fl then
            match tr, fl with
            | TyLit (LitBool true), TyLit (LitBool false) -> cond
            | _ when tr = fl -> tr
            | _ -> TyOp(IfStatic,[cond;tr;fl],type_tr) |> make_tyv_and_push_typed_expr_even_if_unit d
        else on_type_er (trace d) <| sprintf "Types in branches of If do not match.\nGot: %s and %s" (show_ty type_tr) (show_ty type_fl)
    | TyType cond -> on_type_er (trace d) <| sprintf "Expected a bool in conditional.\nGot: %s" (show_ty cond)
```

The first 2 cases should be straightforward.

```
let if_static (d: LangEnv) (cond: Expr) (tr: Expr) (fl: Expr): TypedExpr =
    match tev d cond with
    | TyLit (LitBool true) -> tev d tr
    | TyLit (LitBool false) -> tev d fl
```

At first the conditional is evaluated and if it turns out to be a literal only one of the branches is evaluated.

```
    | TyType (PrimT BoolT) as cond ->
        let b x = cse_add' d cond (TyLit <| LitBool x)
        let tr = tev_assume (b true) d tr
        let fl = tev_assume (b false) d fl
```

Here the two branches are evaluated with an little extra twist. Suppose an example like the following.

```
if x > 0 then
    if x > 0 then
        123
    else
        "qwe"
else
    456
```

In the outer if the conditional `x > 0` will evaluate to an abstract variable and in `if_static` the third branch will get hit.

Then once the conditional of the inner if is evaluated, what it will find in the CSE dictionary is that the `x > 0` can be rewritten to `true`.

```
        let b x = cse_add' d cond (TyLit <| LitBool x)
        let tr = tev_assume (b true) d tr
```

The above two lines are responsible for this.

Here is how `tev_assume` is implemented.

```
// #Type directed partial evaluation
let rec expr_peval (d: LangEnv) (expr: Expr): TypedExpr =
    let inline tev d expr = expr_peval d expr
    let inline apply_seq d x = !d.seq x
    let inline tev_seq d expr = let d = {d with seq=ref id; cse_env=ref !d.cse_env} in tev d expr |> apply_seq d
    let inline tev_assume cse_env d expr = let d = {d with seq=ref id; cse_env=ref cse_env} in tev d expr |> apply_seq d
```

`tev` itself just calls the main evaluation functions. The `expr_eval` function is one of the core passes of Spiral as that is where all the partial evaluation happens. The rest are parsing and code generation.

As `tev_seq` and `tev_assume` are very similar, it would be easier to start with an explanation of `tev_seq`.

In Spiral there are constructs such as if statements, case expressions and join points which require their own scope.

On entry therefore, the let statements that are being carried in the environment need to be cleared instead of being carried into the branch. `seq=ref id` is what represents this. If it was a standard list, it would just be set to `[]`, but here it is set to the identity function.

Then there is the CSE dictionary that also needs to be handled. Since Spiral is lexically scoped it is fine if the expressions inside the local scope get rewritten to expressions that came before it, but what should not happen after the scope is exited for expressions to get rewritten with the ones that are now out of scope.

```
if c then 
    inl q = x + 2
    ...
else
    ...

inl e = x + 2 // Do not want this to be rewritten to `q` inside the if statement.
```

If `cse_env` was a standard .NET mutable dictionary it would need to be copied at this point so that invariant is ensured, but since it is an F# immutable map, what should just be done is replacing the reference to it.

`tev_assume` has similar intent to `tev_seq` except the map for `cse_env` is taken as an argument; it is a more generic version of `tev_seq`.

With this it has been established that the two functions just evaluate an expression in its own scope.

`tev d expr |> apply_seq d`

After that evaluation is done, `apply_seq` is called. `tev` by itself only returns the last expression in the scope. After the call to `tev` the rest of the expressions can be found in `d.seq` where they have been let inserted.

```
let inline tev_seq (d: LangEnv) (expr: TypedExpr): TypedExpr = let d = {d with seq=ref id; cse_env=ref !d.cse_env} in tev d expr |> apply_seq d
```

Appending the last expression to it is quite easy. The last expression just need to be applied to `d.seq` and the result will be all the statements in scope neatly arranged. If the `d.seq` is empty, then it would be the identity function and so applying an expression to it would return the same thing; there wouldn't be any empty statements. This is why `(TypedExpr -> TypedExpr) ref` representation is so convenient.

```
| TyType (PrimT BoolT) as cond ->
    let b x = cse_add' d cond (TyLit <| LitBool x)
    let tr = tev_assume (b true) d tr
    let fl = tev_assume (b false) d fl
    let type_tr, type_fl = get_type tr, get_type fl
    if type_tr = type_fl then
        match tr, fl with
        | TyLit (LitBool true), TyLit (LitBool false) -> cond
        | _ when tr = fl -> tr
        | _ -> TyOp(IfStatic,[cond;tr;fl],type_tr) |> make_tyv_and_push_typed_expr_even_if_unit d
    else on_type_er (trace d) <| sprintf "Types in branches of If do not match.\nGot: %s and %s" (show_ty type_tr) (show_ty type_fl)
| TyType cond -> on_type_er (trace d) <| sprintf "Expected a bool in conditional.\nGot: %s" (show_ty cond)
```

Hopefully what the first 4 lines are doing now should now be clear.

```
let type_tr, type_fl = get_type tr, get_type fl
```

`get_type` is important so it will get its own treatment later, but its essence is in entirety described by its type which is `TypedExpr -> Ty`. That is, it gets the type of the expression.

```
if type_tr = type_fl then
```

Since the conditional is dynamic, the branches need to be compared for equality. Then a tad optimization is done after the check.

```
match tr, fl with
| TyLit (LitBool true), TyLit (LitBool false) -> cond
| _ when tr = fl -> tr
| _ -> TyOp(IfStatic,[cond;tr;fl],type_tr) |> make_tyv_and_push_typed_expr_even_if_unit d
```

The first case does trigger sometimes such as in structural equality for example. The second case just checks if both branches are equal. In that case obviously the if statement is unnecessary.

In the third branch is where the if statement gets let inserted. Note how `make_tyv_and_push_typed_expr_even_if_unit` needs to be called here. If that did not happen and the return was let to be grabbed by `destructure` it would not get printed if it had unit return and would be added to the CSE dictionary there.

That is not the desired behavior.

It is the desired behavior when destructuring unit types from a tuple, or with pure built-in arithmetic expressions, but not with if statements.

```
on_type_er (trace d) <| sprintf "Types in branches of If do not match.\nGot: %s and %s" (show_ty type_tr) (show_ty type_fl)
```

If the types of the branches do not match the above will raise a type error. Here is how it is implemented

```
let on_type_er trace message = TypeError(trace,message) |> raise
```

The exception just takes the trace and the messages and then gets raised.

#### Raising Type Errors

It is possible to raise a type error directly in Spiral.

```
if c then
    ...
else
    error_type "Some error."
```

If the conditional `c` is dynamic then the type error will always get triggered so this would not be very useful, but plenty of times functions of such form will get used with static conditionals. Plenty of times has `assert` been used in the tutorials.

Here is how the `assert` is implemented in the core library. There is a slightly more sophisticated version in `Extern` calls `show` as well.

```
inl assert c msg = 
    inl raise = 
        if lit_is c then error_type
        else failwith unit
    
    if c = false then raise msg
```

Here is how it is implemented in the partial evaluator.

```
| ErrorType,[a] -> tev d a |> fun a -> on_type_er (trace d) <| sprintf "%s" (show_typedexpr a)
```

So it is a simple `tev` and then the `on_type_er` gets called on the result of that. It is not particularly sophisticated, but then it does not have to be.

### 4: Boxing of Union Types

Some languages can afford to be minimalist like Lisp, but Spiral is not that sort of language. It depends on a large number of first class features working seamlessly together. Without union types and term casting it would be harder to ensure convergence and the language would be poorer as a result. The lack of such features in a language with first class staging like Spiral would immediately become obvious and the need for them would arise on its own.

The way boxing works is simple. If the argument being boxed is a subset of the type, then it gets wrapped in a `TyBox` otherwise it is a type error.

Any complexity in the following function is just from needing to handle the various edge cases.

```
let type_box (d: LangEnv) (typec: Expr) (args: Expr): TypedExpr =
    let typec & TyType ty, args = tev2 d typec args
    let substitute_ty = function
        | TyBox(x,_) -> tybox(x,ty)
        | x -> tybox(x,ty)

    let (|TyRecUnion|_|) = function
        | UnionT ty' -> Some ty'
        | RecT key -> Some (set_field (rect_unbox d key))
        | _ -> None
    
    match ty, get_type args with
    | x, r when x = r -> args
    | TyRecUnion ty', UnionT ty_args when Set.isSubset ty_args ty' ->
        let lam = inl' ["typec"; "args"] (op(Case,[v "args"; type_box (v "typec") (v "args")])) |> inner_compile
        apply d (apply d lam typec) args
    | TyRecUnion ty', x when Set.contains x ty' -> substitute_ty args
    | _ -> on_type_er (trace d) <| sprintf "Type constructor application failed. %s does not intersect %s." (show_ty ty) (get_type args |> show_ty)
```

To start things off, here is how `tev2` is implemented.

```
let inline tev2 d a b = tev d a, tev d b
let inline tev3 d a b c = tev d a, tev d b, tev d c
let inline tev4 d a b c d' = tev d a, tev d b, tev d c, tev d d'
```

Not a big deal.

```
let typec & TyType ty, args = tev2 d typec args
let substitute_ty = function
    | TyBox(x,_) -> tybox(x,ty)
    | x -> tybox(x,ty)
```

`substitute_ty` ty is fairly straightforward. The reason why it exists is because the argument being boxed might already by a `TyBox` in which case the box is just replaced.

```
let (|TyRecUnion|_|) = function
    | UnionT ty' -> Some ty'
    | RecT key -> Some (set_field (rect_unbox d key))
    | _ -> None
```

Since the type being boxed to must be a union type, the above active pattern is what is responsible for that. If the type being boxed is a straightforward union type, then it just returns it. But if the type being boxed is a recursive type then it is unrolled a single level and treated like a union type.

```
match ty, get_type args with
| x, r when x = r -> args
| TyRecUnion ty', UnionT ty_args when Set.isSubset ty_args ty' ->
    let lam = inl' ["typec"; "args"] (op(Case,[v "args"; type_box (v "typec") (v "args")])) |> inner_compile
    apply d (apply d lam typec) args
| TyRecUnion ty', x when Set.contains x ty' -> substitute_ty args
| _ -> on_type_er (trace d) <| sprintf "Type constructor application failed. %s does not intersect %s." (show_ty ty) (get_type args |> show_ty)
```

The first case is straightforward. It is true that types can only be boxed into union types, but there is no need to throw an error on `box int64 1` for example. In that case returning the original should be fine.

The second case is a bit less straightforward and deals with the case when a union type is being boxed with a larger union type as the target. Note the call to `Set.isSubset`. The description for it says `Evaluated to 'true' if all the elements of the first set are in the second.` What will happen in this case is that all the elements will get unboxed and then boxed into the new type.

The third case is straightforward and just does the substitution.

The fourth case is straightforward and just raises a type error.

### 5: Unboxing of Union Types

Originally CSE was added to Spiral in order to propagate unboxing information. In the following function that handles the entirety of unboxing, it will be shown how that works.

```
        let case_ d v case =
            let inline assume d v x branch = tev_assume (cse_add' d v x) d branch
            match tev d v with
            | a & TyBox(b,_) -> 
                cse_add d a b
                let r = tev d case
                cse_remove d a
                r
            | (TyV(_, t & (UnionT _ | RecT _)) | TyT(t & (UnionT _ | RecT _))) as v ->
                let make_up_vars_for_ty (l: Ty list): TypedExpr list = List.map (make_up_vars_for_ty d) l
                let map_to_cases (l: TypedExpr list): (TypedExpr * TypedExpr) list = List.map (fun x -> x, assume d v x case) l
                            
                match case_type d t |> make_up_vars_for_ty |> map_to_cases with
                | (_, TyType p) :: cases as cases' -> 
                    if List.forall (fun (_, TyType x) -> x = p) cases then 
                        TyOp(Case,v :: List.collect (fun (a,b) -> [a;b]) cases', p) 
                        |> make_tyv_and_push_typed_expr_even_if_unit d
                    else 
                        let l = List.map (snd >> get_type) cases'
                        on_type_er (trace d) <| sprintf "All the cases in pattern matching clause with dynamic data must have the same type.\nGot: %s" (listt l |> show_ty)
                | _ -> failwith "There should always be at least one clause here."
            | _ -> tev d case
```

Near the very top there is `tev_assume` that was also used in `if_static`.

```
        match tev d v with
        | a & TyBox(b,_) -> 
            d.cse_env := cse_add' d a b
            let r = tev d case
            d.cse_env := cse_remove' d a
            r
```

The first case is straightforward. If the type is a `TyBox _`, that is a staged boxed type instead of being fully boxed then the box is thrown away. `cse_env` is used to rewrite the variable `a` to what is inside it `b` as it goes to evaluate case. When if finishes evaluation the rewrite is removed and the result is returned.

```
        let rec case_type d args_ty =
            let union_case = function
                | UnionT l -> Set.toList l
                | _ -> [args_ty]
            match args_ty with
            | RecT key -> union_case (rect_unbox d key)
            | x -> union_case x
...
            | (TyV(_, t & (UnionT _ | RecT _)) | TyT(t & (UnionT _ | RecT _))) as v ->
                let make_up_vars_for_ty (l: Ty list): TypedExpr list = List.map (make_up_vars_for_ty d) l
                let map_to_cases (l: TypedExpr list): (TypedExpr * TypedExpr) list = List.map (fun x -> x, assume d v x case) l
                            
                match case_type d t |> make_up_vars_for_ty |> map_to_cases with
```

The 4 lines here make the meat of the function. First the boxed type is taken apart. If it is a union type then its member are returned in a list. If it is a recursive type then it is unrolled a level into a union type and then its members are returned into a list.

Here is an example in pseudo-code.

```
union {.Some, int64 | .None} -> [.Some, int64; .None]
```

Since raw types can't be in the case expression what is needed is to assign them variables. `make_up_vars_for_ty` is what does this.

Here is an example in pseudo-code.

```
.Some, int64 -> .Some, var int64
.None -> .None
```

Then those two members are mapped to their case bodies in `map_to_cases`.

This part might be a bit confusing. There aren't any case bodies strictly speaking; there is only one `case`. But that one `case` is in fact enough to cover all the pattern matching needs due to the magic of intensional polymorphism and first class staging. It will be shown later how the pattern compiler works and hopefully it will be clear why this works after that. As a short summary, the patterns are all CPS'd and the static ifs suffice to cover the entirety of the pattern matching needs.

```
                | (_, TyType p) :: cases as cases' -> 
                    if List.forall (fun (_, TyType x) -> x = p) cases then 
                        TyOp(Case,v :: List.collect (fun (a,b) -> [a;b]) cases', p) 
                        |> make_tyv_and_push_typed_expr_even_if_unit d
                    else 
                        let l = List.map (snd >> get_type) cases'
                        on_type_er (trace d) <| sprintf "All the cases in pattern matching clause with dynamic data must have the same type.\nGot: %s" (listt l |> show_ty)
                | _ -> failwith "There should always be at least one clause here."
```

By this point, all the important work has been done and the rest is error checking and let insertion for the code generator in the next phase of compilation.

```
            | _ -> tev d case
```

If the argument is not a caseable (union or recursive) type then the unboxing is skipped. The partial evaluation just proceeds as normal.

This is the entirety of unboxing. 

```
let inline assume d v x branch = tev_assume (cse_add' d v x) d branch
```

As a recap, consider how in the tutorial chapters it was warned against using join points in tandem with partial active patterns and union types. By now it should be clear why this is so. When a join point is entered, the rewriting information in `cse_env` is thrown away. Since `cse_env` is used to propagate unboxing information as shown in the above function, that is where the difficulty arises.

As it stands, unboxing in Spiral is fairly primitive. ML compilers go a lot further in optimizing pattern matching expressions than Spiral and in its current form, Spiral is not really suitable for writing runtime interpreters as its unboxing facilities are not developed enough.

For example, it has structural equality, but...

```
inl ty = .A \/ .B \/ .C \/ .E
inl a = box ty .A |> dyn
inl b = box ty .B |> dyn

a = b
```
```
type Union0 =
    | Union0Case0
    | Union0Case1
    | Union0Case2
    | Union0Case3
let rec method_0((var_0: Union0), (var_1: Union0)): bool =
    match var_0 with
    | Union0Case0 ->
        match var_1 with
        | Union0Case0 ->
            true
        | Union0Case1 ->
            false
        | Union0Case2 ->
            false
        | Union0Case3 ->
            false
    | Union0Case1 ->
        match var_1 with
        | Union0Case0 ->
            false
        | Union0Case1 ->
            true
        | Union0Case2 ->
            false
        | Union0Case3 ->
            false
    | Union0Case2 ->
        match var_1 with
        | Union0Case0 ->
            false
        | Union0Case1 ->
            false
        | Union0Case2 ->
            true
        | Union0Case3 ->
            false
    | Union0Case3 ->
        match var_1 with
        | Union0Case0 ->
            false
        | Union0Case1 ->
            false
        | Union0Case2 ->
            false
        | Union0Case3 ->
            true
let (var_0: Union0) = Union0Case0
let (var_1: Union0) = Union0Case1
method_0((var_0: Union0), (var_1: Union0))
```
This is n^2 compilation for structural equality for fully boxed types. It could be dealt with by introducing a different kind of case, but it is not a priority by any means at the moment.

For `Result` and `Option` types, what Spiral has currently has is sufficient. The rest can be done by propagating types at compile time.

### 6: Join Points

Along with `inl`ineables, join points are Spiral's most iconic feature and what make it the crystallization of the staged functional programming style. 

Currently there are 4 kinds of join points in total and this section will cover the vanilla one. The rest are quite similar to it though. The join point for term casting cannot be placed directly, instead `term_cast` does it.

Join points have a long history in Spiral. When work first started on Spiral the language originally had only inlineables, but some way of handling recursion had been necessary. So then the second thing that was added were `met`hods. They weren't join points at first, but a different kind of function that memoizes its result using a global dictionary.

There wasn't a single great point where methods suddenly evolved into join points. It took the author a considerable amount of refactoring work in order to separate the concept of functions and where they join at the conceptual level. He had never seen such a feature in any other language up to that point so it was quite hard for him to internalize them and how they fit into the language.

Regardless, join points are such an integral part of the whole staged functional programming experiences that language with staging, but no join points can be considered as simply toys and disregarded based on that fact.

There are 2 aspects of join points that need to be kept in mind in order to understand them.

1) Unused variable elimination.

```
inl a _ = ...
inl b _ = ...a... // `a` should be in `b`'s lexical environment when the function is called
inl c _ = ...a...b... // `a` and `b` should be in b's lexical environment when the function is called
...
```

```
inl a _ = ...
inl b _ = ... // empty env
inl c _ = ... // empty env
...
```

Spiral always does unused variable filtering at function boundaries and that is necessary to guarantee predicable convergence for join points.

If the environment always had everything it passed inside the join point, then those sucked in unused arguments would get specialized to as well which would not be good. This kind of optimization would not be necessary in dynamic languages, but is necessary in Spiral.

Strictly speaking, Spiral does not do immediate filtration. It tends to keep a list of unused variables and delay the filtration until the last possible moment, but the semantics of immediate and lazy unused argument filtration are intended to be the same. It is just intended to be a performance optimization.

2) Renaming.

If the filtered arguments into join points are something like `{a=var_3; b=var_6; c=var_10}` they should be renamed to `{a=var_0; b=var_1; c=var_2}` before the expression can be memoized. This is simple enough to do and took the author around 2.5 month in order to understand properly and that is despite thinking about it the whole time.

In practice, Spiral's renamer also collects the join point arguments along the way so the traversal is not repeated. It also collects the renamed arguments as well.

```
    let rec renamer_apply_env' (r: EnvRenamer) (C x) = Map.map (fun k -> renamer_apply_typedexpr' r) x
    and renamer_apply_typedexpr' ({memo=memo; renamer=renamer; ref_call_args=call_args; ref_method_pars=method_pars} as r) e =
        let inline f e = renamer_apply_typedexpr' r e
        let inline rename (n,t as k) =
            match renamer.TryGetValue n with
            | true, v -> v,t
            | false, _ ->
                let n' = renamer.Count
                renamer.Add(n,n')
                call_args := k :: !call_args 
                let k' = n', t
                method_pars := k' :: !method_pars
                k'

        match memo.TryGetValue e with
        | true, e -> e
        | false, _ ->
            match e with
            | TyT _ -> e
            | TyBox (n,t) -> tybox(f n,t)
            | TyList l -> tyvv(List.map f l)
            | TyMap(l,t) -> tymap(renamer_apply_env' r l |> consify_env_term, t)
            | TyV (n,t as k) ->
                let n', _ as k' = rename k
                if n' = n then e else tyv k'
            | TyLit _ -> e
            | TyJoinPoint _ | TyOp _ | TyState _ | TyLet _ -> failwith "Only data structures in the env can be renamed."
            |> fun x -> memo.[e] <- x; x
```

The above is in fact a map over `TypedExpr` that just does memoization using the `memo`. Memoization inside the renamer is an important optimization when doing renaming since the standard and core library function will repeat themselves significantly in nested functions.

There isn't too much need to dwell on the above function. It is merely big, but not particularly complicated and does what it is name says.

With that out of the way, here is the actual join point.

```
let join_point_method (d: LangEnv) (expr: Expr): TypedExpr = 
    let {call_args=call_arguments; method_pars=method_parameters; renamer'=renamer}, renamed_env = renamer_apply_env d.env
    let length = renamer.Count
    let join_point_key: Node<Expr * EnvTerm> = nodify_memo_key (expr, renamed_env)
    
    let ret_ty = 
        let d = {d with env=renamed_env; ltag=ref length}
        let join_point_dict: Dictionary<_,_> = join_point_dict_method
        match join_point_dict.TryGetValue join_point_key with
        | false, _ ->
            join_point_dict.[join_point_key] <- JoinPointInEvaluation ()
            let typed_expr = tev_method d expr
            join_point_dict.[join_point_key] <- JoinPointDone (method_parameters, typed_expr)
            typed_expr
        | true, JoinPointInEvaluation _ -> 
            tev_rec d expr
        | true, JoinPointDone (used_vars, typed_expr) -> 
            typed_expr
        |> get_type

    ty_join_point join_point_key JoinPointMethod call_arguments ret_ty 
    |> make_tyv_and_push_typed_expr_even_if_unit d
```

The `let join_point_key: Node<Expr * EnvTerm> = nodify_memo_key (expr, renamed_env)` line is a performance optimization so dictionary keys can be compared quickly. The actual key is `Expr * EnvTerm`, but a wrapper is put around it that ensures fast comparison.

Here is the `Node` type.

```
type Node<'a>(expr:'a, symbol:int) = 
    member x.Expression = expr
    member x.Symbol = symbol
    override x.ToString() = sprintf "<tag %i>" symbol
    override x.GetHashCode() = symbol
    override x.Equals(y) = 
        match y with 
        | :? Node<'a> as y -> symbol = y.Symbol
        | _ -> failwith "Invalid equality for Node."

    interface IComparable with
        member x.CompareTo(y) = 
            match y with
            | :? Node<'a> as y -> compare symbol y.Symbol
            | _ -> failwith "Invalid comparison for Node."
```

What `Node` does is allows two arbitrary expressions to be assigned an unique `symbol` and be compared using that.

After renaming and making the `join_point_key`, the body of the join point is what comes.

```
let ret_ty = 
    let d = {d with env=renamed_env; ltag=ref length}
```

Now the environment is assigned the renamed environment and the tag reference which is incremented every time a new variable is made is set to one past the highest argument in scope.

```
let join_point_dict: Dictionary<_,_> = join_point_dict_method
match join_point_dict.TryGetValue join_point_key with
| false, _ ->
    join_point_dict.[join_point_key] <- JoinPointInEvaluation ()
    let typed_expr = tev_method d expr
    join_point_dict.[join_point_key] <- JoinPointDone (method_parameters, typed_expr)
    typed_expr
```

This is the heart of the join point. In the first case which is hit when the join point is entered the first time, it sets the dictionary to indicate that the join point is being evaluated in case it is ever called recursively.

Then it calls `tev_method`. That particular method is similar to `tev_seq`.

```
let inline tev_seq d expr = let d = {d with seq=ref id; cse_env=ref !d.cse_env} in tev d expr |> apply_seq d
let inline tev_assume cse_env d expr = let d = {d with seq=ref id; cse_env=ref cse_env} in tev d expr |> apply_seq d
let inline tev_method d expr = let d = {d with seq=ref id; cse_env=ref Map.empty} in tev d expr |> apply_seq d
let inline tev_rec d expr = let d = {d with seq=ref id; cse_env=ref Map.empty; rbeh=AnnotationReturn} in tev d expr |> apply_seq d
```

As can be seen when entering a new scope, all 4 functions set the `seq` to identity. `tev_seq` and `tev_assume` when entering a new scope make a copy of the `cse_env`. `tev_method` and `tev_rec` which are join point functions outright set it to empty.

`tev_method` finishes running and returns the body of the join point, the join point is set to finished and the `typed_expr` is returned from the branch.

```
| true, JoinPointInEvaluation _ -> 
    tev_rec d expr
```

This particular case can only ever happen during recursion. If it hits it means the function has been called recursively. In order for it to not diverge it needs to get the return type.

The `tev_rec` is exactly like `tev_method` except for setting the return behavior to `rbeh=AnnotationReturn`.

There is only a single place where `rbeh` ever comes into play in the language. In the type annotation function.

```
let type_annot d a b =
    match d.rbeh with
    | AnnotationReturn -> tev_annot {d with rbeh=AnnotationDive} b
    | AnnotationDive ->
        let a, b = tev d a, tev_annot d b
        let ta, tb = get_type a, get_type b
        if ta = tb then a else on_type_er (trace d) <| sprintf "Type annotation mismatch.\n%s <> %s" (show_ty ta) (show_ty tb)
```

When `rbeh` is set to annotation return then it won't evaluate the body, but will just evaluate the right side of the annotation instead.

```
let inline tev_seq d expr = let d = {d with seq=ref id; cse_env=ref !d.cse_env} in tev d expr |> apply_seq d
let inline tev_annot d expr = let d = {d with seq=ref id; cse_env=ref !d.cse_env} in tev d expr
```

`tev_annot` is identical to `tev_seq` except it throws away the intermediate statements and only returns the final expression.

```
        | true, JoinPointDone (used_vars, typed_expr) -> 
            typed_expr
```

The last case does no evaluation and just returns the body.

```
        |> get_type

    ty_join_point join_point_key JoinPointMethod call_arguments ret_ty 
    |> make_tyv_and_push_typed_expr_even_if_unit d
```

This is the final stretch. The returned body gets converted into the return type `ret_ty` and then a join point is made and let inserted.

```
let ty_join_point key jp_type args ret_type = TyJoinPoint(key,jp_type,args,ret_type)
```

It might be more readable to make the join point directly, but a lot of the functions in the Spiral compiler that essentially act as the identity might have been been doing memoization in the past, so the author prefers to leave them alone in case inspiration hits him in order to make redesign easier. At any rate, the final part of the join point is extremely straightforward.

Before this chapter is done, a note is needed regarding the join point dictionaries.

```
// #Main
let spiral_peval (settings: CompilerSettings) (Module(N(module_name,_,_,_)) as module_main) = 
    let join_point_dict_method = d0()
    let join_point_dict_closure = d0()
    let join_point_dict_type = d0()
    let join_point_dict_cuda = d0()
```

They are initialized at the very start of compilation and hash via structural equality. Because passing state would be annoying otherwise and since they are only ever added to, they are present during the entire compilation. Parsing and prepass does not touch them, but the code generator reads from them.

Apart from the types and the core library, the entire compiler is a part of a single 3200 line function. The author didn't like stepping into that at first, but once he did so he found that the arrangement works surprisingly well and has had no trouble with it since then.

### 7: The Prepass and Unused Variable Filtering

The prepass works before partial evaluation, but after parsing. Its goal is to convert `Function`s to `FunctionFilt`s which have the set of used variables in their bodies. It also calls `pattern_compile` in order to compile the `Pattern`s into `Expr`s.

```
and Expr = 
    | V of Node<string>
    | Lit of Node<Value>
    | Pattern of Node<Pattern>
    | Function of Node<FunctionCore>
    | FunctionFilt of Node<Set<string> * Node<FunctionCore>>
    | VV of Node<Expr list>
    | Op of Node<Op * Expr list>
    | ExprPos of Pos<Expr>
```

The way it works is rather simple. Prepass is a rightwards fold + map over `Expr`. It also does memoization.

```
    and expr_prepass e =
        e |> memoize prepass_dict (fun e ->
            let inline f e = expr_prepass e
            match e with
            | V (N n) -> Set.singleton n, e
            | Op(N(op',l)) ->
                let l,l' = List.map f l |> List.unzip
                Set.unionMany l, op(op',l')
            | VV (N l) -> 
                let l,l' = List.map f l |> List.unzip
                Set.unionMany l, vv l'
            | FunctionFilt(N (vars,N(name,body))) ->
                Set.remove name vars, e
            | Function(N(name,body)) ->
                let vars,body = f body
                Set.remove name vars, func_filt(vars,nodify_func(name,body))
            | Lit _ -> Set.empty, e
            | Pattern pat -> pattern_compile pat
            | ExprPos p -> 
                let vars, body = f p.Expression
                vars, expr_pos p.Pos body
            )
```

In a nutshell, if it find a `V _` then it adds it to the set. If it finds a function it removes variable with the name of its binding from the set. It does it top to bottom so the thing works.

With this in mind, now it can be revealed what `EnvTerm` is.

```
and EnvTerm = 
| EnvConsed of ConsedNode<Map<string, TypedExpr>>
| Env of Map<string, TypedExpr>
| EnvUnfiltered of Map<string, TypedExpr> * Set<string>
```

In the previous versions of Spiral, the unused arguments were filtered every time `FunctionFilt` was reached, but the author decided he did not want the entire environment to be iterated over every time so he decided to add some laziness to the mix.

Every time `EnvTerm` is accessed it is either converted to `Env` if it is unfiltered or a check is made first against the used variable set. `EnvConsed` is similar to `Env`. The environment is [hash consed](https://en.wikipedia.org/wiki/Hash_consing) in the renamer as a performance optimization. To see how that works check out `SpiralHashConsing.fs`. For the understanding of language semantics understanding hash consing is not particularly important. It might be worth reevaluating whether hash consing is worth having since the evaluator has went through several iteration since that was useful. It might turn out that reference equality in the renamer is all that is needed.

```
let c = function
| Env env -> env
| EnvUnfiltered (env,used_vars) -> Map.filter (fun k _ -> used_vars.Contains k) env
| EnvConsed env -> env.node

let (|C|) x = c x
```

Whenever `C env` is seen as a part of the pattern, this is what happens. If it is unfiltered, then it gets filtered otherwise it is just extracted.

This might seem complicated, but the type system takes care that no mistakes are made when dealing with environments. It was really trivial to move from immediate to lazy filtering.

### 8: Pattern Compilation

```
let pattern_dict = d0()
let rec pattern_compile (pat: Node<_>) = 
    pat |> memoize pattern_dict (fun pat ->
        let node = pat.Symbol
        let pat = pat.Expression

        let new_pat_var =
            let mutable i = 0
            let get_pattern_tag () = 
                let x = i
                i <- i + 1
                x
            fun () -> sprintf " pat_var_%i_%i" node (get_pattern_tag())

        let rec pattern_compile (arg: Expr) (pat: Pattern) (on_succ: Expr) (on_fail: Expr): Expr =
            let inline cp arg pat on_succ on_fail = pattern_compile arg pat on_succ on_fail

            let pat_tuple_helper l =
                List.foldBack (fun pat (c,s,on_succ) -> 
                    let arg = new_pat_var()
                    c + 1, arg :: s,cp (v arg) pat on_succ on_fail) l (0,[],on_succ)
        
            match pat with
            | PatClauses l -> List.foldBack (fun (pat, exp) on_fail -> cp arg pat exp on_fail) l on_fail
            | PatE -> on_succ
            | PatVar x -> l x arg on_succ
            | PatTypeEq (exp,typ) ->
                let on_succ = cp arg exp on_succ on_fail
                if_static (eq_type arg typ) on_succ on_fail
                |> case arg
            | PatTuple l -> 
                let count, args, on_succ = pat_tuple_helper l
                list_taken_cps count arg on_fail (inl' args on_succ) |> case arg
            | PatCons l -> 
                let count, args, on_succ = pat_tuple_helper l
                list_taken_tail_cps count arg on_fail (inl' args on_succ) |> case arg
            | PatActive (a,b) ->
                let pat_var = new_pat_var()
                l pat_var (ap (v a) arg) (cp (v pat_var) b on_succ on_fail)
            | PatPartActive (a,pat) -> 
                let pat_var = new_pat_var()
                let on_succ = inl pat_var (cp (v pat_var) pat on_succ on_fail)
                let on_fail = inl "" on_fail
                ap' (v a) [arg; on_fail; on_succ]
            | PatOr l -> List.foldBack (fun pat on_fail -> cp arg pat on_succ on_fail) l on_fail
            | PatAnd l -> List.foldBack (fun pat on_succ -> cp arg pat on_succ on_fail) l on_succ
            | PatNot p -> cp arg p on_fail on_succ // switches the on_fail and on_succ arguments
            | PatXor l ->
                let state_var = new_pat_var()
                let state_var' = v state_var
                let bool x = lit <| LitBool x
                let rec just_one = function
                    | x :: xs -> 
                        let xs = just_one xs
                        inl state_var 
                            (cp arg x 
                                (if_static state_var' on_fail (ap xs (bool true))) // true case
                                (ap xs state_var')) // false case
                    | [] -> inl state_var (if_static state_var' on_succ on_fail)
                ap (just_one l) (bool false)
            | PatLit x -> 
                let x = lit x
                let on_succ = if_static (eq arg x) on_succ on_fail
                if_static (eq_type arg x) on_succ on_fail |> case arg
            | PatTypeLit x -> 
                if_static (eq_type arg (type_lit_lift x)) on_succ on_fail 
                |> case arg
            | PatTypeLitBind x -> 
                if_static (type_lit_is arg) (l x (type_lit_cast arg) on_succ) on_fail 
                |> case arg
            | PatWhen (p, e) -> cp arg p (if_static e on_succ on_fail) on_fail
            | PatModuleIs p -> module_is_cps arg on_fail (cp arg p on_succ on_fail) |> case arg
            | PatModuleMember name -> module_member_cps arg name on_fail (inl name on_succ) |> case arg
            | PatModuleRebind(name,b) -> 
                let arg' = new_pat_var()    
                module_member_cps arg name on_fail (inl arg' (cp (v arg') b on_succ on_fail)) 
                |> case arg
            | PatPos p -> expr_pos p.Pos (cp arg p.Expression on_succ on_fail)
            | PatTypeTermFunction(a,b) -> 
                let va, vb = new_pat_var(), new_pat_var()
                term_fun_dom_range_cps arg on_fail 
                <| inl' [va; vb] (cp (v va) a (cp (v vb) b on_succ on_fail) on_fail)
                
        let main_arg = new_pat_var()
        let arg = v main_arg
                
        let pattern_compile_def_on_succ = op(ErrorPatClause,[])
        let pattern_compile_def_on_fail = op(ErrorPatMiss,[arg])
        inl main_arg (pattern_compile arg pat pattern_compile_def_on_succ pattern_compile_def_on_fail) |> expr_prepass
        )
```

The pattern compilation function is a rather large function so it is worth going on it in turn. Its purpose is to turn a `Pattern` into an `Expr`ession for easier compilation down the road.

As a brief history of its development, this way of compiling or rather dealing with patterns came only after 4.5 months into Spiral's development and it represents one of the big breakthroughs that gives Spiral the expressiveness that it has now.

Before that Spiral had pattern matching and had so from the start, but instead of compiling `Pattern`s to `Expr`s what it did was try to evaluate them directly in the evaluator. That made dealing with them really, really difficult. Essentially, having another interpreted language directly in the evaluator made the patterns very non-composable and isolated from the rest of the language and during those times as a result Spiral could only deal with simple patterns that could be dealt completely at compile time. Without compiling them it would have been very difficult to implement partially static patterns such as for union types.

Also not doing compilation would necessitate that the prepass also take care of propagating variables through the `Pattern` which made it much more complicated.

Dealing with control flow in `Pattern`s is especially difficult when they are interpreted.

What `pattern_compile` does is not just convert them to `Expr`, but while it is doing that it also CPS's them.

It is worth going through the function top to bottom.

```
let pattern_dict = d0()
let rec pattern_compile (pat: Node<_>) = 
    pat |> memoize pattern_dict (fun pat ->
        let node = pat.Symbol
        let pat = pat.Expression

        let new_pat_var =
            let mutable i = 0
            let get_pattern_tag () = 
                let x = i
                i <- i + 1
                x
            fun () -> sprintf " pat_var_%i_%i" node (get_pattern_tag())
```

This is its entry. In order to take a precaution that a pattern is only compiled once, it is memoized. It slips the authors mind whether this is actually necessary, but here it is.

`new_pat_var` is just a tagger function that returns fresh names.

```
        let rec pattern_compile (arg: Expr) (pat: Pattern) (on_succ: Expr) (on_fail: Expr): Expr =
            let inline cp arg pat on_succ on_fail = pattern_compile arg pat on_succ on_fail
```

If there is a programing problem, the continuation passing style probably has a solution for it. Note that using the `on_fail on_succ` order is preferable to `on_succ on_fail`, but the author got careless this time. This would be fixable, but the type system will not be as helpful as usual because both are of type `Expr` so refactoring is dangerous here. Modules were added in Spiral in order to make deciding function argument order easier and take care of situations like this. The modules are also there for extensibility. In F# since the type system is always on hand it is better to rely on it instead, though the author does hope that Spiral will inspire F# to improve its record syntax.

Instead of diving directly into the function, it would be worth to start near the beginning just before it is called.

```
            let main_arg = new_pat_var()
            let arg = v main_arg
                    
            let pattern_compile_def_on_succ = op(ErrorPatClause,[])
            let pattern_compile_def_on_fail = op(ErrorPatMiss,[arg])
            inl main_arg (pattern_compile arg pat pattern_compile_def_on_succ pattern_compile_def_on_fail) |> expr_prepass
```

Always, the pattern is wrapped in an inlineable, and its main argument is passed into the function. By default, the on_succ and on_fail are errors which will trigger if the evaluator ever reaches them.

`PatClauses` is always the first to trigger and is never found inside another pattern.

```
| PatClauses of (Pattern * Expr) list
```
```
            match pat with
            | PatClauses l -> List.foldBack (fun (pat, exp) on_fail -> cp arg pat exp on_fail) l on_fail
```

Compilation of the pattern is a rightwards fold, meaning it proceeds from the end to the beginning.

`PatClauses` could be simplified by turning it into a combination of the `PatAnd` and the `PatOr` patterns. The way patterns work is as follows - if the pattern is successful, in the later stage the evaluator will call `on_succ`. Hence why `exp` replaces the default `on_succ` in `PatClauses`.

On the other hand if the evaluator fail to find a match, it will call `on_fail`. It can be visualized like so:

```
pat1 pat2 pat3 -> ...
pat4 -> ...
pat5 -> ...
```

Evaluator always starts at `pat1` and then moves to `pat2` if is successful. If it is a failure it goes to `pat4` and works from there.

That the `pattern_compile` does is through the magic of CPS give the evaluator a choice of simply having the continuations there for it to call. It compiles the patterns into such a form that the desired behavior happens.

```
| PatE -> on_succ
```

Suppose a pattern like `inl _ -> body` directly in the language. There is only one clause and only `PatE` here.

So based on what is known it could be expected that it would get compiled as something like `inl main_arg -> on_succ`. `on_succ` is just the body here and so it works.

```
| PatVar x -> l x arg on_succ
```

`PatVar` is similar, but it it the arg must be bound to `x` first.

So `inl x -> body` would be compiled to something like (in pseudo-code):

```
inl pat_var_0_0 -> 
    inl x = pat_var_0_0
    body
```

Here is how `l`et is implemented in the language.

```
let l v b e = ap (inl v e) b
```

Let statements can easily be desugared into function abstraction and then application. In HM style languages there are some advantages to having separate let statements for the sake of generalization, but not in Spiral so it choses the easiest route. It made debugging a pain since it turns the program inside out and hence unprintable in raw form, but that does not matter anymore.

#### Type Equality Pattern

```
            | PatTypeEq (exp,typ) ->
                let on_succ = cp arg exp on_succ on_fail
                if_static (eq_type arg typ) on_succ on_fail
                |> case arg
```

Type equality is the `:` operator when on the pattern side. How this works could be written directly in the language itself as the following.

```
function
| _: int64 -> body1
| _: float32 -> body2
| _: string -> body3
```
```
inl pat_var_1_0 ->
    if eq_type pat_var_1_0 int64 then body1
    elif eq_type pat_var_1_0 float32 then body2
    elif eq_type pat_var_1_0 string then body3
    else on_fail // pattern miss
```

#### Tuples

```
let list_taken_cps count arg on_fail on_succ = op(ListTakeNCPS,[lit (LitInt32 count);arg;on_fail;on_succ])
let list_taken_tail_cps count arg on_fail on_succ = op(ListTakeNTailCPS,[lit (LitInt32 (count-1));arg;on_fail;on_succ])
...
            let pat_tuple_helper l =
                List.foldBack (fun pat (c,s,on_succ) -> 
                    let arg = new_pat_var()
                    c + 1, arg :: s,cp (v arg) pat on_succ on_fail) l (0,[],on_succ)
...
            | PatTuple l -> 
                let count, args, on_succ = pat_tuple_helper l
                list_taken_cps count arg on_fail (inl' args on_succ) |> case arg
```

`pat_tuple_helper` takes the count of the pattern, makes up new pattern variables for each of the pattern arguments and also recursively maps the inner pattern.

Here is an example of how this would work. The `|> case` will be pretended not to exist for the sake of brevity.

```
inl a,b,c -> body
```
```
inl pat_var_2_0 -> // pat_var_2_0 is main_arg
    !ListTakeNCPS(
        3i32
        ,pat_var_2_0 
        ,inl pat_var_2_1 pat_var_2_2 pat_var_2_3 -> 
            inl a = pat_var_2_1
            inl b = pat_var_2_2
            inl c = pat_var_2_3
            body
        ,on_fail // pattern miss
        )
```

As you can see in the above, the pattern compiler is a bit stupid and is doing redundant bindings. This has no runtime impact as `inl`ineables get inlined period, but does cause slowdown at compile time.

Here an example with multiple patterns. 

```
function
| a,b,c -> body1
| q,w -> body2
```
```
inl pat_var_3_0 -> // pat_var_3_0 is main_arg
    !ListTakeNCPS(
        3i32
        ,pat_var_3_0
        ,inl pat_var_3_1 pat_var_3_2 pat_var_3_3 -> 
            inl a = pat_var_3_1
            inl b = pat_var_3_2
            inl c = pat_var_3_3
            body1
        ,!ListTakeNCPS(
            2i32
            ,pat_var_3_0
            ,inl pat_var_3_4 pat_var_3_5 -> 
                inl q = pat_var_3_4
                inl w = pat_var_3_5
                body2
            ,on_fail // pattern_miss
            )
        )
```
Here is how `ListTakeN` operation is implemented in the evaluator.

```
let inline list_taken_template op_name loop d a arg on_fail on_succ = 
    match tev d a with
    | TyLitIndex c -> 
        match tev d arg with
        | TyList args -> loop [] (c,args)
        | _ -> tev d on_fail
    | x -> on_type_er (trace d) "Expected an int literal as the first input to %s.\nGot: %s" op_name (show_typedexpr x)

let list_taken d a arg on_fail on_succ =
    let rec loop args = function
        | 0,[] -> List.foldBack (fun arg on_succ -> apply d on_succ arg) args (tev d on_succ)
        | _,[] | 0, _ -> tev d on_fail
        | c,x :: x' -> loop (x :: args) (c-1,x')
    list_taken_template "ListTakeN" loop d a arg on_fail on_succ
```

Figuring this out will take some specialization by hand.

```
let list_taken (d: LangEnv) (a: Expr) (arg: Expr) (on_fail: Expr) (on_succ: Expr) = 
    match tev d a with
    | TyLitIndex c -> 
        match tev d arg with
        | TyList args -> 
            let rec loop args = function
                | 0,[] -> List.foldBack (fun arg on_succ -> apply d on_succ arg) args (tev d on_succ)
                | _,[] | 0, _ -> tev d on_fail
                | c,x :: x' -> loop (x :: args) (c-1,x')
            loop [] (c,args)
        | _ -> tev d on_fail
    | x -> on_type_er (trace d) "Expected an int literal as the first input to ListTakeN.\nGot: %s" (show_typedexpr x)
```

The recursive loop is a bit convoluted so it is worth going through step by step. The loop can be split into two phases:

1) Making sure that the tuple has exactly the specified number of arguments.

```
                | _,[] | 0, _ -> tev d on_fail
                | c,x :: x' -> loop (x :: args) (c-1,x')
```
That is ensured by this particular segment.

2) Assuming the number arguments in the tuple is correct, then they are all applied to the `on_succ` in correct order.

```
                | 0,[] -> List.foldBack (fun arg on_succ -> apply d on_succ arg) args (tev d on_succ)
```

Why does the loop reverse the argument list before applying it? Absolutely no reason.

If this were the tutorials, the author would quietly fix this, but since the user guide is meant for power users, it would be worth showing how the compiler can be improved in action. Here is the ammended function.

```
let list_taken (d: LangEnv) (a: Expr) (arg: Expr) (on_fail: Expr) (on_succ: Expr) = 
    match tev d a with
    | TyLitIndex c -> 
        match tev d arg with
        | TyList args -> 
            let rec loop = function
                | 0,[] -> List.fold (fun on_succ arg -> apply d on_succ arg) (tev d on_succ) args
                | _,[] | 0, _ -> tev d on_fail
                | c,_ :: x' -> loop (c-1,x')
            loop (c,args)
        | _ -> tev d on_fail
    | x -> on_type_er (trace d) "Expected an int literal as the first input to ListTakeN.\nGot: %s" (show_typedexpr x)
```

This actually sped up the compiler by a few % since the tuple pattern is so frequently used.

Moving on to the cons pattern.

```
            | PatCons l -> 
                let count, args, on_succ = pat_tuple_helper l
                list_taken_tail_cps count arg on_fail (inl' args on_succ) |> case arg
```

It is quite similar to the standard tuple pattern.

So is how it is implemented in the evaluator.

```
let list_taken_tail d a arg on_fail on_succ = 
    match tev d a with
    | TyLitIndex c -> 
        match tev d arg with
        | TyList args -> 
            let rec loop args = function
                | 0,x' -> List.foldBack (fun arg on_succ -> apply d on_succ arg) (tyvv x' :: args) (tev d on_succ)
                | _,[] -> tev d on_fail
                | c,x :: x' -> loop (x :: args) (c-1,x')
            loop [] (c,args)
        | _ -> tev d on_fail
    | x -> on_type_er (trace d) "Expected an int literal as the first input to ListTakeNTail.\nGot: %s" (show_typedexpr x)
```

Note that in the cons pattern now collecting the leftover arguments has a definite purpose.

#### Active Patterns

```
            | PatActive (a,b) ->
                let pat_var = new_pat_var()
                l pat_var (ap (v a) arg) (cp (v pat_var) b on_succ on_fail)
```

Here is an example how this works.

```
inl !dyn x -> body
```
```
inl pat_var_4_0 ->
    inl pat_var_1 = dyn pat_var_4_0
    inl x = pat_var_1
    body
```

Here is the partial active pattern.

```
            | PatPartActive (a,pat) -> 
                let pat_var = new_pat_var()
                let on_succ = inl pat_var (cp (v pat_var) pat on_succ on_fail)
                let on_fail = inl "" on_fail
                ap' (v a) [arg; on_fail; on_succ]
```

Here is an example of its compilation in action.

```
inl f arg on_fail on_succ = on_succ arg
inl @f x -> body
```
```
inl pat_var_5_0 ->
    inl on_succ pat_var_5_1 = 
        inl x = pat_var_5_1
        body
    inl on_fail _ = on_fail // pattern miss
    f pat_var_5_0 on_fail on_succ
```

Spiral's ability to propagate information deeply makes it easy to compile active patterns to CPS'd control flow.

#### Boolean Patterns

```
            | PatOr l -> List.foldBack (fun pat on_fail -> cp arg pat on_succ on_fail) l on_fail
            | PatAnd l -> List.foldBack (fun pat on_succ -> cp arg pat on_succ on_fail) l on_succ
            | PatNot p -> cp arg p on_fail on_succ // switches the on_fail and on_succ arguments
```

`PatOr` links the patterns through the `on_fail`. `PatAnd` links the arguments through the `on_succ`. `PatNot` just switches the arguments.

The examples won't be provided for the above 3, the users can rest assured that they are doing their job.

```
            | PatXor l ->
                let state_var = new_pat_var()
                let state_var' = v state_var
                let bool x = lit <| LitBool x
                let rec just_one = function
                    | x :: xs -> 
                        let xs = just_one xs
                        inl state_var 
                            (cp arg x 
                                (if_static state_var' on_fail (ap xs (bool true))) // true case
                                (ap xs state_var')) // false case
                    | [] -> inl state_var (if_static state_var' on_succ on_fail)
                ap (just_one l) (bool false)
```

`PatXor` is definitely the most complex of all the patterns. Currently it can only be placed inside the module pattern. It is also misleadingly named as unlike the boolean pattern, it cannot be used to flip back and forth.

For `pat0 ^ pat1 ... ^ patn` it ensures that only one of the patterns triggers. And is false when there are no patterns.

It is a bit annoying to sketch out what it does hand by hand, so as an alternative consider this example written in F#.

```
let rec just_one state = function
    | true :: xs -> if state then false else just_one true xs
    | false :: xs -> just_one state xs
    | [] -> state

just_one false [] // false
just_one false [true;false] // true
just_one false [false;false] // false
just_one false [false;true;false] // true
just_one false [false;false;true] // true
just_one false [true;false;true] // false
```

What the `PatXor` does is translate the above into execution flow for the evaluator. It is just one example of how compilation makes composing languages much easier than interpretation. This is also an argument in favor of powerful type systems such as Spiral's because they make composition much easier. After a certain point this translates into a permanent advantage beyond what even dynamic languages offer due to the ability to propagate information through language boundaries.

#### Literal Patterns

```
            | PatLit x -> 
                let x = lit x
                let on_succ = if_static (eq arg x) on_succ on_fail
                if_static (eq_type arg x) on_succ on_fail |> case arg
```

At some point in the future these might get their own dedicated CPS operations just like tuples, but it is interesting to study them the way they are right now. If the later this changes in the source there is no need to be surprised. The meaning of the pattern won't change.

What is happening above is that first a type equality check is made (otherwise the equality could throw a type error) and then the equality is tested for. Here is how that would compile.

```
function
| 1 -> body1
| 2 -> body2
```
```
inl pat_var_6_0 ->
    if eq_type pat_var_6_0 1 then
        if pat_var_6_0 = 1 then body1
        else 
            if eq_type pat_var_6_0 2 then
                if pat_var_6_0 = 2 then body2
                else on_fail // pattern miss
            else on_fail // pattern miss

    elif eq_type pat_var_6_0 2 then
        if pat_var_6_0 = 2 then body2
        else on_fail // pattern miss
    else on_fail // pattern miss
```

The above demonstrates why would it be good to replace the operation with a single CPS'd operation. 

```
            | PatTypeLit x -> 
                if_static (eq_type arg (type_lit_lift x)) on_succ on_fail 
                |> case arg
```

Since type literals are types they are compared using `eq_type` rather than `=`. Trying to do so with `=` would cause a type error.

```
            | PatTypeLitBind x -> 
                if_static (type_lit_is arg) (l x (type_lit_cast arg) on_succ) on_fail 
                |> case arg
```

The language has specialized operations in order to cast type literals to the term level like it is possible with functions. The full list of `Op`eratives will be in the reference.

```
let type_lit_cast x = (TypeLitCast,[x]) |> op
```

Here is how `type_lit_cast` is implemented. The following is how it is done in the evaluator.

```
let type_lit_cast d a =
    match tev d a with
    | TyT (LitT x) -> TyLit x
    | _ -> on_type_er (trace d) "Expected a literal in type literal cast."
```

Type literals are always expected to be naked types.

#### Module Patterns

```
            | PatModuleIs p -> module_is_cps arg on_fail (cp arg p on_succ on_fail) |> case arg
            | PatModuleMember name -> module_member_cps arg name on_fail (inl name on_succ) |> case arg
            | PatModuleRebind(name,b) -> 
                let arg' = new_pat_var()    
                module_member_cps arg name on_fail (inl arg' (cp (v arg') b on_succ on_fail)) 
                |> case arg
```

The patterns remaining are nearly over. Explicit examples won't be provided for the above as it should be straightforward by now. As a note, `PatModuleIs` is only created when the module pattern is empty.

Here is how `module_is_cps` is implemented in the evaluator.

```
let module_is_cps d a on_fail on_succ =
    match tev d a with
    | M(_,_,MapTypeModule) -> tev d on_succ
    | _ -> tev d on_fail
```

Here is the other one.

```
let module_member_cps d a b on_fail on_succ =
    match tev2 d a b with
    | M(layout,env_term,MapTypeModule) & recf, b -> 
        match b with
        | TyLit (LitString n) -> 
            let inline unpack k = v_find env_term n (fun () -> tev d on_fail) k
            match layout with
            | None -> unpack (apply d (tev d on_succ))
            | _ -> unpack (apply d (tev d on_succ) << layout_boxed_unseal d recf)
        | x -> on_type_er (trace d) <| sprintf "Expecting a string as the second argument to ModuleMemberCPS.\nGot: %s" (show_typedexpr x)
    | x,_ -> tev d on_fail
```

Whatever complexity is here, it is because the module pattern has to also work on layout types. Layout types need to be explicitly unsealed instead of reached into directly.

Here is how `v_find` is implemented.

```
let inline v_find env x on_fail on_succ = 
    let run env = 
        match Map.tryFind x env with
        | Some v -> on_succ v
        | None -> on_fail()
    match env with
    | Env env -> run env
    | EnvConsed env -> run env.node
    | EnvUnfiltered (env, used_vars) -> if used_vars.Contains x then run env else on_fail()
```

It roughly as one would expect. It takes an `env` and two continuations and runs them depending on whether it finds the member in the dictionary.

With that in mind, here is `unpack` again with the type of `k` added.

```
let inline unpack (k: TypedExpr -> TypedExpr) = v_find env_term n (fun () -> tev d on_fail) k
```

`k` is merely the `on_succ` argument.

If the code is unclear then what the suggested course of action is to try inlining the code by hand and seeing if starts making sense.

#### When Pattern

```
            | PatWhen (p, e) -> cp arg p (if_static e on_succ on_fail) on_fail
```

This one is straightforward. The author had more difficulty parsing it than implementing it. What it does is first tries the left pattern side and makes the `on_succ` argument the when check.

#### Term Function Type Pattern

```
            | PatTypeTermFunction(a,b) -> 
                let va, vb = new_pat_var(), new_pat_var()
                term_fun_dom_range_cps arg on_fail 
                <| inl' [va; vb] (cp (v va) a (cp (v vb) b on_succ on_fail) on_fail)
```

This the `=>` pattern that was covered during the implementation of `closure_of`. Here is how it is implemented in the evaluator.

```
        let term_fun_dom_range_cps d x on_fail on_succ =
            match tev d x with
            | TyType(TermFunctionT (a,b)) -> 
                let on_succ = tev d on_succ    
                apply d (apply d on_succ (tyt a)) (tyt b)
            | x -> tev d on_fail
```

It a pretty straightforward mapping. It extracts the types and applies them to the `on_succ` continuation otherwise it calls `on_fail`.

#### The Pos Pattern

```
| PatPos p -> expr_pos p.Pos (cp arg p.Expression on_succ on_fail)
```

This one is not placed by the user but by the parser. This pattern is converted into the `ExprPos` node.

Here is how it looks like in the evaluator.

```
let inline add_trace (d: LangEnv) x = {d with trace = x :: d.trace}
...
| ExprPos p -> tev (add_trace d p.Pos) p.Expression
```

The trace is of course what the user gets when he makes a type error.

#### A Note On Case

For the purpose of simplification whenever `|> case arg` has appeared, it has been ignored.

```
let case arg case = (Case,[arg;case]) |> op
```

What it does has been covered in the `Unboxing of Union Types` chapter of the user guide.

### 9: Layout Types

Layout types came relatively late in Spiral's development, about six months in. The reason why that is remarkable is because the author thought for a long time on how to do them and eventually concluded that they were impossible. The reason for that is because having them would mean having to essentially capture chunks of scope and how could something like that possibly be done?

Quite simply.

First, it is time to take the lid off the list of all the `Ty`pes in Spiral. All of these have been demonstrated so far.

```
and Ty =
    | PrimT of PrimitiveType
    | ListT of Ty list
    | LitT of Value
    | MapT of EnvTy * MapType // function or module
    | LayoutT of LayoutType * EnvTerm * MapType
    | TermFunctionT of Ty * Ty
    | UnionT of Set<Ty>
    | RecT of JoinPointKey
    | ArrayT of ArrayType * Ty
    | DotNetTypeT of TypedExpr // macro
    | CudaTypeT of TypedExpr // macro
```

Note the similarities between `MapT` and `LayoutT`.

The type of `EnvTy` is `Map<string,Ty>`. While `EnvTerm` can be thought of as `Map<string,TypedExpr>`. `LayoutType` is rather simple.

```
type LayoutType =
    | LayoutStack
    | LayoutPackedStack
    | LayoutHeap
    | LayoutHeapMutable
```

It is just some extra data denoting what kind of type it is. Functionally though, `MapT` and `LayoutT` can be thought of as duals of each other. `LayoutT` is just a map with a bit more information propagated in it.

Here is how the types are created.

```
let layout_to_none' d = function
    | TyMap _ as a -> a
    | TyType(LayoutT(_,env,t)) as a -> tymap(layout_env_term_unseal d a env,t)
    | x -> on_type_er (trace d) <| sprintf "Cannot turn the argument into a non-layout type. Got: %s" (show_typedexpr x)
let layout_to_none d a = layout_to_none' d (tev d a)

let rec layoutify (layout: LayoutType) (d: LangEnv) = function
    | TyMap(env,t) as a ->
        let {renamer'=r}, env' = renamer_apply_envc env
        if r.Count = 0 then LayoutT(layout,env',t) |> tyt
        else TyOp(layout_to_op layout,[a],LayoutT(layout,env',t)) |> destructure d
    | TyType(LayoutT(layout',_,_)) as a ->
        if layout <> layout' then layout_to_none' d a |> layoutify layout d else a
    | x -> on_type_er (trace d) <| sprintf "Cannot turn the argument into a layout type. Got: %s" (show_typedexpr x)
let layout_to layout d a = layoutify layout d (tev d a)
```

The above could be summarized as - run the renamer and then plug the resulting environment into the `LayoutT`. This is exactly what the join points do for their keys.

If one can understand renaming then one can understand layout types.

The only remaining thing is to show how the can be unsealed.

```
let rec layout_boxed_unseal d recf x =
    let inline f x = layout_boxed_unseal d recf x
    match x with
    | TyV _ as v -> TyOp(MapGetField,[recf;v],get_type v) |> destructure d
    | TyList l -> tyvv (List.map f l)
    | TyBox(a,b) -> tybox (f a, b)
    | TyMap(env, b) -> tymap (layout_env_term_unseal d recf env, b)
    | x -> x
        
and layout_env_term_unseal d recf (C env) = Map.map (fun _ -> layout_boxed_unseal d recf) env |> Env
```

`recf` is in the above two functions is just the variable being unsealed.

Unsealing layout types is a standard map over `TypedExpr`. Note that if the unsealing is done twice on the same variable in local scope, CSE will prevent duplicate work from being done.

```
TyOp(MapGetField,[recf;v],get_type v) |> destructure d
```

This is due to `destructure`. It will send the `TyOp`s to the CSE dictionary and will remember if they tried to be unsealed again.

This is what is needed to understand layout types. With this all the features that are core to the full Spiral experience have been covered in depth.

`layout_boxed_unseal` and `layout_env_term_unseal` are used in various places throughout the evaluator when opening the layout types is needed. This won't be covered in the user guide.

If you've managed to get this far, by now you are reasonably familiar with not just the language, but with its internals as well.

Past this if you would like to know more, looking into the source is a reasonable option. Spiral is not a huge overbearing monolith of a language with hundreds of thousands of lines of code. The evaluator itself is at the time of writing 1.6k lines long. It is not commented, but a significant amount of effort has gone into refactoring it and you can be sure that if there is a feature in the language, you can find it in just one place in the evaluator.

Well, apart from the `-` and `.` operators in the parsing stage. The author hates those things.

If that is not enough, just ask him.

### 10: Macros

In Spiral macros can be treated as types and used to instantiate types which makes them unique in a statically typed language. They are definitely not needed to understand Spiral, but they should be in the user guide nonetheless.

```
let inline codegen_macro' show_typedexpr codegen print_type x = 
    let strb = StringBuilder()
    let inline append (x: string) = strb.Append x |> ignore
    let (|LS|) = function
            | TyLit (LitString x) | TypeString x -> x
            | _ -> failwithf "Iter's first three arguments must be strings."
    let er x = failwithf "Unknown argument in macro. Got: %s" (show_typedexpr x)
    let rec f = function
        | TyList [TypeString "text"; LS x] -> append x
        | TyList [TypeString "arg"; x] -> append (codegen x)
        | TyList [TypeString "args"; TyTuple l] -> append "("; List.map codegen l |> String.concat ", " |> append; append ")"
        | TyList [TypeString "fs_array_args"; TyTuple l] -> append "[|"; List.map codegen l |> String.concat "; " |> append; append "|]"
        | TyList [TypeString "type"; TyType x] -> append (print_type x)
        | TyList [TypeString "types"; TyTuple l] -> append "<"; List.map (get_type >> print_type) l |> String.concat ", " |> append; append ">" 
        | TyList [TypeString "iter"; TyList [LS begin_;LS sep;LS end_;ops]] ->
                append begin_
                match ops with
                | TyList (x :: xs) -> f x; List.iter (fun x -> append sep; f x) xs
                | TyList [] -> ()
                | x -> er x
                append end_
        | x -> er x
    match x with
    | TyList x -> List.iter f x
    | x -> er x
    strb.ToString()
```

This function definitely looks like it came from the code generation phase and that would be correct. `show_typedexpr` is just there for printing errors. `codegen` for printing `TypedExpr`s and `print_type` for printing `Ty`pes are provided by the evaluator and the code generators separately and that allows macros to be printed in one way inside errors and in another way in code while retaining the same structure underneath.

Macros are an ad-hoc feature there just for the sake of language interoperability and while needed, they should not be considered core to the language nor should be used for abstraction. They should be redone into something more principled.

As it is their functionality just arose from needs of various sorts.

List of operations:

1) `text`

Takes in a literal or a type string literal and prints it.

2) `arg`

Takes in an argument and prints it.

2) `args`

Takes in an argument or a tuple of arguments and prints them between parentheses and separated by commas.

3) `fs_array_args`

Takes in an argument or a tuple of arguments and prints them between `[|` and `|]` and separated by semicolons.

4) `type`

Takes in an argument and prints its type.

5) `types`

Takes in an argument or a tuple of arguments and prints their type them between `<` and `>` and separated by commas.

6) `iter`

It takes in the opener, separator and closer as strings or type level strings and a tuple of macro operations and executes them while printing the separator in-between them and the opener and the closer at the beginning and the end respectively.

#### Parser Macros

There are only 3 of them.

```
let case_parser_macro expr = 
    inbuilt_op_core '@' >>= fun a ->
        match a with
        | "PathCuda" -> settings.path_cuda90 |> LitString |> lit |> preturn
        | "PathCub" -> settings.path_cub |> LitString |> lit |> preturn
        | "PathVS2017" -> settings.path_vs2017 |> LitString |> lit |> preturn
        | a -> failFatally <| sprintf "%s is not a valid parser macro." a
```

They are invoked like `inl path_cuda = @PathCuda` and so on. They are just there as a safeguard in case join point dictionaries ever get floated up a level. If that happens changing the compiler setting might give incorrect results if the paths were inserted during the partial evaluation stage since join points would not recognize the changes.

### 11: Operatives and the Core Library

Invoking operatives is the main way the user communicates with the partial evaluator. In Spiral most operations are virtualized and compile to calls to core library functions.

Here is how it can be done.

```
inl (+) a b = !Add(a,b)
```

The `Add` is an `Op`erative and the `!` is the operative invocation unary operator. At the time of writing there are 93 `Op`eratives in total. A full list of them can be found with the rest of the types in `SpiralTypes.fs`.

The core library can be found in `SpiralCoreLib.fs`. It is opened automatically every time the program is compiled.

Here is a copy of it for easy perusal.

```
module Spiral.CoreLib

let module_ = Types.module_
let core =
    (
    "Core",[],"The Core module.",
    """
/// Lifts a literal to the type level.
inl type_lit_lift x = !TypeLitCreate(x)

/// Raises a type error.
inl error_type x = !ErrorType(x)
/// Prints an expression at compile time.
inl print_static x = !PrintStatic(x)
/// Pushes the expression to runtime.
inl dyn x = !Dynamize(x)
/// Creates a term function with the given two types.
inl (=>) a b = !TermFunctionTypeCreate(a,b)
/// Splits the union or recursive type into a tuple.
inl split x = !TypeSplit(x)
/// Boxes a type.
inl box a b = !TypeBox(a,b)
/// Converts module or a function to a stack layout type.
inl stack x = !LayoutToStack(x)
/// Converts module or a function to a packed stack layout type.
inl packed_stack x = !LayoutToPackedStack(x)
/// Converts module or a function to a heap layout type.
inl heap x = !LayoutToHeap(x)
/// Converts module or a function to a mutable heap layout type.
inl heapm x = !LayoutToHeapMutable(x)
/// Converts a layout type to a module or a function.
inl indiv x = !LayoutToNone(x)

/// The type of a boolean.
inl bool = type true 

/// The type of a int64.
inl int64 = type 0i64 
/// The type of a int32.
inl int32 = type 0i32 
/// The type of a int16.
inl int16 = type 0i16 
/// The type of a int8.
inl int8 = type 0i8 

/// The type of a uint64.
inl uint64 = type 0u64 
/// The type of a uint32.
inl uint32 = type 0u32 
/// The type of a uint16.
inl uint16 = type 0u16 
/// The type of a uint8.
inl uint8 = type 0u8

/// The type of a float64.
inl float64 = type 0f64
/// The type of a float32.
inl float32 = type 0f32

/// The type of a string.
inl string = type ""
/// The type of a char.
inl char = type ' '
/// The type of a empty tuple.
inl unit = type ()

/// Casts a type literal to the term level.
inl type_lit_cast x = !TypeLitCast(x)
/// Returns whether the expression is a type literal as a bool.
inl type_lit_is x = !TypeLitIs(x)
/// Cast a function to the term level.
inl term_cast to from = !TermCast(to,from)
/// Does unchecked conversion for primitives.
inl unsafe_convert to from = !UnsafeConvert(to,from) 
/// Unary negation.
inl negate x = !Neg(x)
/// Evaluates an expression and throws away the result.
inl ignore x = ()
/// Returns an expression after evaluating it.
inl id x = x
/// Throws away the second argument and returns the first.
inl const x _ = x
/// Creates a reference.
inl ref x = !ReferenceCreate(x)

/// Creates an array with the given type and the size.
inl array_create typ size = !ArrayCreate(size,typ)
/// Returns the length of an array. Not applicable to Cuda arrays.
inl array_length ar = !ArrayLength(ar)
/// Partial active pattern. In `on_succ` it also passes the type of the array as a type string.
inl array_is x on_fail on_succ = !ArrayIs(x,on_fail,on_succ)
/// Type of an array with the given type.
inl array t = type (array_create t 1)

/// Binary addition.
inl (+) a b = !Add(a,b)
/// Binary subtraction.
inl (-) a b = !Sub(a,b)
/// Binary multiplication.
inl (*) a b = !Mult(a,b)
/// Binary division.
inl (/) a b = !Div(a,b)
/// Binary modulo.
inl (%) a b = !Mod(a,b)

/// Applies the first argument to the second.
inl (|>) a b = b a
/// Applies the second argument to the first.
inl (<|) a b = a b
/// Applies the third argument to the first and then the result of that to the second.
inl (>>) a b x = b (a x)
/// Applies the third argument to the second and then the result of that to the first.
inl (<<) a b x = a (b x)

/// Binary less-than-or-equals.
inl (<=) a b = !LTE(a,b)
/// Binary less-than.
inl (<) a b = !LT(a,b)
/// Binary equals.
inl (=) a b = !EQ(a,b)
/// Binary unequals.
inl (<>) a b = !NEQ(a,b)
/// Binary greater-than.
inl (>) a b = !GT(a,b)
/// Binary greater-than-or-equals.
inl (>=) a b = !GTE(a,b)

/// Bitwise and.
inl (&&&) a b = !BitwiseAnd(a,b)
/// Bitwise or.
inl (|||) a b = !BitwiseOr(a,b)
/// Bitwise xor.
inl (^^^) a b = !BitwiseXor(a,b)

/// Tuple cons.
inl (::) a b = !ListCons(a,b)
/// Shift left.
inl (<<<) a b = !ShiftLeft(a,b)
/// Shift right.
inl (>>>) a b = !ShiftRight(a,b)

/// Gets the first elements of a tuple.
inl fst x :: _ = x
/// Gets the second element of a tuple.
inl snd _ :: x :: _ = x

/// Unary negation.
inl not x = x = false
/// Returns the length of a string.
inl string_length x = !StringLength(x)
/// The .NET String.Format function.
/// https://msdn.microsoft.com/en-us/library/system.string.format(v=vs.110).aspx
/// When its arguments are literals, it evaluates at compile time.
inl string_format a b = !StringFormat(a,b)
/// The F# String.concat function
/// https://msdn.microsoft.com/en-us/visualfsharpdocs/conceptual/string.concat-function-%5Bfsharp%5D
/// When its arguments are literals, it evaluates at compile time.
inl string_concat a b = !StringConcat(a,b)
/// Returns boolean whether the expression is a literal.
inl lit_is x = !LitIs(x)
/// Returns boolean whether the expression is a box (but not an union type.)
inl box_is x = !BoxIs(x)
/// Returns boolean whether the expression is a union or a recursive type (excluding boxes.)
inl caseable_is x = !CaseableIs(x)
/// Returns boolean whether the expression is a union or a recursive type.
inl caseable_box_is x = !CaseableBoxIs(x)
/// Raises an exception at runtime.
inl failwith typ msg = !FailWith(typ,msg)
/// Asserts an expression. If the conditional is a literal it raises a type error instead.
inl assert c msg = 
    inl raise = 
        if lit_is c then error_type
        else failwith unit
    
    if c = false then raise msg
/// Returns the maximum of the two expressions.
inl max a b = if a > b then a else b
/// Returns the minimum of the two expressions.
inl min a b = if a > b then b else a
/// Returns boolean whether the two expressions are equal in their types.
inl eq_type a b = !EqType(a,b)
/// Returns the values of a module in a tuple.
inl module_values x = !ModuleValues(x)
/// Maps over a module.
/// (string type_lit -> a -> b) -> a module -> b module
inl module_map f a = !ModuleMap(f,a)
/// Filters a module at compile time.
/// (string type_lit -> a -> bool) -> a module -> a module
inl module_filter f a = !ModuleFilter(f,a)
/// Folds over a module left to right.
/// (string type_lit -> state -> a -> state) -> state -> a module -> state
inl module_foldl f s a = !ModuleFoldL(f,s,a)
/// Folds over a module right to left.
/// (string type_lit -> a -> state -> state) -> a module -> state -> state
inl module_foldr f a s = !ModuleFoldR(f,s,a)
/// Returns boolean whether the module has a member.
/// a module -> string type_lit -> bool
inl module_has_member m x = !ModuleHasMember(m,x)
/// Unsafe upcast. Unlike the F# compiler, Spiral won't check its correctness.
inl (:>) a b = !UnsafeUpcastTo(b,a)
/// Unsafe downcast. Unlike the F# compiler, Spiral won't check its correctness.
inl (:?>) a b = !UnsafeDowncastTo(b,a)

/// Structural polymorphic equality for every type in the language (apart from functions).
inl (=) a b =
    inl prim_eq = (=)
    inl rec (=) a b =
        inl body = function
            | .(a), .(b) -> a = b
            | a :: as', b :: bs -> a = b && as' = bs
            | {} & a, {} & b -> module_values a = module_values b
            | (), () -> true
            | a, b when eq_type a b -> prim_eq a b // This repeat eq_type check is because unboxed union types might lead to variables of different types to be compared.
            | _ -> false
        if caseable_is a && caseable_is b then join (body (a, b) : bool)
        else body (a, b)
    if eq_type a b then a = b
    else error_type ("Trying to compare variables of two different types. Got:",a,b)

/// Returns the size a type.
/// type -> int64
inl sizeof x = !SizeOf(x)

/// Creates a .NET type from a macro.
inl fs x = !DotNetTypeCreate(x)
/// Creates a Cuda type from a macro.
inl cd x = !CudaTypeCreate(x)

/// Natural Logarithm.
inl log x = !Log(x)
/// Exponent.
inl exp x = !Exp(x)
/// Hyperbolic tangent. 
inl tanh x = !Tanh(x)

/// Macros.
inl macro = {
    /// F# macro.
    fs = inl typ expr -> !MacroFs(typ,expr)
    /// Cuda macro.
    cd = inl typ expr -> !MacroCuda(typ,expr)
    }

{type_lit_lift error_type print_static dyn (=>) cd fs log exp tanh array_create array_length array_is array
 split box stack packed_stack heap heapm indiv bool int64 int32 int16 int8 uint64 uint32 uint16 uint8 float64 float32
 string char unit type_lit_cast type_lit_is term_cast unsafe_convert negate ignore id const ref (+) (-) (*) (/) (%)
 (|>) (<|) (>>) (<<) (<=) (<) (=) (<>) (>) (>=) (&&&) (|||) (^^^) (::) (<<<) (>>>) fst snd not macro
 string_length lit_is box_is failwith assert max min eq_type module_values caseable_is caseable_box_is (:>)
 (:?>) (=) module_map module_filter module_foldl module_foldr module_has_member sizeof string_format string_concat} |> stack
    """) |> module_
```

#### Virtualization

Most operations in Spiral are virtualized, meaning they can be overridden.

```
inl f m =
    open m
    (1 + 2) * (3 + 4)

f {}
```
```
21L
```

If `+` and `*` overriden, the meaning of the expression can change completely.

```
inl f m =
    open m
    (1 + 2) * (3 + 4)

f {
    (+) = inl a b -> string_format "({0} + {1})" (a,b)
    (*) = inl a b -> string_format "({0} * {1})" (a,b)
    }
```
```
"((1 + 2) * (3 + 4))"
```

Currently `if`,`&&`, `||` and `.` are keywords, but there is no reason why they couldn't be virtualized.

The keyword `use` can be overridden by putting parentheses around it. This is how `use` is implemented in `Extern`.

```
inl (use) a b =
    inl r = b a
    FS.Method a .Dispose() unit
    r
```

