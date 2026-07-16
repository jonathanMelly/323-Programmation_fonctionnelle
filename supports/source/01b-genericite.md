# Généricité — abstraire les types

## Le problème

Team Helvetia joue à trois jeux : les matchs Valorant, CS2 et LoL n'ont pas les mêmes
caractéristiques (agents, maps, champions...). Pourtant, le traitement est identique :
stocker une collection, la compter, la parcourir, la filtrer.

Sans généricité, il faudrait trois classes quasi identiques :

```csharp
public class ValorantSeries { private readonly IEnumerable<ValorantMatch> _data; /* ... */ }
public class Cs2Series      { private readonly IEnumerable<Cs2Match> _data;      /* ... */ }
public class LolSeries      { private readonly IEnumerable<LolMatch> _data;      /* ... */ }
```

Trois fois le même code — trois fois les mêmes bugs à corriger.

## Un exemple déjà connu : `List<T>`

La généricité est utilisée depuis longtemps sans y penser :

```csharp
var names   = new List<string>();  // T = string
var scores  = new List<int>();     // T = int
var matches = new List<ValorantMatch>(); // T = ValorantMatch
```

Une seule classe `List<T>` fonctionne pour tous les types. `T` est un **paramètre de type** :
il est remplacé par un type concret au moment de l'utilisation, exactement comme un paramètre
de fonction est remplacé par une valeur au moment de l'appel.

## Classe générique

Définir sa propre classe générique consiste à déclarer le paramètre de type entre chevrons :

```csharp
public class DataSeries<T>
{
    private readonly IEnumerable<T> _data;

    private DataSeries(IEnumerable<T> data) => _data = data;

    public static DataSeries<T> From(IEnumerable<T> source)
        => new DataSeries<T>(source);

    public int Count => _data.Count();
    public IEnumerable<T> Values => _data;
}
```

`DataSeries<T>` ignore tout du contenu de `T` — il ne lit aucune propriété, n'appelle aucune
méthode spécifique. C'est précisément ce qui le rend réutilisable :

```csharp
var valorant = DataSeries<ValorantMatch>.From(valorantMatches);
var cs2      = DataSeries<Cs2Match>.From(cs2Matches);
var weather  = DataSeries<double>.From(temperatures); // même un autre domaine !
```

## Méthode générique

Une méthode peut aussi déclarer son propre paramètre de type, indépendamment de la classe :

```csharp
public static T First<T>(IEnumerable<T> source)
    => source.GetEnumerator().MoveNext() ? source.First() : throw new InvalidOperationException();

// Le compilateur infère T tout seul :
var first = First(scores);  // T = int
var name  = First(names);   // T = string
```

Les méthodes LINQ (`Where`, `Select`, `Aggregate`...) sont toutes des méthodes génériques —
c'est ce qui leur permet de fonctionner sur n'importe quel `IEnumerable<T>`.

## Le lien avec la programmation fonctionnelle

La généricité abstrait les **types** : un seul `DataSeries<T>` pour tous les jeux.

Les fonctions d'ordre supérieur (thématique 02) abstrairont le **comportement** :
une seule méthode `Filter` pour tous les critères, car le critère sera passé en paramètre —
sous forme de fonction.

Les deux se rejoignent dans les **délégués génériques** :

```csharp
Func<T, bool>       // une fonction générique : T en entrée, bool en sortie
Func<string[], T>   // les colonnes d'un CSV en entrée, un T en sortie
```

`Func<T, TResult>` combine les deux abstractions — les types sont génériques ET la logique
est une valeur transportable. C'est la brique de base de tout ce qui suit dans ce cours.
