# Méthodologie

## 1. Observation et analyse
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
- dwMilliseconds : délai d'attente (ici -1, c-a-d attente infinie, ce qui va permettre au shellcode de s'exécuter sans que le thread ne se termine trop tôt.)

Les constantes EXECUTEREADWRITE = 0x40 et COMMIT_RESERVE = 0x3000 correspondent à des flags WinAPI indiquant pour la première que la mémoire sera lisible, modifiable et exécutable (RWX) et pour la seconde la réserve d'une d'une plage d'adresses virtuelles et la garantie de l'allocation réelle de cette plage à une page (commit)  

Le shellcode calc.exe est un payload de démonstration (donc non destructif). Intégré dans le loader sous forme de tableau, les bytes de ce binaire d'instructions machine généré par msfvenom de Metasploit, seront copiés par Marshal.Copy vers la mémoire native à PayAddr. 

## 2. Test
Avant de créer un loader complet, j'ai testé le programme C# sur la VM de développement en maintenant la sécurité Windows Defender et en enlevant l'appel à l'API CreateThread. J'ai pu constater que sans la partie exécutoire, les appels aux autres API Windows natives, bien que constituant un pattern suspect, ne provoquent aucune réaction de sécurité (et pourraient donc probablement être utilisées ensemble dans certaines opérations ordinaires). 

## 3. Choix du loader "Messagebox"
Je choisis un autre loader innofensif (de démonstration) pour C# en Windows x64 : Messagebox. 
Ce loader effectue également un appel P/Invoke aux trois API natives Windows précitées (VirtualAlloc, CreateThread et WaitFor
