# Extensions du langage C#
![Alt text](extension.png)

Le langage C# est un langage relativement riche, toutefois on peut encore l’enrichir selon les besoins spécifiques d’un projet ou d’une entreprise...

## D’ailleurs, LINQ utilise ce mécanisme pour étendre les possibilités des `IEnumerable`, par exemple si on regarde la signature de `Where` :

```csharp
public static System.Linq.IQueryable<TSource> Where<TSource> (/*C’est quoi ça ?*/ this /*?????*/ System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource,bool>> predicate);
```

La *magie* des extensions consiste à définir une fonction `statique et publique` et définir comme premier paramètre 
un élément du type qu'on veut étendre et préfixé par le mot clé `this`. Et tout ça dans une classe elle aussi 
`public et statique`:

```csharp
public static class PeopleExtensions
{
    public static string Greetings(this string name)
    {
        return $"Hello {name}";
    }
}
```

Ce code étend la classe string de C# en ajoutant une méthode `Greetings()`:

```csharp
Console.WriteLine("Bob".Greetings());
```

Ce code affichera
```text
Hello Bob
```

Pour que vos méthodes d'extension soient utilisables, il faut que la classe qui les définit se trouve
- Soit dans le même namespace que votre code
- Soit dans un namespace importé avec `using`
  
### Un peu mieux
En ajoutant une méthode d'extension qui ne retourne rien (`void`), on peut faire encore mieux.

Méthode supplémentaire : 

```csharp
public static void ToConsole(this string subject)
{
    Console.WriteLine(subject);
}
```

Utilisation :

```csharp
"Bob".Greetings().ToConsole();
```

> Cet exemple montre clairement que le chaînage fonctionnel part des données (ici "Bob"). Ainsi
> c'est un point de vue qui peut être déstabilisant, car on avait l'habitude jusque-là de choisir
> d'abord l'action (avec Console.Write)...

## Chaînage avec valeur de retour
Quand une méthode d'extension retourne un élément, cela permet de faire du chaînage.
Avec la méthode suivante :

```csharp
    /// <summary>
    /// Passe en minuscule les éléments fournis, éventuellement de manière aléatoire
    /// </summary>
    /// <param name="subject">Les données en input</param>
    /// <param name="random">Si vrai, aura 50% de faire la transformation</param>
    /// <returns></returns>
    public static IEnumerable<string> ToLower(this IEnumerable<string> subject,bool random=false)
    {
        return subject.Select(text => 
            random?
                randomGenerator.Next(2)==1?
                    text.ToLower():text 
                :text.ToLower());
    }
```

> On constate dans cet exemple qu'une méthode d'extension peut avoir des paramètres standards à la
> suite du premier (ici bool random=false)...

On peut écrire le code suivant :

```csharp
var data = new[] {"BoB","Max","jOelLe","NadiA" };
data
    .Where(name => name.StartsWith("j"))
    .ToLower(true)
    .ToList()
    .ForEach(Console.WriteLine);
```

<details>
<summary>Quel sera le résultat ?</summary>

50% de chance :
```text
jOelle
```
50% de chance :
```text
joelle
```
</details>

## DSL : *D*omain *S*pecific *L*anguage
Un exemple concret de l'utilisation des extensions est de pouvoir créer un `langage spécifique au domaine`.
On entend par là un pseudo-langage qui est plus proche du métier.

### Exemple 1 : FluentAssertions
Ceci a été utilisé pour la librairie d'assertion pour les tests `FluentAssertions`, qui permet
d'écrire les assertions ainsi :

```csharp
[Fact]
public void TestIsMatch()
{
    //Arrange
    var cmd = "--help";

    //Act
    bool result = cmd.IsMatch(Program.OptHelp);

    //Assert
    result.Should().BeTrue("la ligne de commande contient le paramètre --help");
}
```

> `result.Should().BeTrue` se lit et se comprend facilement (le résultat devrait retourner 'vrai')

### Exemple 2 : Cosmos
Pour le projet [cosmos](https://github.com/jonathanMelly/cosmos/tree/master), un DSL a été mis en place pour facilement tester certains aspects du 
langage :

```csharp
[Fact]
public void TestDifferentNumber()
{
    TestBoolean("5".IsDifferentThan("6"), true);
}
```

> Sans connaître le langage Cosmos, on comprend que ce teste vérifie la différence entre deux valeurs...
> Derrière les décors, IsDifferentThan convertit cela en langage Cosmos...

## Zip — combiner deux séquences en parallèle

`Zip` apparie les éléments de deux séquences **position par position** — comme les dents
d'une fermeture éclair :

```csharp
var dates = new[] { "01.03", "08.03", "15.03" };
var kda   = new[] { 2.4, 3.1, 1.8, 2.9 };       // 4 valeurs !

var timeline = dates.Zip(kda, (date, score) => $"{date} → KDA {score}");
// { "01.03 → KDA 2.4", "08.03 → KDA 3.1", "15.03 → KDA 1.8" }
```

Deux propriétés à retenir :

- **Appariement par position** — le 1er élément avec le 1er, le 2e avec le 2e, etc.
  Aucune clé, aucun tri : seule la position compte.
- **Arrêt sur la plus courte** — ici 3 dates pour 4 scores : le 4e score est ignoré
  sans erreur. Le résultat a la longueur de la séquence la plus courte.

Sans le combineur, `Zip` produit des tuples :

```csharp
var pairs = dates.Zip(kda); // IEnumerable<(string, double)>
```

`Zip` est le complément naturel des extensions fluentes : il permet de comparer deux séries
côte à côte dans un pipeline (voir `PairWith` dans l'exercice 06 du fil rouge, ainsi que les
exercices [events](../../exos/events/README.md) et [randoPureZip](../../exos/randoPureZip/README.md)).

## Composition de Fonctions (f ∘ g)

La **composition de fonctions** est l'un des principes fondamentaux de la programmation
fonctionnelle. L'idée est simple : la sortie d'une fonction devient l'entrée de la suivante.

En mathématiques : `(f ∘ g)(x) = f(g(x))`

En C# avec méthodes d'extension : `x.g().f()` — c'est la même chose.

```csharp
// Sans composition
var result = ToUpperCase(RemoveSpaces(Trim(input)));

// Avec extensions — même chose, lecture de gauche à droite
var result = input.Trim().RemoveSpaces().ToUpperCase();
```

### Le contrat de la composition

Pour que le chaînage soit possible, chaque méthode doit **retourner le même type qu'elle
reçoit** (ou un type compatible). C'est le contrat que LINQ respecte :

```csharp
// IEnumerable<T> → Where → IEnumerable<T> → Select → IEnumerable<T> → OrderBy → ...
numbers
    .Where(n => n > 0)          // IEnumerable<int>
    .Select(n => n * 2)         // IEnumerable<int>
    .OrderBy(n => n)            // IOrderedEnumerable<int> (compatible)
    .Take(3)                    // IEnumerable<int>
    .ToList();                  // List<int> — matérialisation
```

### Composition explicite avec `Func`

On peut composer des fonctions manuellement avec `Func` :

```csharp
// Deux fonctions à composer
Func<string, string> trim    = s => s.Trim();
Func<string, string> toLower = s => s.ToLower();

// Composition manuelle
Func<string, string> normalize = s => toLower(trim(s));

// En méthode d'extension générique
public static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g)
    => x => g(f(x));

// Utilisation
Func<string, string> normalize2 = trim.Compose(toLower);
Console.WriteLine(normalize2("  HELLO  ")); // "hello"
```

### Pourquoi la composition est-elle liée à l'immutabilité ?

La composition fonctionnelle n'est possible *que* parce que les fonctions ne modifient
pas leur entrée. Si `Filter` modifiait la liste source au lieu de retourner une nouvelle
liste, `Transform(Filter(data))` serait imprévisible — `data` aurait été altéré.

> L'immutabilité, la pureté et la composition forment un triangle indissociable :
> chacun rend les deux autres possibles et utiles.

## Conclusion ⚖
Les extensions du langage sont une bonne manière d'accélérer le développement en enrichissant
le langage de base avec ce qu'on a besoin de manière répétitive... Bien entendu, cela implique
d'avoir une dépendance supplémentaire vers ces extensions et cet aspect doit être pris en compte
dans la balance au moment de choisir de les utiliser ou pas...