# Exercice 06 — Dashboard ESL

> Partie 5 — Extensions fluentes + `ToCsv()` + `WithFallback()` + `PairWith()` + DSL

**Concepts FP :** Composition · DSL (Domain Specific Language) · Zip comme traitement parallèle
→ Théorie : [Méthodes d'extension et chaînage](../../../supports/source/05-Extension.md) ·
[Composition de fonctions f ∘ g](../../../supports/source/05-Extension.md#composition-de-fonctions-f-∘-g) ·
[DSL](../../../supports/source/05-Extension.md#dsl-domain-specific-language)

## Contexte

L'équipe présente ses stats à un tournoi ESL. Le data analyst prépare un rapport
hebdomadaire : export CSV pour l'organisateur, gestion des données manquantes,
comparaison kills vs assists côte à côte.

Un pipeline fluent lisible comme du langage naturel est l'objectif :

```csharp
kdaLea
    .Smooth(windowSize: 3)
    .WithFallback(0.0, v => double.IsNaN(v))
    .PairWith(kdaDylan)
    .ToCsv("report.csv");
```

**Avant de coder :**

- Pourquoi des méthodes d'*extension* plutôt que des méthodes dans la classe `DataSeries<T>` ?
- Quel contrat chaque méthode doit-elle respecter pour que le chaînage reste possible ?

<details>
<summary>Indice</summary>

Le contrat de la composition : chaque méthode retourne le même type qu'elle reçoit
(ou un type compatible). C'est ce qui permet `x.g().f()` — l'équivalent C# de `(f ∘ g)(x)`.
→ [Le contrat de la composition](../../../supports/source/05-Extension.md#le-contrat-de-la-composition)

</details>

---

## Étape 1 — `ToCsv()` dans `DataSeriesExtensions.cs`

Créer `DataSeries/DataSeriesExtensions.cs` :

```csharp
public static class DataSeriesExtensions
{
    public static void ToCsv(this DataSeries<double> series, string path)
    {
        // générer les lignes "i,valeur" et écrire dans le fichier avec un en-tête
        // ...
    }
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public static void ToCsv(this DataSeries<double> series, string path)
{
    var lines = series.Values.Select((v, i) => $"{i},{v:F4}");
    File.WriteAllLines(path, lines.Prepend("index,value"));
}
```

</details>

---

## Étape 2 — `WithFallback(fallback, isMissing)`

`WithFallback` n'est qu'un `Transform` conditionnel :

```csharp
public static DataSeries<T> WithFallback<T>(
    this DataSeries<T> series, T fallback, Func<T, bool> isMissing)
{
    // ...
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public static DataSeries<T> WithFallback<T>(
    this DataSeries<T> series, T fallback, Func<T, bool> isMissing)
    => series.Transform(v => isMissing(v) ? fallback : v);
```

</details>

---

## Étape 3 — `PairWith()` — kills vs assists côte à côte

`Zip` (ici `PairWith`) combine deux listes élément par élément — pattern fondamental pour
traiter des séries corrélées (kills + assists, température + humidité...).

```csharp
public static DataSeries<(double Left, double Right)> PairWith(
    this DataSeries<double> left, DataSeries<double> right)
{
    // Hint : Zip combine deux séquences élément par élément
    // ...
}
```

<details>
<summary>Voir la solution</summary>

```csharp
public static DataSeries<(double Left, double Right)> PairWith(
    this DataSeries<double> left, DataSeries<double> right)
    => DataSeries<(double, double)>.From(
        left.Values.Zip(right.Values, (l, r) => (l, r))
    );
```

</details>

Comparer kills normalisés et assists normalisés de Léa :

```csharp
var kills   = valorant.Filter(m => m.Player == "Léa").Transform(m => (double)m.Kills);
var assists = valorant.Filter(m => m.Player == "Léa").Transform(m => (double)m.Assists);

var rapport = kills.Normalize().PairWith(assists.Normalize());
foreach (var (k, a) in rapport.Values)
    Console.WriteLine($"kills={k:F2}  assists={a:F2}");
```

---

## DSL final — rapport hebdomadaire en une expression

Les extensions définissent le vocabulaire du domaine. Un bon DSL se lit sans avoir besoin
de connaître l'implémentation :
→ [DSL](../../../supports/source/05-Extension.md#dsl-domain-specific-language)

```csharp
valorant
    .Filter(m => m.Player == "Léa")
    .Transform(m => (double)m.Kills)
    .Normalize()
    .Smooth(3)
    .WithFallback(0.0, v => double.IsNaN(v))
    .ToCsv("lea_kills_smoothed.csv");
```

---

## Étape 4 — Interface CLI

Ajouter `--export <fichier>` pour déclencher l'export CSV depuis la ligne de commande.

**Avant de coder :** À quel endroit du pipeline appeler `ToCsv` — avant ou après `Smooth` ?
Comment récupérer le nom du fichier cible depuis `args` ?

```
dotnet run -- --game valorant --player Léa --stat kda --export lea_kda.csv
```

<details>
<summary>Voir la solution</summary>

```csharp
if (args.Contains("--export"))
{
    var fichier = args[Array.IndexOf(args, "--export") + 1];
    valeurs.ToCsv(fichier);
    Console.WriteLine($"Exporté : {fichier}");
}
```

</details>

---

## Vérification

- `ToCsv` produit un fichier importable dans Excel
- `WithFallback` sur des données propres ne modifie aucun élément
- `PairWith` sur deux séries de longueur différente → Zip s'arrête à la plus courte
- Le pipeline DSL compile et produit un résultat identique aux étapes intermédiaires
