# Méthodologie

## Observation et analyse
La méthode observée dans la solution du workshop pour exécuter le shellcode sans créer de thread via l'API Win32 CreateThread est d'utiliser un **"delegate unmanaged"** pour interpréter l'adresse mémoire contenant le shellcode comme un **pointeur de fonction** et ensuite de l'invoquer. On transforme l'adresse en fonction C# qui ne retourne rien, de manière à exécuter le shellcode dans le thread en cours à la place de créer un autre thread. 
L'allocation de mémoire virtuelle avec des droits RWX et la copie d'octets (en l'occurence le shellcode) via la méthode Marshal.Copy dans cette zone mémoire exécutable restent d'application. 

## Adaptation au shellcode Messagebox
Je reprends le loader créé dans la première partie de l'exercice 1 et j'adapte le code selon l'analyse effectuée sur calc.exe, en ne faisant pas appel à CreateThread mais en utilisant le thread courant via un delegate unmanaged. 

## Avantage mesuré et retour d'expérience
Si l'intérêt de ne pas utiliser CreateThread est sensé diminuer la probabilité de détection d'un patern très connu, la combinaison VirtualAlloc et copie d'octets en mémoire exécutable RWX constituent encore des signes de suspicion. J'en ai pour preuve que lorsque j'ai compilé le code sur Visual Studio installé sur ma VM de développement (sur laquelle j'avais pris soin de désactiver Windows Defender), un pop-up est quand même venu m'avertir de la probabilité de la présence d'un malware. Mon fichier program.cs ainsi que le fichier .exe ont tous deux disparu. Il m'a fallu un moment pour comprendre qu'ils avaient été placé en quarantaine et pouvoir les récupérer.   
