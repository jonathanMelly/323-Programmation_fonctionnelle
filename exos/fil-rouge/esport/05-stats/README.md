# Exercice 05 — Classement de saison

> Partie 4 — `.Fold()` + `.Statistics()` + `.SlidingWindow()`

## Contexte

Établir le classement officiel des 5 joueurs de Team Helvetia pour la saison.
Le coaching staff veut savoir : qui est le plus régulier ? qui progresse le plus vite ?

`.Fold()` est l'outil fondamental qui permet de répondre à toutes ces questions
en une seule abstraction.

---

## Concept FP : Fold — l'agrégation universelle

`Sum`, `Count`, `Max`, `Any`, `All` sont tous des cas particuliers de `Fold`.
Implémenter `Fold` une seule fois suffit à exprimer n'importe quelle agrégation.

```
[a, b, c, d] avec seed s et f :
s → f(s, a) → f(f(s,a), b) → f(f(f(s,a),b), c) → résultat final
```

→ Théorie : [Reduce / Aggregate](../../../supports/source/04-Reduce.md) ·
[Fold — l'agrégation universelle](../../../supports/source/04-Reduce.md#fold-—-l-agregation-universelle)

---

## Étape 1 — Implémenter `.Fold<TResult>()`

**Avant de coder :** quelle méthode LINQ fait exactement ce que décrit le schéma ci-dessus —
accumuler une valeur en appliquant une fonction à chaque élément ?

<details>
<summary>Indice</summary>

`Aggregate(seed, combiner)` — c'est le Fold de LINQ.
La méthode de la bibliothèque n'a qu'à déléguer à `Aggregate`.

</details>

```csharp
public TResult Fold<TResult>(TResult seed, Func<TResult, T, TResult> combiner)
{
    // ...
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public TResult Fold<TResult>(TResult seed, Func<TResult, T, TResult> combiner)
    => _data.Aggregate(seed, combiner);
```

</details>

Réécrire les agrégations classiques avec `Fold` sur les KDA de Léa :

```csharp
var kdaValues = kdaLea; // DataSeries<double>

var somme   = kdaValues.Fold(0.0, (acc, val) => acc + val);
var nb      = kdaValues.Fold(0,   (acc, _)   => acc + 1);
var meilleur = kdaValues.Fold(double.MinValue, (acc, val) => val > acc ? val : acc);

var moyenne = somme / nb;
Console.WriteLine($"KDA moyen de Léa : {moyenne:F2}");
Console.WriteLine($"KDA max de Léa   : {meilleur:F2}");
```

Reproduire pour les 4 autres joueurs et afficher le classement.

---

## Étape 2 — `.SlidingWindow(size)` — progression mensuelle

**Avant de coder :** une fenêtre glissante de taille 5 à partir d'une liste de 13 éléments
produit combien de fenêtres ? Quelle formule générale ?

<details>
<summary>Indice</summary>

`count - size + 1` fenêtres. Pour 13 éléments avec taille 5 : `13 - 5 + 1 = 9` fenêtres.

</details>

```csharp
public IEnumerable<DataSeries<T>> SlidingWindow(int size)
{
    var values = _data.ToList();
    return Enumerable.Range(0, Math.Max(0, values.Count - size + 1))
        .Select(i => // extraire une fenêtre de `size` éléments à partir de l'indice i
        );
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public IEnumerable<DataSeries<T>> SlidingWindow(int size)
{
    var values = _data.ToList();
    return Enumerable.Range(0, Math.Max(0, values.Count - size + 1))
        .Select(i => DataSeries<T>.From(values.Skip(i).Take(size)));
}
```

</details>

Calculer la moyenne KDA par fenêtre de 5 matchs pour Léa :

```csharp
var progression = kdaLea
    .SlidingWindow(5)
    .Select(fenetre => fenetre.Fold(0.0, (acc, v) => acc + v) / 5);

Console.WriteLine("Progression KDA Léa (fenêtres de 5 matchs) :");
foreach (var moy in progression)
    Console.WriteLine($"  {moy:F2}");
```

---

## Étape 3 — `.Statistics()` — qui est le plus régulier ?

```csharp
public record SeriesStats(double Min, double Max, double Mean, double StdDev);

public SeriesStats Statistics()
{
    var values   = _data.Cast<double>().ToList();
    var mean     = // ...
    var variance = // ...
    return new SeriesStats(Min: /* ... */, Max: /* ... */, Mean: mean, StdDev: /* ... */);
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public SeriesStats Statistics()
{
    var values   = _data.Cast<double>().ToList();
    var mean     = values.Aggregate(0.0, (acc, v) => acc + v) / values.Count;
    var variance = values.Aggregate(0.0, (acc, v) => acc + Math.Pow(v - mean, 2)) / values.Count;
    return new SeriesStats(
        Min:    values.Min(),
        Max:    values.Max(),
        Mean:   mean,
        StdDev: Math.Sqrt(variance)
    );
}
```

</details>

Comparer les profils — un écart-type faible = joueur régulier :

```csharp
var statsLea     = kdaLea.Statistics();
var statsRaphael = kdaRaphael.Statistics();
Console.WriteLine($"Léa     — KDA moy : {statsLea.Mean:F2}, écart-type : {statsLea.StdDev:F2}");
Console.WriteLine($"Raphaël — KDA moy : {statsRaphael.Mean:F2}, écart-type : {statsRaphael.StdDev:F2}");
```

Qui mérite la place de titulaire aux playoffs ?

---

## Étape 4 — Interface CLI

Ajouter `--rank` pour afficher le classement des joueurs par KDA moyen,
et `--window <n>` pour afficher la progression sur des fenêtres glissantes.

**Avant de coder :** Comment trier une collection de tuples `(nom, kdaMoyen)` par valeur décroissante ?
Pour `--window`, comment récupérer `n` sous forme d'entier depuis `args` ?

```
dotnet run -- --rank
dotnet run -- --game valorant --player Léa --stat kda --window 3
```

<details>
<summary>Voir la solution</summary>

```csharp
if (args.Contains("--rank"))
{
    var joueurs = new[]
    {
        ("Léa",     kdaLea.Fold(0.0,     (a, v) => a + v) / kdaLea.Count),
        ("Raphaël", kdaRaphael.Fold(0.0, (a, v) => a + v) / kdaRaphael.Count),
        ("Noé",     kdaNoe.Fold(0.0,     (a, v) => a + v) / kdaNoe.Count),
        ("Dylan",   kdaDylan.Fold(0.0,   (a, v) => a + v) / kdaDylan.Count),
        ("Kiara",   kdaKiara.Fold(0.0,   (a, v) => a + v) / kdaKiara.Count),
    };
    foreach (var (nom, kda) in joueurs.OrderByDescending(j => j.Item2))
        Console.WriteLine($"{nom,-10} KDA moy : {kda:F2}");
}

int window = args.Contains("--window")
    ? int.Parse(args[Array.IndexOf(args, "--window") + 1])
    : 5;
```

</details>

---

## Vérification

- `Fold` sur liste vide retourne `seed`
- `SlidingWindow(5)` sur 13 matchs produit 9 fenêtres (13 - 5 + 1 = 9)
- `Statistics().Mean` correspond à `Fold(0.0, (acc,v)=>acc+v) / Count`
- Les écarts-types permettent de distinguer les profils réguliers des profils variables
