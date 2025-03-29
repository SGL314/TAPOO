

var cotas = CotaParlamentar.LerCotasParlamentares("cota_parlamentar.csv");
List<string> list = new List<string> {"ana","abelha","bolo","bola"};

// 1) Total gasto por partido
foreach (var x in cotas.GroupBy(c => c.Partido).Select(g => new {partido = g.Key, total = g.Sum(c => c.ValorLiquido)}).ToArray()){
    Console.WriteLine($"{x.partido}: {x.total}");
}

// 2) Deputados com maior gasto individual (top 5)
foreach (var x in cotas.OrderByDescending(c => c.ValorLiquido)
.Take(5).ToArray()){
    Console.WriteLine($"{x}: {x}");
}

// 3) Gasto médio por mês
foreach (var x in cotas.OrderBy(a => a.Ano)
.GroupBy(c=>(c.DataEmissao.ToString().Length>=7)?(c.DataEmissao.ToString().Substring(3,7)):(c.DataEmissao.ToString()))
.Select(g => new {partido = g.Key, total = g.Sum(c => c.ValorLiquido)}).ToArray()){
    Console.WriteLine($"{x.partido}: {x.total}");
}

// 4) Total gasto em alimentação por deputado (Descricao.Contains("ALIMENTAÇÃO"))
foreach (var x in cotas.OrderBy(p => p.NomeParlamentar)
.GroupBy(k => k.NomeParlamentar)
.Select(z=>new{nome=z.Key,total=z.Sum(k=>(k.Descricao.Contains("ALIMENTAÇÃO"))?k.ValorLiquido:0)})
.ToArray()){
    Console.WriteLine($"{x.nome}: {x.total}");
}

// 5) Lista de fornecedores mais utilizados 
foreach (var x in cotas.OrderBy(p => p.Fornecedor)
.GroupBy(k => k.Fornecedor)
.Select(z => new {nome = z.Key, total = z.Sum(k => 1)})
.OrderByDescending(x => x.total).Take(10).ToArray()){
    Console.WriteLine($"{x.nome}: {x.total}");
}

// 6) Gasto total por UF
foreach (var x in cotas.OrderBy(p => p.UF)
.GroupBy(k => k.UF)
.Select(z => new {nome = z.Key, total = z.Sum(k => k.ValorLiquido)})
.ToArray()){
    Console.WriteLine($"{x.nome}: {x.total}");
}   

// 7) Meses com maior número de documentos emitidos
foreach (var x in cotas.OrderBy(a => a.Ano)
.GroupBy(c=>(c.DataEmissao.ToString().Length >= 7)?(c.DataEmissao.ToString().Substring(3,7)):(c.DataEmissao.ToString()))
.Select(g => new {periodo = g.Key, total = g.Sum(c => 1)})
.OrderByDescending(x => x.total).Take(10).ToArray()){
    Console.WriteLine($"{x.periodo}: {x.total}");
}

// 8) Deputados com despesas acima de R$ 10.000,00
foreach (var x in cotas.OrderBy(p => p.NomeParlamentar)
.GroupBy(k => k.NomeParlamentar)
.Select(k => new {nome = k.Key, total = k.Sum(z => z.ValorLiquido)})
.Where(k => k.total>10000)
.OrderByDescending(x => x.total).ToArray()){
    Console.WriteLine($"{x.nome}: {x.total}");
}   

// 9) Total gasto por tipo de despesa (Descricao)
foreach (var x in cotas.OrderBy(p => p.Descricao)
.GroupBy(k => k.Descricao)
.Select(k => new {nome = k.Key, total = k.Sum(z => z.ValorLiquido)})
.OrderByDescending(x => x.total).ToArray()){
    Console.WriteLine($"{x.nome}: {x.total}");
}   

// 10) Total de gastos por ano
foreach (var x in cotas.OrderBy(a => a.Ano)
.GroupBy(k => k.Ano)
.Select(g => new {periodo = g.Key, total = g.Sum(c => c.ValorLiquido)})
.OrderByDescending(x => x.total).ToArray()){
    Console.WriteLine($"{x.periodo}: {x.total}");
}