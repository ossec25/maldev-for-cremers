# Méthodologie

## Observation/Analyse
A la lecture de l'énoncé et du programme figurant en solution d'exemple, je constate que ce qui m'est demandé est de reprendre le malware généré en exercice 2 et de rendre le processus cible configurable en :  
- Prenant le nom du processus normalisé (en retirant le ".exe") en argument depuis la ligne de commande pour le rechercher
- Si le processus existe, prendre son PID directement
- Si aucun processus ne correspond au nom demandé, lancer le binaire correspondant et récupérer son PID
- Utiliser le bloc injection de l'exercice 2 mais en visant le PID obtenu

## Mise en place

- Phase de vérification des arguments en ligne de commande : 

*if (args.Length != 1)
{
    Console.WriteLine("Usage: MaldevEx2_Injection.exe <targetProcessName>");
    ...
    return;
}*

Par cette phase, le programme vérifie que exactement un argument a été fourni lors de l’exécution.
Cet argument correspond au nom du processus cible.

- Phase de normalisation du nom du processus :

*string targetProcName = args[0].Trim();
if (targetProcName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
    targetProcName = targetProcName.Substring(0, targetProcName.Length - 4);*
    
Le nom du processus fourni par l’utilisateur est nettoyé (suppression des espaces inutiles, retrait éventuel de l’extension .exe).
Cette étape est nécessaire car l’API GetProcessesByName() attend un nom sans extension.

- Phase de recherche du processus cible existant :

*Process[] found = Process.GetProcessesByName(targetProcName);*

Le programme interroge le système pour déterminer si un processus portant ce nom est déjà en cours d’exécution. 
Si le processus existe, le programme récupère l'identifiant de processus (PID) : 

*if (found.Length > 0)
{
    pid = found[0].Id;
}*

Si aucune instance du processus n'est trouvée, le programme tente de lancer l'exe correspondant au processus sollicité en supposant que celui-ci est disponible dans le path utilisateur et il récupère le PID : 

*else
{
    var p = new Process();
    p.StartInfo.FileName = targetProcName + ".exe";
    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    p.Start();
    pid = p.Id;
}*






