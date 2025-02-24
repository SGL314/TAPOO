using System;
using System.Numerics;

namespace CAS;

public abstract class Expressao{
    public abstract override string ToString();
    public abstract Expressao Derivar(Simbolo x);
    public abstract Expressao Simplificar();

    public static Expressao operator +(Expressao a, Expressao b) => new Soma(a, b).Simplificar();
    public static Expressao operator -(Expressao a, Expressao b) => new Subtracao(a, b).Simplificar();
    public static Expressao operator *(Expressao a, Expressao b) => new Multiplicacao(a, b).Simplificar();
    public static Expressao operator /(Expressao a, Expressao b) => new Divisao(a, b).Simplificar();
    public static Simbolo operator ~(Expressao a) => new Simbolo(a.ToString()); // converte em símbolo
    public static Complexo operator !(Expressao a) => (Complexo) a; // converte em símbolo
    public static Expressao operator +(Expressao a,int b) => new Soma(a,new Numero(b)); // converte em complexo
    public static Expressao operator <<(Expressao a,Simbolo x) => a.Derivar(x);
    public static Expressao operator <<(Expressao a,Expressao x) => a<<~x;

    public static implicit operator Expressao(int v) => new Numero(v);
    public static implicit operator Expressao(string s) => new Simbolo(s);
}

public class Numero : Expressao{
    public float valor;
    public Numero(int v) => this.valor = v;
    public Numero(float v) => this.valor = v;
    public override string ToString() => valor.ToString();
    public Numero Oposto() => new Numero(-valor);
    public override Expressao Derivar(Simbolo x) => new Numero(0);
    public override Expressao Simplificar() => this;
    public static Expressao operator <<(Numero a,Simbolo x) => a.Derivar(x);
    public static Expressao operator <<(Numero a,Expressao x) => a<<~x;
    public static implicit operator Numero(float v) => new Numero(v);
    // comparations
    public static bool operator ==(Numero a,Numero b) => a.valor == b.valor;
    public static bool operator !=(Numero a,Numero b) => a.valor != b.valor;
    public static bool operator ==(Numero a,int b) => a.valor == b;
    public static bool operator !=(Numero a,int b) => a.valor != b;
    public static bool operator >(Numero a,Numero b) => a.valor > b.valor;
    public static bool operator <(Numero a,Numero b) => a.valor < b.valor;
    public static bool operator >(Numero a,int b) => a.valor > b;
    public static bool operator <(Numero a,int b) => a.valor < b;
}

public class Complexo : Expressao{
    public Expressao real,imaginaria;
    public Complexo(Expressao real,Expressao imaginaria){
        this.real= real;
        this.imaginaria = imaginaria;
    }
    public override string ToString(){
        if (real is Numero && imaginaria is Numero) if ((real as Numero)==0&&(imaginaria as Numero)==0) return  "0";
        if (real is Numero) if ((real as Numero)==0) return $"{imaginaria.ToString()}i";
        if (imaginaria is Numero){
            if ((imaginaria as Numero)==0) return $"{real.ToString()}";
            if ((imaginaria as Numero)==0) return $"{real.ToString()}";
            if ((imaginaria as Numero)<0) return $"({real.ToString()} - {(imaginaria as Numero).Oposto().ToString()}i)";
        }
        return $"({real.Simplificar().ToString()} + {imaginaria.Simplificar().ToString()}i)";
    }
    public override Expressao Derivar(Simbolo x){
        return new Soma(real.Derivar(x),(Complexo) (0,imaginaria.Derivar(x))).Simplificar();
    }
    public override Complexo Simplificar() => this;
    public static Expressao operator <<(Complexo a,Simbolo x) => a.Derivar(x);
    public static Expressao operator <<(Complexo a,Expressao x) => a<<~x;
    public Complexo Oposto() => new Complexo((Expressao) (-(real as Numero).valor),(Expressao) (-(imaginaria as Numero).valor));

    // conversões implícitas

    public static implicit operator Complexo((int a,int b) t) => new Complexo(new Numero(t.a),new Numero(t.b));
    public static implicit operator Complexo((Expressao a,int b) t) => new Complexo(t.a,new Numero(t.b));
    public static implicit operator Complexo((int a,Expressao b) t) => new Complexo(new Numero(t.a),t.b);

    // operations
    public static Complexo operator +(Complexo a, Complexo b) => !new Soma(a,b).Simplificar();
    public static Complexo operator -(Complexo a, Complexo b) => !new Subtracao(a, b).Simplificar();
    public static Complexo operator *(Complexo a, Complexo b) => !new Multiplicacao(a, b).Simplificar();
    public static Complexo operator /(Complexo a, Complexo b) => !new Divisao(a, b).Simplificar();

    public static Complexo operator +(Complexo a, Numero b) => !new Soma(a,new Complexo(b,0)).Simplificar();
    public static Complexo operator -(Complexo a, Numero b) => !new Subtracao(a, new Complexo(b,0)).Simplificar();
    public static Complexo operator *(Complexo a, Numero b) => !new Multiplicacao(a, new Complexo(b,0)).Simplificar();
    public static Complexo operator /(Complexo a, Numero b) => !new Divisao(a, new Complexo(b,0)).Simplificar();

    public static Complexo operator +(Numero a, Complexo b) => !new Soma(new Complexo(a,0),b).Simplificar();
    public static Complexo operator -(Numero a, Complexo b) => !new Subtracao(new Complexo(a,0),b).Simplificar();
    public static Complexo operator *(Numero a, Complexo b) => !new Multiplicacao(new Complexo(a,0),b).Simplificar();
    public static Complexo operator /(Numero a, Complexo b) => !new Divisao(new Complexo(a,0),b).Simplificar();

    public static Complexo operator +(Complexo a, Simbolo b) => !new Soma(a,new Complexo(b,0)).Simplificar();
    public static Complexo operator -(Complexo a, Simbolo b) => !new Subtracao(a, new Complexo(b,0)).Simplificar();
    public static Complexo operator *(Complexo a, Simbolo b) => !new Multiplicacao(a, new Complexo(b,0)).Simplificar();
    public static Complexo operator /(Complexo a, Simbolo b) => !new Divisao(a, new Complexo(b,0)).Simplificar();

    public static Complexo operator +(Simbolo a, Complexo b) => !new Soma(new Complexo(a,0),b).Simplificar();
    public static Complexo operator -(Simbolo a, Complexo b) => !new Subtracao(new Complexo(a,0),b).Simplificar();
    public static Complexo operator *(Simbolo a, Complexo b) => !new Multiplicacao(new Complexo(a,0),b).Simplificar();
    public static Complexo operator /(Simbolo a, Complexo b) => !new Divisao(new Complexo(a,0),b).Simplificar();

    // public static Complexo operator +(Complexo a,Expressao b) => a+!b;
    // public static Complexo operator -(Complexo a,Expressao b) =>  a-!b;
    // public static Complexo operator *(Complexo a,Expressao b) => a*!b;
    // public static Complexo operator /(Complexo a,Expressao b) => a/!b;

    // public static Complexo operator +(Expressao a, Complexo b) => !a+b;
    // public static Complexo operator -(Expressao a, Complexo b) => !a-b;
    // public static Complexo operator *(Expressao a, Complexo b) => !a*b;
    // public static Complexo operator /(Expressao a, Complexo b) => !a/b;
}

public class Simbolo : Expressao{
    string simbolo;
    public Simbolo(string s) => this.simbolo = s;
    public override string ToString() => simbolo;
    public override Expressao Derivar(Simbolo x) => 
        x.simbolo == simbolo 
            ? new Numero(1) 
            : new Numero(0);
    public override Expressao Simplificar() => this;
}

// REAIS

public class Soma : Expressao{
    private Expressao a, b; // a + b
    private Complexo c,d;
    public Soma(Expressao x, Expressao y){
        this.a = x;
        this.b = y;
    }
    public Soma(Complexo x, Complexo y){
        this.c = x;
        this.d = y;
    }
    public override string ToString(){
        if (a==null){
            return $"({c.ToString()} + {d.ToString()})";
        }else{
            if (b is Numero) if ((b as Numero).valor<0) return $"({a.ToString()} - {(b as Numero).Oposto().ToString()})"; else return $"({a.ToString()} + {b.ToString()})";
            return $"({a.ToString()} + {b.ToString()})";
        }
    }
    public override Expressao Derivar(Simbolo x) => 
        new Soma(a.Derivar(x), b.Derivar(x));

    public override Expressao Simplificar() => (a==null) ? SimplificarC() : SimplificarR();
    private Expressao SimplificarR(){
        if (a is Numero){
            if ((a as Numero)==0) return b;
        }
        if (b is Numero){
            if ((b as Numero)==0) return a;
        }
        if(a is Numero && b is Numero){
            return new Numero((a as Numero).valor + (b as Numero).valor);
        }
        if ((a is Complexo) && (b is Simbolo)){
            refat(a,b);
            return SimplificarC();
        }else if ((a is Simbolo) && (b is Complexo)){
            refat(b,a);
            return SimplificarC();
        }
        return this;
    }

    private Complexo SimplificarC(){
        Expressao real,imaginaria;
        Complexo fA=c,fB=d;
        Console.WriteLine($"{fA.real} {fB.real}");
        real = new Soma(fA.real,fB.real).Simplificar();
        imaginaria = new Soma(fA.imaginaria,fB.imaginaria).Simplificar();
        return new Complexo(real,imaginaria);
    }

    private void refat(Expressao maintain,Expressao rebuild){

        c = (Complexo) maintain;
        d = new Complexo(rebuild,0);
        a = null;
        b = null;
        Console.WriteLine($" {c} --- {d} ");
    }
           
}

public class Subtracao : Expressao{
    private Expressao a, b; // a - b
    private Complexo c,d;
    public Subtracao(Expressao x, Expressao y)
    {
        this.a = x;
        this.b = y;
    }
    public Subtracao(Complexo x, Complexo y)
    {
        this.c = x;
        this.d = y;
    }
    public override string ToString(){
        if (a==null) return $"({c.ToString()} - {d.ToString()})";
        else{
            if (b is Numero) if ((b as Numero).valor<0) return $"({a.ToString()} + {(b as Numero).Oposto().ToString()})"; else return $"({a.ToString()} - {b.ToString()})";
            return $"({a.ToString()} - {b.ToString()})";
        }
    }

    public override Expressao Derivar(Simbolo x) => 
        new Subtracao(a.Derivar(x), b.Derivar(x));
    public override Expressao Simplificar() => (a==null) ? SimplificarC() : SimplificarR();
    private Expressao SimplificarR(){
        if (a is Numero){
            if ((a as Numero)==0) return b;
        }
        if (b is Numero){
            if ((b as Numero)==0) return a;
        }
        if(a is Numero && b is Numero){
            return new Numero((a as Numero).valor - (b as Numero).valor);
        }
        return this;
    }

    private Complexo SimplificarC(){
        Expressao real,imaginaria;
        real = new Subtracao(this.c.real,this.d.real).Simplificar();
        imaginaria = new Subtracao(this.c.imaginaria,this.d.imaginaria).Simplificar();
        return new Complexo(real,imaginaria);
    }
}

public class Multiplicacao : Expressao{
    private Expressao a, b; // a * b
    private Complexo c,d;
    public Multiplicacao(Expressao x, Expressao y){
        this.a = x;
        this.b = y;
    }
    public Multiplicacao(Complexo x, Complexo y)
    {
        this.c = x;
        this.d = y;
    }
    public override string ToString(){
        if (a==null){
            return $"({c.ToString()} * {d.ToString()})";
        }else{
            if (a is Numero && b is Simbolo) return $"{a.ToString()}{b.ToString()}";
            if (b is Numero && a is Simbolo) return $"{b.ToString()}{a.ToString()}";
            return $"({a.ToString()} * {b.ToString()})";
        }
    }

    public override Expressao Derivar(Simbolo x) =>
        new Soma(
            new Multiplicacao(a.Derivar(x), b).Simplificar(),
            new Multiplicacao(a, b.Derivar(x)).Simplificar()).Simplificar();
    public override Expressao Simplificar() => (a==null) ? SimplificarC() : SimplificarR();
    private Expressao SimplificarR(){
        if (a is Numero){
            if ((a as Numero)==0) return new Numero(0);
            if (a as Numero == 1) return b;
        }
        if (b is Numero){
            if ((b as Numero)==0) return new Numero(0);
            if (b as Numero == 1) return a;
        }
        if (a is Numero && b is Numero){
            return new Numero((a as Numero).valor * (b as Numero).valor);
        }
        return this;
    }

    private Complexo SimplificarC(){
        Expressao real,imaginaria;
        real = new Subtracao(new Multiplicacao(this.c.real,this.d.real).Simplificar(),new Multiplicacao(this.c.imaginaria,this.d.imaginaria).Simplificar()).Simplificar();
        imaginaria = new Soma(new Multiplicacao(this.c.real,this.d.imaginaria).Simplificar(),new Multiplicacao(this.c.imaginaria,this.d.real).Simplificar()).Simplificar();
        return new Complexo(real,imaginaria);
    }
}

public class Divisao : Expressao{
    private Expressao a, b; // a / b
    private Complexo c,d;
    public Divisao(Expressao x, Expressao y){
        this.a = x;
        this.b = y;
    }
    public Divisao(Complexo x, Complexo y)
    {
        this.c = x;
        this.d = y;
    }
    public override string ToString() => (a==null) ? $"({c.ToString()} / {d.ToString()})": $"({a.ToString()} / {b.ToString()})";
    public override Expressao Simplificar() => (a==null) ? SimplificarC() : SimplificarR();
    public override Expressao Derivar(Simbolo x) =>
        new Divisao(
            new Subtracao(
                new Multiplicacao(a.Derivar(x), b), 
                new Multiplicacao(a, b.Derivar(x))),
            new Multiplicacao(b, b));
    private Expressao SimplificarR(){
        if(a is Numero && b is Numero){
          return new Numero((a as Numero).valor / (b as Numero).valor);
       }
       return this;
    }

    private Complexo SimplificarC(){
        Expressao real,imaginaria,num_p1,num_p2,dem;
        num_p1 = new Soma(new Multiplicacao(this.c.real,this.d.real).Simplificar(),new Multiplicacao(this.c.imaginaria,this.d.imaginaria).Simplificar()).Simplificar();
        num_p2 = new Subtracao(new Multiplicacao(this.c.imaginaria,this.d.real).Simplificar(),new Multiplicacao(this.c.real,this.d.imaginaria).Simplificar()).Simplificar();

        dem = new Soma(new Multiplicacao(this.d.real,this.d.real).Simplificar(),new Multiplicacao(this.d.imaginaria,this.d.imaginaria).Simplificar()).Simplificar();
        real = num_p1/dem;
        imaginaria = num_p2/dem;
        return new Complexo(real,imaginaria);
    }
}

// COMPLEXOS
