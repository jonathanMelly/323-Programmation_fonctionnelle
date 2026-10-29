# Idées pour 2027
- XCL: commencer par les extensions 
On pourrait leur faire écrire - par exemple - string[] Where(string[] haystack, string needle), le transformer en string[].Where(string needle), puis en string[].Where(Func<string, bool> predicate)

- JMY: ajouter des drills hors fil rouge
- JMY: revoir le fil rouge (moins de bruit...)

# Idéou pour la prochaine mouture 26
- Traiter SelectMany et Concat
 - SelectMany : plusieurs champs (évite une posttransformation)
 - Concat : fait des OR dans l’ordre donné...
 
- Ajouter des activités qui font des mappings de fonctions (ex. KTD et PhotoSaver)
    - par exemple Action a = doThat;
  
## Plot those lines

XCL:
- Ne pas mettre l'affichage de fonctions en facultatif dans le CdC, ça a semé la confusion chez moi
- Imposer une techno de persistence (en rapport avec les modules en cours si possible). Ils se sont rués sur l'idée des csv qui leur a paru facile, ça a causé des déconvenues (parsing, locale, intégrité des données, ...)
- (à réfléchir): fournir quelques US "modèle" qu'ils doivent contextualiser , plutôt que de les écrire from scratch
