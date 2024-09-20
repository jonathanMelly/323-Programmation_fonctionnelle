# SWAPI

Enfilez vos combinaisons, nous partons dans l'hyper-espace !!!

Première étape: [là](https://swapi.dev/)

Vous avez compris de quoi il s'agit ? Alors ... à vous jouer!

## Planète 1

En vous aidant **uniquement** de ce site et de votre documentation personnelle (en d'autre termes **sans aucun recours** à une IA), écrivezun programme qui donne les réponses aux questions suivantes:

1. Quel est le film Star Wars dont le titre est le plus long ?
2. Quel est le personnage qui est présent dans le plus de films ?
3. Quelle est la planète la plus peuplée ?
4. Combien de starfighter X-Wing est-ce que je peux m'acheter si je vends un Star Destroyer ?
5. Est-ce qu'Obi-wan Kenobi peut piloter un Millennium Falcon ?
6. Quelle est le vaisseau le plus rapide en vitesse lumière (vmax = vitesse atmosphérique max * ratio hyperespace) ?
7. Combien de vaisseaux sont plus rapides que la moyenne de la vitesse atmosphérique de tous les vaisseaux ?
8. Quel est le budget nécessaire (en franc suisse (1 crédit = 0.778 CHF)) à l’achat de la flotte totale ?
9. Générer un CSV (vaisseau.txt) contenant les infos suivantes des vaisseaux : 
   - Nom du vaisseau
   - Prix
   - Longueur
   - Films dans lesquels ils apparaissent (nom des films en minuscule séparés par des tirets)
   - Nom des planètes survolées (nom des planètes en minuscule séparées par des tirets)

Vous n'allez pas implémenter toutes ces questions. Faites-en le plus possible en 45 minutes, en choisissant celles qui vous conviennent le mieux.

## Planète 2

Créer un programme en mode console qui:

  - Demande à l'utilisateur d'introduire un titre de film
  - Retrouve le film voulu en calculant la [distance de Levenshtein](https://fr.wikipedia.org/wiki/Distance_de_Levenshtein) avec chaque film de SWAPI. Etablir une valeur de cette distance au delà de laquelle on estime qu'on n'a pas reconnu le titre.
  - Ecrit le "contenu de l'affiche" du film trouvé: 
    - Titre
    - synopsis
    - durée
    - Acteurs

## Planète 3

Emmenez votre programme dans une autre galaxie: le Web!

Au lieu de rendre un résultat sous forme de texte, générez un fichier `.html`.

Servez-vous de [ce fichier](./billboard.html) comme base de travail, injectez vos données au bons endroits. Pour les images, prenez [celle-cis](./sw-affiches.zip).

Votre programme doit ouvrir le fichier résultant dans votre navigateur.

## Planète 4

Pas de film Star Wars sans le fameux texte d'introduction dans l'espace.

Récupérez le `opening_crawl` du film, mettez-le dans [index.html](./crawler/index.html) et lancez-le !
