[![NuGet Badge](https://buildstats.info/nuget/BFshap)](https://www.nuget.org/packages/Brainfuck-Runner/)
# BFShap
It is a BF implementation made with C#.

>DotNet Framework : 6.0

## How to use it to your .Net project.
First, Using NameSpace BrainFuck.BFShap
----------
`using BrainFuck.BFShap`

Next, define twe function .
--------
ex :

`static char input() {
return Console.Read();
 }
 `
 
`static void output(char ASCII){
    Console.Write(ASCII)
}`

Creat BF Cless.
-------------
`BF bf = new BF(input, output);`


Access the "bf.Code" property and enter the bf code.
--------------
`bf.Code = "++++++++++
\[>+++++++>++++++++++>+++>+<<<<-\]
\>++.\>+.+++++++..+++.>++++++++++++++.------------.<<+++++++++++++++.>.+++.------.--------.\>+.";`


Pass an argument "-1" to "Step"
---------
whe Pass an argument "-1" to "Step", bf will run the bf code to the end

`bf.Step(-1);`
