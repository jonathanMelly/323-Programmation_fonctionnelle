# Exercice 02 — Les recrues

> Partie 2 — `DataSeries.FromCsv` + `Enumerable.Range` + `Select` (map sur un range) + seed fixe

## Concepts théoriques

- [Thématique 02 — Filter et fonctions d'ordre supérieur](../../../thematiques/02-filter-fonctions-sup.md)
- [Fonctions d'ordre supérieur](../../../../supports/source/02a-fonctions-sup.md)
- [Closures](../../../../supports/source/02a-fonctions-sup.md#closures-captures-de-variables)
- [Évaluation paresseuse](../../../../supports/source/02b-filter.md#evaluation-paresseuse-deferred-execution)

## Contexte

Les matchs en dur de l'exercice 01 ne suffisent plus : les 75 matchs de la saison attendent
dans `data/`. Ensuite, place aux recrues : Raphaël, Noé, Dylan et Kiara n'ont rejoint
Team Helvetia qu'en cours de saison — leurs matchs précédents sont perdus. Le data analyst
doit simuler des données plausibles pour compléter l'historique.

---

## Étape 1 — `DataSeries<T>.FromCsv` (import fichier)

Première **fonction d'ordre supérieur** concrète du cours : une méthode qui reçoit une *fonction*
en paramètre.

**Avant de coder :** `FromCsv` doit fonctionner pour Valorant, CS2 et LoL.
Comment éviter d'écrire trois méthodes différentes ?
Quel paramètre permet de déléguer la logique de parsing à l'appelant ?

<details>
<summary>Indice sur la signature</summary>

Le parser est une fonction `Func<string[], T>` — elle reçoit les colonnes d'une ligne CSV
et retourne un objet du type souhaité. `FromCsv` reste générique, le domaine reste dans `EsportApp`.
→ [Func et Action](../../../../supports/source/02a-fonctions-sup.md)

</details>

Implémenter la méthode dans `DataSeries.cs` :

```csharp
public static DataSeries<T> FromCsv(string path, Func<string[], T> parser)
{
    // lire le fichier, ignorer la première ligne (en-tête)
    // parser chaque ligne en la découpant par ','
    // ...
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public static DataSeries<T> FromCsv(string path, Func<string[], T> parser)
{
    var lines = File.ReadAllLines(path).Skip(1); // ignorer l'en-tête
    return new DataSeries<T>(lines.Select(line => parser(line.Split(','))));
}
```

</details>

**Avant de coder les parsers :** pour chaque jeu, identifier l'indice de chaque colonne dans le CSV
(la première colonne `date` est à l'indice 0).

```csharp
// Valorant : date(0), player(1), agent(2), kills(3), deaths(4), assists(5), ...
ValorantMatch ParseValorant(string[] cols) => new ValorantMatch(
    cols[1],              // player
    cols[2],              // agent
    int.Parse(cols[3]),   // kills
    // ...
);
```

<details>
<summary>Voir les parsers complets (Valorant, CS2, LoL)</summary>

```csharp
ValorantMatch ParseValorant(string[] cols) => new ValorantMatch(
    cols[1],              // player
    cols[2],              // agent
    int.Parse(cols[3]),   // kills
    int.Parse(cols[4]),   // deaths
    int.Parse(cols[5]),   // assists
    int.Parse(cols[6]),   // headshots
    int.Parse(cols[7]),   // roundsWon
    bool.Parse(cols[8])   // won
);

Cs2Match ParseCs2(string[] cols) => new Cs2Match(
    cols[1],              // player
    cols[2],              // map
    cols[3],              // startSide (côté joué en 1re mi-temps — CT ou T)
    int.Parse(cols[4]),   // kills
    int.Parse(cols[5]),   // deaths
    int.Parse(cols[6]),   // assists
    int.Parse(cols[7]),   // mvps
    bool.Parse(cols[8])   // won
);

LolMatch ParseLol(string[] cols) => new LolMatch(
    cols[1],              // player
    cols[2],              // champion
    int.Parse(cols[4]),   // kills
    int.Parse(cols[5]),   // deaths
    int.Parse(cols[6]),   // assists
    int.Parse(cols[7]),   // cs
    int.Parse(cols[8]),   // visionScore
    bool.Parse(cols[9])   // won
);
```

Les classes `ValorantMatch`, `Cs2Match` et `LolMatch` viennent de l'exercice 01.

</details>

Charger les trois fichiers et vérifier le total :

```csharp
var valorant = DataSeries<ValorantMatch>.FromCsv("data/valorant.csv", ParseValorant);
var cs2      = DataSeries<Cs2Match>.FromCsv("data/cs2.csv", ParseCs2);
var lol      = DataSeries<LolMatch>.FromCsv("data/lol.csv", ParseLol);

Console.WriteLine($"Valorant : {valorant.Count} matchs");
Console.WriteLine($"CS2      : {cs2.Count} matchs");
Console.WriteLine($"LoL      : {lol.Count} matchs");
// Total : 75 matchs
```

Un seul `FromCsv` pour trois formats différents : la logique de parsing est une **valeur**
passée en paramètre. C'est exactement ce qui manquait à l'exercice 01.

---

## Étape 2 — Générer 20 matchs CS2 pour Raphaël

`Enumerable.Range` génère une séquence d'entiers. Combiné avec `Select`, il devient un
**générateur fonctionnel** — l'équivalent d'une boucle for, mais déclaratif :

```csharp
// Impératif
var list = new List<int>();
for (int i = 1; i <= 20; i++)
    list.Add(i * 2);

// Fonctionnel — même résultat
var list = Enumerable.Range(1, 20).Select(i => i * 2);
```

La seed fixe (`new Random(42)`) garantit un résultat **reproductible** — propriété essentielle
pour des données de test : tout le monde obtient les mêmes valeurs.

**Avant de coder :** quelles informations faut-il générer aléatoirement pour un match CS2 ?
Quelles valeurs sont réalistes en MR12 (maximum 24 rounds par match) ?

<details>
<summary>Voir les plages de valeurs réalistes</summary>

- `Map` : tirer au sort parmi `{ "Dust2", "Mirage", "Inferno", "Nuke", "Ancient" }`
- `StartSide` : `"CT"` ou `"T"` (côté de la première mi-temps)
- `Kills` : entre 10 et 27 (MR12 = max 24 rounds, une kill par round est déjà très fort)
- `Deaths` : entre 6 et 17
- `Assists` : entre 0 et 7
- `Mvps` : entre 0 et 4
- `Won` : aléatoire 50/50

</details>

Créer `EsportApp/MatchGenerator.cs` avec la méthode `GenerateCs2` :

```csharp
public static class MatchGenerator
{
    public static IEnumerable<Cs2Match> GenerateCs2(string player, int count, int seed = 42)
    {
        var rng   = new Random(seed);
        var maps  = new[] { "Dust2", "Mirage", /* ... */ };
        var sides = new[] { "CT", "T" };

        return Enumerable.Range(1, count)
            .Select(i => new Cs2Match(
                player,
                maps[rng.Next(maps.Length)],
                // ...
            ));
    }
}
```

> La lambda passée à `Select` utilise `rng`, `maps` et `sides` déclarés *en dehors* d'elle :
> c'est une **closure** — la fonction capture les variables de son environnement.
> → [Closures](../../../../supports/source/02a-fonctions-sup.md#closures-captures-de-variables)

<details>
<summary>Voir la solution complète</summary>

```csharp
public static class MatchGenerator
{
    public static IEnumerable<Cs2Match> GenerateCs2(string player, int count, int seed = 42)
    {
        var rng   = new Random(seed);
        var maps  = new[] { "Dust2", "Mirage", "Inferno", "Nuke", "Ancient" };
        var sides = new[] { "CT", "T" };

        return Enumerable.Range(1, count)
            .Select(i => new Cs2Match(
                player,
                maps[rng.Next(maps.Length)],
                sides[rng.Next(2)],
                rng.Next(10, 28),   // kills
                rng.Next(6, 18),    // deaths
                rng.Next(0, 8),     // assists
                rng.Next(0, 5),     // mvps
                rng.Next(2) == 0    // won
            ));
    }
}
```

</details>

Dans `Program.cs` :

```csharp
var raphaelGenerated = DataSeries<Cs2Match>.From(
    MatchGenerator.GenerateCs2("Raphaël", 20)
);
Console.WriteLine(raphaelGenerated.Count); // 20
```

**Ajouter des contraintes réalistes.** Quelles combinaisons générées sont physiquement
impossibles dans CS2 ?

<details>
<summary>Voir les contraintes à appliquer</summary>

- `kills + assists > 50` : impossible dans un match CS2
- `deaths == 0` : n'arrive jamais sur une série complète de matchs

</details>

Le prédicat de validation est une **valeur** : stockée dans une variable, nommée,
réutilisable pour les 4 joueurs.

```csharp
Func<Cs2Match, bool> isValid = m =>
    m.Kills + m.Assists <= 50 &&
    m.Deaths >= 1;

var raphaelValid = raphaelGenerated.Values.Where(isValid);
Console.WriteLine($"Avant : {raphaelGenerated.Count}, après : {raphaelValid.Count()}");
```

> Ici, `Where` de LINQ fait le travail — l'exercice 03 intégrera `.Filter()`
> directement à la bibliothèque `DataSeries<T>`.

Observer la perte de données — combien de matchs ont été filtrés ?
C'est le comportement attendu : des contraintes métier éliminent des cas impossibles.

---

## Étape 3 — Exporter en CSV

**Avant de coder :** quel format doit avoir le CSV exporté pour être compatible avec `FromCsv`
de l'étape 1 ? Regarder l'en-tête de `data/cs2.csv`.

Écrire une fonction `ExportCs2` qui génère les lignes CSV et les écrit dans un fichier :

```csharp
void ExportCs2(string player, IEnumerable<Cs2Match> matches, string path)
{
    var header = "date,player,map,start_side,kills,deaths,assists,mvps,won";
    var lines = matches.Select((m, i) =>
        // construire la ligne CSV pour chaque match
        // ...
    );
    File.WriteAllLines(path, lines.Prepend(header));
}
```

<details>
<summary>Voir la solution</summary>

```csharp
void ExportCs2(string player, IEnumerable<Cs2Match> matches, string path)
{
    var header = "date,player,map,start_side,kills,deaths,assists,mvps,won";
    var lines = matches.Select((m, i) =>
        $"2024-01-{i + 1:D2},{m.Player},{m.Map},{m.StartSide},{m.Kills},{m.Deaths},{m.Assists},{m.Mvps},{m.Won.ToString().ToLower()}"
    );
    File.WriteAllLines(path, lines.Prepend(header));
}

// Générer et exporter
ExportCs2("Raphaël", raphaelValid, "raphael_generated.csv");
```

</details>

Générer également pour Kiara (CS2), Dylan (Valorant) et Noé (LoL) avec des méthodes
similaires adaptées à chaque format CSV.

> Le paramètre `seed` peut varier par joueur pour obtenir des profils différents :
> `GenerateCs2("Kiara", 20, seed: 7)` → profil AWPer avec plus de kills et plus de variance.

---

## Étape 4 — Interface CLI

Ajouter le flag `--generate <joueur|all>` pour déclencher la génération depuis la ligne de commande.

**Avant de coder :** Si `--generate all` est passé, comment obtenir la liste des quatre joueurs ?
Comment structurer le code pour que `--generate Raphaël` ne génère que ce joueur ?
Pourquoi utiliser `return` après la génération ?

<details>
<summary>Voir la solution</summary>

```csharp
if (args.Contains("--generate"))
{
    var target = args[Array.IndexOf(args, "--generate") + 1];

    var players = target == "all"
        ? new[] { "Raphaël", "Kiara", "Dylan", "Noé" }
        : new[] { target };

    foreach (var player in players)
    {
        var series = DataSeries<Cs2Match>.From(MatchGenerator.GenerateCs2(player, 20));
        ExportCs2(player, series.Values.Where(isValid), $"{player.ToLower()}_generated.csv");
        Console.WriteLine($"{player} : données générées et exportées");
    }
    return;
}
```

Le même prédicat `isValid` sert pour tous les joueurs — une fonction stockée dans
une variable se réutilise comme n'importe quelle valeur.

</details>

---

## Vérification

- 75 matchs chargés depuis les CSV : `valorant.Count` = 25, `cs2.Count` = 25, `lol.Count` = 25
- 4 fichiers CSV créés, chacun avec 15 à 20 lignes (quelques matchs filtrés)
- Changer la seed → valeurs différentes ; même seed → résultat identique à chaque exécution
- `raphaelValid.Count()` < 20 (au moins quelques matchs filtrés par les contraintes)
- Les CSV exportés ont le même format que `data/cs2.csv` — importables avec `FromCsv`
