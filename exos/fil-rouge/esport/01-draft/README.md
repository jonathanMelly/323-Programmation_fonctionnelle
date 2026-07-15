# Exercice 01 — Scouting

> Partie 1 — `DataPoint` + `DataSeries.From` + `DataSeries.FromCsv`

**Concepts FP :** Immutabilité — introduction, approfondie en exercice 07
→ Théorie : [Pureté et immutabilité](../../../supports/source/06-PureteImmutabilite.md#immutabilite) ·
[Impératif ou déclaratif](../../../supports/source/01-paradigmes.md#imperatif-ou-declaratif)

## Contexte

Le manager de Team Helvetia charge les profils du roster pour le prochain tournoi.
Léa joue dans deux jeux différents — ses données Valorant et CS2 ont des colonnes différentes.
Un outil capable de s'adapter aux deux formats est nécessaire.

C'est le rôle de `DataSeries<T>` : le type `T` change selon le jeu, le pipeline reste identique.

Deux projets, deux responsabilités :

```
┌──────────────────┐   utilise    ┌───────────────────────┐
│    EsportApp     │ ───────────► │    DataSeries<T>      │
│  (console app)   │              │   (class library)     │
│ connaît l'esport │              │ générique — ignore    │
│ parse les args   │              │ tout du domaine       │
└──────────────────┘              └───────────────────────┘
```

---

## Étape 1 — Modéliser un match (record immuable)

**Avant de coder :** ouvrir [../data/valorant.csv](../data/valorant.csv) et identifier les colonnes.
Quels champs faut-il pour représenter un match Valorant ?

<details>
<summary>Voir les champs à modéliser</summary>

- `Player` (string) — nom du joueur
- `Agent` (string) — personnage joué
- `Kills` (int)
- `Deaths` (int)
- `Assists` (int)
- `Headshots` (int)
- `RoundsWon` (int)
- `Won` (bool)

</details>

Définir le record dans `DataSeries/DataPoint.cs`, puis le record `ValorantMatch` dans `EsportApp/Program.cs` :

```csharp
public record DataPoint<T>(DateTime Timestamp, T Value);

public record ValorantMatch(
    string Player,
    string Agent,
    int Kills,
    // ...
);
```

<details>
<summary>Voir la solution complète</summary>

```csharp
public record DataPoint<T>(DateTime Timestamp, T Value);

public record ValorantMatch(
    string Player,
    string Agent,
    int Kills,
    int Deaths,
    int Assists,
    int Headshots,
    int RoundsWon,
    bool Won
);
```

</details>

Tenter de modifier un champ après création — que se passe-t-il ?

```csharp
var match = new ValorantMatch("Léa", "Jett", 18, 6, 4, 8, 13, true);
match.Kills = 20; // ?
```

> Un record C# est immuable par défaut. Pour "modifier" une valeur, on crée un nouvel objet
> avec `match with { Kills = 20 }`. L'original reste intact — premier principe FP appliqué.
>
> ```csharp
> var point = new DataPoint<double>(DateTime.Now, 3.14);
> // point.Value = 2.71; // Erreur de compilation — c'est voulu !
> var updated = point with { Value = 2.71 }; // Crée un nouveau record
> ```

---

## Étape 2 — `DataSeries<T>.From` (collection en mémoire)

**Avant de coder :** comment stocker une collection de façon à pouvoir la parcourir ?
Quel type C# représente "une séquence dont on ne connaît pas encore le contenu" ?

<details>
<summary>Indice sur le type à utiliser</summary>

`IEnumerable<T>` — il représente une séquence parcourable sans en connaître la taille ni le type concret.
Un champ `private readonly` empêche toute mutation ultérieure.

</details>

Implémenter dans `DataSeries/DataSeries.cs` :

```csharp
public class DataSeries<T>
{
    private readonly IEnumerable<T> _data;

    private DataSeries(IEnumerable<T> data) => // ...

    public static DataSeries<T> From(IEnumerable<T> source) => // ...

    public int Count => // ...
    public IEnumerable<T> Values => // ...
}
```

<details>
<summary>Voir la solution</summary>

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

> Note : éviter `ToList()` dans le constructeur — la raison sera expliquée en exercice 03 (paresse).

</details>

Vérifier avec trois matchs en dur :

```csharp
var matchs = new[]
{
    new ValorantMatch("Léa", "Jett",  18, 6, 4, 8,  13, true),
    new ValorantMatch("Léa", "Reyna", 22, 8, 2, 11,  9, false),
    new ValorantMatch("Léa", "Neon",  20, 7, 5,  9, 13, true),
};

var series = DataSeries<ValorantMatch>.From(matchs);
Console.WriteLine(series.Count); // 3
```

---

## Étape 3 — `DataSeries<T>.FromCsv` (import fichier)

**Avant de coder :** `FromCsv` doit fonctionner pour Valorant, CS2 et LoL.
Comment éviter d'écrire trois méthodes différentes ?
Quel paramètre permet de déléguer la logique de parsing à l'appelant ?

<details>
<summary>Indice sur la signature</summary>

Le parser est une fonction `Func<string[], T>` — elle reçoit les colonnes d'une ligne CSV
et retourne un objet du type souhaité. `FromCsv` reste générique, le domaine reste dans `EsportApp`.

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
    Player:    cols[1],
    Agent:     cols[2],
    Kills:     int.Parse(cols[3]),
    // ...
);
```

<details>
<summary>Voir les parsers complets (Valorant, CS2, LoL)</summary>

```csharp
ValorantMatch ParseValorant(string[] cols) => new ValorantMatch(
    Player:     cols[1],
    Agent:      cols[2],
    Kills:      int.Parse(cols[3]),
    Deaths:     int.Parse(cols[4]),
    Assists:    int.Parse(cols[5]),
    Headshots:  int.Parse(cols[6]),
    RoundsWon:  int.Parse(cols[7]),
    Won:        bool.Parse(cols[8])
);

record Cs2Match(string Player, string Map, string StartSide, int Kills, int Deaths, int Assists, int Mvps, bool Won);
// StartSide = côté joué en première mi-temps (CT ou T) — chaque match comprend les deux côtés

Cs2Match ParseCs2(string[] cols) => new Cs2Match(
    Player:    cols[1],
    Map:       cols[2],
    StartSide: cols[3],
    Kills:     int.Parse(cols[4]),
    Deaths:    int.Parse(cols[5]),
    Assists:   int.Parse(cols[6]),
    Mvps:      int.Parse(cols[7]),
    Won:       bool.Parse(cols[8])
);

record LolMatch(string Player, string Champion, int Kills, int Deaths, int Assists, int Cs, int VisionScore, bool Won);

LolMatch ParseLol(string[] cols) => new LolMatch(
    Player:      cols[1],
    Champion:    cols[2],
    Kills:       int.Parse(cols[4]),
    Deaths:      int.Parse(cols[5]),
    Assists:     int.Parse(cols[6]),
    Cs:          int.Parse(cols[7]),
    VisionScore: int.Parse(cols[8]),
    Won:         bool.Parse(cols[9])
);
```

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

var leaValorant = valorant.Values.Where(m => m.Player == "Léa");
Console.WriteLine($"Léa (Valorant) : {leaValorant.Count()} matchs");
```

Cette requête est déclarative : elle exprime QUOI faire — pas de boucle, pas de variable muable.
→ [Déclaratif vs Impératif — avec LINQ](../../../supports/source/01-paradigmes.md#declaratif-vs-imperatif-—-avec-linq)

---

## Étape 4 — Interface CLI

Ajouter dans `Program.cs` la gestion des flags `--help` et `--game`.

**Avant de coder :** Comment détecter la présence d'un flag dans `args` sans librairie externe ?
Comment récupérer la valeur qui suit immédiatement (`--game valorant`) ?
Que doit afficher l'application si aucun argument n'est fourni ?

<details>
<summary>Voir la solution</summary>

```csharp
static void Main(string[] args)
{
    if (args.Length == 0 || args.Contains("--help"))
    {
        Console.WriteLine("Usage: EsportApp [--game valorant|cs2|lol]");
        return;
    }

    string? game = null;
    if (args.Contains("--game"))
        game = args[Array.IndexOf(args, "--game") + 1];

    var valorant = DataSeries<ValorantMatch>.FromCsv("data/valorant.csv", ParseValorant);
    var cs2      = DataSeries<Cs2Match>.FromCsv("data/cs2.csv", ParseCs2);
    var lol      = DataSeries<LolMatch>.FromCsv("data/lol.csv", ParseLol);

    if (game == null || game == "valorant")
        Console.WriteLine($"Valorant : {valorant.Count} matchs");
    if (game == null || game == "cs2")
        Console.WriteLine($"CS2      : {cs2.Count} matchs");
    if (game == null || game == "lol")
        Console.WriteLine($"LoL      : {lol.Count} matchs");
}
```

</details>

---

## Vérification

- `valorant.Count` = 25, `cs2.Count` = 25, `lol.Count` = 25
- Léa a 13 matchs Valorant dans le CSV
- Tenter de modifier un record → erreur de compilation attendue
- `DataSeries<T>` ne connaît pas les types `ValorantMatch`, `Cs2Match`, `LolMatch` — généricité validée
