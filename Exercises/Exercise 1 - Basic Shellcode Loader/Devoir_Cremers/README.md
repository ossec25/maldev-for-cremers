# Méthodologie

## 1.Observation et analyse
Pour créer mon propre loader, j'ai tout d'abord examiné la solution présentée dans le workshop et je l'ai comparée avec d'autres solutions trouvées en ligne. 
J'ai constaté que les loaders en C# observés faisaient appel à des API Windows natives en vue d'allouer une zone de mémoire (WinAPI VirtualAlloc) avec des droits exécutables, pour y copier un shellcode avec la méthode statique Marshal.copy, créer un thread (WinAPI CreateThread) dont le point de départ est l'adresse de cette zone mémoire allouée et attendre que le thread se finisse (WinAPI WaitForSingleObject) et donc que le shellcode se soit exécuté dans le même processus. 

Ce mécanisme .NET d'appel à des API Windows natives (présentes dans le kernel32) à partir du langage C# au moyen de déclarations "DLLImport" s'appelle P/Invoke. L'utilisation du mécanisme P/Invoke et de la méthode Marshal.copy suppose l'appel (using) à System.Runtime.interopServices.   

La WinAPI *VirtualAlloc comprend plusieurs paramètres utiles : 
- lpAddress : L'adresse souhaitée, ici IntPtr.Zero (autrement dit aucune adresse sollicitée en particulier)
- dwSize : La taille allouée
- flAllocationType : Mémoire réservée (MEM_RESERVE), Mémoire attribuée (MEM_COMMIT)
- flProtect : Permissions mémoires (ici RWX)
L'appel à la WinAPI VirtualAlloc retourne soit une adresse (IntPtr), soit IntPtr.Zero en cas d'échec.

La WinAPI *CreateThread comprend les paramètres utiles suivants : 
- lpStartAddress : Adresse de départ du programme par le CPU (ici payAddr, c-a-d l'adresse du shellcode)
- lpParameter : paramètre passé au thread (ici, aucun)
- dwCreationFlags : Flag de démarrage du thread (ici 0, c'est à dire démarrage immédiat)

La WinAPI *WaitForSingleObject attend qu'un objet Kernel (ici le thread) soit terminé. 
- dwMilliseconds : délai d'attente (ici -1, c-a-d attente infinie, ce qui va permettre au shellcode de s'exécuter sans que le thread ne se ferme trop tôt. µ

Les constantes EXECUTEREADWRITE = 0x40 et COMMIT_RESERVE = 0x3000

Le shellcode calc.exe est un payload de démonstration (donc non destructif).  
