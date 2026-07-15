# Exercice 02 — Générer les données manquantes

> Partie 2 — `Enumerable.Range` + `Select` (map sur un range) + seed fixe

## Contexte

Les données de Léa couvrent toute la saison.
Raphaël, Noé, Dylan et Kiara n'ont rejoint Team Helvetia qu'en cours de saison —
leurs matchs précédents sont perdus. Le data analyst doit simuler des données plausibles
pour estimer leurs performances passées et compléter l'historique.

---

## Concept FP : Map sur un range

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

→ Théorie : [Fonctions d'ordre supérieur](../../../supports/source/02a-fonctions-sup.md) ·
[Évaluation paresseuse](../../../supports/source/02b-filter.md#evaluation-paresseuse-deferred-execution)

---

## Étape 1 — Générer 20 matchs CS2 pour Raphaël

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
                Player:    player,
                Map:       maps[rng.Next(maps.Length)],
                // ...
            ));
    }
}
```

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
                Player:    player,
                Map:       maps[rng.Next(maps.Length)],
                StartSide: sides[rng.Next(2)],
                Kills:     rng.Next(10, 28),
                Deaths:    rng.Next(6, 18),
                Assists:   rng.Next(0, 8),
                Mvps:      rng.Next(0, 5),
                Won:       rng.Next(2) == 0
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

---

## Étape 2 — Ajouter des contraintes réalistes avec Filter

**Avant de coder :** quelles combinaisons générées sont physiquement impossibles dans CS2 ?

<details>
<summary>Voir les contraintes à appliquer</summary>

- `kills + assists > 50` : impossible dans un match CS2
- `deaths == 0` : n'arrive jamais sur une série complète de matchs

</details>

Appliquer `Filter` (implémenté en exercice 03) pour nettoyer :

```csharp
Func<Cs2Match, bool> valide = m =>
    m.Kills + m.Assists <= 50 &&
    m.Deaths >= 1;

var raphaelValide = raphaelGenerated.Filter(valide);
Console.WriteLine($"Avant : {raphaelGenerated.Count}, après : {raphaelValide.Count}");
```

Observer la perte de données — combien de matchs ont été filtrés ?
C'est le comportement attendu : des contraintes métier éliminent des cas impossibles.

---

## Étape 3 — Exporter en CSV

**Avant de coder :** quel format doit avoir le CSV exporté pour être compatible avec `FromCsv` de l'exercice 01 ?
Regarder l'en-tête de `data/cs2.csv`.

Écrire une fonction `ExportCs2` qui génère les lignes CSV et les écrit dans un fichier :

```csharp
void ExportCs2(string player, IEnumerable<Cs2Match> matchs, string path)
{
    var header = "date,player,map,start_side,kills,deaths,assists,mvps,won";
    var lignes = matchs.Select((m, i) =>
        // construire la ligne CSV pour chaque match
        // ...
    );
    File.WriteAllLines(path, lignes.Prepend(header));
}
```

<details>
<summary>Voir la solution</summary>

```csharp
void ExportCs2(string player, IEnumerable<Cs2Match> matchs, string path)
{
    var header = "date,player,map,start_side,kills,deaths,assists,mvps,won";
    var lignes = matchs.Select((m, i) =>
        $"2024-01-{i + 1:D2},{m.Player},{m.Map},{m.StartSide},{m.Kills},{m.Deaths},{m.Assists},{m.Mvps},{m.Won.ToString().ToLower()}"
    );
    File.WriteAllLines(path, lignes.Prepend(header));
}

// Générer et exporter
ExportCs2("Raphaël", raphaelValide.Values, "raphael_generated.csv");
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
    var cible = args[Array.IndexOf(args, "--generate") + 1];

    var joueurs = cible == "all"
        ? new[] { "Raphaël", "Kiara", "Dylan", "Noé" }
        : new[] { cible };

    foreach (var joueur in joueurs)
    {
        var serie = DataSeries<Cs2Match>.From(MatchGenerator.GenerateCs2(joueur, 20));
        ExportCs2(joueur, serie.Filter(valide).Values, $"{joueur.ToLower()}_generated.csv");
        Console.WriteLine($"{joueur} : données générées et exportées");
    }
    return;
}
```

</details>

---

## Vérification

- 4 fichiers CSV créés, chacun avec 15 à 20 lignes (quelques matchs filtrés)
- Changer la seed → valeurs différentes ; même seed → résultat identique à chaque exécution
- `raphaelValide.Count` < 20 (au moins quelques matchs filtrés par les contraintes)
- Les CSV exportés ont le même format que `data/cs2.csv` — importables avec `FromCsv`
