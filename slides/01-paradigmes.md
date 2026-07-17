---
theme: default
title: "Paradigmes & Généricité"
info: "Impératif vs déclaratif, LINQ, généricité et DataSeries<T>"
author: ETML
transition: slide-left
mdc: true
---

# Paradigmes de programmation

## Fonctionnel avec C# & LINQ

<div class="pt-12">
  <span class="px-2 py-1 rounded bg-blue-500 text-white">
    Thématique 01
  </span>
</div>

---

# Tu écris 15 lignes de boucles pour filtrer une liste.
# Ton collègue fait la même chose en 1 ligne.

<v-clicks>

```csharp
// Toi — décrire chaque étape
var wins = new List<Match>();
foreach (var m in matches)
    if (m.Won) wins.Add(m);
// [cs2✓, val✓]
```

```csharp
// Ton collègue — décrire ce qu'il veut
var wins = matches.Where(m => m.Won);
// [cs2✓, val✓]
```

<div class="mt-4 p-3 bg-blue-700 rounded text-blue-300">

Il ne décrit pas **comment** filtrer — il décrit **ce qu'il veut**.
C'est le passage de l'impératif au déclaratif.

</div>

</v-clicks>

---

# Plan

<v-clicks>

1. **La carte des paradigmes** — se situer dans le paysage
2. **Impératif vs Déclaratif** — deux façons de penser
3. **Généricité** — abstraire les types, pas seulement les valeurs
4. **DataSeries&lt;T&gt;** — la brique du fil rouge

</v-clicks>

---
layout: section
---

# Partie 1
## La carte des paradigmes

---

# Où sommes-nous ?

```text
Paradigmes de programmation
├── Impératif
│       ├── Procédural          (C, Go…)     ← 1ère année ETML
│       ├── Orienté objet       (C#, Java…)  ← 2ème année ETML
│       └── Parallèle/Concurrent
└── Déclaratif
        ├── Fonctionnel         (F#, C#…)    ← 3ème année ETML 😎
        ├── Logique             (Prolog)
        └── Contraintes
```

<v-click>
<div class="mt-4 p-3 bg-orange-100 rounded text-orange-900">

C# est **multi-paradigmes** — impératif et fonctionnel coexistent dans le même fichier.
L'outil ne change pas, c'est la façon de l'utiliser qui change.

</div>
</v-click>

---

# Le même problème, trois époques

<v-clicks>

```csharp {all}
// 1. Procédural — séquence d'ordres  (1ère année)
for (int i = 0; i < 5; i++)
    Console.WriteLine($"Match {i}");
// → Match 0, Match 1, Match 2, Match 3, Match 4
```

```csharp {all}
// 2. Orienté objet — données + comportement  (2ème année)
for (int i = 0; i < 5; i++)
    Console.WriteLine(new Match(i));
// → Match 0, Match 1, Match 2, Match 3, Match 4
```

```csharp {all}
// 3. Fonctionnel — pipeline de transformations  (3ème année)
Enumerable.Range(0, 5)
    .Select(i => $"Match {i}")
    .ToList()
    .ForEach(Console.WriteLine);
// → Match 0, Match 1, Match 2, Match 3, Match 4
```

</v-clicks>

---
layout: section
---

# Partie 2
## Généricité — abstraire les types

---

# Trois jeux, même traitement — sans généricité

Team Helvetia joue à Valorant, CS2 et LoL.
Stocker, compter, filtrer : logique **identique** pour les trois.

<v-clicks>

```csharp
public class ValorantSeries { private IEnumerable<ValorantMatch> _data; /* ... */ }
public class Cs2Series      { private IEnumerable<Cs2Match>     _data; /* ... */ }
public class LolSeries      { private IEnumerable<LolMatch>     _data; /* ... */ }
```

<div class="mt-1 p-1 bg-red-700 rounded text-red-100">

Trois fois le même code → trois fois les mêmes bugs à corriger.

</div>

</v-clicks>

---

# `List<T>` : généricité déjà utilisée sans y penser

```csharp {1-3|5-6|all}
// T = paramètre de TYPE — remplacé à l'utilisation
var names   = new List<string>();         // T = string
var scores  = new List<int>();            // T = int

// Comme un paramètre de fonction, mais pour les types
var matches = new List<ValorantMatch>();  // T = ValorantMatch
```

<v-click>
<div class="mt-4 p-3 bg-blue-700 rounded text-blue-300">

Une seule définition `List<T>` fonctionne pour tous les types — c'est l'abstraction des données.

</div>
</v-click>

---

# Définir `DataSeries<T>`

```csharp {1-7|9-11|all}
public class DataSeries<T>
{
    private readonly IEnumerable<T> _data;
    private DataSeries(IEnumerable<T> data) => _data = data;

    public static DataSeries<T> From(IEnumerable<T> source)
        => new DataSeries<T>(source);

    public int Count             => _data.Count();
    public IEnumerable<T> Values => _data;
}
```

<v-click>

```csharp
var val   = DataSeries<ValorantMatch>.From(matches);  // Count → 12
var temps = DataSeries<double>.From(temperatures);    // Count → 24
// Un seul DataSeries<T> — deux domaines complètement différents
```

</v-click>

---

# Types vs comportement

<div class="grid grid-cols-2 gap-6 mt-4">
<div>

### Généricité de type ← ici
Une structure pour n'importe quel contenu

```csharp
DataSeries<ValorantMatch>
DataSeries<double>
DataSeries<string>
```

**Abstraction des données**

</div>
<v-click>
<div>

### Généricité de comportement ← thématique 02
Une règle passée en paramètre

```csharp
Func<ValorantMatch, bool> isWin
Func<double, bool> isAboveZero
```

**Abstraction du traitement**

</div>
</v-click>
</div>

<v-click>
<div class="mt-4 p-3 bg-orange-100 rounded text-orange-900">

`Func<T, TResult>` combine les deux — types génériques **et** logique transportable.
La brique de base de tout ce qui suit dans ce cours.

</div>
</v-click>

---
layout: center
class: text-center
---

<v-click every=1>

Décrire **ce que l'on veut** — le compilateur se charge du comment

<div class="pt-12 mb-4">
  <span class="px-4 py-2 rounded bg-blue-500 text-white text-xl">
    Déclaratif · Généricité · DataSeries&lt;T&gt;
  </span>
</div>

# Questions ?

<div class="mt-8 text-gray-500">

Prochaine étape : fonctions d'ordre supérieur & Filter

</div>
</v-click>
