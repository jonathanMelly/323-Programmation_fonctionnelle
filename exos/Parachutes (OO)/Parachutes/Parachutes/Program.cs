// Apparence d'un parachutiste avec et sans parachute en ASCII art
string[] withoutParachute =
{
            @"     ",
            @"     ",
            @"     ",
            @"  o  ",
            @" /░\ ",
            @" / \ ",
        };

string[] withParachute =
{
            @" ___ ",
            @"/|||\",
            @"\   /",
            @" \o/ ",
            @"  ░  ",
            @" / \ ",
        };

// Apparence de l'avion en ASCII art
string[] plane =
{
            @"  _                         ",
            @" | \                        ",
            @" |  \       ______          ",
            @" --- \_____/  |_|_\____  |  ",
            @"   \_______ --------- __>-} ",
            @"         \_____|_____/   |  "
        };

// Initialiser la console
Console.Clear();

for (int i = 0; i < plane.Length; i++)
{
    Console.SetCursorPosition(0, i);
    Console.Write(plane[i]);
}

Console.CursorVisible = false;

// TODO: Définir la taille de la console

// Pour les interactions utilisateur
ConsoleKeyInfo keyPressed;

// TODO: Créer le groupe de parachutistes

// TODO: Créer l'avion et embarquer le club

while (true)
{
    // Capturer les entrées utilisateur
    if (Console.KeyAvailable) // L'utilisateur a pressé une touche
    {
        keyPressed = Console.ReadKey(false);
        switch (keyPressed.Key)
        {
            case ConsoleKey.Escape:
                Environment.Exit(0);
                break;
            case ConsoleKey.Spacebar:
                // TODO: Faire sauter un parachutiste de l'avion
                break;
            default:
                break;
        }
    }

    // TODO: Mettre à jour les positions de l'avion et des parachutistes

    // TODO: Dessiner l'avion et les parachutistes

    // Attendre un peu avant de recommencer
    Thread.Sleep(100);
}


