# Pratique de `filter`
Créer un nouveau dossier dans le dossier personnel pour stocker les solutions de l'exercice.

## 1. Jouer avec les mots

### Partie 1 : Filtrage simple
Selon une liste de mots, afficher les éléments qui
 - ne contiennent pas la lettre `x`
 - ont 4 caractères ou plus
 - ont autant de caractères que la moyenne du nombre de caractères de la liste

```c#
string[] words = { "bonjour", "hello", "monde", "vert", "rouge", "bleu", "jaune" };
```

> Si possible, utiliser des lambdas

### Partie 2: Epsilon

Trouver une source fiable sur la répartition des lettres en français (A:8.15%, B:0.97%,...) et afficher les mots avec leur valeur Epsilon et **uniquement ceux dont la valeur Epsilon est comprise entre 0.5 et 0.95**. 

*La valeur Epsilon correspond à la somme des probabilités (pourcentage=probabilité\*100) d'apparation de chaque lettre d'un mot si les lettres sont toute différentes. En cas de lettre présente 2x dans le mots, par exemple, la probabilité est divisée par le nombre de lettres*

#### Exemple de calcul pour Epsilon

|Source|Epsilon|
| :-- | :-------------- |
| ABA | 0.0815/2 + 0.0097 |

> Si vous ne savez pas par quel bout prendre ce problème, regardez [ça](steps.md).

### Partie 3: Dictionnaire
Filtrer désormais dynamiquement les mots qui s'écrivent de la même manière en français et en anglais avec cette liste source:

```csharp
List<string> frenchWords = new List<string>() {
    "Merci",
    "Hotdog",
    "Oui",
    "Non",
    "Désolé",
    "Réunion",
    "Manger",
    "Boire",
    "Téléphone",
    "Ordinateur",
    "Internet",
    "Email",
    "Sandwich",
    "Hello",
    "Taxi",
    "Hotel",
    "Gare",
    "Train",
    "Bus",
    "Métro",
    "Tramway",
    "Vélo",
    "Voiture",
    "Piéton",
    "Feu rouge",
    "Cédez",
    "Ralentir",
    "gauche",
    "droite",
    "Continuer",
    "Sandwich",
    "Retourner",
    "Arrêter",
    "Stationnement",
    "Parking",
    "Interdit",
    "Péage",
    "Trafic",
    "Route",
    "Rond-point",
    "Football",
    "Carrefour",
    "Feu",
    "Panneau",
    "Vitesse",
    "Tramway",
    "Aéroport",
    "Héliport",
    "Port",
    "Ferry",
    "Bateau",
    "Canot",
    "Kayak",
    "Paddle",
    "Surf",
    "Plage",
    "Mer",
    "Océan",
    "Rivière",
    "Lac",
    "Étang",
    "Marais",
    "Forêt",
    "Hello",
    "Montagne",
    "Vallée",
    "Plaine",
    "Désert",
    "Jungle",
    "Savane",
    "Volleyball",
    "Tundra",
    "Glacier",
    "Neige",
    "Pluie",
    "Soleil",
    "Nuage",
    "Vent",
    "Tempête",
    "Ouragan",
    "Tornade",
    "Séisme",
    "Tsunami",
    "Volcan",
    "Éruption",
    "Ciel"
};
```

## 2. Cinéma
Selon la liste de films suivants, réaliser les filtres suivants:

1. Filtrer les films qui ne sont pas du genre "Comédie" or "Drame".
2. Identifier les films dont le rating est inférieur à 7.
3. Afficher les films réalisés avant 2000.
4. Trouver les films qui n'ont pas de doublage en français.
5. Identifier les films non présents sur netflix

```csharp
List<Movie> frenchMovies = new List<Movie>() {
new Movie() { Title = "Le fabuleux destin d'Amélie Poulain", Genre = "Comédie", Rating = 8.3, Year = 2001, LanguageOptions = new string[] {"Français", "English"}, StreamingPlatforms = new string[] {"Netflix", "Hulu"} },
new Movie() { Title = "Intouchables", Genre = "Comédie", Rating = 8.5, Year = 2011, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix", "Amazon"} },
new Movie() { Title = "The Matrix", Genre = "Science-Fiction", Rating = 8.7, Year = 1999, LanguageOptions = new string[] {"English", "Español"}, StreamingPlatforms = new string[] {"Hulu", "Amazon"} },
new Movie() { Title = "La Vie est belle", Genre = "Drame", Rating = 8.6, Year = 1946, LanguageOptions = new string[] {"Français", "Italiano"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie() { Title = "Gran Torino", Genre = "Drame", Rating = 8.2, Year = 2008, LanguageOptions = new string[] {"English"}, StreamingPlatforms = new string[] {"Hulu"} },
new Movie() { Title = "La Haine", Genre = "Drame", Rating = 8.1, Year = 1995, LanguageOptions = new string[] {"Français"}, StreamingPlatforms = new string[] {"Netflix"} },
new Movie() { Title = "Oldboy", Genre = "Thriller", Rating = 8.4, Year = 2003, LanguageOptions = new string[] {"Coréen", "English"}, StreamingPlatforms = new string[] {"Amazon"} }
};
```

>Note: La classe `Movie` devrait avoir les propriétés suivantes: `Title`, `Genre`, `Rating`, `Year`, `LanguageOptions` (qui est un tableau de string), et `StreamingPlatforms` (qui est aussi un tableau de string).

#### Version 2 : Cumul
Réaliser désormais un filtre qui cumule tous les filtres précédents sur le cinéma.

#### Version 3: Dynamique
Pour chaque filtre, laisser l'utilisateur choisir le ou les valeurs de critères (console ou GUI à choix).

## 3. Publication intermédiaire sur GIT
Si ce n'est pas déjà fait, pusher la solution de l'exercice (soigneusement mise dans le dossier personnel du fork) dans le dépôt.

## 4. Hardware

Selon la liste `computerHardware` ci-dessous, filtrer:

1. Les pièces n'étaint pas des "centre de calculs"
2. Les pièces qui dépassent un prix de 500
3. Les CPUs mauvais pour jouer (qui ont une horloge < 3ghz et moins que 4 coeurs).
4. Les configs potables : Les GPUs qui ont au moins 32 coeurs et les CPUs avec au moins 8 coeurs.
5. Les configs AMD

```csharp
List<ComputerHardware> computerHardware = new List<ComputerHardware>() {
    new ComputerHardware() { Name = "Intel Core i7-9700K", Type = "CPU", Price = 400, ClockSpeed = 3.6, Cores = 8, Brand = "Intel" },
    new ComputerHardware() { Name = "AMD Ryzen 9 5950X", Type = "CPU", Price = 700, ClockSpeed = 3.4, Cores = 16, Brand = "AMD" },
    new ComputerHardware() { Name = "NVIDIA GeForce RTX 3080", Type = "GPU", Price = 700, ClockSpeed = 1.7, Cores = 8704, Brand = "NVIDIA" },
    new ComputerHardware() { Name = "AMD Radeon RX 6800 XT", Type = "GPU", Price = 650, ClockSpeed = 2.0, Cores = 72, Brand = "AMD" },
    new ComputerHardware() { Name = "Intel Core i5-10400", Type = "CPU", Price = 200, ClockSpeed = 2.9, Cores = 6, Brand = "Intel" },
    new ComputerHardware() { Name = "AMD Ryzen 5 5600X", Type = "CPU", Price = 300, ClockSpeed = 3.7, Cores = 6, Brand = "AMD" },
    new ComputerHardware() { Name = "NVIDIA GeForce RTX 3060 Ti", Type = "GPU", Price = 400, ClockSpeed = 1.6, Cores = 4864, Brand = "NVIDIA" },
    new ComputerHardware() { Name = "AMD Radeon RX 6700 XT", Type = "GPU", Price = 400, ClockSpeed = 2.4, Cores = 40, Brand = "AMD" },
    new ComputerHardware() { Name = "Intel Core i9-11900K", Type = "CPU", Price = 500, ClockSpeed = 3.2, Cores = 10, Brand = "Intel" },
    new ComputerHardware() { Name = "AMD Ryzen 7 5800X", Type = "CPU", Price = 350, ClockSpeed = 3.9, Cores = 8, Brand = "AMD" },
    new ComputerHardware() { Name = "NVIDIA GeForce RTX 3090", Type = "GPU", Price = 1500, ClockSpeed = 1.4, Cores = 10496, Brand = "NVIDIA" },
    new ComputerHardware() { Name = "AMD Radeon RX 6900 XT", Type = "GPU", Price = 1000, ClockSpeed = 2.0, Cores = 80, Brand = "AMD" },
    new ComputerHardware() { Name = "Intel Core i3-10100", Type = "CPU", Price = 150, ClockSpeed = 3.6, Cores = 4, Brand = "Intel" },
    new ComputerHardware() { Name = "AMD Ryzen 3 5600X", Type = "CPU", Price = 250, ClockSpeed = 3.6, Cores = 6, Brand = "AMD" },
    new ComputerHardware() { Name = "NVIDIA GeForce RTX 3070", Type = "GPU", Price = 500, ClockSpeed = 1.5, Cores = 5888, Brand = "NVIDIA" },
    new ComputerHardware() { Name = "AMD Radeon RX 6700", Type = "GPU", Price = 350, ClockSpeed = 2.3, Cores = 36, Brand = "AMD" },
    new ComputerHardware() { Name = "Intel Core i9-9900K", Type = "CPU", Price = 450, ClockSpeed = 3.2, Cores = 8, Brand = "Intel" },
    new ComputerHardware() { Name = "AMD Ryzen 7 3700X", Type = "CPU", Price = 300, ClockSpeed = 3.6, Cores = 8, Brand = "AMD" },
    new ComputerHardware() { Name = "NVIDIA GeForce RTX 3080 Ti", Type = "GPU", Price = 1200, ClockSpeed = 1.6, Cores = 5888, Brand = "NVIDIA" },
    new ComputerHardware() { Name = "AMD Radeon RX 6800", Type = "GPU", Price = 600, ClockSpeed = 1.8, Cores = 64, Brand = "AMD" }
};
```

> Créer la classe `ComputerHardware` selon les données

### Export CSV
Générer pour chaque filtre, un fichier CSV avec les données sélectionnées

### Export Excel
Idem avec export excel (librairie à choix)

## Maj GIT
Pusher si nécessaire le travail réalisé