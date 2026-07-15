# Paradigmes de programmation

Il existe plusieurs manières d’aborder la communication avec les machines au travers de programmes contenant des instructions, ce qu’on nomme *paradigme*.
Ces paradigmes sont à plusieurs niveaux, voici une vue pour mieux les appréhender:

Paradigmes de programmation  
├── Impératif  
│&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├── Procédural (C, C#*, Javascript*, Python*, Rust*, Go*)  
│&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├── Orienté objet (C#, Java, C++, Python, Rust, Zik, Javascript, Go)  
│&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└── Parallèle/Concurrent (C#, Go, Erlang, Rust, Zik)  
└── Déclaratif  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├── Fonctionnel (F#, Haskell, Lisp, ML, Scala, C#, Rust, Java)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├── Logique (Prolog)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└── Contraintes (CHR, Oz/Mozart, MiniZinc)  
	

*On constate que beaucoup de langages sont "multi-paradigmes", cela s’explique par le fait qu’ils ont évolué pour supporter les nouveaux paradigmes 
devenus populaires, notamment pour les aspects fonctionnels qui peuvent alléger le code source...*

## Impératif ou Déclaratif

On distingue 2 ensembles principaux:

1. Impératif
2. Déclaratif

### Impératif
L’idée est de donner les ordres de manière séquentielle à l’ordinateur qui éxécutera les instructions. Ce modèle comprend notamment les paradigmes suivants:

- **Procédural**
- **Orienté objet**

#### Exemple
Pour afficher 1,2,3,4,5, on pourrait avoir le pseudo-code suivant:

```text
Afficher 1
Afficher 2

DÉCLARER i
POUR i de 3 à 5 FAIRE
    AFFICHER i
FIN POUR
```
Ce code va imposer une suite d'opérations à faire par l'ordinateur dans le but de **construire** un résultat.

### Déclaratif

Le paradigme déclaratif implique de décrire ce que l’on souhaite sans forcément donner tous les détails. On trouve dans cet ensemble notamment le paradigme **Fonctionnel**.

Le pseudo-code pourrait ressembler à:
```
suite = suite_de_chiffres (1,5)
Afficher suite
```
Ce code va déclarer ce qu'**est** le résultat et l'afficher en suite

## Impératif > Procédural
C’est ce qui est fait en 1ère année à l’ETML:

```csharp
Console.Write("Instruction 0");
PrintRemainingInstructions();

void PrintRemainingInstructions()
{
    for(int i=1;i<5;i++)
    {
        Console.Write($"Instruction {i}");
    }
}
```

![Alt text](01-fun1.png)

Chaque instruction est éxécutée l’une après l’autre tout en pouvant aggréger du code dans des fonctions qui sont appelées ici ou là...

## Impératif > Orienté objet
C’est ce qui est fait en 2ème année à l’ETML:

```csharp
Console.WriteLine("Instruction 0");
for (int i = 1; i < 5; i++)
{
    Console.WriteLine(new Instruction());
}

class Instruction
{
    static int generator=1;
    public int Id { get; private set; }

    public Instruction()
    {
        Id = generator++;
    }

    public override string ToString()
    {
        return $"Instruction {Id}";
    }
}

```

![Alt text](01-fun1.png)

L’idée derrière la programmation objet est de réunir les données et les fonctions au sein de classes spécifiques afin d’organiser le code de manière optimale notamment pour travailler en équipe et faciliter la modularité d’un programme...

> Attention toutefois car ce paradigme peut à l’extrême devenir contre-productif car difficile à maintenir ou non indiqué pour les architectures en réseau où il est plus efficace de faire transiter les données uniquement... 
Cet avertissement a pour but d’éviter une polarisation de type *il faut toujours tout faire en objet*.

## Déclaratif > Fonctionnel

C’est ce qui est fait en 3ème année à l’ETML ;-)

Comme son nom l’indique, le paradgime fonctionnel fait la grande part aux fonctions.

### Tout est fonction, ou presque
Ainsi, l’exemple précédent pourrait ressembler à ça:

```csharp
Enumerable.Range(0, 5)
    .Select(i => $"Instruction {i}")
    .ToList()
    .ForEach(instruction => Console.WriteLine(instruction));

```

À noter qu’on peut mélanger les paradigmes, par exemple ci-dessous en utilisant la poo :

```csharp
Enumerable.Range(0, 5)
    .Select(i => new Instruction())
    .ToList()
    .ForEach(instruction => Console.WriteLine(instruction));
```

## Déclaratif vs Impératif — avec LINQ

La différence se voit concrètement avec un filtre sur une liste de nombres :

```csharp
var numbers = new[] { 1, 2, 3, 4, 5, 6 };

// Impératif : comment faire — on décrit les étapes
var result = new List<int>();
foreach (var n in numbers)
    if (n % 2 == 0) result.Add(n);

// Déclaratif (FP) : quoi faire — on décrit le résultat souhaité
var result = numbers.Where(n => n % 2 == 0);
```

> LINQ n’est pas le but — c’est l’outil. Le vrai but est d’apprendre à *penser* de manière
> déclarative : décrire la transformation souhaitée, pas les étapes pour y arriver.

## Les autres
D’autres étiquettes existent pour décrire des manières de programmer, on trouve notamment:

- réactive
- logique
- concurrente
- ...
