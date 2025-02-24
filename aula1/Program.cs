using CAS;

Expressao a = 20;
Expressao b = 5;

Expressao soma = a + b;
Expressao c = 50, x = "x";

Console.WriteLine(soma);
Console.WriteLine(a+c);
Console.WriteLine(soma.Derivar(~b));

Complexo im1 = (10,3);
Expressao d = new Soma(0,7).Simplificar();
Complexo im2 = (-2,d),
im3 = (3,3*x),
im4 = (4,4)
;
Numero n1 = 5;

Console.WriteLine(new Soma(2,-3));
Console.WriteLine($"Complexos:\n{im1}\n");
Console.WriteLine(im2);
Console.WriteLine($"Soma:{im1+im2}\nSubtração:{im1-im2}\nMultiplicação:{im1*im2}\nDivisão:{im1/im2}\n");
Console.WriteLine($"Derivadas:\n[{im3}]' = {im3<<x}\n[{im3*x}]' = {(im3*x)<<x}");
Console.WriteLine($"Simplificações:\n{im4} * {n1} = {im4*n1}\n{im4} + {x} = {im4+x}");

