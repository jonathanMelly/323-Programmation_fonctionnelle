# P_FUN — "Plot Those Lines!" : Guide par Thématique

> Ce document accompagne les spécifications du projet (voir `P_FUN-Specifications.pdf`).
> Il regroupe les étapes P_FUN correspondant à chaque thématique du module 323.

## Thématique 1 — Fondations

- Choisir le domaine du projet (météo, sport, finance, santé, gaming...)
- Modéliser les données comme records immuables en C#
- Implémenter l'import des données réelles dans `DataSeries<T>`
- Vérifier l'immutabilité : tenter de modifier une valeur d'un record et observer l'erreur de compilation
- Écrire une première requête déclarative (`.Where(...)`, `.Count()`) et comparer avec la version impérative en boucle

## Thématique 2 — Filter

- Filtrer les données invalides ou hors-plage avec `.Filter()`
- Nommer le prédicat de filtrage dans une `Func<T, bool>` plutôt qu'un lambda inline
- Demo paresse : construire la query, ajouter un élément à la source, observer le résultat
- Vérifier avec `.HasAny()` si des valeurs aberrantes subsistent dans le dataset

## Thématique 3 — Map

- Transformer les données brutes du domaine (conversion d'unité, normalisation, arrondi...)
- Chaîner `.Filter().Transform().Filter()` en un seul pipeline fluent
- Identifier et nommer une closure dans le code (quelle variable est capturée ? depuis quel scope ?)

## Thématique 4 — Fold

- Calculer les statistiques (min, max, moyenne) sur la série via `.Statistics()`
- Réécrire `Sum` et `Max` sous forme de `.Fold()` pour observer l'équivalence avec `Aggregate`
- Utiliser `.SlidingWindow()` pour afficher une tendance lissée sur le graphique

## Thématique 5 — Extensions

- Écrire au moins 2 méthodes d'extension spécifiques au domaine choisi (ex: `.FilterNightTime()`, `.ToKelvin()`)
- Vérifier que le pipeline se lit comme une phrase naturelle dans ce domaine
- Si plusieurs séries sont disponibles (ex: temp + humidité, kills + deaths), utiliser `.PairWith()`

## Thématique 6 — Purity

- Auditer les fonctions d'import et de transformation : pures ou impures ?
- Identifier et corriger au moins une méthode impure dans la bibliothèque
- Ajouter un test unitaire sur une fonction pure

## Thématique 8 — Recursion (optionnel)

- Explorer s'il existe une structure hiérarchique dans les données (mois/semaines/jours, catégories imbriquées...)
- Utiliser `.Decompose()` pour afficher plusieurs niveaux de détail sur le graphique
- Voir les [Fractales](exos/fractale/README.md) pour la beauté de la récursion visuelle
