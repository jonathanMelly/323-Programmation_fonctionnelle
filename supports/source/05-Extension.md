# Extensions du langage C#
![Alt text](extension.png)

Le langage C# est un langage relativement riche, toutefois on peut encore l’enrichir selon les besoins spécifiques d’un projet ou d’une entreprise...

## D’ailleurs, LINQ utilise ce mécanisme pour étendre les possibilités des `IEnumerable`, par exemple si on regarde la signature de `Where` :

```csharp
public static System.Linq.IQueryable<TSource> Where<TSource> (/*C’est quoi ça ?*/ this /*?????*/ System.Linq.IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource,bool>> predicate);
```

TODO: explications

## Opérateurs
Un cas assez naturel est celui des opérateurs pour les classes personnalisées:

TODO

## Types standards
L’autre cas bien utile est celui de pouvoir enrichir les classes C# fournies par .NET:

TODO