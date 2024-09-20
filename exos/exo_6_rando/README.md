# Exercice: Rando

Dans cet exercice, vous allez manipuler des tracés de parcours de randonnée sous format GPX.

Deux fichiers vous sont fournis, ils ont été générés par le site [SuisseMobile](https://schweizmobil.ch/fr/ete)

## Lecture 

Trouvez un moyen de lire un [fichier `.gpx`](./gpx/) pour le stocker dans une liste d'objets `Trackpoint` dont voici la classe dans son expression la plus simple possible:

```csharp
    class Trackpoint
    {
        private double _latitude;
        private double _longitude;
        private double _elevation;
    }
```

Si vous ne trouvez pas la lib de vos rêves, vous pouvez toujours vous rabattre sur celle-ci:
<details>
<summary>Cliquer ici pour voir la suggestion</summary>

[C# Gpx Reader/Writer](http://dlg.krakow.pl/gpx/?i=1)
</details>

## Ecriture

Sauvegardez votre liste dans un fichier en format `.gpx`, le fichier doit être lisible dans [GPX Studio](https://gpx.studio/app#11.66/46.4303/7.6373).

## Transformation

Maintenant que vous tenez les deux bouts, faites des choses utiles entre deux!

Par exemple:

1. Réduire la définition: on ne garde qu'un point sur cinq 
2. Calculer la longueur du tracé, son dénivelé positif et négatif
3. Combiner: lire deux fichiers et les fusionner en un seul tracé. Attention à ne pas faire n'importe quoi: on refuse de fusionner deux fichiers si le point d'arrivée d'un fichier est trop loin du point de départ de l'autre ou vice versa.
4. Réduire intelligeamment: On ne supprime pas un point tout les X points, on regarde la distance entre un point et le suivant et on supprime les points qui sont trop proches du précédent.
   
## Affichage

Faites un nouveau projet, basé sur cette solution, intégrez-y votre code de lecture du fichier `.gpx`.

### 1. Affichage de la trace avec une échelle pertinente
Codez le bon transformeur pour que la trace GPS devienne une ligne dessinée par [`DrawLines`](https://learn.microsoft.com/en-us/dotnet/api/system.drawing.graphics.drawlines?view=net-8.0)

### 2. Réaliser une animation
Un peu comme avec [relive](https://relive.cc), la trace s’affiche dynamiquement avec un temps de pause 

- identique pour chaque point
- proportionnelle au dénivelé entre les 2 points

### 3. Ajouter la carte en fond
Trouver une image satellite d’un des tracés pour donner une meilleure immersion...