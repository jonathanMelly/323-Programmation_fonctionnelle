# Exercice 03 — Valider les stats

> Partie 2 — `.Filter()` + `.RemoveOutliers()` + `.HasAny()` / `.AllMatch()` + évaluation paresseuse

**Concepts FP :** Fonctions d'ordre supérieur · Closures · Évaluation paresseuse
→ Théorie : [Fonctions d'ordre supérieur](../../../supports/source/02a-fonctions-sup.md) ·
[Closures](../../../supports/source/02a-fonctions-sup.md#closures-captures-de-variables) ·
[Évaluation paresseuse](../../../supports/source/02b-filter.md#evaluation-paresseuse-deferred-execution)

## Contexte

Le dataset contient maintenant les données réelles (CSV) et les données générées (exercice 02).
Avant toute analyse, il faut valider que les contraintes sont respectées dans les trois sources.

---

## Étape 1 — Implémenter `.Filter(predicate)`

**Avant de coder :** `Filter` doit retourner une nouvelle `DataSeries<T>`, pas modifier l'existante.
Quelle méthode LINQ applique un prédicat à une séquence ?

<details>
<summary>Indice</summary>

`Where(predicate)` filtre une `IEnumerable<T>` sans modifier la source.
Il suffit d'envelopper le résultat dans une nouvelle `DataSeries<T>`.

</details>

```csharp
public DataSeries<T> Filter(Func<T, bool> predicate)
{
    // retourner une nouvelle DataSeries contenant seulement les éléments qui satisfont le prédicat
    // ...
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public DataSeries<T> Filter(Func<T, bool> predicate)
    => new DataSeries<T>(_data.Where(predicate));
```

</details>

Observer que `Filter` retourne une **nouvelle** `DataSeries<T>` — la source `_data` n'est
jamais modifiée. C'est l'immuabilité : chaque appel produit un nouvel objet.

Vérifier dans `Program.cs` :

```csharp
var victoires = valorant.Filter(m => m.Won);
Console.WriteLine(valorant.Count);   // 25 — inchangé
Console.WriteLine(victoires.Count);  // sous-ensemble
```

---

## Étape 2 — Implémenter `.RemoveOutliers(isValid)`

**Avant de coder :** quelle est la différence entre `Filter` et `RemoveOutliers` ?
Peut-on éviter de dupliquer du code ?

<details>
<summary>Indice</summary>

`RemoveOutliers(isValid)` garde les éléments valides — c'est exactement `Filter(isValid)`.
Une méthode peut déléguer à une autre méthode de la même classe.

</details>

```csharp
public DataSeries<T> RemoveOutliers(Func<T, bool> isValid)
{
    // ...
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public DataSeries<T> RemoveOutliers(Func<T, bool> isValid)
    => Filter(isValid);
```

</details>

Appliquer à chaque jeu pour éliminer les valeurs impossibles :

```csharp
// Valorant : kills plausibles pour un match compétitif
var valorantValide = valorant.RemoveOutliers(m =>
    m.Kills   >= 0 && m.Kills   <= 50 &&
    m.Deaths  >= 1 && m.Deaths  <= 30 &&
    m.Assists >= 0
);

// CS2 : contraintes similaires
var cs2Valide = cs2.RemoveOutliers(m =>
    m.Kills + m.Assists <= 50 &&
    m.Deaths >= 1
);

// LoL : le support a structurellement peu de kills
var lolValide = lol.RemoveOutliers(m =>
    m.Kills   <= 10 &&
    m.Deaths  >= 1  &&
    m.Assists >= 0  &&
    m.Cs      >= 0
);
```

---

## Étape 3 — `.HasAny()` et `.AllMatch()`

**Avant de coder :** quelles méthodes LINQ répondent à "au moins un" et "tous" ?

<details>
<summary>Indice</summary>

`Any(predicate)` et `All(predicate)` — elles retournent un `bool`.

</details>

```csharp
public bool HasAny(Func<T, bool> predicate)  => // ...
public bool AllMatch(Func<T, bool> predicate) => // ...
```

<details>
<summary>Voir la solution</summary>

```csharp
public bool HasAny(Func<T, bool> predicate)
    => _data.Any(predicate);

public bool AllMatch(Func<T, bool> predicate)
    => _data.All(predicate);
```

</details>

Utilisation :

```csharp
Console.WriteLine(valorantValide.HasAny(m => m.Kills > 20));
// → Léa a-t-elle au moins un match avec plus de 20 kills ?

Console.WriteLine(lolValide.AllMatch(m => m.Deaths >= 1));
// → Tous les matchs de Noé ont-ils au moins 1 mort ?
```

→ Théorie : [Any et All — prédicats HOF](../../../supports/source/02b-filter.md#any-et-all-—-predicats-hof-booleens)

---

## Évaluation paresseuse — observation

`Filter` utilise `Where` qui est **paresseux** : aucune donnée n'est filtrée lors de l'appel.
L'exécution est reportée à la matérialisation (`Count`, `ToList`, `foreach`...).

```csharp
var query = valorant.Filter(m => m.Won);
// Rien n'est filtré encore — query décrit l'opération

var count = query.Count;
// Exécution ICI — tous les éléments sont parcourus maintenant
```

Ajouter un `Console.WriteLine` à l'intérieur du prédicat pour l'observer :

```csharp
var query = valorant.Filter(m =>
{
    Console.WriteLine($"Évaluation de {m.Player}");
    return m.Won;
});
// Aucune ligne affichée — pas encore exécuté

_ = query.Count; // Maintenant les lignes s'affichent
```

Conséquence surprenante de la paresse : la source peut changer **après** la construction de la query.

```csharp
var source = new List<double> { 1.0, 2.0, 3.0 };
var query = DataSeries<double>.From(source).Filter(x => x > 1.5); // Rien n'est filtré encore

source.Add(5.0);                // Modification de la source après

Console.WriteLine(query.Count); // Exécution ICI — 5.0 est inclus !
```

> Demo en classe : construire la query, modifier la source, observer. Surprenant ?
> L'exercice 07 introduira `.Snapshot()` pour figer une série et éviter ce piège.

---

## Étape 4 — Interface CLI

Ajouter les flags `--player <nom>` et `--filter wins|losses|all`.
Ces deux flags se combinent avec `--game` introduit en exercice 01.

**Avant de coder :** Si `--player` est absent, que filtrer ? Si `--filter` vaut `"all"`,
faut-il appliquer un prédicat ? Pourquoi la composabilité de `Filter` simplifie-t-elle
la combinaison de ces deux flags avec `--game` ?

<details>
<summary>Voir la solution</summary>

```csharp
string? player = args.Contains("--player")
    ? args[Array.IndexOf(args, "--player") + 1]
    : null;

string filterMode = args.Contains("--filter")
    ? args[Array.IndexOf(args, "--filter") + 1]
    : "all";
```

`--player` et `--filter` s'enchaînent naturellement : `Filter` retourne une `DataSeries` —
composabilité des flags = composabilité du pipeline.

</details>

---

## Vérification

- `valorant.Count` reste 25 après `Filter` (immuabilité)
- `RemoveOutliers` sur les données réelles ne retire aucun match (données déjà propres)
- `RemoveOutliers` sur les données générées (exercice 02) retire quelques matchs impossibles
- L'observation de la paresse confirme que le prédicat n'est pas appelé avant matérialisation
