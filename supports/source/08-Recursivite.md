# Récursivité

La récursivité est une technique de programmation où une fonction s'appelle elle-même pour résoudre un problème en le divisant en sous-problèmes plus simples, jusqu'à atteindre une condition d'arrêt. C'est un concept central dans de nombreux algorithmes, notamment dans les structures de données comme les arbres ou pour résoudre des problèmes complexes de manière élégante.

Elle se définit comme ceci:

> Une fonction F peut résoudre un problème P de manière récursive si 
>   - Le problème P est trivial. Dans ce cas la fonction retourne directement une valeur
>   - Le problème P peut être divisé en problèmes simples (P1, P2, P3,...) qui peuvent tous être résolus par la fonction F. Dans ce cas, la fonction retourne le résultat de la résolution de tous les sous-problèmes par elle-même

Il est essentiel que la décomposition en problèmes simples débouche sur un problème trivial, faute de quoi on part dans une boucle infinie.

Exemple classique: le calcul de la factorielle d'un nombre n :

Rappel mathématique:

>`6! = 6 * 5 * 4 * 3 * 2 * 1 = 720`

On peut donc écrire la fonction factorielle ainsi en C#:
```csharp
int Factorielle(int x)
{
    int res = 1;
    for (int i = x; i > 1; i--) res *= i;
    return res;
}
```

En observant le rappel ci-dessus, on voit que l'on peut écrire:

>`6! = 6 * 5!` (puisque `5! = 5 * 4 * 3 * 2 * 1`)

Du coup, comme le calcul de `1!` est trivial, on peut écrire la fonction ainsi:

```csharp
int Factorielle(int x)
{
    if (x==1) return 1;
    return x * Factorielle(x - 1);
}
```

> On remarque que l'expression récursive de la fonction n'utilise, dans cet exemple, que des variables immutables...

## Récursion et programmation fonctionnelle

En programmation fonctionnelle, la récursion est la manière naturelle de traiter des structures
qui *se répètent à différentes échelles* (listes, arbres, fractales...).

**Tout algorithme récursif suit le même schéma :**

1. **Cas de base** : le plus petit problème qu'on sait résoudre directement
2. **Règle de combinaison** : résoudre le problème en combinant des solutions plus petites

```csharp
// Somme récursive d'une liste
int Sum(IEnumerable<int> list)
{
    if (!list.Any()) return 0;               // Cas de base : liste vide → 0
    return list.First() + Sum(list.Skip(1)); // Règle : premier + somme du reste
}

// Fibonacci : chaque terme = somme des deux précédents
long Fib(int n) => n <= 1 ? n : Fib(n - 1) + Fib(n - 2);
```

### Récursion et Fold

La récursion et le `Aggregate` (Fold) sont les deux faces de la même pièce.
`Aggregate` *est* la récursion, généralisée et rendue itérative pour éviter les stack overflows.

```csharp
// Sum récursif et Sum via Fold — équivalents conceptuellement
int SumRecursive(IEnumerable<int> list)
    => list.Any() ? list.First() + SumRecursive(list.Skip(1)) : 0;

int SumFold(IEnumerable<int> list)
    => list.Aggregate(0, (acc, val) => acc + val);
```

> Quand la structure du problème *est* récursive (arbre, fractal, décomposition hierarchique),
> la récursion explicite reste plus lisible qu'un Fold. Les deux outils coexistent.

### Performances
Historiquement, les fonctions récursives étaient moins performantes à cause du fait qu’un appel de fonction impliquait d’allouer des éléments sur la pile, ce qui n’est plus le cas avec la technologie `Tail Call Optimisation` alias `TCO`:

![Alt text](stack.png)

Un test non exhaustif et sujet à des optimisations `JIT` avec .NET 6 donne les résultats suivants sur des factiorielles entre 200 et 210:

![Alt text](perf1.png)

### Cas d’application
La récursivité est particulièrement utile pour les problèmes comme les tris (ex. : quicksort), les arbres binaires, ou les algorithmes de recherche.

